using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public static class IntegerMultiplication
    {
        #region YOUR CODE IS HERE

        static public byte[] IntegerMultiply(byte[] X, byte[] Y, int N)
        {
            if (X.Length > Y.Length)
            {
                Array.Resize(ref Y, X.Length);
            }
            else
            {
                Array.Resize(ref X, Y.Length);
            }
            if (X.Length % 2 != 0)
            {
                Array.Resize(ref X, X.Length + 1);
                Array.Resize(ref Y, Y.Length + 1);
            }
            if (N <= 128)
            {
                int m = X.Length; int n = Y.Length;
                byte[] res = new byte[m + n];
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        res[i + j] += (byte)(X[i] * Y[j]);
                        res[i + j + 1] += (byte)(res[i + j] / 10);
                        res[i + j] %= 10;
                    }
                }
                return res;
            }

            N = X.Length;
            int mid = N / 2;
            byte[] A = new byte[mid],
                   B = new byte[mid],
                   C = new byte[mid],
                   D = new byte[mid];
            for (int i = 0; i < mid; ++i)
            {

                A[i] = (X[i]);
                B[i] = X[mid + i];
                C[i] = (Y[i]);
                D[i] = Y[mid + i];
            }
            var tasks = new Task<byte[]>[3];
            tasks[0] = Task.Run(() => IntegerMultiply(A, C, mid));
            tasks[1] = Task.Run(() => IntegerMultiply(B, D, mid));
            tasks[2] = Task.Run(() => IntegerMultiply(FindSum(A, B), FindSum(C, D), mid));

            Task.WaitAll(tasks);

            byte[] M1 = tasks[0].Result;
            byte[] M2 = tasks[1].Result;
            byte[] P3 = tasks[2].Result;

            byte[] Z = FindDiif(P3, FindSum(M1, M2));
            byte[] resultArray = FindSum(FindSum(tenpoweN(Z, N / 2), M1), tenpoweN(M2, N));
            Array.Resize(ref resultArray, N * 2);

            return resultArray;
        }


        private static byte[] tenpoweN(byte[] X, int n)
        {
            byte[] res = new byte[X.Length + n];
            Array.Copy(X, 0, res, n, X.Length);
            return res;
        }
        static public byte[] FindSum(byte[] X, byte[] Y)
        {
            int Max_Size = Math.Max(X.Length, Y.Length);
            byte[] res = new byte[Max_Size];

            byte carry = 0;
            for (int i = 0; i < Max_Size; i++)
            {
                int first_num = 0;
                int sec_num = 0;
                if (i < X.Length)
                    first_num = X[i];
                if (i < Y.Length)
                    sec_num = Y[i];
                byte add = (byte)(first_num + sec_num + carry);
                res[i] = (byte)(add % 10);
                carry = (byte)(add / 10);
            }
            if (carry > 0)
            {
                Array.Resize(ref res, Max_Size + 1);
                res[Max_Size] = carry;
            }
            return res;
        }

        static public byte[] FindDiif(byte[] X, byte[] Y)
        {
            int N = Math.Max(X.Length, Y.Length);
            byte[] sub = new byte[N];
            int borrow = 0, first_num, sec_num;
            for (int i = 0; i < N; i++)
            {
                first_num = 0;
                sec_num = 0;
                if (i < X.Length)
                    first_num = X[i];
                if (i < Y.Length)
                    sec_num = Y[i];
                int res;
                if (borrow == 1)
                {
                    first_num = first_num - 1;
                }
                if (first_num >= sec_num)
                {
                    res = (first_num - sec_num);
                    borrow = 0;
                }
                else
                {
                    first_num = first_num + 10;
                    res = first_num - sec_num;
                    borrow = 1;
                }

                sub[i] = (byte)res;
            }
            return sub;
        }
        // was used to return A+B C+D in on loop but multi task was better


        static public Tuple<byte[], byte[]> FindSum2(byte[] X, byte[] Y, byte[] C, byte[] D)
        {
            int N = Math.Max(X.Length, Y.Length);
            int N2 = Math.Max(C.Length, D.Length);
            byte[] res = new byte[N];
            byte[] res2 = new byte[N2];

            byte carry = 0, c = 0;
            for (int i = 0; i < Math.Max(N, N2); i++)
            {
                int a1 = 0;
                int b1 = 0;
                int a2 = 0;
                int b2 = 0;
                if (i < X.Length)
                    a1 = X[i];
                if (i < Y.Length)
                    b1 = Y[i];
                if (i < C.Length)
                    a2 = C[i];
                if (i < D.Length)
                    b2 = D[i];
                byte add = (byte)(a1 + b1 + carry);
                res[i] = (byte)(add % 10);
                carry = (byte)(add / 10);
                add = (byte)(a2 + b2 + c);
                res2[i] = (byte)(add % 10);
                c = (byte)(add / 10);
            }
            if (carry > 0)
            {
                Array.Resize(ref res, N + 1);
                res[N] = carry;
            }
            if (c > 0)
            {
                Array.Resize(ref res2, N2 + 1);
                res2[N2] = c;
            }
            Tuple<byte[], byte[]> t = new Tuple<byte[], byte[]>(res, res2);
            return t;
        }
    }
    #endregion
}