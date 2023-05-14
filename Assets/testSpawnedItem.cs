using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSpawnedItem : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(speed, 0, 0);
        
    }
}
