# math-struct-benchmark

Benchmarking techniques for math structs in C# in service of data for https://github.com/FNA-XNA/FNA/issues/227

Run the benchmarks: `dotnet run -f netcoreapp2.2 -c Release -- -f "*"`

## Takeaways (based on my results on my PC with these JITs/runtimes)

- Approach A and B appear to have nearly identical results on .NET Framework and .NET Core with RyuJIT, but approach B seems to be generally better on Mono and Legacy JIT with approach A actually running slower than FNA on Mono.
- AggressiveInlining seems to help only on Mono. It seems to help approach A only in Vector2 whereas approach B sees gains for all three vector sizes.
- Across the board if raw performance is the goal then the `Add(ref, ref, out)` method is the right choice over the + operator.
- Vector2 is consistently slower than both Vector3 and Vector4 when using anything but the `Add(ref, ref, out)` static helper. I haven't dug into this to figure out why but it is a particularly interesting thing to note.


## Results on my PC

``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i5-6500 CPU 3.20GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
Frequency=3117188 Hz, Resolution=320.8020 ns, Timer=TSC
.NET Core SDK=2.2.103
  [Host]       : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT
  Clr          : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0
  Core         : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT
  LegacyJitX64 : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0
  LegacyJitX86 : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3260.0
  MonoX64      : Mono 5.18.0 (Visual Studio), 64bit
  MonoX86      : Mono 5.18.0 (Visual Studio), 32bit


```
|                                Method |          Job |        Mean |      Error |     StdDev | Ratio |
|-------------------------------------- |------------- |------------:|-----------:|-----------:|------:|
|                  Vector2_Fna_Operator |          Clr |   922.97 ms |  6.8522 ms |  6.4096 ms |  1.00 |
|                    Vector2_A_Operator |          Clr |   754.79 ms |  4.4768 ms |  4.1876 ms |  0.82 |
| Vector2_A_AggressiveInlining_Operator |          Clr |   757.36 ms |  4.6108 ms |  4.3129 ms |  0.82 |
|                    Vector2_B_Operator |          Clr |   756.02 ms |  4.6636 ms |  4.3624 ms |  0.82 |
| Vector2_B_AggressiveInlining_Operator |          Clr |   757.65 ms |  3.3527 ms |  3.1361 ms |  0.82 |
|                       Vector2_Fna_Add |          Clr |   920.30 ms |  5.1382 ms |  4.8062 ms |  1.00 |
|                Vector2_Fna_Add_By_Ref |          Clr |   121.19 ms |  0.3074 ms |  0.2875 ms |  0.13 |
|                                       |              |             |            |            |       |
|                  Vector2_Fna_Operator |         Core |   921.82 ms |  5.5394 ms |  5.1816 ms |  1.00 |
|                    Vector2_A_Operator |         Core |   757.07 ms |  4.4270 ms |  4.1411 ms |  0.82 |
| Vector2_A_AggressiveInlining_Operator |         Core |   758.36 ms |  3.1376 ms |  2.7814 ms |  0.82 |
|                    Vector2_B_Operator |         Core |   755.75 ms |  2.8961 ms |  2.7090 ms |  0.82 |
| Vector2_B_AggressiveInlining_Operator |         Core |   759.12 ms |  3.1192 ms |  2.9177 ms |  0.82 |
|                       Vector2_Fna_Add |         Core |   919.47 ms |  3.0816 ms |  2.4059 ms |  1.00 |
|                Vector2_Fna_Add_By_Ref |         Core |   121.65 ms |  0.4578 ms |  0.4283 ms |  0.13 |
|                                       |              |             |            |            |       |
|                  Vector2_Fna_Operator | LegacyJitX64 |   918.90 ms |  3.4712 ms |  3.0771 ms |  1.00 |
|                    Vector2_A_Operator | LegacyJitX64 |   756.59 ms |  6.7903 ms |  6.3516 ms |  0.82 |
| Vector2_A_AggressiveInlining_Operator | LegacyJitX64 |   752.55 ms |  3.9338 ms |  3.6797 ms |  0.82 |
|                    Vector2_B_Operator | LegacyJitX64 |   744.80 ms | 10.1570 ms |  9.5009 ms |  0.81 |
| Vector2_B_AggressiveInlining_Operator | LegacyJitX64 |   747.66 ms |  9.8335 ms |  9.1982 ms |  0.81 |
|                       Vector2_Fna_Add | LegacyJitX64 |   905.00 ms | 11.9911 ms | 11.2165 ms |  0.99 |
|                Vector2_Fna_Add_By_Ref | LegacyJitX64 |   119.64 ms |  0.9557 ms |  0.8940 ms |  0.13 |
|                                       |              |             |            |            |       |
|                  Vector2_Fna_Operator | LegacyJitX86 |   499.46 ms |  2.3325 ms |  2.1818 ms |  1.00 |
|                    Vector2_A_Operator | LegacyJitX86 |   274.83 ms |  2.1913 ms |  2.0497 ms |  0.55 |
| Vector2_A_AggressiveInlining_Operator | LegacyJitX86 |   278.59 ms |  0.9401 ms |  0.8794 ms |  0.56 |
|                    Vector2_B_Operator | LegacyJitX86 |   119.48 ms |  1.7245 ms |  1.6131 ms |  0.24 |
| Vector2_B_AggressiveInlining_Operator | LegacyJitX86 |   120.21 ms |  0.2554 ms |  0.2389 ms |  0.24 |
|                       Vector2_Fna_Add | LegacyJitX86 |   496.66 ms |  6.2307 ms |  5.8282 ms |  0.99 |
|                Vector2_Fna_Add_By_Ref | LegacyJitX86 |    29.99 ms |  0.0932 ms |  0.0872 ms |  0.06 |
|                                       |              |             |            |            |       |
|                  Vector2_Fna_Operator |      MonoX64 | 1,578.75 ms | 21.6736 ms | 20.2735 ms |  1.00 |
|                    Vector2_A_Operator |      MonoX64 | 1,650.72 ms | 29.2840 ms | 27.3923 ms |  1.05 |
| Vector2_A_AggressiveInlining_Operator |      MonoX64 |   773.47 ms |  2.7739 ms |  2.5947 ms |  0.49 |
|                    Vector2_B_Operator |      MonoX64 | 1,593.41 ms | 16.6858 ms | 15.6079 ms |  1.01 |
| Vector2_B_AggressiveInlining_Operator |      MonoX64 |   722.83 ms |  1.9669 ms |  1.8398 ms |  0.46 |
|                       Vector2_Fna_Add |      MonoX64 | 1,548.85 ms |  2.2292 ms |  1.8615 ms |  0.98 |
|                Vector2_Fna_Add_By_Ref |      MonoX64 |   286.40 ms |  3.0321 ms |  2.8362 ms |  0.18 |
|                                       |              |             |            |            |       |
|                  Vector2_Fna_Operator |      MonoX86 |   764.67 ms | 11.2047 ms | 10.4809 ms |  1.00 |
|                    Vector2_A_Operator |      MonoX86 |   963.88 ms |  4.5710 ms |  4.2757 ms |  1.26 |
| Vector2_A_AggressiveInlining_Operator |      MonoX86 |   824.04 ms |  9.7282 ms |  9.0997 ms |  1.08 |
|                    Vector2_B_Operator |      MonoX86 |   757.20 ms |  2.7807 ms |  2.6011 ms |  0.99 |
| Vector2_B_AggressiveInlining_Operator |      MonoX86 |   643.64 ms |  4.7458 ms |  4.4392 ms |  0.84 |
|                       Vector2_Fna_Add |      MonoX86 |   773.67 ms |  1.4911 ms |  1.3948 ms |  1.01 |
|                Vector2_Fna_Add_By_Ref |      MonoX86 |   303.99 ms |  4.8474 ms |  4.5342 ms |  0.40 |

|                                Method |          Job |        Mean |     Error |    StdDev | Ratio |
|-------------------------------------- |------------- |------------:|----------:|----------:|------:|
|                  Vector3_Fna_Operator |          Clr |   422.76 ms | 8.2761 ms | 9.8521 ms |  1.00 |
|                    Vector3_A_Operator |          Clr |   117.91 ms | 0.8365 ms | 0.7825 ms |  0.28 |
| Vector3_A_AggressiveInlining_Operator |          Clr |   120.52 ms | 0.2536 ms | 0.2372 ms |  0.28 |
|                    Vector3_B_Operator |          Clr |   120.07 ms | 0.8027 ms | 0.7508 ms |  0.28 |
| Vector3_B_AggressiveInlining_Operator |          Clr |   120.59 ms | 0.5126 ms | 0.4795 ms |  0.28 |
|                       Vector3_Fna_Add |          Clr |   421.95 ms | 2.2950 ms | 2.1467 ms |  1.00 |
|                Vector3_Fna_Add_By_Ref |          Clr |   120.45 ms | 0.7503 ms | 0.7019 ms |  0.28 |
|                                       |              |             |           |           |       |
|                  Vector3_Fna_Operator |         Core |   422.90 ms | 3.2206 ms | 3.0125 ms |  1.00 |
|                    Vector3_A_Operator |         Core |   120.06 ms | 1.3262 ms | 1.2405 ms |  0.28 |
| Vector3_A_AggressiveInlining_Operator |         Core |   120.07 ms | 0.8517 ms | 0.7967 ms |  0.28 |
|                    Vector3_B_Operator |         Core |   120.58 ms | 0.7830 ms | 0.7324 ms |  0.29 |
| Vector3_B_AggressiveInlining_Operator |         Core |   120.11 ms | 0.6044 ms | 0.5653 ms |  0.28 |
|                       Vector3_Fna_Add |         Core |   421.84 ms | 2.8051 ms | 2.6239 ms |  1.00 |
|                Vector3_Fna_Add_By_Ref |         Core |   120.22 ms | 0.3693 ms | 0.3273 ms |  0.28 |
|                                       |              |             |           |           |       |
|                  Vector3_Fna_Operator | LegacyJitX64 |   425.97 ms | 2.3481 ms | 2.1964 ms |  1.00 |
|                    Vector3_A_Operator | LegacyJitX64 |   120.19 ms | 0.4971 ms | 0.4650 ms |  0.28 |
| Vector3_A_AggressiveInlining_Operator | LegacyJitX64 |   120.14 ms | 1.0104 ms | 0.9451 ms |  0.28 |
|                    Vector3_B_Operator | LegacyJitX64 |   120.27 ms | 0.4283 ms | 0.4006 ms |  0.28 |
| Vector3_B_AggressiveInlining_Operator | LegacyJitX64 |   120.44 ms | 0.4381 ms | 0.4098 ms |  0.28 |
|                       Vector3_Fna_Add | LegacyJitX64 |   424.29 ms | 3.7212 ms | 3.4808 ms |  1.00 |
|                Vector3_Fna_Add_By_Ref | LegacyJitX64 |   120.03 ms | 0.8946 ms | 0.8368 ms |  0.28 |
|                                       |              |             |           |           |       |
|                  Vector3_Fna_Operator | LegacyJitX86 |   460.96 ms | 3.2605 ms | 2.8903 ms |  1.00 |
|                    Vector3_A_Operator | LegacyJitX86 |   302.77 ms | 3.0089 ms | 2.8145 ms |  0.66 |
| Vector3_A_AggressiveInlining_Operator | LegacyJitX86 |   301.57 ms | 2.3400 ms | 2.1889 ms |  0.65 |
|                    Vector3_B_Operator | LegacyJitX86 |   150.26 ms | 0.8531 ms | 0.7124 ms |  0.33 |
| Vector3_B_AggressiveInlining_Operator | LegacyJitX86 |   150.53 ms | 0.8667 ms | 0.8107 ms |  0.33 |
|                       Vector3_Fna_Add | LegacyJitX86 |   462.61 ms | 3.6289 ms | 3.3944 ms |  1.00 |
|                Vector3_Fna_Add_By_Ref | LegacyJitX86 |    30.04 ms | 0.2847 ms | 0.2663 ms |  0.07 |
|                                       |              |             |           |           |       |
|                  Vector3_Fna_Operator |      MonoX64 | 1,357.53 ms | 2.8149 ms | 2.6331 ms |  1.00 |
|                    Vector3_A_Operator |      MonoX64 | 1,529.30 ms | 3.6631 ms | 3.4265 ms |  1.13 |
| Vector3_A_AggressiveInlining_Operator |      MonoX64 | 1,526.02 ms | 2.7833 ms | 2.6035 ms |  1.12 |
|                    Vector3_B_Operator |      MonoX64 | 1,296.14 ms | 3.7933 ms | 3.5482 ms |  0.95 |
| Vector3_B_AggressiveInlining_Operator |      MonoX64 |   720.65 ms | 5.1098 ms | 4.7797 ms |  0.53 |
|                       Vector3_Fna_Add |      MonoX64 | 1,361.11 ms | 4.4898 ms | 4.1997 ms |  1.00 |
|                Vector3_Fna_Add_By_Ref |      MonoX64 |   302.14 ms | 0.8183 ms | 0.7654 ms |  0.22 |
|                                       |              |             |           |           |       |
|                  Vector3_Fna_Operator |      MonoX86 |   927.90 ms | 3.5087 ms | 3.2820 ms |  1.00 |
|                    Vector3_A_Operator |      MonoX86 | 1,410.57 ms | 5.4731 ms | 5.1195 ms |  1.52 |
| Vector3_A_AggressiveInlining_Operator |      MonoX86 | 1,420.69 ms | 4.7781 ms | 4.2357 ms |  1.53 |
|                    Vector3_B_Operator |      MonoX86 |   942.67 ms | 3.2693 ms | 3.0581 ms |  1.02 |
| Vector3_B_AggressiveInlining_Operator |      MonoX86 |   646.76 ms | 4.5323 ms | 4.2395 ms |  0.70 |
|                       Vector3_Fna_Add |      MonoX86 |   923.71 ms | 2.2491 ms | 2.1038 ms |  1.00 |
|                Vector3_Fna_Add_By_Ref |      MonoX86 |   462.57 ms | 1.9358 ms | 1.8107 ms |  0.50 |

|                                Method |          Job |        Mean |      Error |     StdDev | Ratio |
|-------------------------------------- |------------- |------------:|-----------:|-----------:|------:|
|                  Vector4_Fna_Operator |          Clr |   420.44 ms |  2.0096 ms |  1.7815 ms |  1.00 |
|                    Vector4_A_Operator |          Clr |   120.29 ms |  0.3692 ms |  0.3453 ms |  0.29 |
| Vector4_A_AggressiveInlining_Operator |          Clr |   117.03 ms |  0.2301 ms |  0.1922 ms |  0.28 |
|                    Vector4_B_Operator |          Clr |   120.29 ms |  0.3423 ms |  0.3202 ms |  0.29 |
| Vector4_B_AggressiveInlining_Operator |          Clr |   117.90 ms |  1.1970 ms |  1.1197 ms |  0.28 |
|                       Vector4_Fna_Add |          Clr |   422.25 ms |  1.3159 ms |  1.2309 ms |  1.00 |
|                Vector4_Fna_Add_By_Ref |          Clr |   119.31 ms |  1.9924 ms |  1.8637 ms |  0.28 |
|                                       |              |             |            |            |       |
|                  Vector4_Fna_Operator |         Core |   420.63 ms |  1.8105 ms |  1.6936 ms |  1.00 |
|                    Vector4_A_Operator |         Core |   118.72 ms |  0.2299 ms |  0.2038 ms |  0.28 |
| Vector4_A_AggressiveInlining_Operator |         Core |   120.30 ms |  0.4126 ms |  0.3859 ms |  0.29 |
|                    Vector4_B_Operator |         Core |   117.16 ms |  0.3781 ms |  0.3352 ms |  0.28 |
| Vector4_B_AggressiveInlining_Operator |         Core |   120.17 ms |  0.3828 ms |  0.3581 ms |  0.29 |
|                       Vector4_Fna_Add |         Core |   416.64 ms |  1.2562 ms |  1.1751 ms |  0.99 |
|                Vector4_Fna_Add_By_Ref |         Core |   120.08 ms |  0.3971 ms |  0.3715 ms |  0.29 |
|                                       |              |             |            |            |       |
|                  Vector4_Fna_Operator | LegacyJitX64 |   412.25 ms |  2.5999 ms |  2.4320 ms |  1.00 |
|                    Vector4_A_Operator | LegacyJitX64 |   120.13 ms |  0.4981 ms |  0.4659 ms |  0.29 |
| Vector4_A_AggressiveInlining_Operator | LegacyJitX64 |   121.43 ms |  0.2010 ms |  0.1880 ms |  0.29 |
|                    Vector4_B_Operator | LegacyJitX64 |   120.13 ms |  0.2502 ms |  0.2218 ms |  0.29 |
| Vector4_B_AggressiveInlining_Operator | LegacyJitX64 |   120.18 ms |  0.4588 ms |  0.4291 ms |  0.29 |
|                       Vector4_Fna_Add | LegacyJitX64 |   421.83 ms |  3.6066 ms |  3.3736 ms |  1.02 |
|                Vector4_Fna_Add_By_Ref | LegacyJitX64 |   120.17 ms |  0.2819 ms |  0.2499 ms |  0.29 |
|                                       |              |             |            |            |       |
|                  Vector4_Fna_Operator | LegacyJitX86 |   481.58 ms |  1.5000 ms |  1.3297 ms |  1.00 |
|                    Vector4_A_Operator | LegacyJitX86 |   452.77 ms |  2.2573 ms |  2.1115 ms |  0.94 |
| Vector4_A_AggressiveInlining_Operator | LegacyJitX86 |   441.68 ms |  1.4232 ms |  1.1884 ms |  0.92 |
|                    Vector4_B_Operator | LegacyJitX86 |   150.19 ms |  0.9830 ms |  0.9195 ms |  0.31 |
| Vector4_B_AggressiveInlining_Operator | LegacyJitX86 |   150.30 ms |  0.4183 ms |  0.3913 ms |  0.31 |
|                       Vector4_Fna_Add | LegacyJitX86 |   478.90 ms |  5.8638 ms |  5.4850 ms |  0.99 |
|                Vector4_Fna_Add_By_Ref | LegacyJitX86 |    29.94 ms |  0.3220 ms |  0.3012 ms |  0.06 |
|                                       |              |             |            |            |       |
|                  Vector4_Fna_Operator |      MonoX64 | 1,579.35 ms | 23.9388 ms | 22.3924 ms |  1.00 |
|                    Vector4_A_Operator |      MonoX64 | 1,811.93 ms |  3.9435 ms |  3.6888 ms |  1.15 |
| Vector4_A_AggressiveInlining_Operator |      MonoX64 | 1,773.79 ms |  5.8764 ms |  5.2093 ms |  1.12 |
|                    Vector4_B_Operator |      MonoX64 | 1,441.87 ms |  6.7154 ms |  6.2816 ms |  0.91 |
| Vector4_B_AggressiveInlining_Operator |      MonoX64 |   908.96 ms |  2.5965 ms |  2.4288 ms |  0.58 |
|                       Vector4_Fna_Add |      MonoX64 | 1,603.01 ms |  3.7647 ms |  3.5215 ms |  1.02 |
|                Vector4_Fna_Add_By_Ref |      MonoX64 |   329.44 ms |  1.0622 ms |  0.9936 ms |  0.21 |
|                                       |              |             |            |            |       |
|                  Vector4_Fna_Operator |      MonoX86 | 1,132.66 ms |  3.4291 ms |  3.0398 ms |  1.00 |
|                    Vector4_A_Operator |      MonoX86 | 1,745.40 ms | 34.8439 ms | 61.0263 ms |  1.55 |
| Vector4_A_AggressiveInlining_Operator |      MonoX86 | 1,693.97 ms |  5.7938 ms |  4.5234 ms |  1.50 |
|                    Vector4_B_Operator |      MonoX86 | 1,168.97 ms |  4.3263 ms |  4.0468 ms |  1.03 |
| Vector4_B_AggressiveInlining_Operator |      MonoX86 |   847.96 ms |  3.5101 ms |  3.2833 ms |  0.75 |
|                       Vector4_Fna_Add |      MonoX86 | 1,122.44 ms |  4.5601 ms |  4.2655 ms |  0.99 |
|                Vector4_Fna_Add_By_Ref |      MonoX86 |   451.18 ms |  1.9223 ms |  1.7981 ms |  0.40 |
