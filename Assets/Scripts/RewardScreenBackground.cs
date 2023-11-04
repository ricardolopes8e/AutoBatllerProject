using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardScreenBackground : MonoBehaviour
{

    public Sprite[] images;

    public List<DropingGem> rainDroplets;

    public GameObject canvas;

    public DropingGem gemPrefab;

    public GameObject parentGO;

    public int amountOfGems = 35;

    // Start is called before the first frame update
    void Start()
    {
        // var createImage = Instantiate(images[Random.Range(0, images.Length)],canvas.transform) as GameObject;
        // rainDroplets.Add(createImage);


        //rainDroplets.Add(fallingImage);

        StartCoroutine(SetupGems());
    }

    public IEnumerator SetupGems()
    {
        float timeDelay;
        for (int i = 0; i < amountOfGems; i++)
        {
            var fallingImage = Instantiate(gemPrefab, new Vector3(Random.Range(1, 1500), 750, 0), Quaternion.identity);
            fallingImage.transform.parent = parentGO.transform;
            fallingImage.GetComponent<DropingGem>().SetupGem();
            timeDelay = Random.Range(0, 0.7f);
            yield return new WaitForSeconds(timeDelay);
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.LogError(transform.position);
    }
}
