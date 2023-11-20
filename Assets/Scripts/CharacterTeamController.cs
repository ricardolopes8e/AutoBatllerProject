using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTeamController : MonoBehaviour
{
    [NonSerialized] public int cost;
    [NonSerialized] public int rank;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI health;
    public Image characterImage;
    [NonSerialized] public string description;
    public Image glow;
    public GameObject characterGO;
    public GameObject noCharacterGO;
    public GameObject rank1Image;
    public GameObject rank2Image;
    public GameObject rank3Image;
    public bool empty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Sell()
    {
        characterGO.SetActive(false);
        noCharacterGO.SetActive(true);
    }
}
