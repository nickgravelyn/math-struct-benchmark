using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace math_struct_benchmark
{
    // Current Vector2 as defined in FNA
    // https://github.com/FNA-XNA/FNA/blob/master/src/Vector2.cs
    public struct Vector2_Fna
    {
        public float X;
        public float Y;

        public Vector2_Fna(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2_Fna operator +(Vector2_Fna value1, Vector2_Fna value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        public static void Add(ref Vector2_Fna value1, ref Vector2_Fna value2, out Vector2_Fna result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }
    }

    public struct Vector2_B
    {
        public float X;
        public float Y;

        public Vector2_B(float x, float y)
        {
            X = x;
            Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2_B operator +(Vector2_B value1, Vector2_B value2)
        {
            Vector2_B result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            return result;
        }
    }

    [ClrJob]
    [CoreJob]
    [LegacyJitX86Job]
    [LegacyJitX64Job]
    [MonoJob("MonoX64", @"C:\Program Files\Mono\bin\mono.exe")]
    [MonoJob("MonoX86", @"C:\Program Files (x86)\Mono\bin\mono.exe")]
    public class Vector2_Benchmarks
    {
        private const int Iterations = 100_000_000;

        [Benchmark(Baseline = true)]
        public void Vector2_Fna_Operator()
        {
            var a = new Vector2_Fna(1, 1);
            var b = new Vector2_Fna(1, 1);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector2_Fna_Add_By_Ref()
        {
            var a = new Vector2_Fna(1, 1);
            var b = new Vector2_Fna(1, 1);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                Vector2_Fna.Add(ref a, ref b, out a);
            }
        }

        [Benchmark]
        public void Vector2_B_Operator()
        {
            var a = new Vector2_B(1, 1);
            var b = new Vector2_B(1, 1);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Numerics_Vector2_Operator()
        {
            var a = new System.Numerics.Vector2(1, 1);
            var b = new System.Numerics.Vector2(1, 1);

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
            var bx = 1f;
            var by = 1f;

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                ax += bx;
                ay += by;
            }
        }
    }
}
