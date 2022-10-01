using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public enum spellTypes
    {
        AreaOfEffect,
        Line,
        Single,
        Buff
    }

    public string spellName;
    public string spellDescription;

    public Sprite spellIcon;
    public spellTypes spellType;
    public int minSpellRange;
    public int maxSpellRange;

    public int attackDamage;
}
