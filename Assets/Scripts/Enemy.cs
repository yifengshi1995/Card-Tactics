using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Character {

    void Start()
    {
        RestoreDefaultStatus();
        Guts = true;
    }

    // Use this for initialization
    void Awake () {
        posX = Mathf.Abs((int)(transform.position.x / 32 + transform.position.y / 16) / 2);
        posY = Mathf.Abs((int)(transform.position.y / 16 - transform.position.x / 32) / 2);
    }
	
	// Update is called once per frame
	void Update () {

        Util.TILES[posX, posY].SetChar(gameObject);

        StatusCalculation();

        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(MakeMove(SelectTarget().Tile));
        }
    }

    Vertex<Tile> SelectTarget()
    {
        //If at least one player is within enemy's range,
        //go for that one if only one player, otherwise 
        //go for the player having least HP


        //If no players within enemy's reachable range, just 
        //go for the nearest one using Manhattan distance
        int closestDistance = 100;
        Vertex<Tile> target = null;
        foreach (GameObject p in Util.PLAYERS)
        {
            int manhattanDistance = Mathf.Abs(posX - p.GetComponent<Player>().posX) + Mathf.Abs(posY - p.GetComponent<Player>().posY);
            if (manhattanDistance < closestDistance)
            {
                closestDistance = manhattanDistance;
                target = Util.GRAPH.Vertices.Find(v => v.Tile == Util.TILES[p.GetComponent<Player>().posX, p.GetComponent<Player>().posY]);
            }
        }

        return target;
    }

    void SelectCards()
    {

    }

    IEnumerator MakeMove(Tile dest)
    {
        /*
         * this method is modified from the player
         * version of moving. Unlike player, the destination
         * of an enemy AI is not necessarily smaller or 
         * equal to the Move capacity of character. So 
         * the destination tile is just for acquire the 
         * shortest path to make enemy move to that tile.
         */

        Dijkstra(Util.GRAPH, Util.GRAPH.Vertices.Find(v => v.Tile.Equals(Util.TILES[posX, posY])));

        //Enemy cannot walk onto the player. it will choose
        //one adjacent tile that has least distance
        Tile destination = null;
        int desDist = 500;
        if (!dest.isLeftBound)
            if (desDist > dist[Util.GRAPH.Vertices.FindIndex(v => v.Tile == Util.TILES[dest.getX(), dest.getY()-1])])
            {
                desDist = dist[Util.GRAPH.Vertices.FindIndex(v => v.Tile == Util.TILES[dest.getX(), dest.getY() - 1])];                
                destination = Util.TILES[dest.getX(), dest.getY() - 1];
            }
        if (!dest.isRightBound)
            if (desDist > dist[Util.GRAPH.Vertices.FindIndex(v => v.Tile == Util.TILES[dest.getX(), dest.getY() + 1])])
            {
                desDist = dist[Util.GRAPH.Vertices.FindIndex(v => v.Tile == Util.TILES[dest.getX(), dest.getY() + 1])];
                destination = Util.TILES[dest.getX(), dest.getY() + 1];
            }
        if (!dest.isDownBound)
            if (desDist > dist[Util.GRAPH.Vertices.FindIndex(v => v.Tile == Util.TILES[dest.getX() - 1, dest.getY()])])
            {
                desDist = dist[Util.GRAPH.Vertices.FindIndex(v => v.Tile == Util.TILES[dest.getX() - 1, dest.getY()])];
                destination = Util.TILES[dest.getX() - 1, dest.getY()];
            }
        if (!dest.isUpBound)
            if (desDist > dist[Util.GRAPH.Vertices.FindIndex(v => v.Tile == Util.TILES[dest.getX() + 1, dest.getY()])])
            {
                desDist = dist[Util.GRAPH.Vertices.FindIndex(v => v.Tile == Util.TILES[dest.getX() + 1, dest.getY()])];
                destination = Util.TILES[dest.getX() + 1, dest.getY()];
            }

        //Get distance from here to destination
        int count = dist[Util.GRAPH.Vertices.FindIndex(v => v.Tile.Equals(destination))];
        //Add the destination tile into the list that 
        //containing steps in order
        Tile current = destination;
        List<Tile> sequence = new List<Tile>();
        //Prevent move to self
        if(current != Util.TILES[posX, posY])
            sequence.Add(current);

        //Since the dijkstra get the path backward,
        //we need to insert the path backward to get the
        //correct order of steps.
        while (count > 0)
        {
            Tile last = prev[Util.GRAPH.Vertices.FindIndex(v => v.Tile.Equals(current))].Tile;        
            count -= Util.GRAPH.Edges.Find(e => ReferenceEquals(e.Start.Tile, last) && ReferenceEquals(e.End.Tile, current)).Cost;
            sequence.Insert(0, last);
            current = last;
        }
        //This method will insert the current location of 
        //character. So the first tile need to be removed
        if(sequence.Count > 0)
            sequence.RemoveAt(0);

        //Since destination is not always within the 
        //movable range, we loop until its capacity gets 0
        count = Move;
        Tile curr = Util.TILES[posX, posY];
        while (count > 0)
        {
            Vector3 target;
            if (sequence.Count > 0 && sequence[0].cost <= count)
            {
                Debug.Log(curr.getPos() + " " + sequence[0].getPos());
                count -= Util.GRAPH.Edges.Find(e => ReferenceEquals(e.Start.Tile, curr) && ReferenceEquals(e.End.Tile, sequence[0])).Cost;

                //if the next tile has cost smaller or equal
                //to move capacity, then move
                target = sequence[0].transform.position;            
                float elapsedTime = 0;
                while (elapsedTime < 0.3f)
                {
                    transform.position = Vector3.Lerp(transform.position, target, elapsedTime / 0.3f);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                curr = sequence[0];
                sequence.RemoveAt(0);
                //Adjust float computational errors
                if (transform.position != target)
                    transform.position = target;
                
            }
            else
            {   
                //Stop moving because next tile has higher cost 
                //than remaining move capacity
                count = -1;
            }
        }

        //Update the location of character
        Util.TILES[posX, posY].SetChar(null);
        posX = Mathf.Abs((int)(transform.position.x / 32 + transform.position.y / 16) / 2);
        posY = Mathf.Abs((int)(transform.position.y / 16 - transform.position.x / 32) / 2);
    }
}
