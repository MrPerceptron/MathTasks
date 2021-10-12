using BenchmarkDotNet.Attributes;
using System.Linq;
using Task2;

namespace Benchmark
{
    [BenchmarkCategory]
    [MemoryDiagnoser]
    [RankColumn]
    public class Task2Benchmark
    {
        [Benchmark]
        public void CheckGetPalendromArr4()
        {
            Task2.Program.TestCode();
        }

        [Benchmark]
        public void GetPalendromArr4()
        {
            Palindrom.GetPalendromArr(9).TakeLast(5).ToList();
        }
    }
}
