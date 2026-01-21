using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace PhysicProject.Models
{
    internal class DataProcess
    {
   

        public void Calculate(Params param1)
        {
            
            double mass_m = param1.Mass_m;
            double rigidity_k = param1.Rigidity_k; 
             
            double period_T = 2 * Math.PI * Math.Sqrt(mass_m / rigidity_k);
            double frequency_v = 1.0 / period_T;
            double c_frequency_w = 2 * Math.PI * frequency_v;
            param1.Period_T = period_T;
            param1.Frequency_v = frequency_v;
            param1.C_frequency_w = c_frequency_w;

            



        }
        public void Graphs(Params param1)
        {
            

            // График функции на основе слайдера
         
            float[] functionData = new float[50];
            
            for (int i = 0; i < functionData.Length; i++)
            {

                float x = (float)(param1.C_frequency_w)*i;
                functionData[i] = (float)(param1.Amplitude_A * Math.Cos(x)  * 0.5 + 50.0);
               
            }
            param1.Function_data = functionData;
            param1.Function_data_l = functionData.Length;

           

            ImGui.Spacing();

        }
    }
}
