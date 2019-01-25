# math-struct-benchmark

Benchmarking techniques for math structs in C# in service of data for https://github.com/FNA-XNA/FNA/issues/227

Run it with `dotnet run -c Release -- -f "*"`.

On Windows you can also run `dotnet run -f net46 -c Release -- -f "*" -r net46` to run against .NET Framework.

## Takeaways

- I see no real discernable difference in results on my two machines between a + operator that invokes the struct constructor vs one that sets fields
- That said both are faster than FNA's current approach of modifying the input parameter
- And as expected the `Add(ref, ref, out)` method is almost always the same performance or better (and in the case of Vector2 for some odd reason it's dramaticly better).
- Vector2 is consistently slower than both Vector3 and Vector4 when using anything but the `Add(ref, ref, out)` static helper. I haven't dug into this to figure out why but it is a particularly interesting thing to note.


## Results on my MacBook

```
BenchmarkDotNet=v0.11.3, OS=macOS Mojave 10.14.3 (18D42) [Darwin 18.2.0]
Intel Core i5-4258U CPU 2.40GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.1.301
  [Host]     : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
```

|                 Method |       Mean |     Error |    StdDev | Ratio | RatioSD |
|----------------------- |-----------:|----------:|----------:|------:|--------:|
|   Vector2_Fna_Operator | 1,155.8 ms | 23.112 ms | 28.383 ms |  1.00 |    0.00 |
|     Vector2_A_Operator |   933.3 ms | 24.738 ms | 34.679 ms |  0.81 |    0.03 |
|     Vector2_B_Operator |   921.9 ms |  2.844 ms |  2.375 ms |  0.79 |    0.02 |
|        Vector2_Fna_Add | 1,133.9 ms | 18.364 ms | 16.279 ms |  0.97 |    0.03 |
| Vector2_Fna_Add_By_Ref |   105.7 ms |  1.463 ms |  1.297 ms |  0.09 |    0.00 |


|                 Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|----------------------- |---------:|---------:|---------:|------:|--------:|
|   Vector3_Fna_Operator | 535.0 ms | 8.361 ms | 7.821 ms |  1.00 |    0.00 |
|     Vector3_A_Operator | 106.7 ms | 1.777 ms | 1.663 ms |  0.20 |    0.00 |
|     Vector3_B_Operator | 107.7 ms | 2.888 ms | 3.653 ms |  0.20 |    0.01 |
|        Vector3_Fna_Add | 533.9 ms | 2.874 ms | 2.547 ms |  1.00 |    0.02 |
| Vector3_Fna_Add_By_Ref | 116.0 ms | 1.614 ms | 1.510 ms |  0.22 |    0.00 |

|                 Method |     Mean |      Error |     StdDev | Ratio | RatioSD |
|----------------------- |---------:|-----------:|-----------:|------:|--------:|
|   Vector4_Fna_Operator | 534.3 ms |  7.6673 ms |  7.1720 ms |  1.00 |    0.00 |
|     Vector4_A_Operator | 143.4 ms |  2.8092 ms |  3.3442 ms |  0.27 |    0.01 |
|     Vector4_B_Operator | 142.0 ms |  2.5382 ms |  2.3743 ms |  0.27 |    0.01 |
|        Vector4_Fna_Add | 528.7 ms | 21.3030 ms | 23.6783 ms |  0.99 |    0.05 |
| Vector4_Fna_Add_By_Ref | 138.5 ms |  0.0884 ms |  0.0783 ms |  0.26 |    0.00 |

## Results on my PC (.NET Core 2.1)

```
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i5-6500 CPU 3.20GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
Frequency=3117186 Hz, Resolution=320.8022 ns, Timer=TSC
.NET Core SDK=2.1.600-preview-009426
  [Host]     : .NET Core 2.1.7 (CoreCLR 4.6.27129.04, CoreFX 4.6.27129.04), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.7 (CoreCLR 4.6.27129.04, CoreFX 4.6.27129.04), 64bit RyuJIT
```

|                 Method |     Mean |      Error |     StdDev | Ratio |
|----------------------- |---------:|-----------:|-----------:|------:|
|   Vector2_Fna_Operator | 924.2 ms |  6.9615 ms |  6.5118 ms |  1.00 |
|     Vector2_A_Operator | 762.2 ms |  3.6343 ms |  3.3996 ms |  0.82 |
|     Vector2_B_Operator | 759.0 ms |  1.5068 ms |  1.4094 ms |  0.82 |
|        Vector2_Fna_Add | 932.4 ms | 12.7012 ms | 11.8807 ms |  1.01 |
| Vector2_Fna_Add_By_Ref | 121.2 ms |  0.1588 ms |  0.1408 ms |  0.13 |

|                 Method |     Mean |     Error |    StdDev | Ratio | RatioSD |
|----------------------- |---------:|----------:|----------:|------:|--------:|
|   Vector3_Fna_Operator | 429.2 ms | 8.2818 ms | 8.5048 ms |  1.00 |    0.00 |
|     Vector3_A_Operator | 120.7 ms | 1.1384 ms | 1.0649 ms |  0.28 |    0.01 |
|     Vector3_B_Operator | 121.3 ms | 0.3825 ms | 0.3578 ms |  0.28 |    0.01 |
|        Vector3_Fna_Add | 426.6 ms | 2.7785 ms | 2.5990 ms |  0.99 |    0.02 |
| Vector3_Fna_Add_By_Ref | 121.1 ms | 0.3077 ms | 0.2878 ms |  0.28 |    0.01 |

|                 Method |     Mean |     Error |    StdDev | Ratio |
|----------------------- |---------:|----------:|----------:|------:|
|   Vector4_Fna_Operator | 424.7 ms | 2.7105 ms | 2.5354 ms |  1.00 |
|     Vector4_A_Operator | 121.3 ms | 0.2793 ms | 0.2613 ms |  0.29 |
|     Vector4_B_Operator | 121.4 ms | 0.2078 ms | 0.1842 ms |  0.29 |
|        Vector4_Fna_Add | 421.5 ms | 4.7251 ms | 4.4198 ms |  0.99 |
| Vector4_Fna_Add_By_Ref | 121.2 ms | 0.5702 ms | 0.5333 ms |  0.29 |

## Results on my PC (.NET Framework 4.7)

```
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i5-6500 CPU 3.20GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
Frequency=3117186 Hz, Resolution=320.8022 ns, Timer=TSC
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0
  Job-XWSWKG : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3260.0

Runtime=Clr  Toolchain=net46
```

|                 Method |     Mean |    Error |    StdDev | Ratio |
|----------------------- |---------:|---------:|----------:|------:|
|   Vector2_Fna_Operator | 925.7 ms | 5.432 ms | 5.0810 ms |  1.00 |
|     Vector2_A_Operator | 762.1 ms | 1.604 ms | 1.5007 ms |  0.82 |
|     Vector2_B_Operator | 763.5 ms | 4.241 ms | 3.9671 ms |  0.82 |
|        Vector2_Fna_Add | 925.7 ms | 5.135 ms | 4.0092 ms |  1.00 |
| Vector2_Fna_Add_By_Ref | 121.2 ms | 1.003 ms | 0.9384 ms |  0.13 |

|                 Method |     Mean |     Error |    StdDev | Ratio |
|----------------------- |---------:|----------:|----------:|------:|
|   Vector3_Fna_Operator | 432.8 ms | 0.9754 ms | 0.8145 ms |  1.00 |
|     Vector3_A_Operator | 121.9 ms | 0.5441 ms | 0.5089 ms |  0.28 |
|     Vector3_B_Operator | 121.1 ms | 1.3576 ms | 1.2699 ms |  0.28 |
|        Vector3_Fna_Add | 431.1 ms | 1.1974 ms | 1.1201 ms |  1.00 |
| Vector3_Fna_Add_By_Ref | 121.8 ms | 0.7878 ms | 0.7369 ms |  0.28 |

|                 Method |     Mean |     Error |    StdDev | Ratio |
|----------------------- |---------:|----------:|----------:|------:|
|   Vector4_Fna_Operator | 428.1 ms | 2.7795 ms | 2.6000 ms |  1.00 |
|     Vector4_A_Operator | 121.8 ms | 0.3078 ms | 0.2728 ms |  0.28 |
|     Vector4_B_Operator | 120.5 ms | 0.2492 ms | 0.2081 ms |  0.28 |
|        Vector4_Fna_Add | 424.3 ms | 2.4599 ms | 2.3010 ms |  0.99 |
| Vector4_Fna_Add_By_Ref | 121.2 ms | 1.0973 ms | 1.0264 ms |  0.28 |
