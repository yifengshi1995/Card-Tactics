using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    private Transform cursor;
    private Tile current;


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update() {

        CurrentTile();
        if (Util.STATE == Util.State.AWAIT)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && !current.isUpBound)
            {
                transform.position = new Vector3(transform.position.x + 32, transform.position.y + 16, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && !current.isDownBound)
            {
                transform.position = new Vector3(transform.position.x - 32, transform.position.y - 16, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !current.isLeftBound)
            {
                transform.position = new Vector3(transform.position.x - 32, transform.position.y + 16, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && !current.isRightBound)
            {
                transform.position = new Vector3(transform.position.x + 32, transform.position.y - 16, transform.position.z);
            }
        }
        else
        if (Util.STATE == Util.State.MOVE)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && !current.isUpBound)
            {
                transform.position = new Vector3(transform.position.x + 32, transform.position.y + 16, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && !current.isDownBound)
            {
                transform.position = new Vector3(transform.position.x - 32, transform.position.y - 16, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !current.isLeftBound)
            {
                transform.position = new Vector3(transform.position.x - 32, transform.position.y + 16, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && !current.isRightBound)
            {
                transform.position = new Vector3(transform.position.x + 32, transform.position.y - 16, transform.position.z);
            }
        }
        else
        if (Util.STATE == Util.State.ATTACK)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && !current.isUpBound)
            {
                transform.position = new Vector3(transform.position.x + 32, transform.position.y + 16, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && !current.isDownBound)
            {
                transform.position = new Vector3(transform.position.x - 32, transform.position.y - 16, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !current.isLeftBound)
            {
                transform.position = new Vector3(transform.position.x - 32, transform.position.y + 16, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && !current.isRightBound)
            {
                transform.position = new Vector3(transform.position.x + 32, transform.position.y - 16, transform.position.z);
            }
        }
    }

    public Tile CurrentTile()
    {
        int x = (int) transform.position.x;
        int y = (int) transform.position.y;
        int cursorX = (x / 32 + y / 16) / 2;
        int cursorY = (y / 16 - x / 32) / 2;
        current = Util.TILES[Mathf.Abs(cursorX), Mathf.Abs(cursorY)];
        return current;
    }
}
