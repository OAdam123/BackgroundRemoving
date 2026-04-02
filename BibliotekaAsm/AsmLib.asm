option casemap:none

; ============================================================
; ProcessImageAsm(pData, width, height, stride, keyR, keyG, keyB, tolerance)
; Pixel format: 32bpp ARGB in memory as BGRA bytes (little endian)
; Remove background: if (dB^2 + dG^2 + dR^2) <= tolerance^2 => alpha = 0
; Uses AVX2 YMM (8 pixels per iteration)
; ============================================================

.data
ALIGN 16                                                                ;wyrownaj dane do 16 bajtow
shufB db 0,4,8,12, 80h,80h,80h,80h, 80h,80h,80h,80h, 80h,80h,80h,80h    ;maska do wyciagania kanalu B
shufG db 1,5,9,13, 80h,80h,80h,80h, 80h,80h,80h,80h, 80h,80h,80h,80h    ;maska do wyciagania kanalu G
shufR db 2,6,10,14,80h,80h,80h,80h, 80h,80h,80h,80h, 80h,80h,80h,80h    ;maska do wyciagania kanalu R

ALIGN 16                                                    ;wyrownaj dane do 16 bajtow
alphaKeepMask dd 00FFFFFFh,00FFFFFFh,00FFFFFFh,00FFFFFFh    ;maska 32-bitowa dla dolnej polowy YMM
              dd 00FFFFFFh,00FFFFFFh,00FFFFFFh,00FFFFFFh    ;maska 32-bitowa dla gornej polowy YMM

.code
PUBLIC ProcessImageAsm

ProcessImageAsm PROC
    ; ---- ZAPAMIETYWANIE STANU ----
    mov     r10, rsp    ;zapamietaj oryginalny rsp

    push    rbx         ;zapamietywanie rejestrow (wymog konwencji)
    push    rsi
    push    rdi
    push    r12
    push    r13
    push    r14
    push    r15

    ; ---- POBIERANIE ARGUMENTOW ----
    mov     rsi, rcx            ; pData
    mov     ebx, edx            ; width
    mov     r12d, r8d           ; height
    mov     r13d, r9d           ; stride

    mov     r14d, dword ptr [r10+40] ; keyR - pobranie ze stosu
    mov     r15d, dword ptr [r10+48] ; keyG - pobranie ze stosu
    mov     r11d, dword ptr [r10+56] ; keyB - pobranie ze stosu
    mov     r9d,  dword ptr [r10+64] ; tolerance (int) - pobranie ze stosu

    ; ---- ZABEZPIECZENIA ----
    test    ebx, ebx    ;  width <= 0 ?
    jle     done        ; tak = koniec
    test    r12d, r12d  ; height <=0 ?
    jle     done        ; tak = koniec

    ; ---- OBLICZENIA WSTEPNE ----
    imul    r9d, r9d    ; tolerance^2

    ; ---- PRZYGOTOWANIE AVX ----
    vmovd   xmm0, r14d              ; keyR -> xmm0
    vpbroadcastd ymm12, xmm0        ; powiel keyR na caly rejestr ymm12
    vmovd   xmm0, r15d              ; keyG -> xmm0
    vpbroadcastd ymm13, xmm0        ; powiel keyG na caly rejestr ymm13
    vmovd   xmm0, r11d              ; keyB -> xmm0
    vpbroadcastd ymm14, xmm0        ; powiel keyB na caly rejestr ymm14
    vmovd   xmm0, r9d               ; tolerance^2 -> xmm0
    vpbroadcastd ymm11, xmm0        ; powiel tolerance^2 na caly rejestr ymm11

    ; ---- LADOWANIE MASEK ----
    vmovdqu xmm8, XMMWORD PTR shufB     ; zaladuj maski tasowania do rejestrow xmm
    vmovdqu xmm9, XMMWORD PTR shufG
    vmovdqu xmm10,XMMWORD PTR shufR

    vmovdqu ymm15, YMMWORD PTR alphaKeepMask    ; zaladuj maske zerowania kanalu alpha do ymm15

    ; ---- PETLE GLOWNE ----
row_loop:
    test    r12d, r12d      ; zostaly wiersze?
    jz      done            ; licznik = 0 => koniec

    mov     rdi, rsi        ; ustaw wskaznik biezacego piksela na poczatek wiersza
    mov     ecx, ebx        ; ustaw licznik kolumn = width

col_loop:
    cmp     ecx, 8          ; czy zostalo mniej niz 8 pikseli?
    jl      scalar_tail     ; tak => scalar_tail

    ; ---- PRZETWARZANIE WEKTOROWE ----
    vmovdqu ymm0, ymmword ptr [rdi]     ; pobierz 8 pikseli (32B) z pamieci do ymm0

    vextracti128 xmm1, ymm0, 0    ; skopiuj dolna polowe (4 piksele) do xmm1
    vextracti128 xmm2, ymm0, 1    ; skopiuj gorna polowe (4 piksele) do xmm2

    ; ---- KANAL NIEBIESKI ----
    vpshufb xmm3, xmm1, xmm8            ; wyciagnij 4 bajty B z dolnej polowy
    vpshufb xmm4, xmm2, xmm8            ; wyciagnij 4 bajty B z gornej polowy
    vpmovzxbd xmm3, xmm3                ; rozszerz dolna polowe do 4 dwords (int32)
    vpmovzxbd xmm4, xmm4                ; rozszerz gorna polowe do 4 dwords (int32)
    vinserti128 ymm3, ymm3, xmm4, 1     ; polacz do ymm3 
    vpsubd  ymm3, ymm3, ymm14           ; oblicz dB = B - keyB
    vpmulld ymm3, ymm3, ymm3            ; (dB)^2

    ; ---- KANAL ZIELONY ----
    vpshufb xmm5, xmm1, xmm9            ; analogicznie dla kanalu B
    vpshufb xmm6, xmm2, xmm9
    vpmovzxbd xmm5, xmm5
    vpmovzxbd xmm6, xmm6
    vinserti128 ymm4, ymm4, xmm6, 1     ; uzyj ymm4 do przechowania wyniku (gora)
    vinserti128 ymm4, ymm4, xmm5, 0     ; dolna polowka do ymm4
    vpsubd  ymm4, ymm4, ymm13           ; oblicz dG = G - keyG
    vpmulld ymm4, ymm4, ymm4            ; (dG)^2

    ; ---- KANAL CZERWONY ----
    vpshufb xmm5, xmm1, xmm10           ; analogicznie dla poprzednich kanalow
    vpshufb xmm6, xmm2, xmm10
    vpmovzxbd xmm5, xmm5
    vpmovzxbd xmm6, xmm6
    vinserti128 ymm5, ymm5, xmm6, 1
    vinserti128 ymm5, ymm5, xmm5, 0
    vpsubd  ymm5, ymm5, ymm12           ; oblicz dR = R - keyR
    vpmulld ymm5, ymm5, ymm5            ; (dR)^2

    ; ---- SUMA KANALOW (DYSTANSU) ----
    vpaddd  ymm3, ymm3, ymm4            ; sum = (dB)^2 + (dG)^2
    vpaddd  ymm3, ymm3, ymm5            ; sum += (dR)^2

    ; ---- POROWNANIE Z TOLERANCJA ----
    vpcmpgtd ymm6, ymm3, ymm11          ; dystans^2 > tolerance^2 ? => keepMask = FFFFFFFF : 00000000

    vpcmpeqd ymm7, ymm7, ymm7           ; wygeneruj maske 32-bitowa = FFFFFFFF
    vpxor   ymm7, ymm7, ymm6            ; odwroc maske => 1 = remove, 0 = keep

    vpand   ymm1, ymm0, ymm6            ; zachowaj oryginalne wartosci (AND FFFFFFFF)

    vpand   ymm2, ymm0, ymm7            ; wez wartosci do wyzerowania (AND 00000000)
    vpand   ymm2, ymm2, ymm15           ; wyzeruj kanal alpha

    vpor    ymm0, ymm1, ymm2            ; polacz: obiekt + przezroczystosc

    ; ---- ZAPIS WYNIKU ----
    vmovdqu ymmword ptr [rdi], ymm0     ; zapisz 8 przetworzonych pikseli do pamieci

    add     rdi, 32     ; przesun wskaznik o 8 pikseli (32B)
    sub     ecx, 8      ; zmniejsz licznik kolumn o 8
    jmp     col_loop    ; wroc na poczatek petli

    ; ---- PRZETWARZANIE SKALARNE (POJEDYNCZE PIXELE) ----
scalar_tail:
    test    ecx, ecx    ; czy zostaly piksele?
    jz      next_row    ; nie => nastepny wiersz

scalar_loop:
    movzx   eax, byte ptr [rdi]       ; pobierz B (rozszerz do 32b)
    movzx   edx, byte ptr [rdi+1]     ; pobierz G (rozszerz do 32b)
    movzx   r8d, byte ptr [rdi+2]     ; pobierz R (rozszerz do 32b)

    sub     eax, r11d       ; dB = B - keyB
    imul    eax, eax        ; (dB)^2

    sub     edx, r15d       ; dG = G - keyG
    imul    edx, edx        ; (dG)^2
    add     eax, edx        ; sum += (dG)^2

    sub     r8d, r14d       ; dR = R - keyR
    imul    r8d, r8d        ; (dR)^2
    add     eax, r8d        ; sum += (dR)^2

    cmp     eax, r9d        ; porownaj sum^2 z tolerance^2
    ja      keep_scalar     ; jesli wieksze => keep_scalar

    mov     byte ptr [rdi+3], 0     ; wyzeruj kanal alpha

keep_scalar:
    add     rdi, 4          ; przesun wskaznik o 1 piksel (4B)
    dec     ecx             ; zmniejsz licznik
    jnz     scalar_loop     ; jesli > 0 => powtorz

next_row:
    add     rsi, r13        ; przesun wskaznik poczatku linii o wartosc stride
    dec     r12d            ; zmniejsz licznik wierszy
    jmp     row_loop        ; wroc do petli wierszy

; ---- KONIEC FUNKCJI ----
done:
    vzeroupper          ; wyczysc rejestry AVX
    pop     r15         ; przywroc rejestry
    pop     r14
    pop     r13
    pop     r12
    pop     rdi
    pop     rsi
    pop     rbx
    ret
ProcessImageAsm ENDP

END
