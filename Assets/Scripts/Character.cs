using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character : MonoBehaviour
{

    [JsonProperty] public string nameCharacter;
    [JsonProperty] public int attack;
    [JsonProperty] public int health;
    [JsonProperty] public int rank;
    [JsonProperty] public int level;
    //private int relic;
    [JsonProperty] public string image;
    [JsonProperty] public string abilityText;


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
