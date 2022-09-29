using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject BalaInicio;
    public GameObject BalaPrefab;
    public float BalaVelocidad;
    
    public float shotRate = 0.5f;
    private float shotRateTime = 0;

    void Update()
    {
        if(Input.GetButton("Fire2"))
        {
        GameObject BalaTemporal = Instantiate(BalaPrefab, BalaInicio.transform.position, BalaInicio.transform.rotation) as GameObject;

        Rigidbody rb = BalaTemporal.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * BalaVelocidad);

        shotRateTime = Time.time + shotRate;

        Destroy(BalaTemporal, 2f);
        }
    }
}
