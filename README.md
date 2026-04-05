[EN] - [English version](#backgroundremoving-en) | [PL] - [Wersja polska](#backgroundremoving-pl)

# BackgroundRemoving [EN]

A desktop application of the **Chroma Key** type used for removing backgrounds from raster images. The project was created to conduct a comparative performance analysis between high-level code (**C++**) and low-level vector optimization (**x64 Assembly**).

## Project Goal
The main objective was to demonstrate the superior performance of an assembly implementation utilizing **SIMD (AVX2)** vector instructions and multithreading compared to standard C++ code during intensive graphic data processing.

## Application Features
- **Background Removal:** Automatically setting the pixel's Alpha channel to zero (full transparency) for colors within a defined tolerance.
- **Dynamic Parameter Control:** Sliders for adjusting the tolerance threshold (Euclidean distance) and the number of CPU threads (1-64).
- **Multithreading:** The image is divided into horizontal stripes processed in parallel by the selected number of threads.
- **Benchmark Module:** A function generating time tests for various image resolutions and thread configurations, with result export to `.csv` format.
- **Interactive Color Selection:** Ability to pick the background color pattern directly from the loaded image by clicking on it.

## Technical Architecture and Optimizations

### Assembly Implementation (x64 AVX2)
The low-level code was designed for maximum data throughput:
- **SIMD Processing:** Utilization of 256-bit **YMM** registers, allowing for simultaneous processing of **8 pixels** in a single iteration.
- **Vector Data Transmission:** Use of the `vpbroadcastd` instruction to broadcast scalar values (RGB components of the key and tolerance) across entire vector registers.
- **Data Conversion:** Application of the `vpmovzxbd` instruction to convert 8-bit color components into 32-bit integers (int) necessary for precise calculations.
- **Masking:** Elimination of costly branching instructions in favor of bitwise operations `vpand`, `vpor`, and `vpcmpgtd` for conditional zeroing of the Alpha channel.

### Technologies
- **Interface Language:** C# (WinForms/WPF).
- **Computational Libraries:** C++ and x64 Assembly (AVX2).
- **Environment:** Visual Studio 2022.
- **Hardware Requirements:** Processor supporting the AVX2 instruction set.

## Performance Analysis
Tests were conducted on an Intel Core i7-9750H processor (6 cores, 12 threads) for image resolutions ranging from 640x360 to 5472x3648 px:

- **Acceleration:** The ASM AVX2 implementation achieved approximately 8 times faster execution time than the C++ code (e.g., 13,673 µs vs 107,940 µs for 1 thread at high resolution).
- **Scalability:** The application shows a nearly linear decrease in execution time when increasing the number of threads (1-4), until memory bus saturation occurs at a very high number of threads (consistent with Amdahl's Law).
- **Efficiency:** Utilizing the full width of vector registers (256-bit) allowed for 8-fold acceleration of calculations compared to sequential processing.

## File Formats
The application automatically converts loaded images (`.bmp`, `.jpg`, `.jpeg`, `.png`) to **32bpp ARGB** format, which is required for the correct operation of AVX2 vectorization.

---
*Project developed for educational purposes.*

---

# BackgroundRemoving [PL]

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
