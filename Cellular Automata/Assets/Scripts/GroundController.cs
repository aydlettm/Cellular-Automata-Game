using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

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
