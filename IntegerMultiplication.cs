using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class IntegerMultiplication
    {
        #region YOUR CODE IS HERE

        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 large integers of N digits in an efficient way [Karatsuba's Method]
        /// </summary>
        /// <param name="X">First large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="Y">Second large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="N">Number of digits (PadLeft of 2)</param>
        /// <returns>Resulting large integer of 2xN digits (left padded with 0's if necessarily) [0: least signif., 2xN-1: most signif.]</returns>
        static public byte[] IntegerMultiply(byte[] X, byte[] Y, int N)
        {
            X = X.Reverse().ToArray();
            Y = Y.Reverse().ToArray();
            byte[] result = Multiply(X, Y, N);
            result = result.Reverse().ToArray();

            return result;
        }
            static public byte[] Multiply(byte[] Y, byte[] X, int N)
        {
            if (N <= 8)
            {
                byte[] result = longtoarray(arraytolong(X) * arraytolong(Y));
                return result;
            }
            else
            {
                int N2 = N;
                int n1 = N2 / 2;

                if (N % 2 != 0)
                {
                    X = AddPadding(X);
                    Y = AddPadding(Y);
                    N2 = N + 1;
                    n1 = N2 / 2;
                }


                byte[] B = X.Take(n1).ToArray();
            byte[] A = X.Skip(n1).ToArray();
            byte[] D = Y.Take(n1).ToArray();
            byte[] C = Y.Skip(n1).ToArray();

                byte[] AmultiplyC = null;
                byte[] BmultiplyD = null;
                Task task1 = Task.Factory.StartNew(() => AmultiplyC = Multiply(A, C, n1));
                Task task2 = Task.Factory.StartNew(() => BmultiplyD = Multiply(B, D, n1));
                byte[] AplusB = Add(A, B);
                byte[] CplusD = Add(C, D);
                Equavilent(ref AplusB, ref CplusD);
                Equavilent(ref CplusD, ref AplusB);
                byte[] Z = null;
                Task task3 = Task.Factory.StartNew(() => Z = Multiply(AplusB, CplusD, AplusB.Length));
                Task.WaitAll(task1, task2, task3);
                byte[] ACplusBD = Add(AmultiplyC, BmultiplyD);
                Z = Subtract(Z, ACplusBD);

                byte[] result = new byte[2 * N2];
                BmultiplyD = AddPaddingToLift(BmultiplyD, N2);
                Z = AddPaddingToLift(Z, n1);
                byte[] result1 = Add(BmultiplyD, Z);
                result = Add(result1, AmultiplyC);
                return result;
            }
        }

        // AddPadding method definition
        static public byte[] AddPadding(byte[] input)
        {
            byte[] output = new byte[input.Length + 1];
            Array.Copy(input, 0, output, 1, input.Length);
            return output;
        }

        static public byte[] AddZeros(byte[] a, int max)
        {
            int n = max - a.Length;
            byte[] newArray = new byte[a.Length + n];
            if (a.Length < max)
            {

                for (int i = 0; i < a.Length; i++)
                {
                    newArray[i + n] = a[i];
                }

                for (int i = 0; i < n; i++)
                {
                    newArray[i] = 0;
                }
            }
            else
            {
                newArray = a;
            }
            return newArray;
        }
        public static byte[] Add(byte[] arr1, byte[] arr2)
        {
            int max = Math.Max(arr2.Length, arr2.Length);
            arr2 = AddZeros(arr2, max);
            arr2 = AddZeros(arr2, max);
            Equavilent(ref arr1, ref arr2);
            byte[] result = new byte[arr1.Length];
            int carry = 0;

            for (int i = arr1.Length - 1; i >= 0; i--)
            {
                int sum = arr1[i] + arr2[i] + carry;
                result[i] = (byte)(sum % 10);
                carry = sum >= 10 ? 1 : 0;
            }
            byte[] newResult = new byte[result.Length + 1];

            if (carry == 1)
            {
                newResult[0] = 1;
                Array.Copy(result, 0, newResult, 1, result.Length);
                result = newResult;
            }

            return result;
        }
        public static byte[] Subtract(byte[] X, byte[] Y)
        {
            Equavilent(ref X, ref Y);
            int length = X.Length;
            byte[] result = new byte[length];
            int borrow = 0;

            for (int i = length - 1; i >= 0; i--)
            {
                int sub = X[i] - Y[i] - borrow;
                if (sub < 0)
                {
                    sub += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                result[i] = (byte)sub;
            }

            return result;
        }
        public static void Equavilent(ref byte[] arr1, ref byte[] arr2)
        {
            int num1 = arr1.Length;
            int num2 = arr2.Length;

            if (num1 < num2)
            {
                Array.Resize(ref arr1, num2);
                Array.Copy(arr1, 0, arr1, num2 - num1, num1);
                Array.Clear(arr1, 0, num2 - num1);
            }
            else if (num2 < num1)
            {
                Array.Resize(ref arr2, num1);
                Array.Copy(arr2, 0, arr2, num1 - num2, num2);
                Array.Clear(arr2, 0, num1 - num2);
            }
        }
        public static long arraytolong(byte[] byteArray)
        {
            long result = 0;
            int length = byteArray.Length;
            int sign = 1;

            for (int i = 0; i < length; i++)
            {
                if (i == 0 && (byteArray[i] & 0x80) != 0)
                {
                    // The first byte is the sign byte for negative numbers
                    sign = -1;
                    result = (byteArray[i] & 0x7F);
                }
                else
                {
                    // Append the current digit to the result
                    result = result * 10 + (byteArray[i] & 0x7F);
                }

              
            }
            return result * sign;
        }
        public static byte[] longtoarray(long number)
        {
            if (number == 0)
            {
                return new byte[] { 0 };
            }

            int length = 0;
            long absNumber = Math.Abs(number);

            while (absNumber > 0)
            {
                length++;
                absNumber /= 10;
            }

            byte[] result = new byte[length];
            int i = length - 1;

            while (number != 0)
            {
                result[i--] = (byte)Math.Abs(number % 10);
                number /= 10;
            }

            if (number < 0)
            {
                result[0] |= 0x80; // set the sign bit for negative numbers
            }

            return result;
        }

        public static byte[] AddPaddingToLift(byte[] arr, int padding)
        {
            byte[] result = new byte[arr.Length + padding];
            Array.Copy(arr, 0, result, 0, arr.Length);
            return result;
        }



        #endregion
    }
}
