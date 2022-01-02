using BenchmarkDotNet.Attributes;

namespace Benchmark
{
    [BenchmarkCategory]
    [MemoryDiagnoser]
    [RankColumn]
    public class Benchmark
    {
        [Benchmark]
        public void MainTest1()
        {
        }

        [Benchmark]
        public void MainTest2()
        {
        }
    }
}
