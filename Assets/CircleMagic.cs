using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CircleMagic : MonoBehaviour
{
    public Transform activeCircle;
    public Transform goalCircle;
    public float actualSize;
    public float maxSize = 1.8f;
    public float minSize = 1f;
    public float shrinkingSpeed = 0.1f;

    public bool shrinking = false;

    public bool end = false;
    public int succeeded = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        setCircle();
    }

    void setCircle()
    {
        float rand = Random.Range(minSize, maxSize);
        activeCircle.localScale = new Vector3(rand, rand, rand);
        actualSize = rand;

        rand = Random.Range(0.5f, minSize - 0.1f);
        goalCircle.localScale = new Vector3(rand, rand, rand);


        shrinking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shrinking)
        {
            actualSize -= shrinkingSpeed * Time.deltaTime;
            activeCircle.localScale = new Vector3(actualSize, actualSize, actualSize);

            if(actualSize <= 0f)
            {
                end = true;
                shrinking = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && shrinking)
        {
            if (actualSize > 0f && actualSize <= goalCircle.localScale.x)
            {
                succeeded++;

                //if (succeeded >= 3)
                //    end = true;
                //else
                    setCircle();
            }
        }
    }
}
