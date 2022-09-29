using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image blackScreen; 
    public float fadeSpeed;
    public bool fadeToBlack, fadeFromBlack;

    public TextMeshProUGUI healtText;
    public Image healthImage;

    public TextMeshProUGUI crystalText;

    public GameObject pauseScreen, optionsScreen;

    public Slider musicVolSlider, sfxVolSlider;

    public string mainMenu, levelSelect;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }


    void Update()
    {
        if(fadeToBlack) //Efecto de transicion al morir
        {
            blackScreen.color = new Color(blackScreen.color.r, 
            blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            
            if(blackScreen.color.a == 1f)
            {
                fadeToBlack = false; 
            }
        }    

        if(fadeFromBlack) //Efecto de transicion al morir Regresando a la trasparencia
        {
            blackScreen.color = new Color(blackScreen.color.r, 
            blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            
            if(blackScreen.color.a == 0f)
            {
                fadeFromBlack = false; 
            }
        }    
    }

    public void Resume()
    {
        GameManager.instance.PauseUnpase();
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }

    public void SetMusicLevel()
    {
        AudioManager.instance.SetMusicLevel();
    }

    public void SetSFXLevel()
    {
        AudioManager.instance.SetSFXLevel();
    }

}
