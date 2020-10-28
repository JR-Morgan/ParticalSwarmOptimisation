using AADP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADP.PSO.Initalisaion
{ 
    public interface IInitalise
    {
        public double[] Initalise(int numberOfAntennae);
    }
}
