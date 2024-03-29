using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMinigameHandler : MonoBehaviour
{
    public List<GameObject> minigames;
    public BattleManager battleManager;
    public MouseController cursor;
    public Camera cam;

    GameObject currentMinigame;

    // Start is called before the first frame update
    void Start()
    {
        battleManager = GetComponent<BattleManager>();
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        if(battleManager.currentSpell.minigameType == Spell.minigameTypes.Direction)
        {
            currentMinigame = Instantiate(minigames[0]);
            currentMinigame.transform.parent = battleManager.gameUI.transform;
            currentMinigame.transform.localScale = Vector3.one * 0.5f;
            currentMinigame.transform.position = cam.WorldToScreenPoint(cursor.character.returnMenuLocation().position);
            currentMinigame.GetComponent<DirectionRepeat>().playDirectionReact();
        }
        else if (battleManager.currentSpell.minigameType == Spell.minigameTypes.Circle)
        {
            currentMinigame = Instantiate(minigames[1]);
            currentMinigame.transform.parent = battleManager.gameUI.transform;
            currentMinigame.transform.localScale = Vector3.one;
            currentMinigame.transform.position = cam.WorldToScreenPoint(cursor.character.returnMenuLocation().position);
            currentMinigame.GetComponent<CircleMagic>().playCircleReact();
        }

    }

    public void triggerAttackFail()
    {
        battleManager.magicMiniGameSuccess = false;
        battleManager.magicAttack(cursor.magicRangeTiles);
    }

    public void triggerAttackSuccess()
    {
        battleManager.magicMiniGameSuccess = true;
        battleManager.magicAttack(cursor.magicRangeTiles);
    }
}
