using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject cpON, cpOFF; //Checkpoints activacion

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.SetSpawnPoint(transform.position);

            //Lista de checkpoints
            Checkpoint[] allCP = FindObjectsOfType<Checkpoint>(); //Busca a todos los checkpoints de la escena
            for(int i = 0; i < allCP.Length; i++)
            {
                allCP[i].cpOFF.SetActive(true); //Se descativa
                allCP[i].cpON.SetActive(false); //se desactivan los checkpoints que no son el actual
            }

            cpOFF.SetActive(false);
            cpON.SetActive(true);
        }
    }
}
