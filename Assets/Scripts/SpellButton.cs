using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellButton : MonoBehaviour
{
    public int spellNum;
    public BattleManager battleManager;

    public void startSpell()
    {
        if (battleManager.cursor.character.spells[spellNum] != null && battleManager.cursor.character.spellSlots >= battleManager.cursor.character.spells[spellNum].cost)
        {
            battleManager.currentSpell = battleManager.cursor.character.spells[spellNum];
            battleManager.beginSpellAttack();
        }
        else
        {
            Debug.Log("Not enough Spell Slots");
        }
    }

}
