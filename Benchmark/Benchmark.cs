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
        private int Test1() => 1;
        private int Test2()
        {
            return 1;
        }

        static int number = int.MaxValue;

        [Benchmark]
        public void MainTest1()
        {
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < number; i++)
                {
                    Test1();
                }
            }
        }
        [Benchmark]
        public void MainTest2()
        {
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < number; i++)
                {
                    Test2();
                }
            }
        }
    }
}
