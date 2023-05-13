using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var position = new Vector3(Random.Range(-5.0f, 10.0f), 0, Random.Range(-5.0f, 10.0f));   
     
        this.transform.position = position;
        
    }
}
