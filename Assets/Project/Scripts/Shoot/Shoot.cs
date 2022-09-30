using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;
    
    public float shotForce = 1500;
    public float shotRate = 0.5f;
    
    private float shotRateTime = 0;

    void Update()
    {
        if(Input.GetButton("Fire2"))
        {
            if(Time.time > shotRateTime)
            {
                GameObject newBullet;

                newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

                newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * shotForce);

                shotForce = Time.time + shotRate;

                Destroy(newBullet, 2f);
            }
        }
    }
}
