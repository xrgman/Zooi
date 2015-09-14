using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApplication1
{
    class FileIO
    {

        public static string saveMeasurements(List<Measurement> measurements, string fileName)
        {
            string filePath = Path.Combine(System.Environment.CurrentDirectory, fileName);
            try
            {
                Stream stream = File.Open(filePath, FileMode.Create);
                using (stream)
                {
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(stream, measurements);
                }
                stream.Close();
            }
            catch(IOException e) //Called when file was unable to create / edit;
            {
                return "ERR";
            }
            return "OK";
        } 

        public static List<Measurement> readMeusurements(string fileName)
        {
            string filePath = Path.Combine(System.Environment.CurrentDirectory, fileName);
            List<Measurement> measurements;
            try
            {
                Stream stream = File.Open(filePath, FileMode.Open);
                using(stream)
                {
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    measurements = (List<Measurement>) bFormatter.Deserialize(stream);
                    stream.Close();
                }

            }
            catch(IOException e)
            {
                return new List<Measurement>();
            }
            return measurements;
        }
    }
}
