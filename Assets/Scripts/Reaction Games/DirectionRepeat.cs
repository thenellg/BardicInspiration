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
        Down
    }

    public List<Sprite> directionSprites = new List<Sprite>();
    public List<Image> visDirections = new List<Image>();

    public List<Directions> randomDirections = new List<Directions>();

    // Start is called before the first frame update
    void Start()
    {
        resetDirections();
    }

    void resetDirections()
    {
        for (int i = 0; i <= 3; i++)
        {
            int rand = Random.Range(0, 4);
            randomDirections.Add((Directions)rand);
            visDirections[i].sprite = directionSprites[rand];
        }
    }

}
