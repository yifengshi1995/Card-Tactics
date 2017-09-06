using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleground : MonoBehaviour {

    public int xMax, yMax;

	// Use this for initialization
	void Start () {
        Util.TILES = new Tile[xMax, yMax];
        Tile[] temp = FindObjectsOfType<Tile>();
        foreach (Tile tile in temp)
        {
            Util.TILES[tile.getX(), tile.getY()] = tile;
        }

        Util.GRAPH = new WeightedGraph<Tile>();
        BuildGraph();
	}

    void BuildGraph()
    {
        foreach(Tile tile in Util.TILES)
        {
            BuildCurrentEdges(tile);
        }
    }

    void BuildCurrentEdges(Tile tile)
    {
        Vertex<Tile> start;
        Vertex<Tile> temp = Util.GRAPH.Vertices.Find(v => ReferenceEquals(v.Tile, tile));

        start = temp ?? new Vertex<Tile>(tile);

        if (!tile.isUpBound)
        {
            Tile adj = Util.TILES[tile.getX() + 1, tile.getY()];
            BuildDirectionalEdge(start, adj);
        }
        if (!tile.isDownBound)
        {
            Tile adj = Util.TILES[tile.getX() - 1, tile.getY()];
            BuildDirectionalEdge(start, adj);
        }
        if (!tile.isLeftBound)
        {
            Tile adj = Util.TILES[tile.getX(), tile.getY() - 1];
            BuildDirectionalEdge(start, adj);
        }
        if (!tile.isRightBound)
        {
            Tile adj = Util.TILES[tile.getX(), tile.getY() + 1];
            BuildDirectionalEdge(start, adj);
        }
    }

    void BuildDirectionalEdge(Vertex<Tile> start, Tile adj)
    {
        Vertex<Tile> end;
        if (Util.GRAPH.Vertices.FindIndex(v => ReferenceEquals(v.Tile, adj)) == -1)
        {
            end = new Vertex<Tile>(adj);
        }
        else
        {
            end = Util.GRAPH.Vertices.Find(v => ReferenceEquals(v.Tile, adj));
        }
        if(Util.GRAPH.Edges.Find(e => ReferenceEquals(e.Start.Tile, start.Tile) && ReferenceEquals(e.End.Tile, end.Tile) && e.Cost == end.Tile.cost) == null)
            Util.GRAPH.AddEdge(new Edge<Tile>(start, end, end.Tile.cost));
        if (Util.GRAPH.Edges.Find(e => ReferenceEquals(e.Start.Tile, end.Tile) && ReferenceEquals(e.End.Tile, start.Tile) && e.Cost == start.Tile.cost) == null)
            Util.GRAPH.AddEdge(new Edge<Tile>(end, start, start.Tile.cost));
    }
}
