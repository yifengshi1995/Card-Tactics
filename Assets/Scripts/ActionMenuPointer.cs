using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenuPointer : MonoBehaviour {

    private GameObject currentPlayer;
    private int index;
    private List<CardOption> selectedCards;
    private Card comboCard;

    public GameObject CurrentPlayer { get { return currentPlayer; } }
    public Card ComboCard { get { return comboCard; } }

    void Awake () {
        comboCard = null;
        selectedCards = new List<CardOption>();
        currentPlayer = null;
        index = 0;
	}

	void Update () {

        switch (Util.STATE)
        {  
            case Util.State.ACTIONMENU:
                {
                    //Move the pointer up and down
                    if (Input.GetKeyDown(KeyCode.DownArrow) && index < Util.ACTIONMENU.GetComponent<ActionMenu>().Cards.Count - 1)
                    {
                        index++;
                        transform.position = new Vector2(transform.position.x, Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index].CardObj.transform.position.y);
                    }

                    else if (Input.GetKeyDown(KeyCode.UpArrow) && index > 0)
                    {
                        index--;
                        transform.position = new Vector2(transform.position.x, Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index].CardObj.transform.position.y);
                    }

                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        //When a combo has formed, all other cards will be disabled until you unselect either cards that form the combo
                        if (ShouldDisable())
                            return;

                        SelectCard();

                        //Check if there are some cards should be disabled
                        ShouldDisable();
                    }

                    else if (Input.GetKeyDown(KeyCode.X))
                    {
                        ExitActionMenu();
                    }

                    else if (Input.GetKeyDown(KeyCode.C))
                    {
                        ConfirmAction();
                    }
                }              
                break;
            case Util.State.MESSAGE:
                {
                    if (Input.anyKeyDown)
                    {
                        Util.MESSAGE.SetActive(false);
                        Util.STATE = Util.State.ACTIONMENU;
                    }            
                }                
                break;
            case Util.State.CONFIRM_ACTION:
                {
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        Util.STATE = Util.State.ATTACK;
                        Util.MESSAGE.SetActive(false);
                        currentPlayer.GetComponent<Player>().ShowActionRange();
                        Util.GENERALMENU.SetActive(false);
                        Util.ACTIONMENU.SetActive(false);
                    }

                    else if (Input.GetKeyDown(KeyCode.X))
                    {
                        Util.STATE = Util.State.ACTIONMENU;
                        Util.MESSAGE.SetActive(false);
                    }                
                }              
                break;     
        }
    }

    public void SetCurrentPlayer(GameObject player)
    {
        currentPlayer = player;
        ResetPointer();
        if (currentPlayer == null)
        {
            Util.ACTIONMENU.SetActive(false);
        }        
        else
        {
            Util.ACTIONMENU.SetActive(true);
        }
    }

    void ResetPointer()
    {
        index = 0;
        transform.localPosition = new Vector2(transform.localPosition.x, 85);
    }

    void SelectCard()
    {
        //Unselected cards have black texts. If select a card, its text will turn to white.
        Color currentCardTextColor = Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index].CardObj.GetComponentInChildren<Text>().color;
        if (currentCardTextColor.Equals(Color.black))
        {
            Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index].CardObj.GetComponentInChildren<Text>().color = Color.white;

            //Add the card to the selected card list , and take its effects
            selectedCards.Add(Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index]);
            Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index].Card.CardEffect(currentPlayer.GetComponent<Player>());
        }
        else //When you cancel your selection, it will go back to black; if cancel a member of a combo, rest of the members will go back to white.
        {
            if (currentCardTextColor.Equals(Color.blue))
            {
                foreach (CardOption c in selectedCards)
                {
                    c.CardObj.GetComponentInChildren<Text>().color = Color.white;
                }
                Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index].CardObj.GetComponentInChildren<Text>().color = Color.black;
                //Cancel effect of combo card and restore members' effects
                comboCard.CancelEffect(currentPlayer.GetComponent<Player>());
                comboCard = null;
                selectedCards.ForEach(c => c.Card.CardEffect(currentPlayer.GetComponent<Player>()));
            }
            else
                Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index].CardObj.GetComponentInChildren<Text>().color = Color.black;

            //Remove such card from selected cards list, and cancel its effects
            selectedCards.Remove(Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index]);
            Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index].Card.CancelEffect(currentPlayer.GetComponent<Player>());
        }

        //If selected cards form a combo, they all will turn to blue.
        List<int> selectedCardsIndex = new List<int>();
        selectedCards.ForEach(c => selectedCardsIndex.Add(c.Card.Index));
        if (SpecialCombinations(selectedCardsIndex))
        {
            foreach (CardOption c in selectedCards)
            {
                c.CardObj.GetComponentInChildren<Text>().color = Color.blue;
            }
        }
    }

    void ExitActionMenu()
    {
        if (comboCard != null)
        {
            comboCard.CancelEffect(currentPlayer.GetComponent<Player>());
            comboCard = null;
        }

        Util.ACTIONMENU.GetComponent<ActionMenu>().Cards.ForEach(c => Destroy(c.CardObj));
        Util.ACTIONMENU.GetComponent<ActionMenu>().Cards.Clear();
        selectedCards.ForEach(c => c.Card.CancelEffect(currentPlayer.GetComponent<Player>()));
        selectedCards.Clear();

        Util.STATE = Util.State.GENERALMENU;
        SetCurrentPlayer(null);
    }

    void ConfirmAction()
    {
        if (selectedCards.Count == 0)
            return;
        else
        {
            if (currentPlayer.GetComponent<Player>().Exhausted)
            {
                Util.MESSAGE.GetComponentInChildren<Text>().text = "COST OVER!";
                Util.STATE = Util.State.MESSAGE;
            }
            else
            {
                Util.MESSAGE.GetComponentInChildren<Text>().text = "Confirm Selections?\nZ(YES) X(NO)";
                Util.STATE = Util.State.CONFIRM_ACTION;
            }
            Util.MESSAGE.SetActive(true);
        }
            
    }

    bool SpecialCombinations(List<int> list)
    {
        list.Sort();
        string combination = "";
        list.ForEach(i => combination += i);
        switch (list.Count)
        {
            case 2:
                if (combination.Equals("25") || combination.Equals("14") || combination.Equals("37"))
                {
                    CombinationEffects(combination);
                    return true;
                } 
                else
                    return false;
            case 3:
                if (combination.Equals("111"))
                {
                    CombinationEffects(combination);
                    return true;
                }
                else
                    return false;
            default:
                return false;
        }
    }

    void CombinationEffects(string combo)
    {
        //Nullify effects of components
        selectedCards.ForEach(c => c.Card.CancelEffect(currentPlayer.GetComponent<Player>()));

        //Take combo's effect
        switch (combo)
        {
            //Fighter
            case "14":
                comboCard = Util.CARD_LIST[8];
                break;
            case "25":
                comboCard = Util.CARD_LIST[6];
                break;
            case "37":
                comboCard = Util.CARD_LIST[10];
                break;
            case "111":
                comboCard = Util.CARD_LIST[9];
                break;
        }     
        comboCard.CardEffect(currentPlayer.GetComponent<Player>());
    }

    /*
     * This method is for:
     * Checking if there are cards should be disabled;
     * Change the color based on the status of cards that not members of the combo;
     * And return a boolean to make the effect of disable/enable.
     * Due to its multiple tasks this method is called twice at both the beginning
     * and the end of the Z Button Press Check.
     */
    bool ShouldDisable()
    {
        
        if(comboCard != null)
        {
            foreach(CardOption c in Util.ACTIONMENU.GetComponent<ActionMenu>().Cards)
            {
                if (selectedCards.Find(c2 => c == c2) == null)
                {
                    c.CardObj.GetComponentInChildren<Text>().color = Color.gray;
                }
            }

            if (Util.ACTIONMENU.GetComponent<ActionMenu>().Cards[index].CardObj.GetComponentInChildren<Text>().color.Equals(Color.gray))
                return true;
            else
                return false;  
        }
        else
        {
            foreach (CardOption c in Util.ACTIONMENU.GetComponent<ActionMenu>().Cards)
            {
                if (c.CardObj.GetComponentInChildren<Text>().color.Equals(Color.gray))
                    c.CardObj.GetComponentInChildren<Text>().color = Color.black;
            }

            return false;
        }
    }
}


