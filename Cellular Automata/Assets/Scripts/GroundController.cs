using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

    public static GroundController instance;

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            DestoryObjects();
    }

    public void DestoryObjects()
    {
        Destroy(gameObject);
    }
}
