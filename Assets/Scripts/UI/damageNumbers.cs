using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class damageNumbers : MonoBehaviour
{
    public bool move = false;
    public float fadeSpeed;
    public float moveSpeed;
    public TextMeshProUGUI damageNumText;
    public RectTransform damageNumPosition;

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            damageNumText.color = new Color(damageNumText.color.r, damageNumText.color.g, damageNumText.color.b, damageNumText.color.a - (Time.deltaTime / fadeSpeed));
            damageNumPosition.position = new Vector3(damageNumPosition.position.x, damageNumPosition.position.y + moveSpeed, damageNumPosition.position.z);


            if (damageNumText.color.a <= 0)
                Destroy(this.gameObject);
        }
    }
}
