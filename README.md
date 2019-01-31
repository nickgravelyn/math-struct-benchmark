# math-struct-benchmark

This branch demonstrates a particular oddity discovered when testing various approaches to + operators for math structs.

In this branch I've stripped the benchmarks down to just two simple structs on .NET Core to isolate the issue.

## Results on my PC

```
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i5-6500 CPU 3.20GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
Frequency=3117189 Hz, Resolution=320.8019 ns, Timer=TSC
.NET Core SDK=2.2.103
  [Host] : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT
  Core   : .NET Core 2.2.1 (CoreCLR 4.6.27207.03, CoreFX 4.6.27207.03), 64bit RyuJIT

Job=Core  Runtime=Core
```

| Method |     Mean |     Error |    StdDev |
|------- |---------:|----------:|----------:|
|  A_Add | 784.9 ms | 7.3740 ms | 6.8977 ms |
|  B_Add | 125.1 ms | 0.7501 ms | 0.7017 ms |

## Disassembly

It's definitely related to the instructions RyuJit is emitting. This is what BenchmarkDotNet gives me from the disassembly:

```assembly
; StructBenchmark.Benchmarks.A_Add()
       sub     rsp,18h
       vzeroupper
       xor     eax,eax
       mov     qword ptr [rsp+10h],rax
       mov     qword ptr [rsp+8],rax
       vmovss  xmm0,dword ptr [00007ff9`9e9e2718]
       vmovss  dword ptr [rsp+10h],xmm0
       vmovss  xmm0,dword ptr [00007ff9`9e9e271c]
       vmovss  dword ptr [rsp+14h],xmm0
       mov     eax,5F5E100h
       jmp     M00_L01
M00_L00:
       vxorps  xmm0,xmm0,xmm0
       vmovss  dword ptr [rsp+8],xmm0
       vmovss  dword ptr [rsp+0Ch],xmm0
       vmovss  xmm0,dword ptr [rsp+10h]
       vaddss  xmm0,xmm0,dword ptr [00007ff9`9e9e2720]
       vmovss  dword ptr [rsp+8],xmm0
       vmovss  xmm0,dword ptr [rsp+14h]
       vaddss  xmm0,xmm0,dword ptr [00007ff9`9e9e2724]
       vmovss  dword ptr [rsp+0Ch],xmm0
       mov     rax,qword ptr [rsp+8]
       mov     qword ptr [rsp+10h],rax
       mov     eax,edx
M00_L01:
       lea     edx,[rax-1]
       test    eax,eax
       jg      M00_L00
       add     rsp,18h
       ret
       add     byte ptr [rax],al
       add     byte ptr [rax],al
       add     byte ptr [rax],al
       add     byte ptr [rax+3Fh],al
       add     byte ptr [rax],al
       add     byte ptr [rax],0
       mov     al,byte ptr [0022040001041940h]
; Total bytes of code 175
```
```assembly
; StructBenchmark.Benchmarks.B_Add()
       vzeroupper
       vmovss  xmm0,dword ptr [00007ff9`9e9f26c0]
       vmovss  xmm1,dword ptr [00007ff9`9e9f26c4]
       vxorps  xmm2,xmm2,xmm2
       mov     eax,5F5E100h
       jmp     M00_L01
M00_L00:
       vaddss  xmm0,xmm0,dword ptr [00007ff9`9e9f26c8]
       vaddss  xmm1,xmm1,dword ptr [00007ff9`9e9f26cc]
       mov     eax,edx
M00_L01:
       lea     edx,[rax-1]
       test    eax,eax
       jg      M00_L00
       ret
       add     byte ptr [rax],al
       add     byte ptr [rax],al
       add     byte ptr [rax+3Fh],al
       add     byte ptr [rax],al
       add     byte ptr [rax],0
       mov     al,byte ptr [0000400000001940h]
       add     byte ptr [rax],cl
       cli
       mov     cl,9Eh
; Total bytes of code 92
```
