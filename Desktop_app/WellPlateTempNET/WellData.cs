using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellPlateTempNET
{
    internal class WellData
    {
        public string Coordinates { get; set; }
        public List<double> TempValues { get; set; }

        public WellData(string coordinates)
        {
            Coordinates = coordinates;
            TempValues = new List<double>();
        }

        public String printRow()
        {
            string combinedString = "";
            if (TempValues.Any())
            {
                combinedString = string.Join(",", TempValues);
                return Coordinates + "," + TempValues.Average() + "," + CalculateStandardDeviation().ToString() + "\r\n";
            }
            else { return Coordinates + ",0,0" + "\r\n"; }
        }

        public double CalcTempAvg()
        {
            if (TempValues.Any())
            {
                return TempValues.Average();
            }
            else return 0;
        }

        public double CalculateStandardDeviation()
        {
            double standardDeviation = 0;
            IEnumerable<double> values = TempValues;
            if (values.Any())
            {
                // Compute the average.     
                double avg = values.Average();

                // Perform the Sum of (value-avg)_2_2.      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));

                // Put it all together.      
                standardDeviation = Math.Sqrt((sum) / (values.Count() - 1));
            }

            return standardDeviation;
        }
    }
}
