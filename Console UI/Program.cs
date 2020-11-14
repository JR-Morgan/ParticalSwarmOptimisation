using AADP;
using AADP.PSO;
using System;

namespace Console_UI
{
    class Program
    {

        /// <param name="args">
        /// 0 - numberOfAntenae : int
        /// 1 - steeringAngle : double
        /// 2 - iterations : int
        /// </param>
        static void Main(string[] args)
        {
            int numberOfAntenae = 3;
            double steeringAngle = 90;
            int numberOfItterations = 100;
            if (args.Length >= 2)
            {
                numberOfAntenae = int.Parse(args[0]);
                steeringAngle = double.Parse(args[1]);
                if(args.Length >= 3)
                {
                    numberOfItterations = int.Parse(args[2]);
                }
            }


            Log log = PSO(numberOfAntenae, steeringAngle, TerminateConditions.FixedItterations(numberOfItterations));
            Console.WriteLine(log.ToString());
        }


        private static Log PSO(int numberOfAntenae, double steeringAngle, Terminate t)
        {
            AntennaArray antenna = new AntennaArray(numberOfAntenae, steeringAngle);
            Swarm swarm = new Swarm(numberOfAntenae - 1, antenna, t, (uint)(20 + Math.Sqrt(numberOfAntenae)));

            return swarm.StartPSO();
        }
    }
}
