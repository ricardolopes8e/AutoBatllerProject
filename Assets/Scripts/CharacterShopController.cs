using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShopController : MonoBehaviour
{
    public TextMeshProUGUI cost;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI health;
    public Image characterImage;
    public Image glow;
    public GameObject saleGO;
    public TextMeshProUGUI saleCost;
    public Image lockImage;


    void Start()
    {
        saleCost.text = "-1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
