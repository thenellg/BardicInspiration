using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    public void showSelectArrow()
    {
        Color temp = GetComponentInChildren<Image>().color;
        GetComponentInChildren<Image>().color = new Color(temp.r, temp.g, temp.b, 1);
    }

    public void hideSelectArrow()
    {
        Color temp = GetComponentInChildren<Image>().color;
        GetComponentInChildren<Image>().color = new Color(temp.r, temp.g, temp.b, 0);
    }

    private void OnMouseEnter()
    {
        showSelectArrow();
    }

    private void OnMouseExit()
    {
        hideSelectArrow();
    }
}
