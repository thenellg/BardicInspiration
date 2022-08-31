using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpTurnInfo : MonoBehaviour
{
    public GameObject turnInfo;
    public float generalScale = 0.66f;
    public float generalMovement = 54;

    public float largerScale = 0.86f;
    public float largerMovement = 62;

    public float constantX = 0;

    public void createUI(int numTurns)
    {
        float yLocation = 4;

        GameObject turn = Instantiate(turnInfo);
        turn.transform.SetParent(this.transform);
        turn.transform.localPosition = new Vector3(constantX, yLocation, 0);
        turn.transform.localScale = new Vector3(largerScale, largerScale, largerScale);
        yLocation -= largerMovement;
        turn.transform.SetSiblingIndex(0);

        for (int i = 1; i < numTurns; i++)
        {
            turn = Instantiate(turnInfo);
            turn.transform.SetParent(this.transform);
            turn.transform.localPosition = new Vector3(constantX, yLocation, 0);
            turn.transform.localScale = new Vector3(generalScale, generalScale, generalScale);

            yLocation -= generalMovement;
            //turn.transform.SetSiblingIndex(1);
        }
    }
}
