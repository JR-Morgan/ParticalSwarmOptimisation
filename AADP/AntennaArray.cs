using System;
using System.Collections.Generic;

namespace AADP
{
    /*
     * Class converted from AntennaArray.java
     */
    public class AntennaArray
    {
        /** Minimum spacing permitted between antennae. */
        public static readonly double MIN_SPACING = 0.25;

        /**
         * Construct an antenna design problem.
         * @param n_ant Number of antennae in our array.
         * @param steering_ang Desired direction of the main beam in degrees.
         */
        public AntennaArray(int n_ant, double steering_ang)
        {
            n_antennae = n_ant;
            steering_angle = steering_ang;
        }
        /**
         * Rectangular bounds on the search space.
         * @return Vector b such that b[i][0] is the minimum permissible value of the
         * ith solution component and b[i][1] is the maximum.
         */
        public double[][] Bounds()
        {
            double[][] bnds = new double[n_antennae][];
            double[] dim_bnd = { 0.0, ((double)n_antennae) / 2.0 };
            for (int i = 0; i < n_antennae; ++i)
                bnds[i] = dim_bnd;
            return bnds;
        }
        /**
         * Check whether an antenna design lies within the problem's feasible
         * region.
         * A design is a vector of n_antennae anntena placements.
         * A placement is a distance from the left hand side of the antenna array.
         * A valid placement is one in which
         *   1) all antennae are separated by at least MIN_SPACING
         *   2) the aperture size (the maximum element of the array) is exactly
         *      n_antennae/2.
         */
        public bool Is_valid(double[] design)
        {
            if (design.Length != n_antennae) return false;
            double[] des = new double[design.Length];
            Array.Copy(design, 0, des, 0, design.Length);
            Array.Sort(des);
            

            //Aperture size is exactly n_antennae/2
            if (Math.Abs(des[^1] - ((double)n_antennae) / 2.0) > 1e-10)
                return false;
            //All antennae lie within the problem bounds
            for (int i = 0; i < des.Length - 1; ++i)
                if (des[i] < Bounds()[i][0] || des[i] > Bounds()[i][1])
                    return false;
            //All antennae are separated by at least MIN_SPACING
            for (int i = 0; i < des.Length - 1; ++i)
                if (des[i + 1] - des[i] < MIN_SPACING)
                    return false;
            return true;
        }



        private class PowerPeak
        {
            public double elevation;
            public double power;

            public PowerPeak(double e, double p)
            {
                elevation = e;
                power = p;
            }
        }

        /**
         * Evaluate an antenna design returning peak SSL.
         * Designs which violate problem constraints will be penalised with extremely
         * high costs.
         * @param design A valid antenna array design.
         */
        public double Evaluate(double[] design)
        {
            if (design.Length != n_antennae)
                throw new Exception(
                        "AntennaArray::evaluate called on design of the wrong size. Expected: " + n_antennae +
                        ". Actual: " +
                        design.Length
                );
            if (!Is_valid(design)) return Double.MaxValue;



            //Find all the peaks in power
            List<PowerPeak> peaks = new List<PowerPeak>();
            PowerPeak prev = new PowerPeak(0.0, Double.MinValue);
            PowerPeak current = new PowerPeak(0.0, Array_factor(design, 0.0));
            for(double elevation = 0.01; elevation <= 180.0; elevation += 0.01){
                PowerPeak next = new PowerPeak(elevation, Array_factor(design, elevation));
                if(current.power >= prev.power && current.power >= next.power)
                    peaks.Add(current);
                prev = current;
                current = next;
            }
            peaks.Add(new PowerPeak(180.0, Array_factor(design,180.0)));

            peaks.Sort((PowerPeak l, PowerPeak r) => l.power > r.power ? -1 : 1);

            //No side-lobes case
            if(peaks.Count<2) return Double.MinValue;
            //Filter out main lobe and then return highest lobe level
            double distance_from_steering = Math.Abs(peaks[0].elevation - steering_angle);
            for(int i=1;i<peaks.Count;++i)
                if(Math.Abs(peaks[i].elevation - steering_angle) < distance_from_steering)
                    return peaks[0].power;

            return peaks[1].power;
        }

        private int n_antennae;
        private double steering_angle;

        private double Array_factor(double[] design, double elevation)
        {
            double steering = 2.0 * Math.PI * steering_angle / 360.0;
            elevation = 2.0 * Math.PI * elevation / 360.0;
            double sum = 0.0;
            foreach (double x in design)
            {
                sum += Math.Cos(2 * Math.PI * x * (Math.Cos(elevation) - Math.Cos(steering)));
            }
            return 20.0 * Math.Log(Math.Abs(sum));
        }
    }

}
