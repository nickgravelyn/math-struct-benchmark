using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace StructBenchmark
{
    public struct A
    {
        public float X;
        public float Y;

        public A(float x, float y)
        {
            X = x;
            Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static A operator +(A value1, A value2)
        {
            A result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            return result;
        }
    }

    public struct B
    {
        public float X;
        public float Y;
        public float Zero;

        public B(float x, float y)
        {
            X = x;
            Y = y;
            Zero = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static B operator +(B value1, B value2)
        {
            B result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Zero = 0;
            return result;
        }
    }

    public class Benchmarks
    {
        private const int Iterations = 100_000_000;

        [Benchmark]
        public void A_Add()
        {
            var a = new A(1, 2);
            var b = new A(4, 5);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }

        [Benchmark]
        public void B_Add()
        {
            var a = new B(1, 2);
            var b = new B(4, 5);

            var iterations = Iterations;
            while (iterations-- > 0)
            {
                a = a + b;
            }
        }
    }

    class Program
    {
        static void Main(string[] args) =>
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
