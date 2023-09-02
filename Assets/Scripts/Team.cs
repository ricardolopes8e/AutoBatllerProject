using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Team : MonoBehaviour
{
    private Character[] characters;

    // Start is called before the first frame update
    void Start()
    {
        characters = new Character[5];
    }
}
