using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{

    private string nameConsumable;
    private int level;
    private string image;
    private string abilityText;

    // Start is called before the first frame update
    void Start()
    {
        nameConsumable = "Meat";
        level = 1;
        image = "Meat.png";
        abilityText = "Placeholder text";
    }
}
