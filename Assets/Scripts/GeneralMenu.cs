using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralMenu : MonoBehaviour {

    private List<Selection> selections;
    private GameObject player;

    public List<Selection> Selections { get { return selections; } }

	// Use this for initialization
	void Awake () {
        player = null;
        gameObject.SetActive(false);
        selections = new List<Selection>();
        foreach(Text text in GetComponentsInChildren<Text>())
        {
            selections.Add(new Selection(text));
            
        }
	}
	
	// Update is called once per frame
	void Update() {
        if(player != null)
        {
            if (!player.GetComponent<Player>().CanMove)
            {
                selections[0].SetAvaliability(false);
            }
            else
            {
                selections[0].SetAvaliability(true);
            }

            if (!player.GetComponent<Player>().CanAct)
            {
                selections[1].SetAvaliability(false);
            }
            else
            {
                selections[1].SetAvaliability(true);
            }
        }
	}

    public void SetCurrentPlayer(GameObject p)
    {
        player = p;
    }
}

public class Selection
{
    private Text option;
    private bool isAvaliable;

    public Text Option { get { return option; } }
    public bool IsAvaliable { get { return isAvaliable; } }

    public Selection(Text option)
    {
        this.option = option;
        isAvaliable = true;
    }

    public void SetAvaliability(bool avaliability)
    {
        if (avaliability)
        {
            isAvaliable = true;
            Option.color = new Color(0f, 0f, 0f);
        }
        else
        {
            isAvaliable = false;
            Option.color = new Color(0.7f, 0.7f, 0.7f);
        }
    }
}
