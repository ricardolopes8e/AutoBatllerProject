
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellSlot : MonoBehaviour, IDropHandler
{
    public ShopController shopController;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.ToString().StartsWith("Char_"))
        {
            shopController.SellCharacter(System.Int32.Parse(eventData.pointerDrag.ToString().Split("_")[1]) -1);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.LogError(col.gameObject.name);
        if (col.gameObject.name == "Sell Area")
        {

        }
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogError(other.gameObject.name);
    }
}
