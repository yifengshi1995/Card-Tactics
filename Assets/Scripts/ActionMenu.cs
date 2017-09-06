using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour {

    private GameObject currentPlayer;
    private List<CardOption> cards;
    private Transform rightMenu;

    public List<CardOption> Cards { get { return cards; } }

	void Awake () {
        cards = new List<CardOption>();
        currentPlayer = null;
        gameObject.SetActive(false);
        rightMenu = transform.Find("RightMenu");
    }
	
	void Update () {

        //Display Status

        rightMenu.Find("PlayerHP").GetComponent<Text>().text = " HP  " + currentPlayer.GetComponent<Player>().CHealth;
        StatusModifier("PlayerHP");

        rightMenu.Find("PlayerATK").GetComponent<Text>().text = " STR " + currentPlayer.GetComponent<Player>().CStr;
        StatusModifier("PlayerATK");

        rightMenu.Find("PlayerINT").GetComponent<Text>().text = " INT " + currentPlayer.GetComponent<Player>().CInt;
        StatusModifier("PlayerINT");

        rightMenu.Find("PlayerDEF").GetComponent<Text>().text = " DEF " + currentPlayer.GetComponent<Player>().CDef;
        StatusModifier("PlayerDEF");

        rightMenu.Find("PlayerMDF").GetComponent<Text>().text = " RES " + currentPlayer.GetComponent<Player>().CRes;
        StatusModifier("PlayerMDF");

        rightMenu.Find("PlayerStamina").GetComponent<Text>().text = " STAMINA " + currentPlayer.GetComponent<Player>().CStamina;
        StatusModifier("PlayerStamina");

        if(Util.AM_POINTER.GetComponent<ActionMenuPointer>().ComboCard != null)
        {
            rightMenu.Find("ComboCard").GetComponent<Text>().text = " " + Util.AM_POINTER.GetComponent<ActionMenuPointer>().ComboCard.Name;
        }
        else
        {
            rightMenu.Find("ComboCard").GetComponent<Text>().text = "";
        }
    }

    public void SetCurrentPlayer(GameObject player)
    {
        currentPlayer = player;
    }

    public void DisplayCards()
    {

        for(int i = 0; i < currentPlayer.GetComponent<Player>().Hand.Length; i++)
        {
            //Get the name of ith card in hand
            Card currentCard = Util.CARD_LIST[currentPlayer.GetComponent<Player>().Hand[i]];

            //Load the empty prefab of Card in Attack Menu
            GameObject currentCardObj = (GameObject) Resources.Load("Prefabs/Card");

            //Assign the image and text of this card 
            Texture2D img = (Texture2D)Resources.Load("UI/CardSmallImage/" + currentCard.Name);
            currentCardObj.GetComponentInChildren<Image>().sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
            currentCardObj.GetComponentInChildren<Text>().text = currentCard.Name;

            //Instantiate the prefab and put it as the child of Left Menu of Attack Menu
            GameObject cardObject = Instantiate(currentCardObj, new Vector2(0, 85 - i * 50), Quaternion.identity);
            cardObject.transform.SetParent(transform.Find("LeftMenu").transform, false);

            cards.Add(new CardOption(cardObject, currentCard));
        }

        Util.AM_POINTER.GetComponent<ActionMenuPointer>().SetCurrentPlayer(currentPlayer);
    }

    void StatusModifier(string name)
    {
        // Change the color of text based on the comparison
        // between the calculated value and the original value
        int calculatedValue = 0;
        int originalValue = 0;
        switch (name)
        {
            case "PlayerHP":
                originalValue = currentPlayer.GetComponent<Player>().Health;
                calculatedValue = currentPlayer.GetComponent<Player>().CHealth;
                break;
            case "PlayerATK":
                originalValue = currentPlayer.GetComponent<Player>().Strength;
                calculatedValue = currentPlayer.GetComponent<Player>().CStr;
                break;
            case "PlayerINT":
                originalValue = currentPlayer.GetComponent<Player>().Intelligence;
                calculatedValue = currentPlayer.GetComponent<Player>().CInt;
                break;
            case "PlayerDEF":
                originalValue = currentPlayer.GetComponent<Player>().Defense;
                calculatedValue = currentPlayer.GetComponent<Player>().CDef;
                break;
            case "PlayerMDF":
                originalValue = currentPlayer.GetComponent<Player>().Resistence;
                calculatedValue = currentPlayer.GetComponent<Player>().CRes;
                break;
            case "PlayerStamina":
                originalValue = currentPlayer.GetComponent<Player>().Stamina;
                calculatedValue = currentPlayer.GetComponent<Player>().CStamina;
                break;

        }

        if (calculatedValue != originalValue)
        {

            if (calculatedValue > originalValue)
            {
                rightMenu.Find(name).GetComponent<Text>().color = Color.green;
            }
            if (calculatedValue < originalValue)
            {
                rightMenu.Find(name).GetComponent<Text>().color = Color.red;
            }
        }
        else
        {
            rightMenu.Find(name).GetComponent<Text>().color = Color.black;
        }
    }
}

public class CardOption
{
    private GameObject cardObj;
    private Card card;

    public GameObject CardObj { get { return cardObj; } }
    public Card Card { get { return card; } }

    public CardOption(GameObject cardObj, Card card)
    {
        this.cardObj = cardObj;
        this.card = card;
    }
}
