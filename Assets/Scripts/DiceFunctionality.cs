using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFunctionality : MonoBehaviour
{
    //There will be 2 types of dice. d20s and d6s. (If I expand this maybe also d4s and d8s)
    //This script will be called whenever and account requires a dice roll. It will randomly generate a number,
    //spawn in a dice for the animation of this for the player and then send back the actual number.
    //Once the animation is finished, the dice will fade and then the object will be deleted from the heirarchy.

    public int getRandomNumber(int diceType, bool isPlayer)
    {
        if (diceType <= 0)
            diceType = 1;

        int randNum = (Random.Range(1, diceType) + Random.Range(1, diceType))/2;
        
        if (isPlayer)
        {
            if(diceType == 6)
            {
                //spawn d6 prefab
                //Trigger animations
                //Making individual dice animations will set the animator Int to randNum
            }
            else if(diceType == 20)
            {
                //spawn d20 prefab
                //Trigger animations
                //Making individual dice animations will set the animator Int to randNum
            }
        }
        
        Debug.Log(randNum);
        return randNum;
    }
}
