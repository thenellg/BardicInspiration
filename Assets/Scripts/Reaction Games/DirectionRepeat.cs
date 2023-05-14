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
    public bool selection = true;
    public int action = 0;
    public bool end = false;

    public float vertical;
    public float horizontal;
    public Directions playerInput = Directions.None;

    public MagicMinigameHandler minigameHandler;

    // Start is called before the first frame update
    void Start()
    {
        minigameHandler = FindObjectOfType<MagicMinigameHandler>();
        resetDirections();
    }

    void Update()
    {   
        if(action >= 4)
        {
            end = true;
            selection = false;
        }

        if (selection)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if ((Mathf.Abs(horizontal) > 0.0001f || Mathf.Abs(vertical) > 0.0001f))
            {
                if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                {
                    if (horizontal > 0f)
                        playerInput = Directions.Right;
                    else
                        playerInput = Directions.Left;
                }
                else
                {
                    if (vertical > 0f)
                        playerInput = Directions.Up;
                    else
                        playerInput = Directions.Down;
                }
            }

            Debug.Log(playerInput);

            if (horizontal != 0 || vertical != 0)
            {
                if (randomDirections[action] == playerInput)
                {
                    selection = false;
                    visDirections[action].color = Color.red;
                    Invoke("resetSelection", 0.4f);
                    playerInput = Directions.None;
                    action++;
                    if (action >= 4)
                        end = true;
                }
                else
                {
                    horizontal = vertical = 0f;
                    action = 0;
                    playerInput = Directions.None;
                    selection = false;
                    Invoke("resetSelection", 0.4f);
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
        randomDirections.Clear();

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

        if (action >= 4)
            minigameHandler.triggerAttackSuccess();
        else
            minigameHandler.triggerAttackFail();

        Destroy(gameObject);

        Debug.Log("Success");
    }

    public int playDirectionReact()
    {
        StartCoroutine(directionReact());

        return 0;
        //return succeeded;
    }
}
