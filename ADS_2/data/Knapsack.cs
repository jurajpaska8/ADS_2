﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ADS_2.data
{
    class Knapsack
    {
        public int ItemsCount { get; set; }
        public int MaxWeight { get; set; }
        public int MaxFragileItemsCount { get; set; }

        private int[][] itemsArray;

        public Knapsack(int itemsCount, int maxWeight, int maxFragileItemsCount, int[][] itemsArray)
        {
            ItemsCount = itemsCount;
            MaxWeight = maxWeight;
            MaxFragileItemsCount = maxFragileItemsCount;
            this.itemsArray = itemsArray;
        }

        public int[] GetValues()
        {
            return Array.ConvertAll(itemsArray, arr => arr[1]);
        }

        public int[] GetWeights()
        {
            return Array.ConvertAll(itemsArray, arr => arr[2]);
        }

        public bool[] GetIsFragile()
        {
            return Array.ConvertAll(itemsArray, arr => arr[3] == 1);
        }

        public int KnapSackProblemBasic(int W, int[] values, int[] weights, int n)
        {
            // row with zeros, column with zeros
            int[,] arr = new int[n + 1, W + 1];

            // construct table
            for (int i = 0; i <= n; i++)
            {
                for (int w = 0; w <= W; w++)
                {
                    // first row and first column with zeros
                    if (i == 0 || w == 0)
                    {
                        arr[i,w] = 0;
                    }
                    // if we can not take item due to its weight
                    else if (weights[i - 1] > w)
                    {
                        arr[i,w] = arr[i - 1,w];
                    }
                    // else we choose maximum with/without selecting item
                    else
                    {
                        arr[i,w] = Math.Max(arr[i - 1,w], values[i - 1] + arr[i - 1,w - weights[i - 1]]);
                    }
                }
            }
            return arr[n,W];
        }

        public int KnapSackProblemItemLimitAttempt1(int W, int n, int count, int[] values, int[] weights, bool[] isFragile)
        {
            // 3 dim. matrix with zeros 
            int[][][] arr = new int[count + 1][][];
            int[][][] fragileArr = new int[count + 1][][];
            int[][][] nonFragileArr = new int[count + 1][][];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new int[n + 1][];
                fragileArr[i] = new int[n + 1][];
                nonFragileArr[i] = new int[n + 1][];

                for (int j = 0; j < arr[i].Length; j++)
                {
                    arr[i][j] = new int[W + 1];
                    fragileArr[i][j] = new int[W + 1];
                    nonFragileArr[i][j] = new int[W + 1];
                }
            }

            // construct table
            for (int z = 0; z <= count; z++)
            {
                for (int y = 0; y <= n; y++)
                {
                    for (int x = 0; x <= W; x++)
                    {
                        // first row and first column with zeros
                        if (x == 0 || y == 0 || z == 0)
                        {
                            arr[z][y][x] = 0;
                        }
                        // if we can not take item due to its weight
                        else if (weights[y - 1] > x)
                        {
                            arr[z][y][x] = arr[z][y - 1][x];
                            fragileArr[z][y][x] = fragileArr[z][y - 1][x];
                            nonFragileArr[z][y][x] = nonFragileArr[z][y - 1][x];

                        }
                        // else we choose maximum with/without selecting item
                        else
                        {
                            if (!isFragile[y - 1])
                            {
                                arr[z][y][x] = Math.Max(arr[z][y - 1][x],
                                                        arr[z - 1][y - 1][x - weights[y - 1]] + values[y - 1]);

                                // increment counters
                                if ((arr[z - 1][y - 1][x - weights[y - 1]] + values[y - 1]) > arr[z][y - 1][x])
                                {
                                    nonFragileArr[z][y][x] = nonFragileArr[z - 1][y - 1][x - weights[y - 1]] + 1;
                                }
                                else
                                {
                                    nonFragileArr[z][y][x] = nonFragileArr[z][y - 1][x];
                                }
                                fragileArr[z][y][x] = fragileArr[z][y - 1][x];

                            }
                            
                            // if item is fragile 
                            else
                            {
                                int currentFragileCount = fragileArr[z][y - 1][x];
                                if (currentFragileCount >= 10)
                                {
                                    arr[z][y][x] = arr[z][y - 1][x];
                                }
                                else
                                {
                                    arr[z][y][x] = Math.Max(arr[z][y - 1][x],
                                                            arr[z - 1][y - 1][x - weights[y - 1]] + values[y - 1]);
                                }


                                // increment counters
                                if ((arr[z - 1][y - 1][x - weights[y - 1]] + values[y - 1]) > arr[z][y - 1][x] && currentFragileCount < 10)
                                {
                                    fragileArr[z][y][x] = fragileArr[z - 1][y - 1][x - weights[y - 1]] + 1;
                                }
                                else
                                {
                                    fragileArr[z][y][x] = fragileArr[z][y - 1][x];
                                }
                                nonFragileArr[z][y][x] = nonFragileArr[z][y - 1][x];

                            }
                        }
                    }
                }
            }
            // max fragile
            int maxFrag = 0;
            for (int i = 0; i <= count; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    for (int k = 0; k <= W; k++)
                    {
                        if (fragileArr[i][j][k] > maxFrag) maxFrag = fragileArr[i][j][k];
                    }
                }
            }
            return arr[count][n][W];
        }

        public int KnapSackProblemItemLimit(int W, int n, int fragileCount, int[] values, int[] weights, bool[] isFragile)
        {
            // 3 dim. matrix with zeros 
            int[][][] arr = new int[fragileCount + 1][][];
            int[][][] fragileArr = new int[fragileCount + 1][][];
            int[][][] nonFragileArr = new int[fragileCount + 1][][];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new int[n + 1][];
                fragileArr[i] = new int[n + 1][];
                nonFragileArr[i] = new int[n + 1][];

                for (int j = 0; j < arr[i].Length; j++)
                {
                    arr[i][j] = new int[W + 1];
                    fragileArr[i][j] = new int[W + 1];
                    nonFragileArr[i][j] = new int[W + 1];
                }
            }

            // knapsack without fragile items
            for (int y = 0; y <= n; y++)
            {
                for (int x = 0; x <= W; x++)
                {
                    // first row and first column with zeros
                    if (x == 0 || y == 0)
                    {
                        arr[0][y][x] = 0;
                    }
                    // if we can not take item due to its weight
                    else if (weights[y - 1] > x)
                    {
                        arr[0][y][x] = arr[0][y - 1][x];
                        fragileArr[0][y][x] = fragileArr[0][y - 1][x];
                        nonFragileArr[0][y][x] = nonFragileArr[0][y - 1][x];
                    }
                    // else we choose maximum with/without selecting item
                    else
                    {
                        if (!isFragile[y - 1])
                        {
                            arr[0][y][x] = Math.Max(arr[0][y - 1][x],
                                                    arr[0][y - 1][x - weights[y - 1]] + values[y - 1]);

                            // increment counters
                            if ((arr[0][y - 1][x - weights[y - 1]] + values[y - 1]) > arr[0][y - 1][x])
                            {
                                nonFragileArr[0][y][x] = nonFragileArr[0][y - 1][x - weights[y - 1]] + 1;
                            }
                            else
                            {
                                nonFragileArr[0][y][x] = nonFragileArr[0][y - 1][x];
                            }
                            fragileArr[0][y][x] = fragileArr[0][y - 1][x];
                        }
                        else
                        {
                            arr[0][y][x] = arr[0][y - 1][x];
                            nonFragileArr[0][y][x] = nonFragileArr[0][y - 1][x];
                            fragileArr[0][y][x] = fragileArr[0][y - 1][x];
                        }
                    }
                }
            }

            // construct table
            for (int z = 1; z <= fragileCount; z++)
            {
                for (int y = 0; y <= n; y++)
                {
                    for (int x = 0; x <= W; x++)
                    {
                        // first row and first column with zeros
                        if (x == 0 || y == 0)
                        {
                            arr[z][y][x] = 0;
                        }
                        // if we can not take item due to its weight
                        else if (weights[y - 1] > x)
                        {
                            arr[z][y][x] = arr[z][y - 1][x];
                            fragileArr[z][y][x] = fragileArr[z][y - 1][x];
                            nonFragileArr[z][y][x] = nonFragileArr[z][y - 1][x];

                        }
                        // else we choose maximum with/without selecting item
                        else
                        {
                            if (!isFragile[y - 1])
                            {
                                arr[z][y][x] = Math.Max(arr[z][y - 1][x],
                                                        arr[z - 1][y - 1][x - weights[y - 1]] + values[y - 1]);

                                // increment counters
                                if ((arr[z - 1][y - 1][x - weights[y - 1]] + values[y - 1]) > arr[z][y - 1][x])
                                {
                                    nonFragileArr[z][y][x] = nonFragileArr[z - 1][y - 1][x - weights[y - 1]] + 1;
                                }
                                else
                                {
                                    nonFragileArr[z][y][x] = nonFragileArr[z][y - 1][x];
                                }
                                fragileArr[z][y][x] = fragileArr[z][y - 1][x];

                            }

                            // if item is fragile 
                            else
                            {
                                int currentFragileCount = fragileArr[z - 1][y - 1][x];
                                if (currentFragileCount >= z)
                                {
                                    arr[z][y][x] = arr[z][y - 1][x];
                                }
                                else
                                {
                                    arr[z][y][x] = Math.Max(arr[z][y - 1][x],
                                                            arr[z - 1][y - 1][x - weights[y - 1]] + values[y - 1]);
                                }


                                // increment counters
                                if ((arr[z - 1][y - 1][x - weights[y - 1]] + values[y - 1]) > arr[z][y - 1][x] && currentFragileCount < 10)
                                {
                                    fragileArr[z][y][x] = fragileArr[z - 1][y - 1][x - weights[y - 1]] + 1;
                                }
                                else
                                {
                                    fragileArr[z][y][x] = fragileArr[z][y - 1][x];
                                }
                                nonFragileArr[z][y][x] = nonFragileArr[z][y - 1][x];

                            }
                        }
                    }
                }
            }
            // max fragile
            int maxFrag = 0;
            for (int i = 0; i <= fragileCount; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    for (int k = 0; k <= W; k++)
                    {
                        if (fragileArr[i][j][k] > maxFrag) maxFrag = fragileArr[i][j][k];
                    }
                }
            }
            return arr[fragileCount][n][W];
        }
    }
}