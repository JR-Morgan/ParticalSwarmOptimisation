﻿using AADP.PSO.Initalisaion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AADP.PSO
{
    internal delegate double ParticleCostFunction(double[] design);
    class Particle
    {
        private static double InertialCoefficient { get; set; } = 1 / (2 * Math.Log(2));
        private static double CognativeCoefficient { get; set; } = 0.5f + Math.Log(2);
        private static double SocialCoefficient { get; set; } = 0.5f + Math.Log(2);

        private double[] Design => Vector.ToDesign(Position);
        public Vector Position { get; protected set; }
        public Vector Velocity { get; protected set; }

        #region GlobalBest
        public static double[] GlobalBestDesign => Vector.ToDesign(GlobalBest);
        public static Vector GlobalBest { get; protected set; }
        public static double GlobalBestCost { get; protected set; } = double.MaxValue;
        #endregion

        #region PersonalBest
        public double[] PersonalBestDesign => Vector.ToDesign(PersonalBest);
        public Vector PersonalBest { get; protected set; }
        protected double PersonalBestCost = double.MaxValue;

        private readonly ParticleCostFunction costFunction;
        #endregion


        public Particle(int dimentions, ParticleCostFunction costFunction, IInitalise initalise)
        {
            Velocity = Vector.FromDesign(initalise.Initalise(dimentions + 1)) / 2;
            Position = Vector.FromDesign(initalise.Initalise(dimentions + 1));


            this.costFunction = costFunction;

            EvaluateCost();
        }

        public void Update()
        {
            UpdateVelocity();
            UpdatePosition();
            EvaluateCost();
        }

        private void EvaluateCost()
        {
            double cost = costFunction(Design);

            if (cost < PersonalBestCost)
            {
                PersonalBest = Position;
                PersonalBestCost = cost;

                if (cost < GlobalBestCost)
                {
                    Console.WriteLine(cost);
                    GlobalBest = Position;
                    GlobalBestCost = cost;
                }
            }

           
        }

        private void UpdatePosition()
        {
            
            Position += Velocity;
        }

        private void UpdateVelocity()
        {
            Vector r1 = Vector.NormalRandom(Velocity.Count),
                   r2 = Vector.NormalRandom(Velocity.Count);
            Velocity = InertialCoefficient * Velocity + CognativeCoefficient * r1 * ( PersonalBest - Position) + SocialCoefficient * r2 - (GlobalBest - Position);
        }
    }
}
