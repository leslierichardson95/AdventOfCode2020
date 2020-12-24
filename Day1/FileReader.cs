using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    class FileReader
    {
        private StreamReader sr;

        public FileReader (string filePath)
        {
            sr = new StreamReader(filePath);
        }

        public List<int> ConvertToIntArray()
        {
            List<int> arr = new List<int>();
            string line = sr.ReadLine();

            while (line != null)
            {
                int number = Int32.Parse(line);
                arr.Add(number);
                line = sr.ReadLine();
            }
            return arr;
        }

        public void PrintFile()
        {
            string text = sr.ReadToEnd();
            Console.Write(text);
        }
    }
}
