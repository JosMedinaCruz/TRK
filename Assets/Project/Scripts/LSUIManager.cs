using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LSUIManager : MonoBehaviour
{
    public static LSUIManager instance;

    public TextMeshProUGUI lNameText;

    public GameObject lNamePanel;

    public TextMeshProUGUI crystalText;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        
    }
}
