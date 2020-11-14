using System;
using System.Collections.Generic;
using System.Text;

namespace AADP
{
    /// <summary>
    /// Encapsulates the results of the particle search
    /// </summary>
    public struct Log
    {
        public double GlobalBestCost;
        public double[] GlobalBest;
        public double MsElapsed;


        public readonly override string ToString()
        {
            return $"Global Best: {FormatArray(GlobalBest)}\n" +
                $"Cost: {GlobalBestCost}\n" +
                $"Time Elapsed: {MsElapsed}ms";
        }

        private readonly string FormatArray(double[] array)
        {
            StringBuilder s = new StringBuilder();
            s.Append("{ ");

            s.Append(array[0]);

            for (int i = 1; i<array.Length; i++)
            {
                s.Append(", ");
                s.Append(array[i]);
            }

            s.Append(" }");
            return s.ToString();
        }
    }
}
