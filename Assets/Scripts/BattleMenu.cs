using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour {

    private GameObject player;
    private GameObject enemy;
    private GameObject playerStatus;
    private GameObject enemyStatus;
    private Text[] playerTexts;
    private Text[] enemyTexts;

	void Awake () {
        player = null;
        enemy = null;
        gameObject.SetActive(false);

        playerStatus = transform.Find("PlayerStatus").gameObject;
        enemyStatus = transform.Find("EnemyStatus").gameObject;

        playerTexts = playerStatus.GetComponentsInChildren<Text>();
        enemyTexts = enemyStatus.GetComponentsInChildren<Text>();
	}
	
	void Update () {
        if (player != null && enemy != null)
            UpdateStatus();
	}

    public void UpdateStatus()
    {
        playerTexts[1].text = " HP  " + player.GetComponent<Player>().CHealth;
        playerTexts[2].text = " STR " + player.GetComponent<Player>().CStr;
        playerTexts[3].text = " INT " + player.GetComponent<Player>().CInt;
        playerTexts[4].text = " DEF " + player.GetComponent<Player>().CDef;
        playerTexts[5].text = " RES " + player.GetComponent<Player>().CRes;

        enemyTexts[1].text = " HP  " + enemy.GetComponent<Enemy>().CHealth;
        enemyTexts[2].text = " STR " + enemy.GetComponent<Enemy>().CStr;
        enemyTexts[3].text = " INT " + enemy.GetComponent<Enemy>().CInt;
        enemyTexts[4].text = " DEF " + enemy.GetComponent<Enemy>().CDef;
        enemyTexts[5].text = " RES " + enemy.GetComponent<Enemy>().CRes;
    }

    public void SetCharacter(GameObject p, GameObject e)
    {
        player = p;
        enemy = e;
    }
}
