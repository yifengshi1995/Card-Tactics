using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedGraph<T> {
    List<Edge<T>> edges;
    List<Vertex<T>> vertices;

    public List<Edge<T>> Edges { get { return edges; } }
    public List<Vertex<T>> Vertices { get { return vertices; } }

    public WeightedGraph()
    {
        edges = new List<Edge<T>>();
        vertices = new List<Vertex<T>>();
    }

    public WeightedGraph(List<Edge<T>> edges, List<Vertex<T>> vertices)
    {
        this.edges = edges;
        this.vertices = vertices;
    }

    public void AddEdge(Edge<T> edge)
    {
        edges.Add(edge);
        if(vertices.Find(e => ReferenceEquals(e.Tile, edge.Start.Tile)) == null)
            vertices.Add(edge.Start);
        if (vertices.Find(e => ReferenceEquals(e.Tile, edge.End.Tile)) == null)
            vertices.Add(edge.End);
    }

    public void RemoveEdge(Edge<T> edge)
    {
        edges.Remove(edge);
        edge.Start.RemoveNeighbor(edge.End);
        edge.End.RemoveNeighbor(edge.Start);
    }
}

public class Edge<T>
{
    Vertex<T> start, end;
    int cost;

    public Vertex<T> Start { get { return start; } }
    public Vertex<T> End { get { return end; } }
    public int Cost { get { return cost; } }

    public Edge(Vertex<T> start, Vertex<T> end, int cost)
    {
        this.start = start;
        this.end = end;
        this.cost = cost;
        if(start.Neighbors.Find(v => ReferenceEquals(v.Tile, end.Tile)) == null)
            start.AddNeighbor(end);
        if (end.Neighbors.Find(v => ReferenceEquals(v.Tile, start.Tile)) == null)
            end.AddNeighbor(start);
    }
}

public class Vertex<T>
{
    Tile tile;
    List<Vertex<T>> neighbors;

    public Tile Tile { get { return tile; } }
    public List<Vertex<T>> Neighbors { get { return neighbors; } }

    public Vertex(Tile tile)
    {
        this.tile = tile;
        neighbors = new List<Vertex<T>>();
    }

    public void AddNeighbor(Vertex<T> neighbor)
    {
        neighbors.Add(neighbor);
    }

    public void RemoveNeighbor(Vertex<T> neighbor)
    {
        neighbors.Remove(neighbor);
    }
}
