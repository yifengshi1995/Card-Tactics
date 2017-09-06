using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    private Transform cursor;
    private List<GameObject> moveRange, actionRange;
    private int[] deck, hand;
    private List<int> tempDeck;
    private bool canMove, canAct, finished;

    public bool CanMove { get { return canMove; } }
    public bool CanAct { get { return canAct; } }
    public bool Finished { get { return finished; } }
    //public List<GameObject> MoveRange { get { return moveRange; } }
    public int[] Dist { get { return dist; } }
    public Vertex<Tile>[] Prev { get { return prev; } }
    public int[] Hand { get { return hand; } }

    void Start()
    {
        RestoreDefaultStatus();

        deck = new int[20]{ 1, 1, 1, 1, 1, 2, 2, 2, 3, 3, 4, 4, 4, 5, 5, 7, 7, 7, 11, 12 };
        hand = new int[5] { 0, 0, 0, 0, 0 };
        Shuffle();
        Reload();
    }

	void Awake() {

        posX = Mathf.Abs((int)(transform.position.x / 32 + transform.position.y / 16) / 2);
        posY = Mathf.Abs((int)(transform.position.y / 16 - transform.position.x / 32) / 2);

        canMove = true;
        canAct = true;
    }
	
	void Update() {

        StatusCalculation();
        SetPosition();

        if (!finished)
        {
            if (Util.STATE == Util.State.AWAIT)
            {
                if (Input.GetKeyDown(KeyCode.Z) && Util.CURSOR.position == transform.position)
                {
                    Util.GENERALMENU.GetComponent<GeneralMenu>().SetCurrentPlayer(gameObject);
                    Util.GM_POINTER.GetComponent<GeneralMenuPointer>().SetCurrentPlayer(gameObject);
                    Util.ACTIONMENU.GetComponent<ActionMenu>().SetCurrentPlayer(gameObject);                    
                    Util.STATE = Util.State.GENERALMENU;
                }
            }

            else if (Util.STATE == Util.State.MOVE && Util.GM_POINTER.GetComponent<GeneralMenuPointer>().CurrentPlayer.name == name)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Tile temp = Util.CURSOR.GetComponent<Cursor>().CurrentTile();
                    if (dist != null && dist[Util.GRAPH.Vertices.FindIndex(v => ReferenceEquals(v.Tile, temp))] <= Move && temp.CharOnThis == null)
                    {
                        StartCoroutine(Moving(temp));
                    }
                }

                else if (Input.GetKeyDown(KeyCode.X))
                {
                    Debug.Log("Cancel Move");
                    ClearMoveRange();
                    Util.STATE = Util.State.GENERALMENU;
                    Util.GENERALMENU.SetActive(true);
                }

            }

            else if (Util.STATE == Util.State.ATTACK && Util.GM_POINTER.GetComponent<GeneralMenuPointer>().CurrentPlayer.name == name)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if(Util.CURSOR.GetComponent<Cursor>().CurrentTile().CharOnThis != null)
                    {
                        Util.STATE = Util.State.BATTLEMENU;
                        GameObject enemy = Util.CURSOR.GetComponent<Cursor>().CurrentTile().CharOnThis;
                        Util.BATTLEMENU.SetActive(true);
                        Util.BATTLEMENU.GetComponent<BattleMenu>().SetCharacter(gameObject, enemy);
                        
                    }
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    Util.STATE = Util.State.ACTIONMENU;
                    ClearActionRange();
                    Util.ACTIONMENU.SetActive(true);
                    Util.GENERALMENU.SetActive(true);
                }
            }

            else if(Util.STATE == Util.State.BATTLEMENU && Util.GM_POINTER.GetComponent<GeneralMenuPointer>().CurrentPlayer.name == name)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (Util.CURSOR.GetComponent<Cursor>().CurrentTile().CharOnThis != null)
                    {
                        StartCoroutine(BattleHandling(Util.CURSOR.GetComponent<Cursor>().CurrentTile().CharOnThis));
                    }                 
                }

                else if (Input.GetKeyDown(KeyCode.X))
                {
                    Util.BATTLEMENU.SetActive(false);
                    Util.STATE = Util.State.ATTACK;
                }
            }
        }
        else
        {
            if (moveRange != null && moveRange.Count > 0)
                ClearMoveRange();
            if (actionRange != null && actionRange.Count > 0)
                ClearActionRange();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            finished = false;
            canMove = true;
        }
            
	}

    public void FindMoveRange()
    {
        moveRange = new List<GameObject>();
        Dijkstra(Util.GRAPH, Util.GRAPH.Vertices.Find(v => ReferenceEquals(v.Tile, Util.TILES[posX, posY])));

        for (int i = 0; i < dist.Length; i++)
        {
            if(dist[i] <= Move)
            {
                Tile toAdd = Util.GRAPH.Vertices.ElementAt(i).Tile;
                GameObject mr = (GameObject)Instantiate(Resources.Load("Prefabs/MoveRange"), toAdd.transform.position, toAdd.transform.rotation);
                mr.GetComponent<SpriteRenderer>().sortingLayerName = "Cursor";
                mr.GetComponent<SpriteRenderer>().sortingOrder = 0;
                moveRange.Add(mr);
            }
        }

    }

    public void ShowActionRange()
    {
        actionRange = new List<GameObject>();
        Debug.Log("A");
        int i = Range;
        List<Vertex<Tile>> actionVertices = new List<Vertex<Tile>>();
        actionVertices.Add(Util.GRAPH.Vertices.Find(ve => ve.Tile == Util.TILES[posX, posY]));
        actionVertices = RecursiveFindActionRange(actionVertices, Util.TILES[posX, posY], i);

        foreach(Vertex<Tile> v in actionVertices)
        {
            GameObject ar = (GameObject)Instantiate(Resources.Load("Prefabs/AttackRange"), v.Tile.transform.position, v.Tile.transform.rotation);
            ar.GetComponent<SpriteRenderer>().sortingLayerName = "Cursor";
            ar.GetComponent<SpriteRenderer>().sortingOrder = 0;
            actionRange.Add(ar);
        }
        
    }

    List<Vertex<Tile>> RecursiveFindActionRange(List<Vertex<Tile>> list, Tile tile, int count)
    {
        if (count == 0)
        {
            if(list.Find(v => v == Util.GRAPH.Vertices.Find(ve => ve.Tile == tile)) == null)
                list.Add(Util.GRAPH.Vertices.Find(v => v.Tile == tile));
        }
        else
        {
            foreach (Vertex<Tile> v in Util.GRAPH.Vertices.Find(v => v.Tile == tile).Neighbors)
            {
                if(list.Find(ve => ve == v) == null)
                    list.Add(v);            
                RecursiveFindActionRange(list, v.Tile, count - 1);
            }
        }

        return list;

    }

    public void TurnEnd()
    {
        Util.TILES[posX, posY].SetChar(null);
        posX = Mathf.Abs((int)(transform.position.x / 32 + transform.position.y / 16) / 2);
        posY = Mathf.Abs((int)(transform.position.y / 16 - transform.position.x / 32) / 2);
        finished = true;
    }

    public void ClearMoveRange()
    {
        this.moveRange.ForEach(c => Destroy(c));
        this.moveRange.Clear();
    }

    public void ClearActionRange()
    {
        this.actionRange.ForEach(c => Destroy(c));
        this.actionRange.Clear();
    }

    System.Collections.IEnumerator Moving(Tile dest)
    {
        /*
         * Making player to move when press Z 
         * on any enmpty tiles that within player's 
         * Move capacity.
         */

        //set canMove to false to prevent second move
        canMove = false;

        //Get distance from here to destination
        int count = dist[Util.GRAPH.Vertices.FindIndex(v => ReferenceEquals(v.Tile, dest))];

        //Add the destination tile into the list that 
        //containing steps in order
        Tile current = dest;
        List<Tile> sequence = new List<Tile>();
        //Prevent move to self
        if (current != Util.TILES[posX, posY])
            sequence.Add(current);

        //Since the dijkstra get the path backward,
        //we need to insert the path backward to get the
        //correct order of steps.
        while (count > 0)
        {
            Tile last = prev[Util.GRAPH.Vertices.FindIndex(v => ReferenceEquals(v.Tile, current))].Tile;
            count -= Util.GRAPH.Edges.Find(e => e.Start.Tile == last && e.End.Tile == current).Cost;
            sequence.Insert(0, last);
            current = last;
        }

        //This method will insert the current location of 
        //character. So the first tile need to be removed
        sequence.RemoveAt(0);

        //redefine variable count, start moving using
        //Lerp to get a relatively smooth moving animation
        count = sequence.Count;
        for (int i = 0; i < sequence.Count; i++)
        {
            Vector3 target = sequence[i].transform.position;

            float elapsedTime = 0;
            while (elapsedTime < 0.3f)
            {
                transform.position = Vector3.Lerp(transform.position, target, elapsedTime / 0.3f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            //Adjust float computational errors
            if (transform.position != target)
                transform.position = target;
        }

        //Update the location of character
        Util.TILES[posX, posY].SetChar(null);
        posX = Mathf.Abs((int)(transform.position.x / 32 + transform.position.y / 16) / 2);
        posY = Mathf.Abs((int)(transform.position.y / 16 - transform.position.x / 32) / 2);

        //After moving back to general menu.
        Util.STATE = Util.State.GENERALMENU;
        Util.GENERALMENU.SetActive(true);
    }

    System.Collections.IEnumerator BattleHandling(GameObject target)
    {
        yield return new WaitForSeconds(0.5f);
        Util.BATTLESYSTEM.GetComponent<BattleSystem>().Battle(gameObject, target);
        yield return new WaitForSeconds(1f);
        Util.BATTLEMENU.SetActive(false);
        finished = true;
        Util.STATE = Util.State.AWAIT;
    }

    void Reload()
    {
        if (tempDeck[0] == 0)
            Shuffle();

        for(int i = 0; i < hand.Length; i ++)
        {
            if(hand[i] == 0)
            {
                hand[i] = tempDeck[0];
                tempDeck.RemoveAt(0);
                tempDeck.Add(0);
                
            }
        }

        Array.Sort(hand);
    }

    void Shuffle()
    {
        System.Random rand = new System.Random();
        for(int i = 0; i < deck.Length - 1; i++)
        {
            int j = rand.Next(i, deck.Length);
            int temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }

        tempDeck = deck.ToList();
    }
}
