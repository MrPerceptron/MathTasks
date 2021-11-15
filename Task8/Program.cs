using System;
using System.Collections.Generic;

namespace Task8
{
    internal class Program
    {
        static void Main()
        {
            CheckHashValueMethod(new int[] { 1, 2, 3, 4 });
        }
        static void CheckHashValueMethod(int[] checkingArr) 
        {
            List<List<int>> newArr = new();
            int i = 0;
            do
            {
                i++;
                newArr.Add(new(checkingArr));
            } while (NextSet(checkingArr));
            PrintFullArr(newArr);
            Console.WriteLine(i);
        }
        static void PrintFullArr(List<List<int>> arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                for (int j = 0; j < arr[i].Count; j++)
                    Console.Write($"{arr[i][j]}\t");
                Console.WriteLine();
            }
        }
        static void Swap(int[] arr, int i, int j)
        {
            int s = arr[i];
            arr[i] = arr[j];
            arr[j] = s;
        }
        static bool NextSet(int[] arr)
        {
            int j = arr.Length - 2;

            while (j != -1 && arr[j] >= arr[j + 1]) j--;
            if (j == -1)
                return false; // больше перестановок нет

            int k = arr.Length - 1;
            while (arr[j] >= arr[k]) 
                k--;

            Swap(arr, j, k);
            int l = j + 1, r = arr.Length - 1;

            while (l < r)
                Swap(arr, l++, r--);
            return true;
        }
    }
}
