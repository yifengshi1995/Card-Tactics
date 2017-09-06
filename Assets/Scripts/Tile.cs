using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public int cost;
    //0 = Ground, 1 = Forest, 2 = Mountain, 3 = Water
    public int type;
    public bool isLeftBound, isRightBound, isUpBound, isDownBound;

    private GameObject charOnThis = null;

    public GameObject CharOnThis { get { return charOnThis; } }
    public int RealCost { get { return realCost; } }

    private int realCost;

    void Start()
    {
        realCost = cost;
    }

    public int getX()
    {
        return Mathf.Abs((int)((transform.position.x / 32 + transform.position.y / 16) / 2));
    }

    public int getY()
    {
        return Mathf.Abs((int)((transform.position.y / 16 - transform.position.x / 32) / 2));
    }

    public string getPos()
    {
        return Mathf.Abs((int)((transform.position.x / 32 + transform.position.y / 16) / 2)) + ", " + Mathf.Abs((int)((transform.position.y / 16 - transform.position.x / 32) / 2));
    }

    public void SetChar(GameObject c)
    {
        charOnThis = c;
    }
}
