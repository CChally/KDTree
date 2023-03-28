using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Point
{
    class Point
    {
        private float[] coord; // coordinate storage
        private int Dim;

        public Point(int dim) 
        {
            coord = new float[dim];
            Dim = dim;
        }

        public int getDim() 
        { 
            return Dim; 
        }

        public float get(int i) 
        { 
            return coord[i]; 
        }

        public void set(int i, float x) 
        { 
            coord[i] = x; 
        }

        public bool equals(Point other) 
        { 
            for(int i = 0; i < Dim; i++)
            {
                if (coord[i] != other.coord[i])
                    return false;
            }
            return true;
        }

        public float distanceTo(Point other)
        {
            double x = 0;
            for (int i = 0;i < Dim; i++)
            {
                x += (Math.Pow(Convert.ToDouble(coord[i]), 2.0));
            }
            x = Math.Sqrt(x);
            return Convert.ToSingle(x);
        }

        public String toString() 
        {
            string str = "(";
            for(int i = 0; i < Dim;i++)
                str += (coord[i].ToString() + ", ");

            str = str.Substring(0, str.Length - 2);
            str += ")";

            return str;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            Point point = new Point(50);

            for (int i = 0; i < point.getDim(); i++)
                point.set(i, (rnd.NextSingle() + Convert.ToSingle(rnd.Next(100))));

            Console.WriteLine(point.toString());
        }
    }
}
