using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{

    public string nameConsumable;
    public int level;
    public string image;
    public string abilityText;

    // Start is called before the first frame update
    void Start()
    {
        nameConsumable = "Meat";
        level = 1;
        image = "Meat.png";
        abilityText = "Placeholder text";
    }
}
