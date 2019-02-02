# math-struct-benchmark

Benchmarking techniques for math structs in C# in service of data for https://github.com/FNA-XNA/FNA/issues/227

Run the benchmarks: `dotnet run -f netcoreapp2.2 -c Release -- -f "*"`

## Results on my PC

``` ini
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i5-6500 CPU 3.20GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
Frequency=3117185 Hz, Resolution=320.8023 ns, Timer=TSC
.NET Core SDK=2.2.103
  [Host]       : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT
  Clr          : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0
  Core         : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT
  LegacyJitX64 : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0
  LegacyJitX86 : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3260.0
  MonoX64      : Mono 5.18.0 (Visual Studio), 64bit
  MonoX86      : Mono 5.18.0 (Visual Studio), 32bit
```

|                    Method |       Jit | Platform | Runtime |        Mean |     Error |    StdDev | Ratio |
|-------------------------- |---------- |--------- |-------- |------------:|----------:|----------:|------:|
|      Vector2_Fna_Operator |    RyuJit |      X64 |     Clr |   922.14 ms | 4.1927 ms | 3.9218 ms |  1.00 |
|    Vector2_Fna_Add_By_Ref |    RyuJit |      X64 |     Clr |   121.48 ms | 0.3504 ms | 0.3278 ms |  0.13 |
|        Vector2_B_Operator |    RyuJit |      X64 |     Clr |   757.82 ms | 3.9541 ms | 3.6987 ms |  0.82 |
| Numerics_Vector2_Operator |    RyuJit |      X64 |     Clr |   285.92 ms | 1.5591 ms | 1.4584 ms |  0.31 |
|     Separate_Local_Floats |    RyuJit |      X64 |     Clr |   121.37 ms | 0.5239 ms | 0.4645 ms |  0.13 |
|                           |           |          |         |             |           |           |       |
|      Vector2_Fna_Operator |    RyuJit |      X64 |    Core |   922.29 ms | 3.0219 ms | 2.8266 ms |  1.00 |
|    Vector2_Fna_Add_By_Ref |    RyuJit |      X64 |    Core |   121.50 ms | 0.4503 ms | 0.4212 ms |  0.13 |
|        Vector2_B_Operator |    RyuJit |      X64 |    Core |   759.98 ms | 2.3472 ms | 2.0807 ms |  0.82 |
| Numerics_Vector2_Operator |    RyuJit |      X64 |    Core |   285.03 ms | 1.3802 ms | 1.2235 ms |  0.31 |
|     Separate_Local_Floats |    RyuJit |      X64 |    Core |   120.31 ms | 1.7522 ms | 1.6390 ms |  0.13 |
|                           |           |          |         |             |           |           |       |
|      Vector2_Fna_Operator | LegacyJit |      X64 |     Clr |   922.80 ms | 2.1350 ms | 1.9971 ms |  1.00 |
|    Vector2_Fna_Add_By_Ref | LegacyJit |      X64 |     Clr |   120.73 ms | 1.3679 ms | 1.2795 ms |  0.13 |
|        Vector2_B_Operator | LegacyJit |      X64 |     Clr |   757.90 ms | 1.1626 ms | 1.0307 ms |  0.82 |
| Numerics_Vector2_Operator | LegacyJit |      X64 |     Clr |   285.48 ms | 1.3164 ms | 1.2313 ms |  0.31 |
|     Separate_Local_Floats | LegacyJit |      X64 |     Clr |   121.39 ms | 1.0034 ms | 0.8895 ms |  0.13 |
|                           |           |          |         |             |           |           |       |
|      Vector2_Fna_Operator | LegacyJit |      X86 |     Clr |   488.08 ms | 4.8625 ms | 4.5484 ms |  1.00 |
|    Vector2_Fna_Add_By_Ref | LegacyJit |      X86 |     Clr |    30.15 ms | 0.4445 ms | 0.4158 ms |  0.06 |
|        Vector2_B_Operator | LegacyJit |      X86 |     Clr |    97.60 ms | 1.5222 ms | 1.4238 ms |  0.20 |
| Numerics_Vector2_Operator | LegacyJit |      X86 |     Clr |   271.69 ms | 1.3041 ms | 1.2198 ms |  0.56 |
|     Separate_Local_Floats | LegacyJit |      X86 |     Clr |    30.11 ms | 0.1377 ms | 0.1150 ms |  0.06 |
|                           |           |          |         |             |           |           |       |
|      Vector2_Fna_Operator |    RyuJit |      X64 | MonoX64 | 1,592.60 ms | 3.7630 ms | 3.5199 ms |  1.00 |
|    Vector2_Fna_Add_By_Ref |    RyuJit |      X64 | MonoX64 |   289.58 ms | 3.3579 ms | 3.1409 ms |  0.18 |
|        Vector2_B_Operator |    RyuJit |      X64 | MonoX64 |   724.50 ms | 3.9091 ms | 3.6565 ms |  0.45 |
| Numerics_Vector2_Operator |    RyuJit |      X64 | MonoX64 |   758.91 ms | 2.3680 ms | 1.8488 ms |  0.48 |
|     Separate_Local_Floats |    RyuJit |      X64 | MonoX64 |   271.56 ms | 1.7974 ms | 1.6813 ms |  0.17 |
|                           |           |          |         |             |           |           |       |
|      Vector2_Fna_Operator |    RyuJit |      X64 | MonoX86 |   778.76 ms | 2.7323 ms | 2.5558 ms |  1.00 |
|    Vector2_Fna_Add_By_Ref |    RyuJit |      X64 | MonoX86 |   426.49 ms | 2.4002 ms | 2.2452 ms |  0.55 |
|        Vector2_B_Operator |    RyuJit |      X64 | MonoX86 |   634.28 ms | 1.4864 ms | 1.3904 ms |  0.81 |
| Numerics_Vector2_Operator |    RyuJit |      X64 | MonoX86 |   838.69 ms | 6.9621 ms | 6.1717 ms |  1.08 |
|     Separate_Local_Floats |    RyuJit |      X64 | MonoX86 |   266.28 ms | 2.3754 ms | 2.2219 ms |  0.34 |

|                    Method |       Jit | Platform | Runtime |        Mean |      Error |     StdDev | Ratio |
|-------------------------- |---------- |--------- |-------- |------------:|-----------:|-----------:|------:|
|      Vector3_Fna_Operator |    RyuJit |      X64 |     Clr |   427.06 ms |  1.8953 ms |  1.7728 ms |  1.00 |
|    Vector3_Fna_Add_By_Ref |    RyuJit |      X64 |     Clr |   120.61 ms |  0.7234 ms |  0.6766 ms |  0.28 |
|        Vector3_B_Operator |    RyuJit |      X64 |     Clr |   120.68 ms |  0.8109 ms |  0.7585 ms |  0.28 |
| Numerics_Vector3_Operator |    RyuJit |      X64 |     Clr |   120.15 ms |  1.1919 ms |  1.1149 ms |  0.28 |
|     Separate_Local_Floats |    RyuJit |      X64 |     Clr |   120.72 ms |  0.3689 ms |  0.3270 ms |  0.28 |
|                           |           |          |         |             |            |            |       |
|      Vector3_Fna_Operator |    RyuJit |      X64 |    Core |   421.20 ms |  4.3005 ms |  3.5911 ms |  1.00 |
|    Vector3_Fna_Add_By_Ref |    RyuJit |      X64 |    Core |   121.18 ms |  0.5391 ms |  0.5043 ms |  0.29 |
|        Vector3_B_Operator |    RyuJit |      X64 |    Core |   120.72 ms |  1.2305 ms |  1.1510 ms |  0.29 |
| Numerics_Vector3_Operator |    RyuJit |      X64 |    Core |   121.52 ms |  0.3093 ms |  0.2742 ms |  0.29 |
|     Separate_Local_Floats |    RyuJit |      X64 |    Core |   121.56 ms |  0.6858 ms |  0.6415 ms |  0.29 |
|                           |           |          |         |             |            |            |       |
|      Vector3_Fna_Operator | LegacyJit |      X64 |     Clr |   429.99 ms |  1.3993 ms |  1.3089 ms |  1.00 |
|    Vector3_Fna_Add_By_Ref | LegacyJit |      X64 |     Clr |   121.31 ms |  1.1790 ms |  1.1028 ms |  0.28 |
|        Vector3_B_Operator | LegacyJit |      X64 |     Clr |   120.97 ms |  0.6729 ms |  0.6294 ms |  0.28 |
| Numerics_Vector3_Operator | LegacyJit |      X64 |     Clr |   121.43 ms |  1.0689 ms |  0.9999 ms |  0.28 |
|     Separate_Local_Floats | LegacyJit |      X64 |     Clr |   121.31 ms |  0.4595 ms |  0.4299 ms |  0.28 |
|                           |           |          |         |             |            |            |       |
|      Vector3_Fna_Operator | LegacyJit |      X86 |     Clr |   477.90 ms |  2.0010 ms |  1.8717 ms |  1.00 |
|    Vector3_Fna_Add_By_Ref | LegacyJit |      X86 |     Clr |    30.31 ms |  0.0789 ms |  0.0738 ms |  0.06 |
|        Vector3_B_Operator | LegacyJit |      X86 |     Clr |   140.53 ms |  0.7391 ms |  0.6913 ms |  0.29 |
| Numerics_Vector3_Operator | LegacyJit |      X86 |     Clr |   278.81 ms |  4.7911 ms |  4.4816 ms |  0.58 |
|     Separate_Local_Floats | LegacyJit |      X86 |     Clr |    30.31 ms |  0.2358 ms |  0.2206 ms |  0.06 |
|                           |           |          |         |             |            |            |       |
|      Vector3_Fna_Operator |    RyuJit |      X64 | MonoX64 | 1,368.46 ms |  8.8560 ms |  8.2839 ms |  1.00 |
|    Vector3_Fna_Add_By_Ref |    RyuJit |      X64 | MonoX64 |   304.70 ms |  0.7046 ms |  0.6591 ms |  0.22 |
|        Vector3_B_Operator |    RyuJit |      X64 | MonoX64 |   727.11 ms |  5.6541 ms |  5.2889 ms |  0.53 |
| Numerics_Vector3_Operator |    RyuJit |      X64 | MonoX64 | 1,535.94 ms |  4.5348 ms |  4.2418 ms |  1.12 |
|     Separate_Local_Floats |    RyuJit |      X64 | MonoX64 |   277.66 ms |  3.4412 ms |  3.2189 ms |  0.20 |
|                           |           |          |         |             |            |            |       |
|      Vector3_Fna_Operator |    RyuJit |      X64 | MonoX86 |   929.81 ms |  3.0898 ms |  2.8902 ms |  1.00 |
|    Vector3_Fna_Add_By_Ref |    RyuJit |      X64 | MonoX86 |   473.07 ms |  1.4609 ms |  1.3665 ms |  0.51 |
|        Vector3_B_Operator |    RyuJit |      X64 | MonoX86 |   650.97 ms |  6.4649 ms |  6.0473 ms |  0.70 |
| Numerics_Vector3_Operator |    RyuJit |      X64 | MonoX86 | 1,398.85 ms | 20.5702 ms | 19.2414 ms |  1.50 |
|     Separate_Local_Floats |    RyuJit |      X64 | MonoX86 |   294.63 ms |  1.8721 ms |  1.7512 ms |  0.32 |

|                    Method |       Jit | Platform | Runtime |        Mean |     Error |    StdDev | Ratio |
|-------------------------- |---------- |--------- |-------- |------------:|----------:|----------:|------:|
|      Vector4_Fna_Operator |    RyuJit |      X64 |     Clr |   415.46 ms | 3.9390 ms | 3.6845 ms |  1.00 |
|    Vector4_Fna_Add_By_Ref |    RyuJit |      X64 |     Clr |   120.55 ms | 0.3185 ms | 0.2979 ms |  0.29 |
|        Vector4_B_Operator |    RyuJit |      X64 |     Clr |   120.43 ms | 0.5366 ms | 0.5019 ms |  0.29 |
| Numerics_Vector4_Operator |    RyuJit |      X64 |     Clr |   120.35 ms | 0.4287 ms | 0.4010 ms |  0.29 |
|     Separate_Local_Floats |    RyuJit |      X64 |     Clr |   120.47 ms | 0.4617 ms | 0.4319 ms |  0.29 |
|                           |           |          |         |             |           |           |       |
|      Vector4_Fna_Operator |    RyuJit |      X64 |    Core |   421.71 ms | 2.8103 ms | 2.6288 ms |  1.00 |
|    Vector4_Fna_Add_By_Ref |    RyuJit |      X64 |    Core |   120.50 ms | 0.4628 ms | 0.4329 ms |  0.29 |
|        Vector4_B_Operator |    RyuJit |      X64 |    Core |   120.43 ms | 0.5915 ms | 0.5533 ms |  0.29 |
| Numerics_Vector4_Operator |    RyuJit |      X64 |    Core |   120.42 ms | 0.6265 ms | 0.5860 ms |  0.29 |
|     Separate_Local_Floats |    RyuJit |      X64 |    Core |   120.67 ms | 1.0563 ms | 0.9880 ms |  0.29 |
|                           |           |          |         |             |           |           |       |
|      Vector4_Fna_Operator | LegacyJit |      X64 |     Clr |   423.82 ms | 1.8137 ms | 1.6966 ms |  1.00 |
|    Vector4_Fna_Add_By_Ref | LegacyJit |      X64 |     Clr |   118.66 ms | 1.0212 ms | 0.9553 ms |  0.28 |
|        Vector4_B_Operator | LegacyJit |      X64 |     Clr |   120.64 ms | 0.4436 ms | 0.4150 ms |  0.28 |
| Numerics_Vector4_Operator | LegacyJit |      X64 |     Clr |   121.20 ms | 0.6131 ms | 0.5735 ms |  0.29 |
|     Separate_Local_Floats | LegacyJit |      X64 |     Clr |   120.53 ms | 0.9324 ms | 0.8721 ms |  0.28 |
|                           |           |          |         |             |           |           |       |
|      Vector4_Fna_Operator | LegacyJit |      X86 |     Clr |   447.24 ms | 2.8635 ms | 2.5385 ms |  1.00 |
|    Vector4_Fna_Add_By_Ref | LegacyJit |      X86 |     Clr |    30.16 ms | 0.0863 ms | 0.0807 ms |  0.07 |
|        Vector4_B_Operator | LegacyJit |      X86 |     Clr |   183.99 ms | 0.4588 ms | 0.3582 ms |  0.41 |
| Numerics_Vector4_Operator | LegacyJit |      X86 |     Clr |   362.83 ms | 2.1249 ms | 1.9877 ms |  0.81 |
|     Separate_Local_Floats | LegacyJit |      X86 |     Clr |    30.06 ms | 0.3850 ms | 0.3601 ms |  0.07 |
|                           |           |          |         |             |           |           |       |
|      Vector4_Fna_Operator |    RyuJit |      X64 | MonoX64 | 1,604.73 ms | 4.1507 ms | 3.8826 ms |  1.00 |
|    Vector4_Fna_Add_By_Ref |    RyuJit |      X64 | MonoX64 |   325.68 ms | 3.6948 ms | 3.4561 ms |  0.20 |
|        Vector4_B_Operator |    RyuJit |      X64 | MonoX64 |   928.08 ms | 5.1081 ms | 4.7781 ms |  0.58 |
| Numerics_Vector4_Operator |    RyuJit |      X64 | MonoX64 |   265.10 ms | 0.9620 ms | 0.8033 ms |  0.17 |
|     Separate_Local_Floats |    RyuJit |      X64 | MonoX64 |   303.63 ms | 1.5118 ms | 1.4142 ms |  0.19 |
|                           |           |          |         |             |           |           |       |
|      Vector4_Fna_Operator |    RyuJit |      X64 | MonoX86 | 1,112.42 ms | 2.2699 ms | 1.8955 ms |  1.00 |
|    Vector4_Fna_Add_By_Ref |    RyuJit |      X64 | MonoX86 |   452.32 ms | 3.7417 ms | 3.5000 ms |  0.41 |
|        Vector4_B_Operator |    RyuJit |      X64 | MonoX86 |   849.67 ms | 2.7341 ms | 2.5575 ms |  0.76 |
| Numerics_Vector4_Operator |    RyuJit |      X64 | MonoX86 |   270.69 ms | 0.7299 ms | 0.6827 ms |  0.24 |
|     Separate_Local_Floats |    RyuJit |      X64 | MonoX86 |   279.46 ms | 1.2819 ms | 1.1991 ms |  0.25 |
