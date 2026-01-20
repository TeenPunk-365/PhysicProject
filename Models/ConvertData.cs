using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhysicProject.Models
{
    internal class ConvertData
    {
        public static double DataToDouble(string num1)
        {
            try
            {
                if (num1 != null)
                {
                    return double.Parse(num1);
                }
                return double.NaN;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка: Числа введены некорректно!");
                return 0;

            }
        }
        
    }
}
