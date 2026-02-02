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
        public float[] Function_data { get; set; } = new float[50];
        public int Function_data_l { get; set; }
        public bool Bat { get; set; } = false;

        //Wave
        public int N { get; set; } = 200;
        public float h { get; set; } = 0.01f;
        public float tau { get; set; } = 13f;
        public float beta { get; set; } = 3f;
        public float[] U { get; set; } = new float[200];
        public float[] U_prev { get; set; } = new float[200];
        public float[] U_next { get; set; } = new float[200];









    }
}
