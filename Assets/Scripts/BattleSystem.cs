using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour {


	void Start () {
		
	}
	
	void Update ()
    {
     
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
}
