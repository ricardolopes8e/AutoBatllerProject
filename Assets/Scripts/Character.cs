using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    private string nameCharacter;
    private int attack;
    private int health;
    private int rank;
    private int level;
    //private int relic;
    private string image;
    private string abilityText;


    void Start()
    {
        nameCharacter = "placeHolder";
        attack = 1;
        health = 1;
        rank = 1;
        level = 1;
        image = "placeholder.png";
        abilityText = "Placeholder text";
    }

}
