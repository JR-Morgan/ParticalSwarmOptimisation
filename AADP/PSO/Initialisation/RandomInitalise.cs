using System;
using System.Security.Cryptography.X509Certificates;

namespace AADP.PSO.Initalisaion
{
    class RandomInitalise : IInitalise
    {
        private static Random random = new Random();
        public static int Seed { set => random = new Random(value); }

        /// <summary>
        /// Returns a randomly generated valid design
        /// </summary>
        /// <param name="numberOfAntennae"></param>
        /// <returns></returns>
        public double[] Initalise(int numberOfAntennae)
        {
            
            var AntennaArray = new AntennaArray(numberOfAntennae, 0d);

            return GenerateDesign(numberOfAntennae, AntennaArray, random);

        }



        private static double[] GenerateDesign(int numberOfAntennae, AntennaArray antenna, Random random, int counter = 0)
        {
            if (counter > 50) throw new Exception("Timed out trying to Generate a valid design");

            double[] design = new double [numberOfAntennae];

            double[][] bounds = antenna.Bounds();
            for (int i = 0; i < numberOfAntennae -1; i++)
            {
                design[i] = random.NextDouble() * (bounds[i][1] - bounds[i][0]) + bounds[i][0];
            }
            design[^1] = numberOfAntennae / 2d;
            return antenna.Is_valid(design) ? design : GenerateDesign(numberOfAntennae, antenna, random, ++counter);
        }
    }
}
