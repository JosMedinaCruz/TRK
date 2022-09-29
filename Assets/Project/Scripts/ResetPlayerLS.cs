using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerLS : MonoBehaviour
{
    public static ResetPlayerLS instance;

    public Vector3 respawnPosition;

    public void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {   
            PlayerController.instance.gameObject.SetActive(false); //Evitar bug
            PlayerController.instance.transform.position = respawnPosition;
            PlayerController.instance.gameObject.SetActive(true);
        }
    }
}
