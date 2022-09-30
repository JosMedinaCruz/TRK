using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LSLevelEntry : MonoBehaviour
{
    public string levelName, levelToCheck, displayName;

    public bool canLoadLevel;

    public GameObject mapPointActive, mapPointInactive;

    private bool _levelUnlocked;

    private bool levelLoading;

    private void Start()
    {
        if(PlayerPrefs.GetInt(levelToCheck + "_unlocked") == 1 || levelToCheck == "")//Cuando pasamos el nivel
        {
            mapPointActive.SetActive(true);
            mapPointInactive.SetActive(false);
            _levelUnlocked = true;
        }
        else
        {
            mapPointActive.SetActive(false);
            mapPointInactive.SetActive(true);
            _levelUnlocked = false;
        }

        if(PlayerPrefs.GetString("CurrentLevel") == levelName) //Mueve al personaje a la posicion del playerprefs
        {
            PlayerController.instance.transform.position = transform.position; 
            ResetPlayerLS.instance.respawnPosition = transform.position;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canLoadLevel && _levelUnlocked && !levelLoading) //Cuando pase por la esfera del LS y pase el nivel
        {
            StartCoroutine("LevelLoadWaiter");
            levelLoading = true;
        }
    }

     private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {   
            canLoadLevel = true;

            LSUIManager.instance.lNamePanel.SetActive(true);
            LSUIManager.instance.lNameText.text = displayName;

            if(PlayerPrefs.HasKey(levelName + "_crystals"))
            {
                LSUIManager.instance.crystalText.text = PlayerPrefs.GetInt(levelName + "_crystals").ToString();
            }
            else//Sino hemos agarrado cristales
            {
                LSUIManager.instance.crystalText.text = "???";
            }
        }
    }

     private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {   
            canLoadLevel = true;

            LSUIManager.instance.lNamePanel.SetActive(false);
        }
    }

    public IEnumerator LevelLoadWaiter()
    {
        PlayerController.instance.stopMove = true; //SE detiene
        UIManager.instance.fadeToBlack = true;

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(levelName);
        PlayerPrefs.SetString("CurrentLevel", levelName);//Cual es el nivel actual en el que se esta
    }
}
