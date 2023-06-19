using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MapHider : MonoBehaviour 
{
    // Start is called before the first frame update
    void Awake()
    {
       if (NetworkManager.Singleton.IsHost)  {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0));
       } else {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
       }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
