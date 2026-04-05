# Information Theory & Cryptography Labs

## 📖 Overview
A collection of educational cryptographic implementations developed for an Information Theory course. The repository contains two distinct applications:
- **Lab 1:** Classical block cipher (Decimation) with interactive step-by-step visualization, built on Windows Forms.
- **Lab 2:** Stream cipher (LFSR-based XOR) with cross-platform UI, built on Avalonia & MVVM.

Both projects emphasize algorithmic transparency, input validation, and pedagogical visualization of cryptographic processes.

## 🏗️ Project Structure
```
InformationTheory/
├── Lab1/                          # Classical Crypto Systems
│   └── CryptoSystems/             # .NET 10 WinForms application
│       ├── ICryptoSystem.cs       # Extensible cipher interface
│       ├── DecimationCryptoSystem.cs # Core decimation logic
│       └── Form1.cs               # WinForms UI & DataGridView integration
├── lab2/                          # LFSR Stream Cipher (Primary)
│   ├── threadCipher/              # .NET 9 Avalonia application
│   │   ├── LfsrCipher.cs          # Core LFSR & XOR logic
│   │   └── ViewModels/Views/      # MVVM architecture
│   ├── testFiles/                 # Sample media & text for validation
│   ├── screenshots/               # UI & process documentation
│   └── Отчёт.docx                 # Lab report & analysis
└── README.md
```
> **Note:** `lab2var11` contains a variant of Lab 2 with identical architecture and additional test files.

## ⚙️ Prerequisites
| Component | Requirement |
|-----------|-------------|
| **.NET SDK** | `10.0` (Lab 1), `9.0` (Lab 2) |
| **IDE** | Visual Studio 2022, JetBrains Rider, or VS Code + C# Dev Kit |
| **OS** | Windows (Lab 1); Windows/macOS/Linux (Lab 2) |
| **Dependencies** | Avalonia UI `11.3.12`, CommunityToolkit.Mvvm `8.2.1` |

## 🚀 Quick Start
### Build & Run Lab 1 (WinForms)
```bash
cd Lab1/CryptoSystems/CryptoSystems
dotnet restore
dotnet run
```

### Build & Run Lab 2 (Avalonia)
```bash
cd lab2/threadCipher/threadCipher
dotnet restore
dotnet run
```

## 🔐 Lab 1: Classical Crypto Systems
### Algorithm
Implements a **Decimation Cipher** over a 33-character Russian alphabet (`а`–`я`, including `ё`, mapped `0–32`).
- **Encryption:** `C = (P × K) mod 33`
- **Decryption:** `P = (C × K⁻¹) mod 33` (resolved iteratively via modular adjustment)
- **Key Validation:** Automatically verifies `gcd(K, 33) = 1`. If invalid, adjusts to the nearest coprime integer and logs the correction.

### Architecture
- `ICryptoSystem`: Interface defining cipher operations. Includes `DataGridView` parameters for direct UI population (educational design choice).
- `DecimationCryptoSystem`: Handles alphabet mapping, key validation, and cryptographic transformations.
- **UI Integration:** `DataGridView` displays character mapping, arithmetic steps, and final output for pedagogical clarity.

### Usage
1. Launch the application.
2. Enter a numeric key (auto-corrected if non-coprime with 33).
3. Input Russian plaintext or ciphertext.
4. Observe step-by-step transformation in the data grid.

## 🌊 Lab 2: LFSR Stream Cipher
### Algorithm
Implements a **Linear Feedback Shift Register (LFSR)** stream cipher using byte-wise XOR encryption.
- **Characteristic Polynomial:** `x³⁰ + x¹⁶ + x¹⁵ + x + 1`
- **State Register:** 30-bit unsigned integer (`uint`)
- **Feedback Taps:** Bits 30, 16, 15, and 1
- **Operation:** Generates a pseudo-random keystream bit-by-bit, packed into bytes, and XORed with input data.

### Architecture
- **Framework:** Avalonia UI + CommunityToolkit.Mvvm
- **Pattern:** MVVM (ViewModel-First)
- **Core Class:** `LfsrCipher` manages register initialization, bit generation, and stream processing.
- **Input:** Exactly 30-character binary string (e.g., `101100110101110010101101011010`)

### Usage
1. Launch the application.
2. Provide a valid 30-bit binary seed.
3. Load any file (`.txt`, `.jpg`, `.mp3`, `.mp4`).
4. Execute encryption/decryption and inspect the keystream preview (first 64 bits).

## 🧪 Testing & Validation
- **Test Suite:** Pre-configured sample files in `lab2/testFiles/` covering text, images, audio, and video.
- **Verification:** Round-trip encryption/decryption ensures bit-perfect data recovery. Intentional key mismatches produce corrupted output (`testTextUnEncWrong.txt`) to demonstrate stream cipher properties.
- **Documentation:** 
  - `Отчёт.docx` contains methodology, results, and cryptographic analysis.
  - `Lfsr Регистр.xlsx` details register state transitions and period analysis.
  - `screenshots/` documents UI states and processing steps.

## ⚠️ Disclaimer
This repository is intended for **educational purposes only**. Implementations prioritize algorithmic transparency and pedagogical value over production-grade security, side-channel resistance, or cryptographic best practices. **Do not use for securing sensitive or production data.**

## 📄 License
[Insert License Here]  
*Example: MIT License or University Academic Use Policy*

---
*Generated for academic documentation. For questions or contributions, refer to the course instructor or repository maintainer.*