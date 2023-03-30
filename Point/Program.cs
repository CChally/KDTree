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


        // constructor
        public KDNode(Point point, int cutDim)
        { 
            this.point = point;
            this.cutDim = cutDim;
            left = right = null;
        }

        // is x in left subtree?
        public bool inLeftSubtree(Point x)
        { 
            return x.Get(cutDim) < point.Get(cutDim);   
        }

        //Override ToString()
        public override string ToString()
        {
            return "(" +" "+ point.ToString() + ")" + cutDim.ToString();
        }
    }

    class KDTree
    {
        public KDNode root;
        float maxVal;

        // default constructor
        //root is null and max float value of a point is 100
        public KDTree()
        {
            this.root = null;
            maxVal = 100.0f;
        }

        //overloaded constructor 1
        //custom root and custom max point value
        public KDTree(KDNode root, int max)
        {
            this.root = root;
            maxVal = max;
        }


        //overloaded cuntstructor 2
        //custom max point value
        public KDTree(int max)
        {
            this.root = null;
            maxVal = max;
        }


        // Insert method
        //Inserts a point p into the KDTree
        // takes a point p, a KDNode x to start at, and the cutting dimension cutDim to start at
        public bool Insert(Point p,ref KDNode x, int cutDim)
        {
            for (int i = 0; i < p.getDim(); i++)   //Check if p's values are all less than the max value set for the tree
            {
                if (p.Get(i) > maxVal)
                    return false;
            }

            if (x == null)  //reached the bottom of the tree
            {
                x = new KDNode(p, cutDim); // create new leaf
            }
            else if (x.point.Equals(p)) //found a matching point in the tree
            {
                return false;
            }
            else if (x.inLeftSubtree(p))  // insert into left subtree
            { 
                return Insert(p, ref x.left, (x.cutDim + 1) % p.getDim());
            }
            else  // insert into right subtree
            { 
                return Insert(p, ref x.right, (x.cutDim + 1) % p.getDim());
            }

            return true; //if bottom of method reached point succsessfully inserted
            
        }

        public bool Delete(Point p)
        {
            return true;
        }

        //First Contains() method called by user
        //takes a Point p and returns true if point is in the tree otherwise false
        public bool Contains(Point p)   
        {
            return Contains(p, root);
        }

        //Second Contains() method called by first method^^^^
        //takes the point from the first method and the root of the tree as arguments
        //returns true if point in tree otherwise false
        private bool Contains(Point p, KDNode root)
        {
            if (root.point.Equals(p))       //if point is in tree return true
                return true;
            else if (root.inLeftSubtree(p) && root.left != null )  //check left subtree if not null
                return Contains(p, root.left);
            else if(root.right != null)                 //check right subtree if not null
                return Contains(p,root.right);  
            else
                return false;       //if nothing found return false
        }

        bool FindMin()
        {
            return true;
        }

        public void Print()     //not mine found on stack ofverflow
        {
            int depth = 0;
            Print(root, 1, depth);
        }

        public void Print(KDNode p, int padding, int depth)
        {
            if (p != null)
            {
                if (p.right != null)
                {
                    Print(p.right, padding + 4, depth+1);
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
                Console.Write(depth + p.ToString() + "\n ");
                if (p.left != null)
                {
                    Console.Write(" ".PadLeft(padding) + "\\\n");
                    Print(p.left, padding + 4, depth + 1);
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
            Point point = new Point(2);

            for (int i = 0; i < point.getDim(); i++)
                point.Set(i,  (MathF.Round(rnd.NextSingle(), 3) + Convert.ToSingle(rnd.Next(150))));
                if(!tree.Insert(point, ref tree.root, j % point.getDim()))
                    Console.WriteLine("Insertion failed");
        }
        Console.WriteLine();
        tree.Print();
        Console.WriteLine();

        Console.WriteLine("check if tree contains a point");
        do
        {
            Point p = new Point(2);
            Console.WriteLine("Enter Coordinates one");
            p.Set(0, Convert.ToSingle(Console.ReadLine()));
            Console.WriteLine("Enter Coordinates two");
            p.Set(1, Convert.ToSingle(Console.ReadLine()));
            Console.WriteLine(tree.Contains(p));
            Console.WriteLine("any key to continue");
        } while (Console.ReadKey().Key != ConsoleKey.Q);
    }
}

