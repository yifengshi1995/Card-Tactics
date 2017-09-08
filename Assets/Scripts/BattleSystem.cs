using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour {

    private float turnBannerCounter;
    private List<GameObject> activePlayer;
    public List<GameObject> activeEnemy;
    public bool lastEnemyFinished;

	void Awake () {
        turnBannerCounter = 2f;
        activePlayer = new List<GameObject>();
        activeEnemy = new List<GameObject>();
    }
	
	void Update ()
    {
        //Make the banners show only 2 seconds and then officially start turn
        if (turnBannerCounter <= 0)
        {
            if (Util.STATE == Util.State.PLAYER_TURN_START)
                PlayerTurnStart();
            else if (Util.STATE == Util.State.ENEMY_TURN_START)
                EnemyTurnStart();

            turnBannerCounter = 2f;
        }
        if (Util.STATE == Util.State.PLAYER_TURN_START || Util.STATE == Util.State.ENEMY_TURN_START)
            turnBannerCounter -= Time.deltaTime;

        //Show respective turn start banners
        if(Util.STATE == Util.State.PLAYER_TURN_START)
        {
            GameObject.Find("Canvas").transform.Find("PlayerTurnStart").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("PlayerTurnStart").gameObject.SetActive(false);
        }

        if (Util.STATE == Util.State.ENEMY_TURN_START)
        {
            GameObject.Find("Canvas").transform.Find("EnemyTurnStart").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("EnemyTurnStart").gameObject.SetActive(false);
        }

        //When all players/enemies have finished actions, exchange to opponents' turn
        if (activePlayer.Count == 0 && Util.STATE == Util.State.AWAIT)
            Util.STATE = Util.State.ENEMY_TURN_START;
        else if (activeEnemy.Count == 0 && Util.STATE == Util.State.ENEMY_ACTION)
            Util.STATE = Util.State.PLAYER_TURN_START;

        if(Util.STATE == Util.State.ENEMY_ACTION && lastEnemyFinished)
        {
            activeEnemy[0].GetComponent<Enemy>().Action();
            lastEnemyFinished = false;
        }

        if (Input.GetKeyDown(KeyCode.O))
            Util.STATE = Util.State.PLAYER_TURN_START;
    }

    public void Battle(GameObject attacker, GameObject defender)
    {
        Character attackerC = attacker.GetComponent<Character>();
        Character defenderC = null;
        if (defender != null)
        {
            defenderC = defender.GetComponent<Character>();
            Debug.Log(defenderC.CHealth);
            defenderC.TakeDamage(attackerC.CStr - defenderC.CDef);
            Debug.Log(defenderC.CHealth);
        }
        else
        {

        }
        
    }

    public void PlayerTurnStart()
    {
        Util.STATE = Util.State.AWAIT;
        Util.PLAYERS.ForEach(p =>
        {
            activePlayer.Add(p);
            p.GetComponent<Player>().CanMove = true;
            p.GetComponent<Player>().StartOperation();
        });
    }

    public void EnemyTurnStart()
    {
        Util.STATE = Util.State.ENEMY_ACTION;
        Util.ENEMIES.ForEach(e =>
        {
            activeEnemy.Add(e);
            e.GetComponent<Enemy>().StartOperation();
        });
        lastEnemyFinished = true;
    }


    public void DeactivatePlayer(GameObject player)
    {
        activePlayer.Remove(player);
    }
}
