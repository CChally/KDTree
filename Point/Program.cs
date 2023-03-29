// Point Class
    // Represents a point in the n-th dimension
    public class Point
    {
        private float[] coord { get; set; } // Coordinate storage 

        // Constructor
        // Creates a zero point in the specified dimension
        public Point(int dim)
        {
            coord = new float[dim]; // Create array to store 
        }

        // getDim
        // Returns the dimension the point is in
        public int getDim()
        {
            return coord.Length;
        }

        // Get
        // Returns the i-th dimension value in a coordinate
        public float Get(int i)
        {
            return coord[i];
        }

        // Set
        // Sets the i-th dimension value in a cordinate
        public void Set(int i, float value)
        {
            coord[i] = value;
        }

        // distanceTo
        // Returns the Euclidean distance between two points in the nth-dimensions
        public float distanceTo(Point other)
        {
            double distance = 0;

            for (int i = 0; i < getDim(); i++) // For each dimension
            {
                distance += Math.Pow(other.coord[i] - this.coord[i], 2);
            }
            return (float)Math.Sqrt(distance);
        }

        // Equals Override
        // Returns if a point in k-th dimensional space 
        public override bool Equals(Object? obj)
        {
            if (obj != null) // Null check
            {
                Point p = (Point)obj; // Downcast 
                for (int i = 0; i < getDim(); i++) // For each dimension
                {
                    if (this.coord[i] != p.coord[i]) return false; // Compare values
                }
                return true;
            }
            return false; // Null
        }

        // ToString Override
        // Converts the point to a string
        public override string ToString()
        {
            string pointString = "( "; // Empty string

            for (int i = 0; i < getDim(); i++) // For each dimension, concatenate the dimension value to the return string
            {
                pointString += coord[i]+ " ";
            }
            pointString += ")";
            return pointString;
        }


    }


    class KDNode
    { // node in a kd-tree
        public Point point; // splitting point
        public int cutDim; // cutting dimension
        public KDNode left; // children
        public KDNode right;

        public KDNode(Point point, int cutDim)
        { // constructor
            this.point = point;
            this.cutDim = cutDim;
            left = right = null;
        }
        public bool inLeftSubtree(Point x)
        { // is x in left subtree?
            bool whodatis = x.Get(cutDim) < point.Get(cutDim);
            Console.WriteLine(whodatis);    
            return whodatis;
        }

        public string toString()
        {
            return "(" + cutDim.ToString() +" "+ point.ToString() + ")";
        }
    }

    class KDTree
    {
        public KDNode root;
        float maxVal;

        public KDTree()
        {
            this.root = null;
            maxVal = 100;
        }

        public KDTree(KDNode root, int max)
        {
            this.root = root;
            maxVal = max;
        }

        public bool Insert(Point p, KDNode x, int cutDim)
        {
            
            if (x == null)
            { // fell out of tree
                x = new KDNode(p, cutDim); // create new leaf
            }
            else if (x.point.Equals(p))
            {
                return false;
            }
            else if (x.inLeftSubtree(p))
            { // insert into left subtree
                Insert(p, x.left, (x.cutDim + 1) % p.getDim());
                Console.WriteLine(x.cutDim +1 % p.getDim());
            }
            else
            { // insert into right subtree
                Insert(p, x.right, (x.cutDim + 1) % p.getDim());
                Console.WriteLine(x.cutDim + 1 % p.getDim());
            }

            return true;
            
        }

        public bool Delete(Point p)
        {
            return true;
        }

        public bool Contains(Point p)
        {
            return true;
        }

        bool FindMin()
        {
            return true;
        }

        public void Print()
        {
            Print(root, 4);
        }

        public void Print(KDNode p, int padding)
        {
            if (p != null)
            {
                if (p.right != null)
                {
                    Print(p.right, padding + 4);
                }
                if (padding > 0)
                {
                    Console.Write(" ".PadLeft(padding));
                }
                if (p.right != null)
                {
                    Console.Write("/\n");
                    Console.Write(" ".PadLeft(padding));
                }
                Console.Write(p.point.ToString() + "\n ");
                if (p.left != null)
                {
                    Console.Write(" ".PadLeft(padding) + "\\\n");
                    Print(p.left, padding + 4);
                }
            }
        }
    }
class Program
{
    static void Main(string[] args)
    {
        Random rnd = new Random();
        KDTree tree = new KDTree();

        for (int j = 0; j < 10; j++)
        {
            Point point = new Point(3);

            for (int i = 0; i < point.getDim(); i++)
                point.Set(i,  (rnd.NextSingle() + Convert.ToSingle(rnd.Next(100))));
            Console.WriteLine(point.ToString());    
            Console.WriteLine(tree.Insert(point, tree.root, j % point.getDim()));
        }


        //tree.Print();
        Console.ReadKey();
    }
}

