using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 respawnPosition;

    public GameObject deathEffect;

    public int currentCrystal;

    public int levelEndMusic;

    public string levelToLoad; //nombre del nivel

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        respawnPosition = PlayerController.instance.transform.position;  // Se accede al PlayerController

        AddCrystal(0);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpase();
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnWaiter());
        HealthManager.instance.PlayerKilled();
    }

    public IEnumerator RespawnWaiter() // Corutina --> Para implementar una espera
    {
        PlayerController.instance.gameObject.SetActive(false);

        CameraController.instance.cmBrain.enabled = false; // Desactiva la cámara cuando muere 

        UIManager.instance.fadeToBlack = true;

        Instantiate(deathEffect, PlayerController.instance.transform.position + new Vector3(0f,2f,0f), PlayerController.instance.transform.rotation);//Aparecen los objetos en el mundo

        yield return new WaitForSeconds(2f);

        UIManager.instance.fadeFromBlack = true;
        
        PlayerController.instance.transform.position = respawnPosition;

        CameraController.instance.cmBrain.enabled = true; // Respawn
        PlayerController.instance.gameObject.SetActive(true);

        HealthManager.instance.ResetHealth();
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint;
        Debug.Log("Spawn set");
    }

    public void AddCrystal(int crystalsToAdd)
    {
        currentCrystal += crystalsToAdd;
        UIManager.instance.crystalText.text = "" + currentCrystal;
    }

    public void PauseUnpase()
    {
        if(UIManager.instance.pauseScreen.activeInHierarchy)
        {
            UIManager.instance.pauseScreen.SetActive(false);
            Time.timeScale = 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else //Si esta apagado
        {
            UIManager.instance.pauseScreen.SetActive(true);
            UIManager.instance.CloseOptions();

            Time.timeScale = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
        }
    }

    public IEnumerator LevelEndWaiter()
    {
        AudioManager.instance.PlayMusic(levelEndMusic);
        PlayerController.instance.stopMove = true;
       
        yield return new WaitForSeconds(3f);
        
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);

        if(PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_crystals"))//Si ya agarro cierta cantidad de cristales en el nivel
        {
            if(currentCrystal > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_crystals")) //Cristales mayores que en la escena actual
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_crystals", currentCrystal);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_crystals", currentCrystal);
        }


        SceneManager.LoadScene(levelToLoad);

    }
}
