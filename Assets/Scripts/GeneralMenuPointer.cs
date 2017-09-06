using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GeneralMenuPointer : MonoBehaviour {

    private GameObject currentPlayer;
    private int index;

    public GameObject CurrentPlayer { get { return currentPlayer; } }

	// Use this for initialization
	void Awake() {
        currentPlayer = null;
        index = 0;
    }
	
	// Update is called once per frame
	void Update() {

        if (Util.STATE == Util.State.GENERALMENU)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) && index < Util.GENERALMENU.GetComponent<GeneralMenu>().Selections.Count - 1)
            {
                index++;
                transform.position = new Vector2(transform.position.x, Util.GENERALMENU.GetComponent<GeneralMenu>().Selections[index].Option.transform.position.y);
            }
     
            if (Input.GetKeyDown(KeyCode.UpArrow) && index > 0)
            {
                index--;
                transform.position = new Vector2(transform.position.x, Util.GENERALMENU.GetComponent<GeneralMenu>().Selections[index].Option.transform.position.y);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (index == 0 && currentPlayer.GetComponent<Player>().CanMove)
                {
                    
                    currentPlayer.GetComponent<Player>().FindMoveRange();
                    Util.STATE = Util.State.MOVE;
                    Util.GENERALMENU.SetActive(false);
                }

                if(index == 1 && currentPlayer.GetComponent<Player>().CanAct)
                {
                    Util.ACTIONMENU.SetActive(true);
                    Util.ACTIONMENU.GetComponent<ActionMenu>().DisplayCards();
                    Util.STATE = Util.State.ACTIONMENU;
                }

                if(index == 2)
                {

                }

                if (index == 3)
                {
                    currentPlayer.GetComponent<Player>().TurnEnd();
                    Util.GENERALMENU.GetComponent<GeneralMenu>().SetCurrentPlayer(null);
                    Util.GM_POINTER.GetComponent<GeneralMenuPointer>().SetCurrentPlayer(null);
                    Util.ACTIONMENU.GetComponent<ActionMenu>().SetCurrentPlayer(null);
                    Util.STATE = Util.State.AWAIT;        
                    Util.GENERALMENU.SetActive(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.X) && currentPlayer.GetComponent<Player>().CanAct && !currentPlayer.GetComponent<Player>().CanMove)
            {
                currentPlayer.GetComponent<Player>().CancelMove();
            }
            else if (Input.GetKeyDown(KeyCode.X) && currentPlayer.GetComponent<Player>().CanAct && currentPlayer.GetComponent<Player>().CanMove)
            {
                currentPlayer.GetComponent<Player>().ClearMoveRange();
                Util.GENERALMENU.GetComponent<GeneralMenu>().SetCurrentPlayer(null);
                Util.GM_POINTER.GetComponent<GeneralMenuPointer>().SetCurrentPlayer(null);
                Util.ACTIONMENU.GetComponent<ActionMenu>().SetCurrentPlayer(null);
                Util.STATE = Util.State.AWAIT;
            }
        }  
	}

    public void SetCurrentPlayer(GameObject player)
    {
        currentPlayer = player;
        ResetPointer();
        if (currentPlayer == null)
            Util.GENERALMENU.SetActive(false);
        else
        {
            Util.GENERALMENU.SetActive(true);
        } 
    }

    public Selection GetCurrentSelection()
    {
        return Util.GENERALMENU.GetComponent<GeneralMenu>().Selections[index];
    }

    void ResetPointer()
    {
        index = 0;
        transform.localPosition = new Vector2(transform.localPosition.x, Util.GENERALMENU.GetComponent<GeneralMenu>().Selections[0].Option.transform.localPosition.y);
    }
}
