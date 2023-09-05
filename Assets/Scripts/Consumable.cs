using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Consumable : MonoBehaviour
{

    [JsonProperty] public string nameConsumable;
    [JsonProperty] public int level;
    [JsonProperty] public string image;
    [JsonProperty] public string abilityText;

    // Start is called before the first frame update
    void Start()
    {
        nameConsumable = "Meat";
        level = 1;
        image = "Meat.png";
        abilityText = "Placeholder text";
    }
}
