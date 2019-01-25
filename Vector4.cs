using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

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

        public static Vector4_Fna Add(Vector4_Fna value1, Vector4_Fna value2)
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

    public struct Vector4_A
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4_A(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Vector4_A operator +(Vector4_A value1, Vector4_A value2)
        {
            return new Vector4_A(
                value1.X + value2.X,
                value1.Y + value2.Y,
                value1.Z + value2.Z,
                value1.W + value2.W);
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

        public static Vector4_B operator +(Vector4_B value1, Vector4_B value2)
        {
            Vector4_B result;
            result.W = value1.W + value2.W;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            return result;
        }
    }

    [ClrJob, MonoJob, CoreJob]
    public class Vector4_Benchmarks
    {
        private const int Iterations = 100_000_000;

        [Benchmark(Baseline = true)]
        public void Vector4_Fna_Operator()
        {
            var a = new Vector4_Fna(1, 2, 3, 4);
            var b = new Vector4_Fna(4, 5, 6, 7);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector4_A_Operator()
        {
            var a = new Vector4_A(1, 2, 3, 4);
            var b = new Vector4_A(4, 5, 6, 7);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector4_B_Operator()
        {
            var a = new Vector4_B(1, 2, 3, 4);
            var b = new Vector4_B(4, 5, 6, 7);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector4_Fna_Add()
        {
            var a = new Vector4_Fna(1, 2, 3, 4);
            var b = new Vector4_Fna(4, 5, 6, 7);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = Vector4_Fna.Add(a, b);
            }
        }

        [Benchmark]
        public void Vector4_Fna_Add_By_Ref()
        {
            var a = new Vector4_Fna(1, 2, 3, 4);
            var b = new Vector4_Fna(4, 5, 6, 7);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                Vector4_Fna.Add(ref a, ref b, out a);
            }
        }
    }
}
