using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace math_struct_benchmark
{
    // Current Vector4 as defined in FNA
    // https://github.com/FNA-XNA/FNA/blob/master/src/Vector4.cs
    public struct Vector4_Fna
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4_Fna(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Vector4_Fna operator +(Vector4_Fna value1, Vector4_Fna value2)
        {
            value1.W += value2.W;
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
        }

        public static void Add(ref Vector4_Fna value1, ref Vector4_Fna value2, out Vector4_Fna result)
        {
            result.W = value1.W + value2.W;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }
    }

    public struct Vector4_B
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4_B(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4_B operator +(Vector4_B value1, Vector4_B value2)
        {
            Vector4_B result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            result.W = value1.W + value2.W;
            return result;
        }
    }

    [ClrJob]
    [CoreJob]
    [LegacyJitX86Job]
    [LegacyJitX64Job]
    [MonoJob("MonoX64", @"C:\Program Files\Mono\bin\mono.exe")]
    [MonoJob("MonoX86", @"C:\Program Files (x86)\Mono\bin\mono.exe")]
    public class Vector4_Benchmarks
    {
        private const int Iterations = 100_000_000;

        [Benchmark(Baseline = true)]
        public void Vector4_Fna_Operator()
        {
            var a = new Vector4_Fna(1, 1, 1, 1);
            var b = new Vector4_Fna(1, 1, 1, 1);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector4_Fna_Add_By_Ref()
        {
            var a = new Vector4_Fna(1, 1, 1, 1);
            var b = new Vector4_Fna(1, 1, 1, 1);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                Vector4_Fna.Add(ref a, ref b, out a);
            }
        }

        [Benchmark]
        public void Vector4_B_Operator()
        {
            var a = new Vector4_B(1, 1, 1, 1);
            var b = new Vector4_B(1, 1, 1, 1);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Numerics_Vector4_Operator()
        {
            var a = new System.Numerics.Vector4(1, 1, 1, 1);
            var b = new System.Numerics.Vector4(1, 1, 1, 1);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Separate_Local_Floats()
        {
            var ax = 1f;
            var ay = 1f;
            var az = 1f;
            var aw = 1f;
            var bx = 1f;
            var by = 1f;
            var bz = 1f;
            var bw = 1f;

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                ax += bx;
                ay += by;
                az += bz;
                aw += bw;
            }
        }
    }
}
