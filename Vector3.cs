using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace math_struct_benchmark
{
    // Current Vector3 as defined in FNA
    // https://github.com/FNA-XNA/FNA/blob/master/src/Vector3.cs
    public struct Vector3_Fna
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3_Fna(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3_Fna operator +(Vector3_Fna value1, Vector3_Fna value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
        }

        public static void Add(ref Vector3_Fna value1, ref Vector3_Fna value2, out Vector3_Fna result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }
    }

    public struct Vector3_B
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3_B(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3_B operator +(Vector3_B value1, Vector3_B value2)
        {
            Vector3_B result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            return result;
        }
    }

    [ClrJob]
    [CoreJob]
    [LegacyJitX86Job]
    [LegacyJitX64Job]
    [MonoJob("MonoX64", @"C:\Program Files\Mono\bin\mono.exe")]
    [MonoJob("MonoX86", @"C:\Program Files (x86)\Mono\bin\mono.exe")]
    public class Vector3_Benchmarks
    {
        private const int Iterations = 100_000_000;

        [Benchmark(Baseline = true)]
        public void Vector3_Fna_Operator()
        {
            var a = new Vector3_Fna(1, 1, 1);
            var b = new Vector3_Fna(1, 1, 1);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector3_Fna_Add_By_Ref()
        {
            var a = new Vector3_Fna(1, 1, 1);
            var b = new Vector3_Fna(1, 1, 1);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                Vector3_Fna.Add(ref a, ref b, out a);
            }
        }

        [Benchmark]
        public void Vector3_B_Operator()
        {
            var a = new Vector3_B(1, 1, 1);
            var b = new Vector3_B(1, 1, 1);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Numerics_Vector3_Operator()
        {
            var a = new System.Numerics.Vector3(1, 1, 1);
            var b = new System.Numerics.Vector3(1, 1, 1);

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
            var bx = 1f;
            var by = 1f;
            var bz = 1f;

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                ax += bx;
                ay += by;
                az += bz;
            }
        }
    }
}
