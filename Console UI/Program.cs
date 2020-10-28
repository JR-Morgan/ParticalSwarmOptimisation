using AADP;
using AADP.PSO;
using System;

namespace Console_UI
{
    class Program
    {

        static void Main(string[] args)
        {
            int numberOfAntenae = 3;
            AntennaArray antenna = new AntennaArray(numberOfAntenae, 90);
            Swarm swarm = new Swarm(antenna, TerminateConditions.TimeOut(5000), (uint)(20 + Math.Sqrt(numberOfAntenae)));

            Log result = swarm.StartPSO();

            Console.WriteLine(result.ToString()); 
                
        }
    }
}
