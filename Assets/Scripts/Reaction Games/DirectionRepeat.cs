using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionRepeat : MonoBehaviour
{
    public enum Directions
    {
        Left,
        Right,
        Up,
        Down,
        None
    }

    public List<Sprite> directionSprites = new List<Sprite>();
    public List<Image> visDirections = new List<Image>();

    public List<Directions> randomDirections = new List<Directions>();
    bool selection = true;
    public int action = 0;
    public bool end = false;

    public float vertical;
    public float horizontal;
    public Directions input = Directions.None;

    // Start is called before the first frame update
    void Start()
    {
        resetDirections();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        if (selection)
        {
            Directions input = new Directions();

            if ((Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f))
            {
                if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                {
                    if (horizontal > 0f)
                        input = Directions.Right;
                    else
                        input = Directions.Left;
                }
                else
                {
                    if (vertical > 0f)
                        input = Directions.Up;
                    else
                        input = Directions.Down;
                }
            }

            if (horizontal != 0 || vertical != 0)
            {
                if (randomDirections[action] == input)
                {
                    selection = false;
                    visDirections[action].color = Color.red;
                    Invoke("resetSelection", 0.4f);
                    action++;
                    if (action >= 4)
                        end = true;
                }
                else
                {
                   resetDirections();
                }
            }
        }
    }

    void resetSelection()
    {
        selection = true;
    }

    void resetDirections()
    {
        //input = Directions.None;
        randomDirections.Clear();
        action = 0;

        for (int i = 0; i <= 3; i++)
        {
            int rand = Random.Range(0, 4);
            randomDirections.Add((Directions)rand);
            visDirections[i].sprite = directionSprites[rand];
            visDirections[i].color = Color.white;
        }
    }

    IEnumerator directionReact()
    {
        while (end == false)
        {
            yield return null;
        }

        Debug.Log("Success");
    }

    public int playDirectionReact()
    {
        StartCoroutine(directionReact());

        return 0;
        //return succeeded;
    }
}
