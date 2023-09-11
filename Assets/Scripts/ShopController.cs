using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using Unity.Burst.CompilerServices;

using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Unity.IO.LowLevel.Unsafe;

public class ShopController : MonoBehaviour
{
    private Team team;

    private int currentLevel;

    [SerializeField] private TextMeshProUGUI goldLabel;
    private int goldCount;

    public List<List<Character>> organizedCharacters = new List<List<Character>>();
    public List<List<Consumable>> organizedConsumables = new List<List<Consumable>>();

    public Button rerollButton;
    public Button freezeButton;

    private int rerollCost = 1;
    public TextMeshProUGUI rerollCostLabel;

    #region shop

    [SerializeField] private TextMeshProUGUI characterShop1Level;
    [SerializeField] private TextMeshProUGUI characterShop1Attack;
    [SerializeField] private TextMeshProUGUI characterShop1Health;
    [SerializeField] private Image characterShop1Sprite;

    [SerializeField] private TextMeshProUGUI characterShop2Level;
    [SerializeField] private TextMeshProUGUI characterShop2Attack;
    [SerializeField] private TextMeshProUGUI characterShop2Health;
    [SerializeField] private Image characterShop2Sprite;

    [SerializeField] private TextMeshProUGUI characterShop3Level;
    [SerializeField] private TextMeshProUGUI characterShop3Attack;
    [SerializeField] private TextMeshProUGUI characterShop3Health;
    [SerializeField] private Image characterShop3Sprite;

    [SerializeField] private TextMeshProUGUI characterShop4Level;
    [SerializeField] private TextMeshProUGUI characterShop4Attack;
    [SerializeField] private TextMeshProUGUI characterShop4Health;
    [SerializeField] private Image characterShop4Sprite;

    [SerializeField] private TextMeshProUGUI characterShop5Level;
    [SerializeField] private TextMeshProUGUI characterShop5Attack;
    [SerializeField] private TextMeshProUGUI characterShop5Health;
    [SerializeField] private Image characterShop5Sprite;

    [SerializeField] private TextMeshProUGUI consumable1ShopLevel;
    [SerializeField] private Image consumableShop1Sprite;

    [SerializeField] private TextMeshProUGUI consumable2ShopLevel;
    [SerializeField] private Image consumableShop2Sprite;

    [SerializeField] private TextMeshProUGUI consumable3ShopLevel;
    [SerializeField] private Image consumableShop3Sprite;

    #endregion


    // Label strings to load
    private List<string> keys = new List<string>() { };

    // Operation handle used to load and release assets
    AsyncOperationHandle<IList<Sprite>> loadHandle;

    // Start is called before the first frame update
    void Start()
    {

        currentLevel = 1;
        goldCount = 10;
        goldLabel.text = goldCount.ToString();
        rerollCostLabel.text = rerollCost.ToString();
        team = new Team();
        FillOrganizedCharacters();
        FillOrganizedConsumables();

        StartCoroutine(FillShop());
    }

    private void FillOrganizedCharacters()
    {
        var charactersSerialized = File.ReadAllText(PlayerData.Instance.characterSpritesPath + "/characters.json");
        var characters = JsonConvert.DeserializeObject<List<Character>>(charactersSerialized);

        List<Character> auxList;

        for (int i = 1; i < 6; i++)
        {
            auxList = new List<Character>();
            foreach (var character in characters)
            {
                if (character.level == i)
                {
                    auxList.Add(character);
                    keys.Add(character.image);
                }
            }
            organizedCharacters.Add(auxList);
        }

    }

    private void FillOrganizedConsumables()
    {
        var consumablesSerialized = File.ReadAllText(PlayerData.Instance.consumableSpritesPath + "/consumables.json");
        var consumables = JsonConvert.DeserializeObject<List<Consumable>>(consumablesSerialized);

        List<Consumable> auxList;

        for (int i = 1; i < 4; i++)
        {
            auxList = new List<Consumable>();
            foreach (var consumable in consumables)
            {
                if (consumable.level == i)
                {
                    auxList.Add(consumable);
                    keys.Add(consumable.image);
                }
            }
            organizedConsumables.Add(auxList);
        }

    }

    private void Sprite_Completed(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Sprite result = handle.Result;
            // Sprite ready for use
        }
    }

    public void RerollShop()
    {
        RemoveGold(rerollCost);
        StartCoroutine(FillShop());
    }

    public void RemoveGold(int amount)
    {
        goldCount -= amount;
        goldLabel.text = goldCount.ToString();
        if (goldCount == 0)
        {
            rerollButton.gameObject.SetActive(false);
        }
    }

    public void AddGold(int amount)
    {
        goldCount += amount;
        goldLabel.text = goldCount.ToString();
        if (goldCount != 0)
        {
            rerollButton.gameObject.SetActive(true);
        }
    }


    public IEnumerator FillShop()
    {
        //yield return LoadAddressables();
        yield return new WaitForSeconds(0.01f);
        var r = new System.Random();
        int rInt;
        Character character = null;
        for (int i = 0; i < 5; i++)
        {
            rInt = r.Next(0, organizedCharacters[currentLevel - 1].Count);
            if (rInt == organizedCharacters[currentLevel - 1].Count)
                rInt--;

            character = organizedCharacters[currentLevel - 1][rInt];

            string pathAdressable = "Assets" + PlayerData.Instance.characterSpritesPath.Split("Assets")[1] + "/" + character.image;
            switch (i)
            {
                case 0:
                    characterShop1Level.text = character.level.ToString();
                    characterShop1Attack.text = character.attack.ToString();
                    characterShop1Health.text = character.health.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle1 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle1.Completed += Sprite_Completed_CharShop1;
                    break;
                case 1:
                    characterShop2Level.text = character.level.ToString();
                    characterShop2Attack.text = character.attack.ToString();
                    characterShop2Health.text = character.health.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle2 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle2.Completed += Sprite_Completed_CharShop2;
                    break;
                case 2:
                    characterShop3Level.text = character.level.ToString();
                    characterShop3Attack.text = character.attack.ToString();
                    characterShop3Health.text = character.health.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle3 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle3.Completed += Sprite_Completed_CharShop3;
                    break;
                case 3:
                    characterShop4Level.text = character.level.ToString();
                    characterShop4Attack.text = character.attack.ToString();
                    characterShop4Health.text = character.health.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle4 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle4.Completed += Sprite_Completed_CharShop4;
                    break;
                case 4:
                    characterShop5Level.text = character.level.ToString();
                    characterShop5Attack.text = character.attack.ToString();
                    characterShop5Health.text = character.health.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle5 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle5.Completed += Sprite_Completed_CharShop5;
                    break;
            }
        }

        Consumable consumable = null;

        for (int i = 0; i < 5; i++)
        {
            rInt = r.Next(0, organizedConsumables[currentLevel - 1].Count);
            if (rInt == organizedConsumables[currentLevel - 1].Count)
                rInt--;

            consumable = organizedConsumables[currentLevel - 1][rInt];

            string pathAdressable = "Assets" + PlayerData.Instance.consumableSpritesPath.Split("Assets")[1] + "/" + consumable.image;
            switch (i)
            {
                case 0:
                    consumable1ShopLevel.text = consumable.level.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle1 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle1.Completed += Sprite_Completed_ConsumableShop1;
                    break;
                case 1:
                    consumable2ShopLevel.text = character.level.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle2 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle2.Completed += Sprite_Completed_ConsumableShop2;
                    break;
                case 2:
                    consumable3ShopLevel.text = character.level.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle3 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle3.Completed += Sprite_Completed_ConsumableShop3;
                    break;
            }

        }
    }

    private void Sprite_Completed_CharShop1(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            characterShop1Sprite.sprite = handle.Result;

        }
    }
    private void Sprite_Completed_CharShop2(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            characterShop2Sprite.sprite = handle.Result;

        }
    }
    private void Sprite_Completed_CharShop3(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            characterShop3Sprite.sprite = handle.Result;

        }
    }
    private void Sprite_Completed_CharShop4(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            characterShop4Sprite.sprite = handle.Result;

        }
    }
    private void Sprite_Completed_CharShop5(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            characterShop5Sprite.sprite = handle.Result;

        }
    }

    private void Sprite_Completed_ConsumableShop1(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            consumableShop1Sprite.sprite = handle.Result;

        }
    }
    private void Sprite_Completed_ConsumableShop2(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            consumableShop2Sprite.sprite = handle.Result;

        }
    }
    private void Sprite_Completed_ConsumableShop3(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            consumableShop3Sprite.sprite = handle.Result;

        }
    }

}