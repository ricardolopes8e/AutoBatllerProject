using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    public string PlayerName()
    {
        return Application.dataPath;
    }

    public string characterSpritesPath { get; private set; }
    public string consumableSpritesPath { get; private set; }

    private void Awake()
    {
        Debug.LogError(Application.dataPath);
        //characterSpritesPath = Path.Combine(Application.dataPath,"Images","Characters");
        //consumableSpritesPath = Path.Combine(Application.dataPath, "Images", "Consumables");

        characterSpritesPath = Application.dataPath + "/Images/Characters";
        consumableSpritesPath = Application.dataPath + "/Images/Consumables";
        Debug.LogError(consumableSpritesPath);
    }
}
