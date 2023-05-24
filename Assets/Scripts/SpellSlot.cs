using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{
    public Color unused;
    public Color used;

    public Image visibleSpellSlot;

    private void Start()
    {
        visibleSpellSlot = this.GetComponent<Image>();
    }

    public void setUsed()
    {
        GetComponent<Image>().color = used;
    }


}
