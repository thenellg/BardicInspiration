using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGame : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Bottom,
        Right,
        Top
    }

    [Space]
    [Header("To use Instantiate BlockGame prefab and call playBlockGame()")]
    [Space]

    public GameObject rotatingWall;
    public GameObject crystal;
    public GameObject outerSpell;

    public Direction currentDirection;
    public Direction outerDirection;
    public int succeeded = 0;

    public bool rotating = false;
    int rotateDirection = 90;

    public float spellDistance = 105f;

    // Start is called before the first frame update
    void Start()
    {
        resetDirection();
    }

    void resetDirection()
    {
        currentDirection = (Direction)Random.Range(0, 4);
        outerDirection = currentDirection;
        while(outerDirection == currentDirection)
            outerDirection = (Direction)Random.Range(0, 4);

        //Setting wall direction
        if (currentDirection == Direction.Left)
            rotatingWall.transform.rotation = Quaternion.Euler(0,0,-90);
        else if (currentDirection == Direction.Bottom)
            rotatingWall.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (currentDirection == Direction.Right)
            rotatingWall.transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (currentDirection == Direction.Top)
            rotatingWall.transform.rotation = Quaternion.Euler(0, 0, 180);

        if (outerDirection == Direction.Left)
            outerSpell.transform.localPosition = new Vector3(-spellDistance, 0f, 0f);
        else if (outerDirection == Direction.Bottom)
            outerSpell.transform.localPosition = new Vector3(0f, -spellDistance, 0f);
        else if (outerDirection == Direction.Right)
            outerSpell.transform.localPosition = new Vector3(spellDistance, 0f, 0f);
        else if (outerDirection == Direction.Top)
            outerSpell.transform.localPosition = new Vector3(0f, spellDistance, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotating)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                rotateDirection = -90;

                if ((int)currentDirection != 0)
                    currentDirection = (Direction)((int)currentDirection - 1);
                else
                    currentDirection = (Direction)3;

                StartCoroutine(rotate90());
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                rotateDirection = 90;

                if ((int)currentDirection != 3)
                    currentDirection = (Direction)((int)currentDirection + 1);
                else
                    currentDirection = (Direction)0;

                StartCoroutine(rotate90());
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                if (currentDirection == outerDirection)
                {
                    succeeded++;
                    resetDirection();
                }
            }
        }
    }

    IEnumerator rotate90()
    {
        rotating = true; 
        float timeElapsed = 0;
        Quaternion startRotation = rotatingWall.transform.rotation;
        Quaternion targetRotation = rotatingWall.transform.rotation * Quaternion.Euler(0, 0, rotateDirection);
        while (timeElapsed < 0.5f)
        {
            rotatingWall.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / 0.5f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        rotatingWall.transform.rotation = targetRotation;
        rotating = false;
    }

    IEnumerator blockGameplay()
    {
        while(succeeded < 3)
        {
            yield return null;
        }

        Debug.Log("Success");
    }

    public bool playBlockGame()
    {
        StartCoroutine(blockGameplay());

        if (succeeded == 3)
            return true;
        else
            return false;
    }
}
