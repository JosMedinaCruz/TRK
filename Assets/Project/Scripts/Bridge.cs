using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public string levelToUnlock;

    void Start()
    {
        if(PlayerPrefs.GetInt(levelToUnlock + "_unlocked") ==0)
        { 
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
