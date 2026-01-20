using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PhysicProject.Models
{
    public class Params
    {
        public double Mass_m { get; set; } 
        public double Rigidity_k { get; set; }
        public double Period_T { get; set; }
        public double Frequency_v { get; set; }
        public double C_frequency_w { get; set; }
        public float Amplitude_A { get; set; }
        public float[] Function_data { get; set; }
        public int Function_data_l { get; set; }
        public bool Bat { get; set; } = false;   









    }
}
