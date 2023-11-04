using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.LogError(col.gameObject.name);
        if (col.gameObject.name == "Sell Area")
        {

        }
    }
}
