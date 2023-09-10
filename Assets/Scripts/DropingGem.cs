using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropingGem : MonoBehaviour
{
    public Sprite[] gems;

    public SpriteRenderer gemSprite;

    public Image gemImage;

    public float rotateSpeed;

    public int rotateDir;

    private float fallSpeed;
    private float sizeVariant;

    // Start is called before the first frame update
    void Start()
    {
        
       
    }
    public void SetupGem()
    {
        sizeVariant = Random.Range(70f, 150f);
        fallSpeed = Random.Range(1000f, 2500f);
        var random = new System.Random();
        rotateDir = random.Next(0, 2);
        rotateSpeed = Random.Range(1, 5);
        this.gameObject.transform.localScale = new Vector3 (sizeVariant, sizeVariant, 0);
        var position = new Vector3(Random.Range(1, 1200), 750, 0);
        Quaternion rotation = Quaternion.Euler(Time.deltaTime * rotateSpeed * rotateDir, 0, 0);
        gemImage.sprite = gems[Random.Range(0, gems.Length)];
        this.gameObject.transform.SetPositionAndRotation(position, rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.down * fallSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
        if (transform.localPosition.y < -800)
            Respawn();
    }

    void Respawn()
    {
        SetupGem();
    }
}
