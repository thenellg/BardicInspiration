using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterCreator : MonoBehaviour
{
    private DiceFunctionality roll;
    private int rolledNum = 0;
    public string nextLevel;
    
    [Header("Text")]
    public TextMeshProUGUI visName;
    public TextMeshProUGUI visSpeed;
    public TextMeshProUGUI visAttack;
    public TextMeshProUGUI visDefense;
    public TextMeshProUGUI visSpecial;

    [Header("Buttons")]
    public GameObject buttonAttack;
    public GameObject buttonSpeed;
    public GameObject buttonSpecial;
    public GameObject buttonDefense;
    public GameObject rollButton;
    public GameObject playButton;


    public CharStat character = new CharStat();
    public int characterID = 0;
    private GameSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        roll = FindObjectOfType<DiceFunctionality>();
        settings = FindObjectOfType<GameSettings>();
    }

    void setExtra()
    {
        character.health = (character.attack + character.speed + character.defense + character.special) /2;
        character.attackRangeMin = 1;
        while(character.attackRangeMax < character.attackRangeMin)
        {
            character.attackRangeMax = Random.Range(1, 2);
        }
        character.characterName = visName.text;
    }

    private void Update()
    {
        visSpeed.text = character.speed.ToString();
        visAttack.text = character.attack.ToString();
        visDefense.text = character.defense.ToString();
        visSpecial.text = character.special.ToString();
    }

    private void checkStats()
    {
        if(character.attack != 0 && character.speed != 0 && character.defense != 0 && character.special != 0)
        {
            rollButton.SetActive(false);
            playButton.SetActive(true);
        }
    }

    public void setSpeed()
    {
        character.speed = rolledNum;
        rolledNum = 0;
        Destroy(buttonSpeed);
        checkStats();
    }
    public void setAttack()
    {
        character.attack = rolledNum; 
        rolledNum = 0;
        Destroy(buttonAttack);
        checkStats();
    }
    public void setDefense()
    {
        character.defense = rolledNum; 
        rolledNum = 0;
        Destroy(buttonDefense);
        checkStats();
    }
    public void setSpecial()
    {
        character.special = rolledNum; 
        rolledNum = 0;
        Destroy(buttonSpecial);
        checkStats();
    }

    public void rollDice()
    {
        rolledNum = roll.getRandomNumber(20, true);
    }

    public void loadLevel()
    {
        settings.customCharacterID = characterID;
        settings.character = character;

        SceneManager.LoadScene(nextLevel);//, LoadSceneMode.Additive);
    }

}
