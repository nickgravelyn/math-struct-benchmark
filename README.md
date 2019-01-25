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

|                 Method |  Job | Runtime |       Mean |      Error |     StdDev | Ratio |
|----------------------- |----- |-------- |-----------:|-----------:|-----------:|------:|
|   Vector2_Fna_Operator |  Clr |     Clr |   926.1 ms |  3.5690 ms |  3.3384 ms |  1.00 |
|     Vector2_A_Operator |  Clr |     Clr |   758.1 ms |  2.2406 ms |  2.0959 ms |  0.82 |
|     Vector2_B_Operator |  Clr |     Clr |   756.5 ms |  3.7399 ms |  3.4983 ms |  0.82 |
|        Vector2_Fna_Add |  Clr |     Clr |   920.0 ms |  6.5745 ms |  6.1498 ms |  0.99 |
| Vector2_Fna_Add_By_Ref |  Clr |     Clr |   120.5 ms |  0.7253 ms |  0.6784 ms |  0.13 |
|                        |      |         |            |            |            |       |
|   Vector2_Fna_Operator | Core |    Core |   922.0 ms |  3.6479 ms |  3.2338 ms |  1.00 |
|     Vector2_A_Operator | Core |    Core |   760.9 ms |  4.5682 ms |  4.2731 ms |  0.83 |
|     Vector2_B_Operator | Core |    Core |   763.4 ms |  9.6481 ms |  8.0566 ms |  0.83 |
|        Vector2_Fna_Add | Core |    Core |   920.8 ms |  3.6375 ms |  3.4026 ms |  1.00 |
| Vector2_Fna_Add_By_Ref | Core |    Core |   121.0 ms |  0.8492 ms |  0.7944 ms |  0.13 |
|                        |      |         |            |            |            |       |
|   Vector2_Fna_Operator | Mono |    Mono | 1,586.6 ms |  9.4894 ms |  7.9240 ms |  1.00 |
|     Vector2_A_Operator | Mono |    Mono | 1,654.5 ms | 15.5379 ms | 14.5342 ms |  1.04 |
|     Vector2_B_Operator | Mono |    Mono | 1,592.2 ms | 12.1608 ms | 11.3752 ms |  1.00 |
|        Vector2_Fna_Add | Mono |    Mono | 1,597.0 ms | 21.7713 ms | 20.3649 ms |  1.00 |
| Vector2_Fna_Add_By_Ref | Mono |    Mono |   288.7 ms |  5.6220 ms |  5.5216 ms |  0.18 |

|                 Method |  Job | Runtime |       Mean |      Error |     StdDev | Ratio | RatioSD |
|----------------------- |----- |-------- |-----------:|-----------:|-----------:|------:|--------:|
|   Vector3_Fna_Operator |  Clr |     Clr |   429.6 ms |  0.9551 ms |  0.8934 ms |  1.00 |    0.00 |
|     Vector3_A_Operator |  Clr |     Clr |   119.6 ms |  1.4059 ms |  1.3151 ms |  0.28 |    0.00 |
|     Vector3_B_Operator |  Clr |     Clr |   120.4 ms |  1.0257 ms |  0.9595 ms |  0.28 |    0.00 |
|        Vector3_Fna_Add |  Clr |     Clr |   425.9 ms |  2.8644 ms |  2.6793 ms |  0.99 |    0.01 |
| Vector3_Fna_Add_By_Ref |  Clr |     Clr |   120.5 ms |  0.6991 ms |  0.6540 ms |  0.28 |    0.00 |
|                        |      |         |            |            |            |       |         |
|   Vector3_Fna_Operator | Core |    Core |   423.4 ms |  1.3847 ms |  1.2952 ms |  1.00 |    0.00 |
|     Vector3_A_Operator | Core |    Core |   120.3 ms |  1.0686 ms |  0.9996 ms |  0.28 |    0.00 |
|     Vector3_B_Operator | Core |    Core |   120.4 ms |  0.4082 ms |  0.3818 ms |  0.28 |    0.00 |
|        Vector3_Fna_Add | Core |    Core |   423.9 ms |  2.2251 ms |  2.0813 ms |  1.00 |    0.01 |
| Vector3_Fna_Add_By_Ref | Core |    Core |   120.3 ms |  0.3145 ms |  0.2942 ms |  0.28 |    0.00 |
|                        |      |         |            |            |            |       |         |
|   Vector3_Fna_Operator | Mono |    Mono | 1,355.4 ms | 13.3924 ms | 12.5272 ms |  1.00 |    0.00 |
|     Vector3_A_Operator | Mono |    Mono | 1,505.3 ms | 15.7724 ms | 13.9818 ms |  1.11 |    0.01 |
|     Vector3_B_Operator | Mono |    Mono | 1,290.7 ms |  6.0409 ms |  5.6506 ms |  0.95 |    0.01 |
|        Vector3_Fna_Add | Mono |    Mono | 1,339.2 ms | 14.8444 ms | 13.8854 ms |  0.99 |    0.02 |
| Vector3_Fna_Add_By_Ref | Mono |    Mono |   301.4 ms |  1.2928 ms |  1.0796 ms |  0.22 |    0.00 |

|                 Method |  Job | Runtime |       Mean |      Error |     StdDev | Ratio | RatioSD |
|----------------------- |----- |-------- |-----------:|-----------:|-----------:|------:|--------:|
|   Vector4_Fna_Operator |  Clr |     Clr |   426.0 ms |  1.1866 ms |  1.1099 ms |  1.00 |    0.00 |
|     Vector4_A_Operator |  Clr |     Clr |   119.3 ms |  1.5977 ms |  1.4945 ms |  0.28 |    0.00 |
|     Vector4_B_Operator |  Clr |     Clr |   120.3 ms |  0.6027 ms |  0.5638 ms |  0.28 |    0.00 |
|        Vector4_Fna_Add |  Clr |     Clr |   420.3 ms |  4.1376 ms |  3.8703 ms |  0.99 |    0.01 |
| Vector4_Fna_Add_By_Ref |  Clr |     Clr |   120.2 ms |  0.4154 ms |  0.3886 ms |  0.28 |    0.00 |
|                        |      |         |            |            |            |       |         |
|   Vector4_Fna_Operator | Core |    Core |   419.2 ms |  3.5831 ms |  3.3517 ms |  1.00 |    0.00 |
|     Vector4_A_Operator | Core |    Core |   120.0 ms |  0.5244 ms |  0.4905 ms |  0.29 |    0.00 |
|     Vector4_B_Operator | Core |    Core |   120.1 ms |  0.5371 ms |  0.5024 ms |  0.29 |    0.00 |
|        Vector4_Fna_Add | Core |    Core |   414.3 ms |  4.5828 ms |  4.2867 ms |  0.99 |    0.01 |
| Vector4_Fna_Add_By_Ref | Core |    Core |   120.3 ms |  0.5199 ms |  0.4863 ms |  0.29 |    0.00 |
|                        |      |         |            |            |            |       |         |
|   Vector4_Fna_Operator | Mono |    Mono | 1,587.2 ms | 23.0488 ms | 21.5599 ms |  1.00 |    0.00 |
|     Vector4_A_Operator | Mono |    Mono | 1,809.2 ms |  4.2698 ms |  3.7851 ms |  1.14 |    0.02 |
|     Vector4_B_Operator | Mono |    Mono | 1,441.2 ms | 11.4853 ms | 10.7433 ms |  0.91 |    0.01 |
|        Vector4_Fna_Add | Mono |    Mono | 1,606.5 ms | 11.0136 ms | 10.3021 ms |  1.01 |    0.01 |
| Vector4_Fna_Add_By_Ref | Mono |    Mono |   329.5 ms |  1.6074 ms |  1.5036 ms |  0.21 |    0.00 |
