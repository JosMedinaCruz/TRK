using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPosition : MonoBehaviour
{
    private GameObject tracker;

    private Material grassMat;

    private Vector3 trackerPos;
   

    // Start is called before the first frame update
    void Start()
    {   
        grassMat = GetComponent<Renderer>().material;
        tracker = GameObject.Find("tracker");
        
    }

    // Update is called once per frame
    void Update()
    {
        trackerPos = tracker.GetComponent<Transform>().position;

        grassMat.SetVector("_trackerPosition", trackerPos);
    }
}
