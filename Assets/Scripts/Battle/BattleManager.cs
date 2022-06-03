using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> turnOrder;
    public List<GameObject> playerTeam;
    public List<GameObject> enemyTeam;
    public int turnNumber = 0;
    public ActionMenu actionMenu;
    public MouseController cursor;

    private void Start()
    {
        MouseController temp = FindObjectOfType<MouseController>();
    }
    public void isRoundOver()
    {
        if (enemyTeam.Count == 0)
        {
            //Game Over Win
        }
        else if (playerTeam.Count == 0)
        {
            //Game Over Lose
        }
    }

    public void onTurnSwap()
    {
        actionMenu.updateInfo();
    }
}
