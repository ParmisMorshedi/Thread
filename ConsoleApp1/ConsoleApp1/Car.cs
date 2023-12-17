using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Car
    {
        public string Name { get; set; }
        public int Speed { get;  set; }
        public double Distance { get;  set; }
        

        public Car(string name)
        {
            Name = name;
            Speed = 120;
            Distance = 0;
           


        }


    }

   
}

