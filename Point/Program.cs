using System.Linq;
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
        string pointString = "("; // Empty string

        for (int i = 0; i < getDim(); i++) // For each dimension, concatenate the dimension value to the return string
        {
            pointString += coord[i]+ ", ";
        }
        pointString = pointString.Substring(0, pointString.Length - 2) + ")";
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
        return point.ToString();// + cutDim.ToString();
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
        maxVal = 99.0f;
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
    // Inserts a point p into the KDTree
    // takes a point p, and the cutting dimension cutDim to start at
    public bool Insert(Point p, int cutDim)
    {
              return Insert(p, ref root, cutDim);
    }
    // Private Insert method
    // Inserts a point p into the KDTree
    // takes a point p, a KDNode x to start at, and the cutting dimension cutDim to start at
    private bool Insert(Point p,ref KDNode x, int cutDim)
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


    //public Delete
    //deletes a point in the tree if found
    //takes a point p as an argurment
    public bool delete(Point p)
    {
        bool found = true;
        delete(p, root, ref found);
        return found;
    }

    //private Delete
    //called by public delete, recursively navigates throughkd tree to find the point returns null if not found
    //deletes the point and finds a suitible replacement in either subtree
    //takes the point p to delete, and the root node as arguments
    private KDNode delete(Point x, KDNode p, ref bool found)

    {
        if (p == null)
        { // fell out of tree?
            found = false;
            return null;
        }
        else if (p.point.Equals(x))
        { // found it
            if (p.right != null)
            { // take replacement from right
                p.point = findMin(p.right, p.cutDim);
                p.right = delete(p.point, p.right, ref found);
            }
            else if (p.left != null)
            { // take replacement from left
                p.point = findMin(p.left, p.cutDim);
                p.right = delete(p.point, p.left, ref found); // move left subtree to right!
                p.left = null; // left subtree is now empty
            }
            else
            { // deleted point in leaf
                p = null; // remove this leaf
            }
        }
        else if (p.inLeftSubtree(x))
        {
            p.left = delete(x, p.left, ref found); // delete from left subtree
        }
        else
        { // delete from right subtree
            p.right = delete(x, p.right, ref found);
        }
        return p;
    }

    // private findMin()
    // used by delete to find replacement for deleted node
    // get min point along dim i
    // takes node p to be deleted and its cutting dimension i as arguments
    private Point findMin(KDNode p, int i)
    { 
        if (p == null) // fell out of tree?
        { 
            return null;
        }
        if (p.cutDim == i)// cutting dimension matches i?
        { 
            if (p.left == null) // no left child?
                return p.point; // use this point
            else
                return findMin(p.left, i); // get min from left subtree
        }
        else// it may be in either side
        { 
            Point q = minAlongDim(p.point, findMin(p.left, i), i);
            return minAlongDim(q, findMin(p.right, i), i);
        }
    }

    // private minAlongDim 
    // return smallerof two points on dimension i
    private Point minAlongDim(Point p1, Point p2, int i)
    { 
        if (p2 == null || p1.Get(i) <= p2.Get(i)) 
            return p1;
        else
            return p2;
    }





    public void Print()     
    {
        int depth = 0;
        int order = 0;

        List<string> levels = new List<string>();
        Print(root, depth, levels, ref order);
        Console.WriteLine();
        int i = 0;
        foreach (string level in levels)
        {
            Console.WriteLine(level);
            i ++;


            if (i % 2 == 0)
            {
                if (i % 4 == 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;
            }
            else
                Console.ResetColor();
            }
        Console.ResetColor();
    }

    
    private void Print(KDNode p, int depth, List<string> levels, ref int order)
    {
        while (levels.Count < (2* (depth + 2)))
        {
            levels.Add("");
        }

        if ((p.left != null)) 
        {
            Print(p.left, depth + 1, levels, ref order);  
        }


        string s = levels.ElementAt(depth*2);
        string s2 = levels.ElementAt((depth * 2)+1);

        levels.RemoveAt(depth*2);
        s = s + String.Concat(Enumerable.Repeat(" ", Math.Abs((order * 12) - s.Length))) +" " + p.ToString();
        s = s + String.Concat(Enumerable.Repeat(" ", Math.Abs(12 - p.ToString().Length)));

        levels.Insert(depth * 2, s);


        levels.RemoveAt(depth * 2 +1);
        s2 = s2 + String.Concat(Enumerable.Repeat(" ", (order * 12) - s2.Length ) ) ;

        if (p.left != null)
            s2 = s2 + "/";
        if (p.right != null)
            s2 = s2 + String.Concat(Enumerable.Repeat(" ", 12)) + "\\";

        levels.Insert((depth * 2) + 1, s2);
        

        order++;

        if ((p.right != null))
        {
            Print(p.right, depth + 1, levels, ref order);
        }

        

    }
}


class Program
{
    static void Main(string[] args)
    {
        Random rnd = new Random();
        KDTree tree = new KDTree(); //create an empty tree using default constructor (max value for a point is 99 for ease of printing)

        Point root = new Point(4);
      
        for (int j = 0; j < 10; j++)//insert 10 points
        {
            Point point = new Point(4);//create an empty point

            for (int i = 0; i < point.getDim(); i++) //set value for all of the points dimensions
                point.Set(i,  (MathF.Round(rnd.NextSingle(), 1) + Convert.ToSingle(rnd.Next(100)))); //set random value for a point

                if(!tree.Insert(point, j % point.getDim())) //insert point 
                    Console.WriteLine("Insertion failed");  //if point can't be inserted display meassage
        }

        Console.WriteLine();
        tree.Print();//print out tree 
        Console.WriteLine();

        Console.WriteLine("see if tree contains a point");
        do
        {
            Point p = new Point(2);  //create empty point
            Console.WriteLine("\n Enter Coordinate one");  //user set custom value to point
            p.Set(0, Convert.ToSingle(Console.ReadLine()));
            Console.WriteLine("Enter Coordinate two");
            p.Set(1, Convert.ToSingle(Console.ReadLine()));


            
            Console.WriteLine(tree.Contains(p));      //test Contains method

            Console.WriteLine();
            Console.WriteLine("any key to continue, q to quit");
        } while (Console.ReadKey().Key != ConsoleKey.Q);
    }
}

