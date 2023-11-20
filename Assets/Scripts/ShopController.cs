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
//using UnityEngine.UIElements;

public class ShopController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldLabel;
    private int goldCount;

    public List<List<Character>> organizedCharacters = new List<List<Character>>();
    public List<List<Consumable>> organizedConsumables = new List<List<Consumable>>();

    public List<CharacterTeamController> teamCharactersControllers;
    public List<CharacterShopController> shopCharactersControllers;
    public List<ConsumableShopController> consumableControllers;

    public Button rerollButton;
    public Button freezeButton;

    private int rerollCost = 1;
    public TextMeshProUGUI rerollCostLabel;

    private Color[] raritiesColors = new Color[5];

    private int[,] rarityTable = new int[9, 5] { {100,0,0,0,0},
                                            {100,0,0,0,0},
                                            {75,25,0,0,0},
                                            {55,30,15,0,0},
                                            {45,33,20,2,5},
                                            {25,40,30,5,0},
                                            {19,30,35,15,1},
                                            {16,20,35,25,4},
                                            {9,15,30,30,16}};

    private int saleOdds;

    #region level

    [SerializeField]
    private TextMeshProUGUI currentLevelLabel;
    private int currentLevel;

    [SerializeField]
    private TextMeshProUGUI currentExpLabel;
    private int currentExp;

    [SerializeField]
    private TextMeshProUGUI levelExpLabel;
    private int levelExp;

    [SerializeField]
    private GameObject levelExpGO;

    private int[] expPerLevel= new int[9] {2,4,10,20,32,40,60,80,-1};

    #endregion



    // Label strings to load
    private List<string> keys = new List<string>() { };

    


    // Operation handle used to load and release assets
    AsyncOperationHandle<IList<Sprite>> loadHandle;

    // Start is called before the first frame update
    void Start()
    {
        raritiesColors[0] = new Color(255, 255, 255, 0.5f);
        raritiesColors[1] = new Color(0, 255, 0, 0.5f);
        raritiesColors[2] = new Color(0, 0, 255, 0.5f);
        raritiesColors[3] = new Color(255, 0, 255, 0.5f);
        raritiesColors[4] = new Color(255, 220, 0, 0.5f);

        saleOdds = 5;
        currentLevel = 1;
        currentLevelLabel.text = currentLevel.ToString();
        currentExp = 0;
        currentExpLabel.text = currentExp.ToString();
        levelExp = expPerLevel[currentLevel - 1];
        levelExpLabel.text = levelExp.ToString();
        goldCount = 0;
        AddGold(900);
        goldLabel.text = goldCount.ToString();
        rerollCostLabel.text = rerollCost.ToString();
        levelExpGO.SetActive(true);

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

        for (int i = 1; i < 6; i++)
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

    public int GenerateCharacterRarity()
    {
        var r = new System.Random();
        int rInt;
        rInt = r.Next(0, 100) +1;
        int sum = 0;

        for (int i=0;i< 9;i++)
        {
            sum += rarityTable[currentLevel - 1, i];
            if (rInt <= sum)
            {
                return i;
            }
        }

        return 4;
    }


    public IEnumerator FillShop()
    {
        //yield return LoadAddressables();
        yield return new WaitForSeconds(0.01f);
        var r = new System.Random();
        int rInt;
        int generatedRarity;
        Character character = null;
        for (int i = 0; i < 5; i++)
        {
            generatedRarity = GenerateCharacterRarity();
            rInt = r.Next(0, organizedCharacters[generatedRarity].Count);
            
            character = organizedCharacters[generatedRarity][rInt];

            string pathAdressable = "Assets" + PlayerData.Instance.characterSpritesPath.Split("Assets")[1] + "/" + character.image;

            shopCharactersControllers[i].cost.text = character.cost.ToString();
            shopCharactersControllers[i].attack.text = character.attack.ToString();
            shopCharactersControllers[i].health.text = character.health.ToString();
            shopCharactersControllers[i].glow.color = raritiesColors[character.level - 1];
            rInt = r.Next(0, 100);
            if (rInt < saleOdds)
            {
                shopCharactersControllers[i].saleCost.text = Mathf.CeilToInt((float)character.cost / (float)2).ToString();
                shopCharactersControllers[i].saleGO.SetActive(true);
            }
            else
            {
                shopCharactersControllers[i].saleGO.SetActive(false);
                shopCharactersControllers[i].saleCost.text = "-1";
            }
            switch (i)
            {
                case 0:
                    AsyncOperationHandle<Sprite> SpriteHandle1 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle1.Completed += Sprite_Completed_CharShop1;
                    break;
                case 1:
                    AsyncOperationHandle<Sprite> SpriteHandle2 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle2.Completed += Sprite_Completed_CharShop2;
                    break;
                case 2:
                    AsyncOperationHandle<Sprite> SpriteHandle3 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle3.Completed += Sprite_Completed_CharShop3;
                    break;
                case 3:
                    AsyncOperationHandle<Sprite> SpriteHandle4 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle4.Completed += Sprite_Completed_CharShop4;
                    break;
                case 4:
                    AsyncOperationHandle<Sprite> SpriteHandle5 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle5.Completed += Sprite_Completed_CharShop5;
                    break;
            }
        }

        Consumable consumable = null;

        for (int i = 0; i < 3; i++)
        {
            generatedRarity = GenerateCharacterRarity();
            rInt = r.Next(0, organizedConsumables[generatedRarity].Count);

            consumable = organizedConsumables[generatedRarity][rInt];

            string pathAdressable = "Assets" + PlayerData.Instance.consumableSpritesPath.Split("Assets")[1] + "/" + consumable.image;

            consumableControllers[i].cost.text = consumable.cost.ToString();
            consumableControllers[i].glow.color = raritiesColors[consumable.level - 1];
            rInt = r.Next(0, 100);
            if (rInt < saleOdds)
            {
                consumableControllers[i].saleCost.text = Mathf.CeilToInt((float)consumable.cost / (float)2).ToString();
                consumableControllers[i].saleGO.SetActive(true);
            }
            else
            {
                consumableControllers[i].saleGO.SetActive(false);
                consumableControllers[i].saleCost.text = "-1";
            }

            switch (i)
            {
                case 0:
                    AsyncOperationHandle<Sprite> SpriteHandle1 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle1.Completed += Sprite_Completed_ConsumableShop1;
                    break;
                case 1:
                    AsyncOperationHandle<Sprite> SpriteHandle2 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle2.Completed += Sprite_Completed_ConsumableShop2;
                    break;
                case 2:
                    AsyncOperationHandle<Sprite> SpriteHandle3 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle3.Completed += Sprite_Completed_ConsumableShop3;
                    break;
            }

        }
    }

    public void SellCharacter(int position)
    {
        int gold = (int)(teamCharactersControllers[position].cost / 2) * teamCharactersControllers[position].rank;
        if (gold < 1)
            gold = 1;
        AddGold(gold);
        teamCharactersControllers[position].Sell();
    }

    public void BuyExp()
    {
        AddExp(4);
        RemoveGold(4);
        
    }

    private void AddExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= levelExp)
        {
            currentLevel++;
            currentLevelLabel.text = currentLevel.ToString();
            if (expPerLevel[currentLevel] > 0)
            {
                currentExp -= levelExp;
                currentExpLabel.text = currentExp.ToString();
                levelExp = expPerLevel[currentLevel];
                levelExpLabel.text = levelExp.ToString();
            }
            else
            {
                levelExpGO.SetActive(false);
            }
        }
        else
        {
            currentExpLabel.text = currentExp.ToString();
        }
    }


    private void Sprite_Completed_CharShop1(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            shopCharactersControllers[0].characterImage.sprite = handle.Result;
        }
    }
    private void Sprite_Completed_CharShop2(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            shopCharactersControllers[1].characterImage.sprite = handle.Result;
        }
    }
    private void Sprite_Completed_CharShop3(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            shopCharactersControllers[2].characterImage.sprite = handle.Result;
        }
    }
    private void Sprite_Completed_CharShop4(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            shopCharactersControllers[3].characterImage.sprite = handle.Result;
        }
    }
    private void Sprite_Completed_CharShop5(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            shopCharactersControllers[4].characterImage.sprite = handle.Result;
        }
    }

    private void Sprite_Completed_ConsumableShop1(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            consumableControllers[0].consumableImage.sprite = handle.Result;

        }
    }
    private void Sprite_Completed_ConsumableShop2(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            consumableControllers[1].consumableImage.sprite = handle.Result;

        }
    }
    private void Sprite_Completed_ConsumableShop3(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            consumableControllers[2].consumableImage.sprite = handle.Result;

        }
    }

}