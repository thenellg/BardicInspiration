using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockGameBlock : MonoBehaviour
{
    public Sprite moss;
    public Sprite noMoss;
    public Color brickColor;


    void Start()
    {
        Image block = GetComponent<Image>();

        int rand = Random.Range(1, 10) % 2;

        if (rand == 0)
            block.sprite = moss;
        else
            block.sprite = noMoss;

        block.color = brickColor;
    }
}
