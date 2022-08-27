using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    //Animator

    public bool moving = false;
    public bool moveBack = false;
    public float AddX;
    public float AddY;
    public float MoveBackSpeed = 0.1f;
    public Vector3 tileLocation;

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            this.transform.position = new Vector3(this.transform.position.x + AddX, this.transform.position.y + AddY, this.transform.position.z);
        }
        else if (moveBack)
        {
            var step = MoveBackSpeed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, tileLocation, step);
        }

        if(this.transform.position == tileLocation)
        {
            moving = false;
            moveBack = false;
        }
    }

    public void setDamageMove(float x, float y)
    {
        AddX = x;
        AddY = y;
        moving = true;
        moveBack = false;
    }

    public void setDamageMoveBack(Vector3 tile)
    {
        moving = false;
        tileLocation = tile;
        moveBack = true;
    }
}
