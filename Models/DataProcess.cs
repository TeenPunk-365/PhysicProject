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

           

  

        }
        public static void Solve(Params param1)
        {
            int N = 200;
            float h = param1.h;
            float beta = param1.beta;

            // Инициализация массивов
            float[] U = new float[N];      // текущий слой (i)
            float[] U_prev = new float[N]; // предыдущий слой (i-1)
            float[] U_next = new float[N]; // следующий слой (i+1)

            // Начальное условие: два солитона
            float x0_1 = (float)(N * h * 0.3);
            float x0_2 = (float)(N * h * 0.7);

            for (int j = 0; j < N; j++)
            {
                float x = j * h;

                // Первый солитон (больший)
                float sechArg1 = (float)(Math.Sqrt(1.0 / (2.0 * beta * 0.0001)) * (x - x0_1));
                float soliton1 = (float)(12.0 * beta * 0.0001 * Math.Pow(1.0 / Math.Cosh(sechArg1), 2));

                // Второй солитон (меньший)
                float sechArg2 = (float)(Math.Sqrt(0.5 / (2.0 * beta * 0.0001)) * (x - x0_2));
                float soliton2 = (float)(6.0 * beta * 0.0001 * Math.Pow(1.0 / Math.Cosh(sechArg2), 2));

                U[j] = soliton1 + soliton2;
                U_prev[j] = U[j];
                param1.U[j] = U[j];
                param1.U_prev[j] = U[j];
            }

        }
        public static void GraphWave(Params param1)
        {
            // Выполняем шаги по времени
            int numSteps = 90;
            int N = 200;
            float h = param1.h;
            float tau = param1.tau;
            float beta = param1.beta;
            float[] U = param1.U;
            float[] U_prev = param1.U_prev;
            float[] U_next = new float[N];
            for (int step = 0; step < numSteps; step++)
            {
                // Формула 4.8: U_j^{i+1} = U_j^{i-1} - (τ/h) U_j^i (U_{j+1}^i - U_{j-1}^i) 
                //               - β(τ/h³) (U_{j+2}^i - 2U_{j+1}^i + 2U_{j-1}^i - U_{j-2}^i)

                float coeff1 = (float)(tau * 0.0001 / h);
                float coeff2 = (float)(beta * 0.0001 * tau * 0.0001 / (h * h * h));

                for (int j = 2; j < N - 2; j++)
                {
                    float term1 = U[j] * (U[j + 1] - U[j - 1]);
                    float term2 = U[j + 2] - 2 * U[j + 1] + 2 * U[j - 1] - U[j - 2];

                    U_next[j] = U_prev[j] - coeff1 * term1 - coeff2 * term2;
                }

                // Периодические граничные условия
                U_next[0] = U_next[N - 4];
                U_next[1] = U_next[N - 3];
                U_next[N - 2] = U_next[2];
                U_next[N - 1] = U_next[3];
                // Сдвиг временных слоев
                Array.Copy(U, U_prev, N);
                Array.Copy(U_next, U, N);
            }

            param1.U = U;
            param1.N = N;

          
        }
    }
}
