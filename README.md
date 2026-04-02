# Chroma Key Image Processor (x64) 📸

Aplikacja desktopowa typu **Chroma Key** (kluczowanie kolorem) służąca do usuwania tła z obrazów rastrowych. Projekt został stworzony w celu przeprowadzenia analizy porównawczej wydajności między kodem wysokopoziomowym (**C++**) a niskopoziomową optymalizacją wektorową (**Asembler x64**).

## Cel projektu
Głównym założeniem było udowodnienie wyższej wydajności implementacji asemblerowej wykorzystującej instrukcje wektorowe **SIMD (AVX2)** oraz wielowątkowość w porównaniu do standardowego kodu C++ podczas intensywnego przetwarzania danych graficznych.

## Funkcje aplikacji
- **Usuwanie tła:** Automatyczne ustawianie kanału Alfa piksela na zero (pełna przezroczystość) dla kolorów mieszczących się w zdefiniowanej tolerancji.
- **Dynamiczna kontrola parametrów:** Suwaki do regulacji progu tolerancji (dystans euklidesowy) oraz liczby wątków procesora (1-64).
- **Wielowątkowość:** Obraz dzielony jest na poziome pasy przetwarzane równolegle przez wybraną liczbę wątków.
- **Moduł Benchmark:** Funkcja generująca testy czasowe dla różnych rozdzielczości obrazu i konfiguracji wątków, z eksportem wyników do formatu `.csv`.
- **Interaktywny wybór koloru:** Możliwość pobrania wzorca koloru tła bezpośrednio z wczytanego obrazu klikając na niego.

## Architektura techniczna i optymalizacje

### Implementacja Asembler (x64 AVX2)
Kod niskopoziomowy został zaprojektowany z myślą o maksymalnej przepustowości danych:
- **Przetwarzanie SIMD:** Wykorzystanie 256-bitowych rejestrów **YMM**, co pozwala na jednoczesną obróbkę **8 pikseli** w jednej iteracji.
- **Wektorowa transmisja danych:** Użycie instrukcji `vpbroadcastd` do powielania wartości skalarnych (składowe RGB wzorca i tolerancja) na całe rejestry wektorowe.
- **Konwersja danych:** Zastosowanie instrukcji `vpmovzxbd` do konwersji 8-bitowych składowych koloru na 32-bitowe liczby całkowite (int) niezbędne do precyzyjnych obliczeń.
- **Maskowanie:** Wyeliminowanie kosztownych instrukcji skoku (branching) na rzecz operacji bitowych `vpand`, `vpor` oraz `vpcmpgtd` do warunkowego zerowania kanału Alfa.

### Technologie
- **Język interfejsu:** C# (WinForms/WPF).
- **Biblioteki obliczeniowe:** C++ oraz Asembler x64 (AVX2).
- **Środowisko:** Visual Studio 2022.
- **Wymagania sprzętowe:** Procesor obsługujący zestaw instrukcji AVX2.

## Analiza wydajności
Testy przeprowadzono na procesorze Intel Core i7-9750H (6 rdzeni, 12 wątków) dla obrazów o rozdzielczościach od 640x360 do 5472x3648 px:

- **Przyspieszenie:** Implementacja ASM AVX2 osiągnęła około 8-krotnie krótszy czas wykonania niż kod C++ (np. 13 673 µs vs 107 940 µs dla 1 wątku przy wysokiej rozdzielczości).
- **Skalowalność:** Aplikacja wykazuje niemal liniowy spadek czasu wykonania przy zwiększaniu liczby wątków (1-4), aż do nasycenia szyny pamięci przy bardzo dużej liczbie wątków (zgodnie z prawem Amdahla).
- **Efektywność:** Wykorzystanie pełnej szerokości rejestrów wektorowych (256-bit) pozwoliło na 8-krotne przyspieszenie obliczeń względem przetwarzania sekwencyjnego.

## Formaty plików
Aplikacja automatycznie konwertuje wczytane obrazy (`.bmp`, `.jpg`, `.jpeg`, `.png`) do formatu **32bpp ARGB**, co jest wymagane dla poprawnego działania wektoryzacji AVX2.

---
*Projekt zrealizowany w celach edukacyjnych.*
