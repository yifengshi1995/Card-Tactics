using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfo : MonoBehaviour {

    public Text type;
    public Text position;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update() {
        Tile current = Util.CURSOR.GetComponent<Cursor>().CurrentTile();

        switch (current.type)
        {
            case 0:
                type.text = "Ground";
                break;
            case 1:
                type.text = "Forest";
                break;
            case 2:
                type.text = "Mountain";
                break;
            case 3:
                type.text = "Water";
                break;

        }

        position.text = current.getX() + ", " + current.getY();
    }
}
