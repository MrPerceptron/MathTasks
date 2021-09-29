using BenchmarkDotNet.Attributes;
using Task2;

namespace Benchmark
{
    [BenchmarkCategory]
    [MemoryDiagnoser]
    [RankColumn]
    public class Task2Benchmark
    {
        private Palindrom _palindrom = new();

        [Benchmark]
        public void GetPalendromArr1()
        {
            Palindrom.GetPalendromArr(1);
        }

        [Benchmark]
        public void GetPalendromArr5()
        {
            Palindrom.GetPalendromArr(5);
        }

        [Benchmark]
        public void GetPalendromArr9()
        {
            Palindrom.GetPalendromArr(9);
        }
    }
}
