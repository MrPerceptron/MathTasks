using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Task6;

namespace Benchmark
{
    [BenchmarkCategory]
    [MemoryDiagnoser]
    [RankColumn]
    public class Benchmark
    {

        [Benchmark]
        public void Test1()
        {
        }
        [Benchmark]
        public void Test2()
        {
        }
    }
}
