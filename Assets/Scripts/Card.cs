using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private string name;
    private int index;
    private int maxNum;
    private int lvlReq;

    public string Name { get { return name; } }
    public int Index { get { return index; } }
    public int MaxNum { get { return maxNum; } }
    public int LvlReg { get { return lvlReq; } }

    public Card(string name, int index)
    {
        this.name = name;
        this.index = index;

        //Assign the level requirement of each card by its index
        switch (index)
        {
            //Fighter
            case 3:
                lvlReq = 2;
                break;
            case 4:
                lvlReq = 4;
                break;
            case 11:
                lvlReq = 7;
                break;
            case 12:
                lvlReq = 10;
                break;
            default:
                lvlReq = 1;
                break;
        }

        //Assign the max number in deck of each card by its index
        switch (index)
        {
            //Fighter
            case 1:
                maxNum = 10;
                break;
            case 2:
                maxNum = 8;
                break;
            case 3:
                maxNum = 2;
                break;
            case 4:
            case 5:
                maxNum = 4;
                break;
            case 7:
                maxNum = 6;
                break;
            case 11:
            case 12:
                maxNum = 1;
                break;

        }
    }

    public void CardEffect(Character c)
    {
        switch (index)
        {
            //Fighter
            case 1:
                c.StrOperand.Insert(0, 3);
                c.StaminaOperand.Add(-2);
                c.Range = Mathf.Max(c.Range, 1);
                break;
            case 2:
                c.DefOperand.Insert(0, 2);
                c.StaminaOperand.Add(-2);
                break;
            case 3:
                c.StrOperand.Add(2.0f);
                c.StaminaOperand.Add(-4);
                break;
            case 4:
                c.StrOperand.Insert(0, 1);
                c.StaminaOperand.Add(-3);
                c.Range = Mathf.Max(c.Range, 2);
                break;
            case 5:
                c.HPOperand.Add((int)(c.Health*0.3));
                c.StaminaOperand.Add(-3);
                break;
            case 6:
                c.DefOperand.Insert(0, 3);
                c.Guts = true;
                c.StaminaOperand.Add(-5);
                break;
            case 7:
                c.DefOperand.Add(1.5f);
                c.StaminaOperand.Add(-3);
                break;
            case 8:
                c.StrOperand.Insert(0, 4);
                c.StrOperand.Add(1.2f);
                c.StaminaOperand.Add(-5);
                c.Range = Mathf.Max(c.Range, 2);
                break;
            case 9:
                c.StrOperand.Insert(0, 12);
                c.DefOperand.Add(0.5f);
                c.StaminaOperand.Add(-6);
                break;
            case 10:
                c.DefOperand.Add(2.0f);
                c.JustBlock = true;
                c.StaminaOperand.Add(-7);
                break;
            case 11:
                c.StaminaOperand.Add(1);
                break;
            case 12:
                c.LastStand = true;
                c.StaminaOperand.Add(-1);
                break;

        }
    }

    public void CancelEffect(Character c)
    {
        switch (index)
        {
            //Fighter
            case 1:
                c.StrOperand.Remove(3);
                c.StaminaOperand.Remove(-2);
                c.Range = 0;
                break;
            case 2:
                c.DefOperand.Remove(2);
                c.StaminaOperand.Remove(-2);
                break;
            case 3:
                c.StrOperand.Remove(2.0f);
                c.StaminaOperand.Remove(-4);
                break;
            case 4:
                c.StrOperand.Remove(1);
                c.StaminaOperand.Remove(-3);
                c.Range = 0;
                break;
            case 5:
                c.HPOperand.Remove((int)(c.Health * 0.3));
                c.StaminaOperand.Remove(-3);
                break;
            case 6:
                c.DefOperand.Remove(3);
                c.Guts = true;
                c.StaminaOperand.Remove(-5);
                break;
            case 7:
                c.DefOperand.Remove(1.5f);
                c.StaminaOperand.Remove(-3);
                break;
            case 8:
                c.StrOperand.Remove(4);
                c.StrOperand.Remove(1.2f);
                c.StaminaOperand.Remove(-5);
                c.Range = 0;
                break;
            case 9:
                c.StrOperand.Remove(12);
                c.DefOperand.Remove(0.5f);
                c.StaminaOperand.Remove(-6);
                break;
            case 10:
                c.DefOperand.Remove(2.0f);
                c.JustBlock = true;
                c.StaminaOperand.Remove(-7);
                break;
            case 11:
                c.StaminaOperand.Remove(1);
                break;
            case 12:
                c.LastStand = true;
                c.StaminaOperand.Remove(-1);
                break;

        }
    }

    //Cards of Fighter
    public static Card card1 = new Card("Cleave", 1);
    public static Card card2 = new Card("Buckler", 2);
    public static Card card3 = new Card("Charge", 3);
    public static Card card4 = new Card("Throw", 4);
    public static Card card5 = new Card("Mighty Will", 5);
    public static Card card6 = new Card("Guts", 6);
    public static Card card7 = new Card("Blocking", 7);
    public static Card card8 = new Card("Sprint Strike", 8);
    public static Card card9 = new Card("Sacrifice", 9);
    public static Card card10 = new Card("Just Blocking", 10);
    public static Card card11 = new Card("Zealous", 11);
    public static Card card12 = new Card("Last Stand", 12);
}
