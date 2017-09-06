using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character: MonoBehaviour {

    //Base status
    public int Health;
    public int Strength;
    public int Intelligence;
    public int Defense;
    public int Resistence;
    public int Stamina;
    public int Move;

    // Utilities 
    public int posX, posY;
    protected int[] dist;
    protected Vertex<Tile>[] prev;

    /*
    public abstract void Move(int x, int y);
    public abstract void Attack(GameObject obj);
    public abstract void UndoMove();
    public abstract void Finish();

    public abstract void CheckStatus();
    */

    //Storing operands involving the calculation of status.
    public List<object> HPOperand { get; set; }
    public List<object> StrOperand { get; set; }
    public List<object> IntOperand { get; set; }
    public List<object> DefOperand { get; set; }
    public List<object> ResOperand { get; set; }
    public List<object> StaminaOperand { get; set; }

    //The modified health value need to be kept after each battle
    private int health;

    //Calculated status. The status used in real battle.
    public int CHealth { get; set; }
    public int CStr { get; set; }
    public int CInt { get; set; }
    public int CDef { get; set; }
    public int CRes { get; set; }
    public int CStamina { get; set; }
    public int Range { get; set; }

    //Evasion Percentage
    public float Evade { get; set; }

    //Special buffs/debuffs
    public bool Guts { get; set; }
    public bool LastStand { get; set; }
    public bool JustBlock { get; set; }
    public bool Exhausted { get; set; }

    void Start()
    {
        RestoreDefaultStatus();        
    }

    public void RestoreDefaultStatus()
    {
        health = Health;

        CHealth = health;
        CStr = Strength;
        CInt = Intelligence;
        CDef = Defense;
        CRes = Resistence;
        CStamina = Stamina;

        HPOperand = new List<object>();
        StrOperand = new List<object>();
        IntOperand = new List<object>();
        DefOperand = new List<object>();
        ResOperand = new List<object>();
        StaminaOperand = new List<object>();
        Evade = 0f;
        Guts = LastStand = JustBlock = false;
    }

    public void StatusCalculation()
    {
        //Calculate Health
        CHealth = health;
        foreach (object obj in HPOperand)
        {
            if (obj.GetType().Equals(typeof(int)))
                CHealth += (int)obj;
            else if (obj.GetType().Equals(typeof(float)))
                CHealth = (int)(CHealth * ((float)obj));
        }

        if (CHealth > Health)
            CHealth = Health;

        //Calculate STR
        CStr = Strength;
        foreach (object obj in StrOperand)
        {
            if (obj.GetType().Equals(typeof(int)))
                CStr += (int)obj;
            else if (obj.GetType().Equals(typeof(float)))
                CStr = (int)(CStr * ((float)obj));
        }

        //Calculate INT
        CInt = Intelligence;
        foreach (object obj in IntOperand)
        {
            if (obj.GetType().Equals(typeof(int)))
                CInt += (int)obj;
            else if (obj.GetType().Equals(typeof(float)))
                CInt = (int)(CInt * ((float)obj));
        }

        //Calculate DEF
        CDef = Defense;
        foreach (object obj in DefOperand)
        {
            if (obj.GetType().Equals(typeof(int)))
                CDef += (int)obj;
            else if (obj.GetType().Equals(typeof(float)))
                CDef = (int)(CDef * ((float)obj));
        }

        //Calculate Res
        CRes = Resistence;
        foreach (object obj in ResOperand)
        {
            if (obj.GetType().Equals(typeof(int)))
                CRes += (int)obj;
            else if (obj.GetType().Equals(typeof(float)))
                CRes = (int)(CRes * ((float)obj));
        }

        //Calculate Stamina
        CStamina = Stamina;
        foreach (object obj in StaminaOperand)
        {
            if (obj.GetType().Equals(typeof(int)))
                CStamina += (int)obj;
            else if (obj.GetType().Equals(typeof(float)))
                CStamina = (int)(CStamina * ((float)obj));
        }

        if (CStamina < 0)
            Exhausted = true;
        else
            Exhausted = false;
    }

    protected void Dijkstra(WeightedGraph<Tile> graph, Vertex<Tile> origin)
    {
        /*
         *  Dijkstra's Algorithm with some small modification. 
         *  Reason not using A* is:
         *  1. Player need to know the movable range regardless the target;
         *  2. Enemy AI need to know if some players are in its range or not
         *     to decide the target.
         *  So basically, knowing the distance from origin to all 
         *  tiles is the first thing to do.
         */

        //Initialize arrays storing distance from origin and the previous 
        //step of each vertex in the path from origin to such vertex
        dist = new int[graph.Vertices.Count];
        prev = new Vertex<Tile>[graph.Vertices.Count];

        //An array marking which vertex has already been checked
        int[] tempDist = new int[graph.Vertices.Count];

        List<Vertex<Tile>> vertexList = new List<Vertex<Tile>>();

        //Set all initial distance to obviously large number; and
        //Set all initial prev tiles to null.
        foreach (Vertex<Tile> v in graph.Vertices)
        {
            dist[graph.Vertices.IndexOf(v)] = 500;
            tempDist[graph.Vertices.IndexOf(v)] = 500;
            prev[graph.Vertices.IndexOf(v)] = null;
            vertexList.Add(v);
        }

        //Set the origin dist to be 0
        dist[graph.Vertices.IndexOf(origin)] = 0;
        tempDist[graph.Vertices.IndexOf(origin)] = 0;


        //Need a counter to stop the loop because the code 
        //style does not allow elements in vertex list to be removed
        int count = graph.Vertices.Count;
        while (count > 0)
        {
            count--;
            //Choose the vertex that has least cost and mark it as already viewed
            Vertex<Tile> u = vertexList.ElementAt(Array.IndexOf(tempDist, tempDist.Min()));
            tempDist[Array.IndexOf(tempDist, tempDist.Min())] = 500;

            //Iterate over the neighbors of vertex u
            foreach (Vertex<Tile> v in u.Neighbors)
            {
                Vertex<Tile> vtemp = graph.Vertices.Find(ve => ve.Tile.Equals(v.Tile));
                int localLeast;
                //Exclude vertices that have been occupied by other characters.
                //Otherwise store the local least cost right now    
                if (vtemp.Tile.CharOnThis != null)
                    localLeast = 500;
                else
                    localLeast = dist[graph.Vertices.IndexOf(u)] + graph.Edges.Find(e => ReferenceEquals(e.Start, u) && ReferenceEquals(e.End, vtemp)).Cost;

                //If the local least is less than stored cost, replace it
                if (localLeast < dist[graph.Vertices.IndexOf(vtemp)])
                {
                    dist[graph.Vertices.IndexOf(vtemp)] = localLeast;
                    tempDist[graph.Vertices.IndexOf(vtemp)] = localLeast;
                    prev[graph.Vertices.IndexOf(vtemp)] = u;
                }
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        Debug.Log(dmg);
        health -= dmg;

        if (health <= 0)
        {
            if (Guts)
            {
                health = 1;
            }
            else
            {
                Destroy(gameObject);
            }
        }
            
    }
}
