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

        public static Vector3_Fna Add(Vector3_Fna value1, Vector3_Fna value2)
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

    public struct Vector3_A
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3_A(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3_A operator +(Vector3_A value1, Vector3_A value2)
        {
            return new Vector3_A(
                value1.X + value2.X,
                value1.Y + value2.Y,
                value1.Z + value2.Z);
        }
    }

    public struct Vector3_A_AggressiveInlining
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3_A_AggressiveInlining(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3_A_AggressiveInlining operator +(Vector3_A_AggressiveInlining value1, Vector3_A_AggressiveInlining value2)
        {
            return new Vector3_A_AggressiveInlining(
                value1.X + value2.X,
                value1.Y + value2.Y,
                value1.Z + value2.Z);
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

        public static Vector3_B operator +(Vector3_B value1, Vector3_B value2)
        {
            Vector3_B result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            return result;
        }
    }

    public struct Vector3_B_AggressiveInlining
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3_B_AggressiveInlining(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3_B_AggressiveInlining operator +(Vector3_B_AggressiveInlining value1, Vector3_B_AggressiveInlining value2)
        {
            Vector3_B_AggressiveInlining result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            return result;
        }
    }

    [ClrJob, MonoJob, CoreJob]
    public class Vector3_Benchmarks
    {
        private const int Iterations = 100_000_000;

        [Benchmark(Baseline = true)]
        public void Vector3_Fna_Operator()
        {
            var a = new Vector3_Fna(1, 2, 3);
            var b = new Vector3_Fna(4, 5, 6);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector3_A_Operator()
        {
            var a = new Vector3_A(1, 2, 3);
            var b = new Vector3_A(4, 5, 6);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector3_A_AggressiveInlining_Operator()
        {
            var a = new Vector3_A_AggressiveInlining(1, 2, 3);
            var b = new Vector3_A_AggressiveInlining(4, 5, 6);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector3_B_Operator()
        {
            var a = new Vector3_B(1, 2, 3);
            var b = new Vector3_B(4, 5, 6);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector3_B_AggressiveInlining_Operator()
        {
            var a = new Vector3_B_AggressiveInlining(1, 2, 3);
            var b = new Vector3_B_AggressiveInlining(4, 5, 6);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void Vector3_Fna_Add()
        {
            var a = new Vector3_Fna(1, 2, 3);
            var b = new Vector3_Fna(4, 5, 6);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = Vector3_Fna.Add(a, b);
            }
        }

        [Benchmark]
        public void Vector3_Fna_Add_By_Ref()
        {
            var a = new Vector3_Fna(1, 2, 3);
            var b = new Vector3_Fna(4, 5, 6);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                Vector3_Fna.Add(ref a, ref b, out a);
            }
        }
    }
}
