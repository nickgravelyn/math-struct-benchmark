using BenchmarkDotNet.Running;

namespace math_struct_benchmark
{
    class Program
    {
        static void Main(string[] args)
            => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
