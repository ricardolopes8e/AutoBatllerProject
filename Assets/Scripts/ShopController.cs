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
    private Team team;



    [SerializeField] private TextMeshProUGUI goldLabel;
    private int goldCount;

    public List<List<Character>> organizedCharacters = new List<List<Character>>();
    public List<List<Consumable>> organizedConsumables = new List<List<Consumable>>();

    public Button rerollButton;
    public Button freezeButton;

    private int rerollCost = 1;
    public TextMeshProUGUI rerollCostLabel;

    private Color[] raritiesColors = new Color[5];

    #region shop

    [SerializeField] private TextMeshProUGUI characterShop1Cost;
    [SerializeField] private TextMeshProUGUI characterShop1Attack;
    [SerializeField] private TextMeshProUGUI characterShop1Health;
    [SerializeField] private Image characterShop1Sprite;
    [SerializeField] private Image characterShop1Glow;

    [SerializeField] private TextMeshProUGUI characterShop2Cost;
    [SerializeField] private TextMeshProUGUI characterShop2Attack;
    [SerializeField] private TextMeshProUGUI characterShop2Health;
    [SerializeField] private Image characterShop2Sprite;
    [SerializeField] private Image characterShop2Glow;

    [SerializeField] private TextMeshProUGUI characterShop3Cost;
    [SerializeField] private TextMeshProUGUI characterShop3Attack;
    [SerializeField] private TextMeshProUGUI characterShop3Health;
    [SerializeField] private Image characterShop3Sprite;
    [SerializeField] private Image characterShop3Glow;

    [SerializeField] private TextMeshProUGUI characterShop4Cost;
    [SerializeField] private TextMeshProUGUI characterShop4Attack;
    [SerializeField] private TextMeshProUGUI characterShop4Health;
    [SerializeField] private Image characterShop4Sprite;
    [SerializeField] private Image characterShop4Glow;

    [SerializeField] private TextMeshProUGUI characterShop5Cost;
    [SerializeField] private TextMeshProUGUI characterShop5Attack;
    [SerializeField] private TextMeshProUGUI characterShop5Health;
    [SerializeField] private Image characterShop5Sprite;
    [SerializeField] private Image characterShop5Glow;

    [SerializeField] private TextMeshProUGUI consumable1ShopCost;
    [SerializeField] private Image consumable1ShopGlow;
    [SerializeField] private Image consumableShop1Sprite;

    [SerializeField] private TextMeshProUGUI consumable2ShopCost;
    [SerializeField] private Image consumable2ShopGlow;
    [SerializeField] private Image consumableShop2Sprite;

    [SerializeField] private TextMeshProUGUI consumable3ShopCost;
    [SerializeField] private Image consumable3ShopGlow;
    [SerializeField] private Image consumableShop3Sprite;

    private int[,] rarityTable = new int[9, 5] { {100,0,0,0,0},
                                            {100,0,0,0,0},
                                            {75,25,0,0,0},
                                            {55,30,15,0,0},
                                            {45,33,20,2,5},
                                            {25,40,30,5,0},
                                            {19,30,35,15,1},
                                            {16,20,35,25,4},
                                            {9,15,30,30,16}};

    #endregion

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
        team = new Team();
        team = GetComponent<Team>();
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
            switch (i)
            {
                case 0:
                    characterShop1Cost.text = character.cost.ToString();
                    characterShop1Attack.text = character.attack.ToString();
                    characterShop1Health.text = character.health.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle1 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle1.Completed += Sprite_Completed_CharShop1;
                    characterShop1Glow.color = raritiesColors[character.level - 1];
                    break;
                case 1:
                    characterShop2Cost.text = character.cost.ToString();
                    characterShop2Attack.text = character.attack.ToString();
                    characterShop2Health.text = character.health.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle2 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle2.Completed += Sprite_Completed_CharShop2;
                    characterShop2Glow.color = raritiesColors[character.level - 1];
                    break;
                case 2:
                    characterShop3Cost.text = character.cost.ToString();
                    characterShop3Attack.text = character.attack.ToString();
                    characterShop3Health.text = character.health.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle3 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle3.Completed += Sprite_Completed_CharShop3;
                    characterShop3Glow.color = raritiesColors[character.level - 1];
                    break;
                case 3:
                    characterShop4Cost.text = character.cost.ToString();
                    characterShop4Attack.text = character.attack.ToString();
                    characterShop4Health.text = character.health.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle4 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle4.Completed += Sprite_Completed_CharShop4;
                    characterShop4Glow.color = raritiesColors[character.level - 1];
                    break;
                case 4:
                    characterShop5Cost.text = character.cost.ToString();
                    characterShop5Attack.text = character.attack.ToString();
                    characterShop5Health.text = character.health.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle5 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle5.Completed += Sprite_Completed_CharShop5;
                    characterShop5Glow.color = raritiesColors[character.level - 1];
                    break;
            }
        }

        Consumable consumable = null;

        for (int i = 0; i < 5; i++)
        {
            generatedRarity = GenerateCharacterRarity();
            rInt = r.Next(0, organizedConsumables[generatedRarity].Count);

            consumable = organizedConsumables[generatedRarity][rInt];

            string pathAdressable = "Assets" + PlayerData.Instance.consumableSpritesPath.Split("Assets")[1] + "/" + consumable.image;
            switch (i)
            {
                case 0:
                    consumable1ShopCost.text = consumable.cost.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle1 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle1.Completed += Sprite_Completed_ConsumableShop1;
                    consumable1ShopGlow.color = raritiesColors[consumable.level - 1];
                    break;
                case 1:
                    consumable2ShopCost.text = consumable.cost.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle2 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle2.Completed += Sprite_Completed_ConsumableShop2;
                    consumable2ShopGlow.color = raritiesColors[consumable.level - 1];
                    break;
                case 2:
                    consumable3ShopCost.text = consumable.cost.ToString();
                    AsyncOperationHandle<Sprite> SpriteHandle3 = Addressables.LoadAsset<Sprite>(pathAdressable);
                    SpriteHandle3.Completed += Sprite_Completed_ConsumableShop3;
                    consumable3ShopGlow.color = raritiesColors[consumable.level - 1];
                    break;
            }

        }
    }

    public void SellCharacter(int position)
    {
        
    }

    public void BuyExp()
    {
        currentExp += 4;
        RemoveGold(4);
        if (currentExp >= levelExp)
        {
            currentLevel++;
            currentLevelLabel.text = currentLevel.ToString();
            if (expPerLevel[currentLevel]>0)
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