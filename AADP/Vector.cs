using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AADP
{
    /// <summary>
    /// Encapsulates a N dimensional vector
    /// </summary>
    struct Vector : IEnumerable
    {
        public double Limit => (this.Count + 1d) / 2d;

        /// <summary>
        /// Converts a Vector position into an antenna design
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static double[] ToDesign(Vector position)
        {
            List<double> d = new List<double>(position.Values)
            {
                position.Limit
            };
            return d.ToArray();
        }

        /// <summary>
        /// Converts a design into a position vector
        /// </summary>
        /// <param name="design">the design to be converted</param>
        /// <returns>a vector with data from <paramref name="design"/></returns>
        public static Vector FromDesign(double[] design)
        {
            var temp = design.SkipLast(1).ToArray();
            return new Vector() { Values = temp };
        }



        public static Vector Zero(int dimensions) => new Vector() { Values = new double[dimensions] };
        private static Random random = new Random();
        public static int Seed { set => random = new Random(value); }

        public double[] Values { get; set; }
        public readonly int Count => Values.Length;

        #region Random
        private static double NextNormalRandom(double mean = 0, double stdDev = 1)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }

        /// <summary>
        /// Generates a random vector position using normally distributed 
        /// </summary>
        /// <param name="dimensions">The number of dimensions</param>
        /// <param name="upper">The upper standard deviation of the values generated</param>
        /// <param name="lower">The lower standard deviation of the values generated</param>
        /// <returns>A random vector</returns>
        public static Vector NormalRandom(int dimensions, Vector lower, Vector upper)
        {
            double[] values = new double[dimensions];

            for (int i=0; i<dimensions; i++)
            {
                values[i] = NextNormalRandom() * (upper[i] - lower[i]) + lower[i];
            }

            return new Vector() { Values = values };
        }

        /// <summary>
        /// Generates a random vector position using normally distributed 
        /// </summary>
        /// <param name="dimensions">The number of dimensions</param>
        /// <param name="upper">The upper standard deviation of the values generated</param>
        /// <param name="lower">The lower standard deviation of the values generated</param>
        /// <returns>A random vector</returns>
        public static Vector NormalRandom(int dimensions, double lower = -0.5, double upper = +0.5)
        {
            double[] values = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                values[i] = NextNormalRandom() * (upper - lower) + lower;
            }

            return new Vector() { Values = values };
        }

        /// <summary>
        /// Generates a random vector position using uniform distributed 
        /// </summary>
        /// <param name="dimensions">The number of dimensions</param>
        /// <param name="lowerBound">The upper bound of the values generated</param>
        /// <param name="upperBound">The lower bound of the values generated</param>
        /// <returns></returns>
        public static Vector LinearRandom(int dimensions, Vector lowerBound, Vector upperBound)
        {
            double[] values = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                values[i] = (random.NextDouble() * (upperBound[i] - lowerBound[i])) + lowerBound[i];
            }
            return new Vector() { Values = values };
        }

        /// <summary>
        /// Generates a random vector position using uniform distributed 
        /// </summary>
        /// <param name="dimensions">The number of dimensions</param>
        /// <param name="lowerBound">The upper bound of the values generated</param>
        /// <param name="upperBound">The lower bound of the values generated</param>
        /// <returns></returns>
        public static Vector LinearRandom(int dimensions, double lowerBound, double upperBound)
        {
            double[] values = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                values[i] = (random.NextDouble() * (upperBound - lowerBound)) + lowerBound;
            }
            return new Vector() { Values = values };
        }
        #endregion

        #region Static Mathematics Functions
        public static Vector Hadamard(Vector a, Vector b) => Operate(a, b, (a, b) => (a * b));
        public static Vector Add(Vector a, Vector b)      => Operate(a, b, (a, b) => (a + b));
        public static Vector Sub(Vector a, Vector b)      => Operate(a, b, (a, b) => (a - b));
        public static Vector Divide(Vector a, Vector b)   => Operate(a, b, (a, b) => (a / b));

        public static Vector Hadamard(Vector vector, double value) => Operate(vector, value,  (a, b) => (a * b));
        public static Vector Add(Vector vector, double value)     => Operate(vector, value,  (a, b) => (a + b));
        public static Vector Sub(Vector vector, double value)     => Operate(vector, value,  (a, b) => (a - b));
        public static Vector Divide(Vector vector, double value)  => Operate(vector, value,  (a, b) => (a / b));

        public static Vector Hadamard(Vector vector, int value) => Operate(vector, value, (a, b) => (a * b));
        public static Vector Add(Vector vector, int value) => Operate(vector, value, (a, b) => (a + b));
        public static Vector Sub(Vector vector, int value) => Operate(vector, value, (a, b) => (a - b));
        public static Vector Divide(Vector vector, int value) => Operate(vector, value, (a, b) => (a / b));
        #endregion

        #region operators
        public double this[int i]
        {
            get => Values[i];
            set => Values[i] = value;
        }
        #region addition
        public static Vector operator +(Vector a, Vector b) => Add(a, b);
        public static Vector operator +(Vector a, double b) => Add(a, b);
        public static Vector operator +(double a, Vector b) => Add(b, a);
        public static Vector operator +(Vector a, int b)    => Add(a, b);
        public static Vector operator +(int a, Vector b)    => Add(b, a);
        #endregion

        #region subtraction
        public static Vector operator -(Vector a, Vector b) => Sub(a, b);
        public static Vector operator -(Vector a, double b) => Sub(a, b);
        public static Vector operator -(double a, Vector b) => Sub(b, a);
        public static Vector operator -(Vector a, int b)    => Sub(a, b);
        public static Vector operator -(int a, Vector b)    => Sub(b, a);

        #endregion

        #region multiplication
        public static Vector operator *(Vector a, Vector b) => Hadamard(a, b);
        public static Vector operator *(Vector a, double b) => Hadamard(a, b);
        public static Vector operator *(double a, Vector b) => Hadamard(b, a);
        public static Vector operator *(Vector a, int b)    => Hadamard(a, b);
        public static Vector operator *(int a, Vector b) => Hadamard(b, a);
        #endregion

        #region divide
        public static Vector operator /(Vector a, Vector b) => Divide(a, b);
        public static Vector operator /(Vector a, double b) => Divide(a, b);
        public static Vector operator /(Vector a, int b)    => Divide(a, b);
        #endregion
        #endregion

        #region Operation
        private delegate double Operation<T>(double a , T b) where T : struct;
        private static Vector Operate(Vector vector1, Vector vector2, Operation<double> op)
        {
            if (vector1.Count != vector2.Count) throw new ArgumentException($"{nameof(vector1)}.{nameof(Vector.Count)} and {nameof(vector2)}.{nameof(Vector.Count)} should be the same");
            
            double[] result = new double[vector1.Count];
            for(int i = 0; i < vector1.Count; i++)
            {
                result[i] = op(vector1[i],vector2[i]);
            }
            return new Vector() { Values = result };
        }

        private static Vector Operate<T>(Vector vector1, T value, Operation<T> op) where T:struct
        {
            double[] result = new double[vector1.Count];
            for (int i = 0; i < vector1.Count; i++)
            {
                result[i] = op(vector1[i], value);
            }
            return new Vector() { Values = result };
        }
        #endregion

        public readonly IEnumerator GetEnumerator() => Values.GetEnumerator();
    }
}
