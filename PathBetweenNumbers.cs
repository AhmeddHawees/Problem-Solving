using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{

    public static class PathBetweenNumbers
    {
        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given two numbers X and Y, find the min number of operations to convert X into Y
        /// Allowed Operations:
        /// 1.	Multiply the current number by 2 (i.e. replace the number X by 2 × X)
        /// 2.	Subtract 1 from the current number (i.e. replace the number X by X – 1)
        /// 3.	Append the digit 1 to the right of current number (i.e. replace the number X by 10 × X + 1).
        /// </summary>
        /// <param name="X">start number</param>
        /// <param name="Y">target number</param>
        /// <returns>min number of operations to convert X into Y</returns>
        public static int Find(int X, int Y)
        {
            if (X == Y)
            {
                return 0;
            }

            // Initialize a queue and a dictionary to keep track of visited numbers
            Queue<(int, int)> QueueNumbers = new Queue<(int, int)>();
            Dictionary<int, int> Distances = new Dictionary<int, int>();
            Dictionary<int, string> Colors = new Dictionary<int, string>();

            // Enqueue the starting number X with a distance of 0 and add it to the visited dictionary with color GREY
            QueueNumbers.Enqueue((X, 0));
            Distances[X] = 0;
            Colors[X] = "GREY";

            // While the queue is not empty, keep processing the numbers
            while (QueueNumbers.Count != 0)
            {
                // Dequeue the next number in the queue and the distance from starting number
                var (CurrentNumber, distance) = QueueNumbers.Dequeue();

                // If the current number is equal to the target Y, return the distance from starting number
              

                // If we can double the current number and it is less than Y, add it to the queue with a distance incremented by 1 and color GREY
                if (CurrentNumber < Y && (!Distances.ContainsKey(CurrentNumber * 2) || Distances[CurrentNumber * 2] <= 0) )
                {
                    int doubledNumber = CurrentNumber * 2;
                    QueueNumbers.Enqueue((doubledNumber, distance + 1));
                    Distances[doubledNumber] = distance + 1;
                    Colors[doubledNumber] = "GREY";
                }

                // If we can subtract 1 from the current number and it is greater than 1, add it to the queue with a distance incremented by 1 and color GREY
                if (CurrentNumber > 1 && (!Distances.ContainsKey(CurrentNumber - 1) || Distances[CurrentNumber - 1] <= 0))
                {
                    int decreasedNumber = CurrentNumber - 1;
                    QueueNumbers.Enqueue((decreasedNumber, distance + 1));
                    Distances[decreasedNumber] = distance + 1;
                    Colors[decreasedNumber] = "GREY";
                }

                // If we can append a 1 to the current number and it is less than or equal to 2Y, add it to the queue with a distance incremented by 1 and color GREY
                int appendedNumber = CurrentNumber * 10 + 1;
                if (appendedNumber <= 2 * Y && (!Distances.ContainsKey(appendedNumber) || Distances[appendedNumber] <= 0))
                {
                    QueueNumbers.Enqueue((appendedNumber, distance + 1));
                    Distances[appendedNumber] = distance + 1;
                    Colors[appendedNumber] = "GREY";
                }
                if (CurrentNumber == Y)
                {
                    return distance;
                }
                // Set the color of the current number to BLACK to indicate that it has been processed
                Colors[CurrentNumber] = "BLACK";
            }

            // If we can't reach Y from X, return -1
            return -1;
        }

        #endregion

    }
}
