using ADS_2.data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADS_2
{
    class Program
    {
        static void test1()
        {
            FileHandler fileHandler = new FileHandler("C:/Users/z0045c9c/source/repos/ADS_2/ADS_2/data/predmety.txt");
            Knapsack knapsack = fileHandler.CreateKnapsack();
            int[] values = knapsack.GetValues();
            int[] weights = knapsack.GetWeights();
            bool[] isFragile = knapsack.GetIsFragile();
            int itemsCount = knapsack.ItemsCount;
            int maxWeight = knapsack.MaxWeight;
            int fragileCount = knapsack.MaxFragileItemsCount;


            // select only fragile items 
            List<int> listOfFragileIndices = new List<int>();
            for (int i = 0; i < itemsCount; i++)
            {
                if (isFragile[i])
                {
                    listOfFragileIndices.Add(i);
                }
            }

            int onlyFragileCount = listOfFragileIndices.Count;
            int[] onlyFragileValues = new int[onlyFragileCount];
            int[] onlyFragileWeights = new int[onlyFragileCount];

            for (int i = 0; i < onlyFragileCount; i++)
            {
                onlyFragileValues[i] = values[listOfFragileIndices.ElementAt(i)];
                onlyFragileWeights[i] = weights[listOfFragileIndices.ElementAt(i)];
            }

            // knapsack fragile 
            int maxValueOnlyFragile = knapsack.KnapSackProblemBasic(maxWeight, onlyFragileValues, onlyFragileWeights, onlyFragileCount); //fragileCount

            // non fragile values
            int[] nonFragileValues = new int[itemsCount - onlyFragileCount];
            int[] nonFragileWeights = new int[itemsCount - onlyFragileCount];

            int idx = 0;
            for (int i = 0; i < itemsCount; i++)
            {
                if (!isFragile[i])
                {
                    nonFragileValues[idx] = values[i];
                    nonFragileWeights[idx] = weights[i];
                    idx++;
                }
            }
        }
        static void Main(string[] args)
        {
            FileHandler fileHandler = new FileHandler("C:/Users/z0045c9c/source/repos/ADS_2/ADS_2/data/predmety.txt");
            Knapsack knapsack = fileHandler.CreateKnapsack();
            int[] values = knapsack.GetValues();
            int[] weights = knapsack.GetWeights();
            bool[] isFragile = knapsack.GetIsFragile();
            int itemsCount = knapsack.ItemsCount;
            int maxWeight = knapsack.MaxWeight;
            int fragileCount = knapsack.MaxFragileItemsCount;


            // select only fragile items 
            List<int> listOfFragileIndices = new List<int>();
            // select only nonfragile
            List<int> listOfNonFragileIndices = new List<int>();

            for (int i = 0; i < itemsCount; i++)
            {
                if (isFragile[i])
                {
                    listOfFragileIndices.Add(i);
                }
                else 
                {
                    listOfNonFragileIndices.Add(i);
                }
            }
            //TODO check values on nonfragile items

            //knapsack with limit
            int sum = knapsack.KnapSackProblemItemLimit(maxWeight, itemsCount, fragileCount, values, weights, isFragile);
            Console.WriteLine("Hello World!");
        }
    }
}
