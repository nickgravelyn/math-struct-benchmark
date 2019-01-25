# math-struct-benchmark

Benchmarking techniques for math structs in C# in service of data for https://github.com/FNA-XNA/FNA/issues/227

Run it with `dotnet run -c Release -- -f "*"`.


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
