using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class AliBabaInParadise
    {
        #region YOUR CODE IS HERE
        #region FUNCTION#1: Calculate the Value
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given the Camels maximum load and N items, each with its weight and profit 
        /// Calculate the max total profit that can be carried within the given camels' load
        /// </summary>
        /// <param name="camelsLoad">max load that can be carried by camels</param>
        /// <param name="itemsCount">number of items</param>
        /// <param name="weights">weight of each item</param>
        /// <param name="profits">profit of each item</param>
        /// <returns>Max total profit</returns>
        public static int SolveValue(int camelsLoad, int itemsCount, int[] weights, int[] profits)
        {
            int[,] matrix = new int[camelsLoad + 1, itemsCount + 1];
            return SolveValueHelper(camelsLoad, itemsCount, weights, profits, matrix);
        }

        private static int SolveValueHelper(int camelsLoad, int itemsCount, int[] weights, int[] profits, int[,] matrix)
        {
            if (camelsLoad == 0 || itemsCount == 0)
            {
                return 0;
            }

            if (matrix[camelsLoad, itemsCount] != 0)
            {
                return matrix[camelsLoad, itemsCount];
            }

            int result;

            if (weights[itemsCount - 1] > camelsLoad)
            {
                result = SolveValueHelper(camelsLoad, itemsCount - 1, weights, profits, matrix);
            }
            else
            {
                result = profits[itemsCount - 1] + SolveValueHelper(camelsLoad - weights[itemsCount - 1], itemsCount, weights, profits, matrix);
                if (result < SolveValueHelper(camelsLoad, itemsCount - 1, weights, profits, matrix))
                {
                    result = SolveValueHelper(camelsLoad, itemsCount - 1, weights, profits, matrix);
                }
                
            }

            matrix[camelsLoad, itemsCount] = result;
            return result;
        }


        #endregion

        #region FUNCTION#2: Construct the Solution
        //Your Code is Here:
        //==================
        /// <returns>Tuple array of the selected items to get MAX profit (stored in Tuple.Item1) together with the number of instances taken from each item (stored in Tuple.Item2)
        /// OR NULL if no items can be selected</returns>
        static public Tuple<int, int>[] ConstructSolution(int camelsLoad, int itemsCount, int[] weights, int[] profits)
        {
            Dictionary<int, int> lastitem = new Dictionary<int, int>();
            Dictionary<int, int> max = new Dictionary<int, int>();

            for (int i = 1; i <= camelsLoad; i++)
            {
                for (int j = 0; j < itemsCount; j++)
                {
                    if (weights[j] <= i)
                    {
                        int currentProfit = profits[j] + (max.TryGetValue(i - weights[j], out int value) ? value : 0);
                        if (!max.ContainsKey(i) || currentProfit > max[i])
                        {
                            max[i] = currentProfit;
                            lastitem[i] = j;
                        }
                    }
                }
            }

            List<int> itemIndices = new List<int>();
            int remainder = camelsLoad;
            while (remainder > 0 && lastitem.ContainsKey(remainder))
            {
                int currentItem = lastitem[remainder];
                itemIndices.Add(currentItem);
                remainder -= weights[currentItem];
            }

            Dictionary<int, int> itemCounts = new Dictionary<int, int>();
            int totalWeight = 0;
            foreach (int itemIndex in itemIndices)
            {
                int itemWeight = weights[itemIndex];
                if (totalWeight + itemWeight <= camelsLoad)
                {
                    totalWeight += itemWeight;
                    if (itemCounts.ContainsKey(itemIndex))
                    {
                        itemCounts[itemIndex]++;
                    }
                    else
                    {
                        itemCounts[itemIndex] = 1;
                    }
                }
            }

            Tuple<int, int>[] solution = new Tuple<int, int>[itemCounts.Count];
            int index = 0;
            foreach (var entry in itemCounts)
            {
                solution[index] = Tuple.Create(entry.Key + 1, entry.Value);
                index++;
            }

            return solution;
        }


        #endregion

        #endregion
    }
}
