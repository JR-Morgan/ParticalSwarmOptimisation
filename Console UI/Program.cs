using AADP;
using AADP.PSO;
using System;

namespace Console_UI
{
    class Program
    {

        static void Main(string[] args)
        {
            Log log = PSO(3, 90, TerminateConditions.FixedItterations(100));
            Console.WriteLine(log.ToString());        }


        private static Log PSO(int numberOfAntenae, int steeringAngle, Terminate t)
        {
            AntennaArray antenna = new AntennaArray(numberOfAntenae, steeringAngle);
            Swarm swarm = new Swarm(numberOfAntenae - 1, antenna, t, (uint)(20 + Math.Sqrt(numberOfAntenae)));

            return swarm.StartPSO();

            
        }
    }
}
