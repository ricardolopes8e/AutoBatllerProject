using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;


public class ShopController : MonoBehaviour
{
    private Team team;

    public List<List<Character>> organizedCharacters;


    // Start is called before the first frame update
    void Start()
    {
        team = new Team();
        fillOrganizedCharacters();
    }

    private void fillOrganizedCharacters()
    {
        var charactersSerialized = File.ReadAllText(PlayerData.Instance.characterSpritesPath + "/characters.json");
        Debug.LogError(charactersSerialized);
        var characters = JsonConvert.DeserializeObject<List<Character>>(charactersSerialized);
        Debug.LogError(characters.Count);
    }
}
