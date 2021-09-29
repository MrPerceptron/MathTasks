using BenchmarkDotNet.Running;
using System;

namespace Benchmark
{
    internal class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<Task2Benchmark>();
        }
    }
}
