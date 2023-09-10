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

    public List<List<Character>> organizedCharacters = new List<List<Character>>();

    #region shopCharacters

    public Button rerollButton;
    public Button freezeButton;

    private int rerollCost = 1;
    public TextMeshProUGUI rerollCostLabel;

    private bool lockSelected;

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

    #endregion


    // Label strings to load
    public List<string> keys = new List<string>() {};

    // Operation handle used to load and release assets
    AsyncOperationHandle<IList<Sprite>> loadHandle;

    // Start is called before the first frame update
    void Start()
    {
        
        currentLevel = 1;
        team = new Team();
        lockSelected = false;
        FillOrganizedCharacters();
        
        StartCoroutine(FillShop());
    }


    // Load Addressables by Label
    public IEnumerator LoadAddressables()
    {
        float x = 0, z = 0;
        loadHandle = Addressables.LoadAssetsAsync<Sprite>(
            keys,
            addressable =>
            {
                Debug.LogError("in");
            }, Addressables.MergeMode.Union, // How to combine multiple labels 
            false); // Whether to fail and release if any asset fails to load

        yield return loadHandle;
    }

    private void OnDestroy()
    {
        Addressables.Release(loadHandle);
        // Release all the loaded assets associated with loadHandle
        // Note that if you do not make loaded addressables a child of this object,
        // then you will need to devise another way of releasing the handle when
        // all the individual addressables are destroyed.
    }

    private void FillOrganizedCharacters()
    {
        var charactersSerialized = File.ReadAllText(PlayerData.Instance.characterSpritesPath + "/characters.json");
        var characters = JsonConvert.DeserializeObject<List<Character>>(charactersSerialized);

        List<Character> auxList;
        
        for (int i=1; i<6;i++)
        {
            auxList = new List<Character>();
            foreach (var character in characters)
            {
                if (character.level == i)
                {
                    auxList.Add(character);
                    Debug.LogError(keys);
                    Debug.LogError(character.nameCharacter);
                    keys.Add(character.nameCharacter);
                }
            }
            organizedCharacters.Add(auxList);
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

    public IEnumerator FillShop()
    {
        yield return LoadAddressables();
        var r = new System.Random();
        int rInt;
        Character character = null;
        for (int i=0; i<5;i++)
        {
            rInt = r.Next(0, organizedCharacters[currentLevel - 1].Count - 1);

            character = organizedCharacters[currentLevel - 1][rInt];

            AsyncOperationHandle<Sprite> spriteHandle = Addressables.LoadAssetAsync<Sprite>(PlayerData.Instance.characterSpritesPath + "/" + character.image);
            spriteHandle.Completed += Sprite_Completed;

            switch (i)
            {
                case 0:
                    characterShop1Level.text = character.level.ToString();
                    characterShop1Attack.text = character.attack.ToString();
                    characterShop1Health.text = character.health.ToString();
                    Debug.LogError(character.image);
                    characterShop1Sprite.sprite = Resources.Load<Sprite>(character.image);
                    break;
                case 1:
                    characterShop2Level.text = character.level.ToString();
                    characterShop2Attack.text = character.attack.ToString();
                    characterShop2Health.text = character.health.ToString();
                    break;
                case 2:
                    characterShop3Level.text = character.level.ToString();
                    characterShop3Attack.text = character.attack.ToString();
                    characterShop3Health.text = character.health.ToString();
                    break;
                case 3:
                    characterShop4Level.text = character.level.ToString();
                    characterShop4Attack.text = character.attack.ToString();
                    characterShop4Health.text = character.health.ToString();
                    break;
                case 4:
                    characterShop5Level.text = character.level.ToString();
                    characterShop5Attack.text = character.attack.ToString();
                    characterShop5Health.text = character.health.ToString();
                    break;
            }
            
        } 
    }

    public void DisableClickTeam()
    {

    }

    public void EnableClickTeam()
    {

    }

    public void LockClicked()
    {
        lockSelected = !lockSelected;
        if(lockSelected)
        {
            DisableClickTeam();
        }
        else
        {
            EnableClickTeam();
        }
        
    }

}
