using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Util : MonoBehaviour {

    public enum State
    {
        START,
        SELECT_MENU,
        AWAIT,
        GENERALMENU,
        MOVE,
        ACTIONMENU,
        MESSAGE,
        CONFIRM_ACTION,
        ATTACK,
        BATTLEMENU,
        ENEMY_ACTION,
        RESULT
    }

    public static Dictionary<int, Card> CARD_LIST = new Dictionary<int, Card>()
    {
        //Fighter
        { 1, Card.card1 },
        { 2, Card.card2 },
        { 3, Card.card3 },
        { 4, Card.card4 },
        { 5, Card.card5 },
        { 6, Card.card6 },
        { 7, Card.card7 },
        { 8, Card.card8 },
        { 9, Card.card9 },
        { 10, Card.card10 },
        { 11, Card.card11 },
        { 12, Card.card12 },
    };

    public static Tile[,] TILES;
    public static WeightedGraph<Tile> GRAPH;
    public static List<GameObject> PLAYERS;
    public static List<GameObject> ENEMIES;
    public static GameObject GENERALMENU;
    public static GameObject GM_POINTER;
    public static GameObject ACTIONMENU;
    public static GameObject AM_POINTER;
    public static GameObject BATTLEMENU;
    public static Transform CURSOR;
    public static State STATE;
    public static GameObject MESSAGE;
    public static GameObject BATTLESYSTEM;

    // Use this for initialization
    void Awake() {
        STATE = State.AWAIT;

        GENERALMENU = GameObject.Find("Canvas").transform.Find("GeneralMenu").gameObject;
        GM_POINTER = GameObject.Find("Canvas").transform.Find("GeneralMenu").Find("Pointer").gameObject;
        ACTIONMENU = GameObject.Find("Canvas").transform.Find("ActionMenu").gameObject;
        AM_POINTER = GameObject.Find("Canvas").transform.Find("ActionMenu").Find("Pointer").gameObject;
        BATTLEMENU = GameObject.Find("Canvas").transform.Find("BattleMenu").gameObject;
        CURSOR = GameObject.Find("Cursor").transform;
        BATTLESYSTEM = GameObject.Find("BattleSystem");
        PLAYERS = new List<GameObject>();
        ENEMIES = new List<GameObject>();
        MESSAGE = GameObject.Find("Canvas").transform.Find("MessageWindow").gameObject;
        MESSAGE.SetActive(false);

        //Initialize the list of all characters

        if(GameObject.FindGameObjectsWithTag("Player") != null)
            PLAYERS.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        if(GameObject.FindGameObjectsWithTag("Enemy") != null)
            ENEMIES.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }
	
	// Update is called once per frame
	void Update () {     
	}
}

