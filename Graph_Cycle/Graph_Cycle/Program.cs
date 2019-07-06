

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


class Program
{
    //  Graph modelled as list of edges
    public static int[] base_cycle = { 0, 1, 2, 3};
    public static List<int> cycle_clockwise = new List<int> { 0, 1, 2, 4, 3};
    public static List<int[]> tempcy;
    

    static List<int[]> cycles = new List<int[]>();
    
    //this is 2D List initialization
    static List<List<int>> g = new List<List<int>>
    {   new List<int> { 0,1},
        new List<int> { 1,2},
        new List<int> { 2,3},
        new List<int> { 3,0},
        new List<int> { 0,4},
        new List<int> { 1,4},
        new List<int> { 2,4},
        new List<int> { 3,4}
    
    };
    public static void Insert_Path(int x, int y)
    {
        g.Add(new List<int> { x, y });

    }
    static void Main(string[] args)
    {
    

        for (int i = 0; i < g.Count; i++)
            for (int j = 0; j < g[i].Count; j++)
            {
                //Console.WriteLine("graph[{0},{1}]  {2}", i, j, graph[i, j]);
                findNewCycles(new int[] { g[i][j] });

            }

        foreach (int[] cy in cycles)
        {
            string s = "" + cy[0];

            for (int i = 1; i < cy.Length; i++)
                s += "," + cy[i];


            Console.WriteLine(s);

        }
        Console.WriteLine("Lenght of cycles is " + cycles.Count + "\n\n.......After Ascending order.....");
    
        Find_Circuit(cycles);
    
    }

    static void findNewCycles(int[] path)
    {
        //Console.WriteLine("pathlength {0}", path.Length);
        int n = path[0];
        int x;
        int[] sub = new int[path.Length + 1];

        for (int i = 0; i < g.Count; i++)
            for (int y = 0; y <= 1; y++)
                if (g[i][y] == n)
                //  edge referes to our current node
                {
                    x = g[i][(y + 1) % 2];
                    if (!visited(x, path))
                    //  neighbor node not on path yet
                    {
                        sub[0] = x;
                        Array.Copy(path, 0, sub, 1, path.Length);

                        //  explore extended path
                        findNewCycles(sub);
                    }
                    else if ((path.Length > 2) && (x == path[path.Length - 1]))
                    //  cycle found
                    {
                        int[] p = normalize(path);
                        int[] inv = invert(p);
                        if (isNew(p) && isNew(inv))
                            cycles.Add(p);
                    }
                }
    }

    static bool equals(int[] a, int[] b)
    {
        bool ret = (a[0] == b[0]) && (a.Length == b.Length);

        for (int i = 1; ret && (i < a.Length); i++)
            if (a[i] != b[i])
            {
                ret = false;
            }

        return ret;
    }

    static int[] invert(int[] path)
    {
        int[] p = new int[path.Length];

        for (int i = 0; i < path.Length; i++)
            p[i] = path[path.Length - 1 - i];

        return normalize(p);
    }

    //  rotate cycle path such that it begins with the smallest node
    static int[] normalize(int[] path)
    {
        int[] p = new int[path.Length];
        int x = smallest(path);
        int n;

        Array.Copy(path, 0, p, 0, path.Length);

        while (p[0] != x)
        {
            n = p[0];
            Array.Copy(p, 1, p, 0, p.Length - 1);
            p[p.Length - 1] = n;
        }

        return p;
    }

    static bool isNew(int[] path)
    {
        bool ret = true;

        foreach (int[] p in cycles)
            if (equals(p, path))
            {
                ret = false;
                break;
            }

        return ret;
    }

    static int smallest(int[] path)
    {
        int min = path[0];

        foreach (int p in path)
            if (p < min)
                min = p;

        return min;
    }

    static bool visited(int n, int[] path)
    {
        bool ret = false;

        foreach (int p in path)
            if (p == n)
            {
                ret = true;
                break;
            }

        return ret;
    }

    static void Find_Circuit(List<int[]> cy)
    {
        for (int i = 0; i < cy.Count; i++)
        {
            for (int j = i + 1; j < cy.Count; j++)
            {
                if (cy[i].Length > cy[j].Length)
                { //then swap
                    int[] temp = cy[i];
                    cy[i] = cy[j];
                    cy[j] = temp;
                }
            }

        }

        //print all ascending cycles acc. to cycles lenght
        for (int i = 0; i < cycles.Count; i++)
        {
            string s = "" + cycles[i][0];
            //string s = "";
            for (int j = 1; j < cycles[i].Length; j++)
            {
                s += "," + cycles[i][j];



            }
            Console.WriteLine(s);
        }
        Find_RealCircuit(cycles);
    }

    static void Find_RealCircuit(List<int[]> ck)
    {
        tempcy = ck;



        if (tempcy.Count >= 3)
        {
            for (int i = 0; i < tempcy.Count - 1; i++)
            {
                IEnumerable<int> union = tempcy[i].Union(tempcy[i + 1]);
                var un = union.ToArray();
                for (int j = i + 2; j < tempcy.Count; j++)
                {
                    var unique_list = un.Except(tempcy[j]).Concat(tempcy[j].Except(un));
                    var count_unique_list = unique_list.ToArray();
                    if (count_unique_list.Length == 0)
                    {
                        tempcy.Remove(tempcy[j]);
                    }

                }

            }

            for (int i = 0; i < tempcy.Count - 1; i++)
            {
                for (int k = 0; k < tempcy.Count - 1; k++)
                {
                    IEnumerable<int> union = tempcy[i].Union(tempcy[k + 1]);
                    var un = union.ToArray();
                    for (int j = i + 2; j < tempcy.Count; j++)
                    {
                        var unique_list = un.Except(tempcy[j]).Concat(tempcy[j].Except(un));
                        var count_unique_list = unique_list.ToArray();
                        if (count_unique_list.Length == 0)
                        {
                            tempcy.Remove(tempcy[j]);
                        }

                    }
                }



            }



        }
        Console.WriteLine("\nMain_List_Graph");
        //Delete the base size cycle which has occured
        for (int i = 0; i < tempcy.Count; i++)
        {
            var unique_list_base = tempcy[i].Except(base_cycle).Concat(base_cycle.Except(tempcy[i]));
            var count_unique_list_base = unique_list_base.ToArray();
            if (count_unique_list_base.Length == 0)
            {
                tempcy.Remove(tempcy[i]);

            }
        }



        for (int i = 0; i < tempcy.Count; i++)
        {
            string s = "" + tempcy[i][0];
            //string s = "";
            for (int j = 1; j < cycles[i].Length; j++)
            {
                s += "," + tempcy[i][j];
            }
            Console.WriteLine(s);
        }


    }

    public static void Rotate_clockwise()
    {
        for (int i = 0; i < tempcy.Count; i++)
        {
            for (int j = 0; j < tempcy.Count - 2; j++)
            {
                if (cycle_clockwise.IndexOf(tempcy[i][j]) > cycle_clockwise.IndexOf(tempcy[i][j + 1]))
                {
                    //then swap
                    int temp = tempcy[i][j];
                    tempcy[i][j] = tempcy[i][j + 1];
                    tempcy[i][j + 1] = temp;

                }
            }
        }
    }
}//end of Class Program





