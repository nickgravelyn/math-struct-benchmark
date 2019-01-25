# math-struct-benchmark

Benchmarking techniques for math structs in C# in service of data for https://github.com/FNA-XNA/FNA/issues/227

Run the benchmarks: `dotnet run -f netcoreapp2.2 -c Release -- -f "*"`

## Takeaways (based on my results on my PC with these JITs/runtimes)

- Vector2 is consistently slower than both Vector3 and Vector4 when using anything but the `Add(ref, ref, out)` static helper. I haven't dug into this to figure out why but it is a particularly interesting thing to note.
- Approach A and B appear to have nearly identical results on .NET Framework and .NET Core, but approach B seems to be generally better on Mono.
- That said both are faster than FNA's current implementation which requires a defensive copy of the input struct that is modified.
- Across the board if raw performance is the goal then the `Add(ref, ref, out)` method is the right choice over the + operator.


## Results on my PC

```
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i5-6500 CPU 3.20GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
Frequency=3117188 Hz, Resolution=320.8020 ns, Timer=TSC
.NET Core SDK=2.2.103
  [Host] : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0
  Core   : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT
  Mono   : Mono 5.18.0 (Visual Studio), 64bit
```

|                                Method |  Job | Runtime |       Mean |      Error |     StdDev | Ratio |
|-------------------------------------- |----- |-------- |-----------:|-----------:|-----------:|------:|
|                  Vector2_Fna_Operator |  Clr |     Clr |   921.6 ms |  6.0457 ms |  5.6551 ms |  1.00 |
|                    Vector2_A_Operator |  Clr |     Clr |   754.8 ms |  2.8285 ms |  2.2083 ms |  0.82 |
| Vector2_A_AggressiveInlining_Operator |  Clr |     Clr |   761.0 ms |  1.9000 ms |  1.7772 ms |  0.83 |
|                    Vector2_B_Operator |  Clr |     Clr |   756.4 ms |  2.4614 ms |  2.3024 ms |  0.82 |
| Vector2_B_AggressiveInlining_Operator |  Clr |     Clr |   757.8 ms |  2.4700 ms |  2.3104 ms |  0.82 |
|                       Vector2_Fna_Add |  Clr |     Clr |   920.6 ms |  3.7406 ms |  3.4990 ms |  1.00 |
|                Vector2_Fna_Add_By_Ref |  Clr |     Clr |   121.4 ms |  0.5492 ms |  0.5137 ms |  0.13 |
|                                       |      |         |            |            |            |       |
|                  Vector2_Fna_Operator | Core |    Core |   921.3 ms |  3.1016 ms |  2.9013 ms |  1.00 |
|                    Vector2_A_Operator | Core |    Core |   760.2 ms |  1.2316 ms |  1.1520 ms |  0.83 |
| Vector2_A_AggressiveInlining_Operator | Core |    Core |   757.4 ms |  3.7193 ms |  3.4791 ms |  0.82 |
|                    Vector2_B_Operator | Core |    Core |   758.7 ms |  1.3381 ms |  1.2516 ms |  0.82 |
| Vector2_B_AggressiveInlining_Operator | Core |    Core |   757.0 ms |  2.7201 ms |  2.4113 ms |  0.82 |
|                       Vector2_Fna_Add | Core |    Core |   925.2 ms |  3.7842 ms |  3.1600 ms |  1.00 |
|                Vector2_Fna_Add_By_Ref | Core |    Core |   121.1 ms |  0.6152 ms |  0.5755 ms |  0.13 |
|                                       |      |         |            |            |            |       |
|                  Vector2_Fna_Operator | Mono |    Mono | 1,600.9 ms |  5.6055 ms |  5.2434 ms |  1.00 |
|                    Vector2_A_Operator | Mono |    Mono | 1,664.4 ms | 11.9979 ms | 11.2228 ms |  1.04 |
| Vector2_A_AggressiveInlining_Operator | Mono |    Mono |   780.1 ms |  1.7614 ms |  1.6476 ms |  0.49 |
|                    Vector2_B_Operator | Mono |    Mono | 1,614.6 ms | 13.3821 ms | 12.5176 ms |  1.01 |
| Vector2_B_AggressiveInlining_Operator | Mono |    Mono |   725.6 ms |  2.7452 ms |  2.5679 ms |  0.45 |
|                       Vector2_Fna_Add | Mono |    Mono | 1,598.8 ms | 19.3937 ms | 18.1409 ms |  1.00 |
|                Vector2_Fna_Add_By_Ref | Mono |    Mono |   290.9 ms |  3.0025 ms |  2.8085 ms |  0.18 |

|                                Method |  Job | Runtime |       Mean |      Error |     StdDev | Ratio |
|-------------------------------------- |----- |-------- |-----------:|-----------:|-----------:|------:|
|                  Vector3_Fna_Operator |  Clr |     Clr |   434.9 ms |  1.8788 ms |  1.7574 ms |  1.00 |
|                    Vector3_A_Operator |  Clr |     Clr |   121.4 ms |  0.6024 ms |  0.5635 ms |  0.28 |
| Vector3_A_AggressiveInlining_Operator |  Clr |     Clr |   121.2 ms |  0.4746 ms |  0.4440 ms |  0.28 |
|                    Vector3_B_Operator |  Clr |     Clr |   120.9 ms |  0.7485 ms |  0.7002 ms |  0.28 |
| Vector3_B_AggressiveInlining_Operator |  Clr |     Clr |   121.2 ms |  0.4910 ms |  0.4100 ms |  0.28 |
|                       Vector3_Fna_Add |  Clr |     Clr |   429.2 ms |  2.6856 ms |  2.5121 ms |  0.99 |
|                Vector3_Fna_Add_By_Ref |  Clr |     Clr |   121.5 ms |  0.7791 ms |  0.7288 ms |  0.28 |
|                                       |      |         |            |            |            |       |
|                  Vector3_Fna_Operator | Core |    Core |   427.0 ms |  1.2048 ms |  1.1270 ms |  1.00 |
|                    Vector3_A_Operator | Core |    Core |   120.8 ms |  1.1024 ms |  1.0312 ms |  0.28 |
| Vector3_A_AggressiveInlining_Operator | Core |    Core |   121.4 ms |  0.1838 ms |  0.1719 ms |  0.28 |
|                    Vector3_B_Operator | Core |    Core |   121.1 ms |  0.2948 ms |  0.2757 ms |  0.28 |
| Vector3_B_AggressiveInlining_Operator | Core |    Core |   121.7 ms |  0.6684 ms |  0.6252 ms |  0.28 |
|                       Vector3_Fna_Add | Core |    Core |   426.4 ms |  1.6772 ms |  1.5689 ms |  1.00 |
|                Vector3_Fna_Add_By_Ref | Core |    Core |   121.2 ms |  0.6818 ms |  0.6044 ms |  0.28 |
|                                       |      |         |            |            |            |       |
|                  Vector3_Fna_Operator | Mono |    Mono | 1,369.2 ms | 14.2765 ms | 13.3543 ms |  1.00 |
|                    Vector3_A_Operator | Mono |    Mono | 1,531.6 ms |  9.4790 ms |  8.8667 ms |  1.12 |
| Vector3_A_AggressiveInlining_Operator | Mono |    Mono | 1,534.9 ms | 14.3913 ms | 13.4616 ms |  1.12 |
|                    Vector3_B_Operator | Mono |    Mono | 1,293.8 ms |  3.6068 ms |  3.3738 ms |  0.95 |
| Vector3_B_AggressiveInlining_Operator | Mono |    Mono |   707.5 ms |  4.9791 ms |  4.6574 ms |  0.52 |
|                       Vector3_Fna_Add | Mono |    Mono | 1,354.6 ms |  9.1426 ms |  8.5520 ms |  0.99 |
|                Vector3_Fna_Add_By_Ref | Mono |    Mono |   298.5 ms |  0.3875 ms |  0.3435 ms |  0.22 |

|                                Method |  Job | Runtime |       Mean |      Error |     StdDev | Ratio |
|-------------------------------------- |----- |-------- |-----------:|-----------:|-----------:|------:|
|                  Vector4_Fna_Operator |  Clr |     Clr |   424.8 ms |  1.2396 ms |  1.1595 ms |  1.00 |
|                    Vector4_A_Operator |  Clr |     Clr |   120.3 ms |  1.1110 ms |  1.0392 ms |  0.28 |
| Vector4_A_AggressiveInlining_Operator |  Clr |     Clr |   120.4 ms |  0.3927 ms |  0.3673 ms |  0.28 |
|                    Vector4_B_Operator |  Clr |     Clr |   120.7 ms |  0.4893 ms |  0.4576 ms |  0.28 |
| Vector4_B_AggressiveInlining_Operator |  Clr |     Clr |   120.4 ms |  0.4801 ms |  0.4490 ms |  0.28 |
|                       Vector4_Fna_Add |  Clr |     Clr |   421.7 ms |  2.4758 ms |  2.3158 ms |  0.99 |
|                Vector4_Fna_Add_By_Ref |  Clr |     Clr |   120.5 ms |  0.3924 ms |  0.3671 ms |  0.28 |
|                                       |      |         |            |            |            |       |
|                  Vector4_Fna_Operator | Core |    Core |   420.5 ms |  2.9875 ms |  2.7945 ms |  1.00 |
|                    Vector4_A_Operator | Core |    Core |   120.0 ms |  0.3908 ms |  0.3656 ms |  0.29 |
| Vector4_A_AggressiveInlining_Operator | Core |    Core |   119.9 ms |  1.1980 ms |  1.1207 ms |  0.29 |
|                    Vector4_B_Operator | Core |    Core |   120.3 ms |  0.4047 ms |  0.3785 ms |  0.29 |
| Vector4_B_AggressiveInlining_Operator | Core |    Core |   120.2 ms |  0.9068 ms |  0.8482 ms |  0.29 |
|                       Vector4_Fna_Add | Core |    Core |   422.2 ms |  1.5395 ms |  1.4400 ms |  1.00 |
|                Vector4_Fna_Add_By_Ref | Core |    Core |   120.1 ms |  0.9023 ms |  0.8440 ms |  0.29 |
|                                       |      |         |            |            |            |       |
|                  Vector4_Fna_Operator | Mono |    Mono | 1,604.6 ms |  4.4701 ms |  4.1814 ms |  1.00 |
|                    Vector4_A_Operator | Mono |    Mono | 1,812.1 ms |  3.8327 ms |  3.5851 ms |  1.13 |
| Vector4_A_AggressiveInlining_Operator | Mono |    Mono | 1,816.6 ms |  6.7810 ms |  6.3430 ms |  1.13 |
|                    Vector4_B_Operator | Mono |    Mono | 1,447.1 ms | 10.6923 ms | 10.0016 ms |  0.90 |
| Vector4_B_AggressiveInlining_Operator | Mono |    Mono |   909.8 ms |  2.6427 ms |  2.4720 ms |  0.57 |
|                       Vector4_Fna_Add | Mono |    Mono | 1,603.7 ms |  4.1525 ms |  3.8843 ms |  1.00 |
|                Vector4_Fna_Add_By_Ref | Mono |    Mono |   329.0 ms |  1.1922 ms |  1.1151 ms |  0.21 |
