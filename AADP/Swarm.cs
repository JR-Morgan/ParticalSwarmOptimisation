using AADP.PSO;
using AADP.PSO.Initalisaion;
using System;
using System.Collections.Generic;
using System.Text;

namespace AADP
{
    public delegate bool Terminate();
    public class Swarm
    {
        private LinkedList<Particle> particles;
        private Terminate terminate;
        private AntennaArray antenna;

        public Swarm(int dimensions, AntennaArray antenna, Terminate terminalCondition, uint numberOfParticles)
        {
            particles = new LinkedList<Particle>();
            this.terminate = terminalCondition;
            this.antenna = antenna;


            for (int i=0; i<numberOfParticles; i++)
            {
                

                particles.AddLast(new Particle(
                    dimentions: dimensions,
                    costFunction: ParticleCost,
                    initalise: (IInitalise)new RandomInitalise()
                    ));

            }
        }

        private double ParticleCost(double[] design) => antenna.Evaluate(design);

        public Log StartPSO()
        {

            DateTime startTime = DateTime.Now;


            while (!terminate())
            {
                foreach (Particle p in particles) p.Update();
            }

            DateTime endTime = DateTime.Now;

            return new Log()
            {
                GlobalBest = Particle.GlobalBestDesign,
                GlobalBestCost = Particle.GlobalBestCost,
                MsElapsed = (float)endTime.Subtract(startTime).TotalMilliseconds,
            };
        }



    }
}
