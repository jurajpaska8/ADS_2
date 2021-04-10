using System;
using System.Collections.Generic;
using System.IO;

namespace ADS_2.data
{
    class FileHandler
    {
        public string Path { get; set; }
        public FileHandler(string path)
        {
            Path = path;
        }

        private string[] ReadLines()
        {
            string[] lines = File.ReadAllLines(Path);
            return lines;
        }

        public Knapsack CreateKnapsack()
        {
            // read lines
            string[] lines = ReadLines();
            // create int array
            int[][] arrayItems = new int[lines.Length - 3][];
            int itemsCount = int.Parse(lines[0]);
            int maxWeight = int.Parse(lines[1]);
            int maxFragileItemsCount = int.Parse(lines[2]);
            for(int i = 0; i < lines.Length - 3; i++)
            {
                int[] myInts = Array.ConvertAll(lines[i + 3].Split(" "), s => int.Parse(s));
                arrayItems[i] = myInts;
            }
            
            return new Knapsack(itemsCount, maxWeight, maxFragileItemsCount, arrayItems);
        }

        public static void WriteItemsIntoFile(string fileName, int value, LinkedList<int> items)
        {
            LinkedList<string> stringToWrite = new LinkedList<string>();
            stringToWrite.AddLast(value.ToString());
            stringToWrite.AddLast(items.Count.ToString());
            foreach(int id in items)
            {
                stringToWrite.AddLast(id.ToString());
            }
            File.WriteAllLines(fileName, stringToWrite);
        }
    }
}
