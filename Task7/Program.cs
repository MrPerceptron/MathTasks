using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using static Task7.Program;

namespace Task7
{
    internal class Program
    {
        class Tree
        {
            public Node Root { get; set; } = new();
        }
        class Node
        {
            public Node Parent { get; set; }
            public Node[] Positions { get; set; }
            public int Value { get; set; }
        }
        static void Main()
        {
            Tree tree = new();
            tree.Root.Positions = new Node[] { new Node { Value = 2 }, new Node { Value = 3 }, new Node { Value = 4 } };
        }
    }
}

/*
        static int SumBinaryTree(Node node)
        {
            int returnValue = 0;

            returnValue += node.Value;

            if (node.Positions != null)
            {
                for (int i = 0; i < node.Positions.Length; i++)
                {
                    //if (node.Positions[i] != null) // Вроде не нужно
                    returnValue += SumBinaryTree(node.Positions[i]);
                }
            }

            return returnValue;
        }
        public class Storage : List<int>
        {
            public Storage() : base() { }
            public Storage(int capacity) : base(capacity) { }
            public Storage(IEnumerable<int> collection) : base(collection) { }

            public new int this[int index]
            {
                get
                {
                    int returnValue = base[index % Count];
                    Remove(returnValue);
                    return returnValue;
                }
            }
            public int GetMinimalNumber()
            {
                int minNumber = base[0];

                for (int i = 1; i < Count; i++)
                    if (base[i] < minNumber)
                        minNumber = base[i];

                Remove(minNumber);
                return minNumber;
            }
        }
        */