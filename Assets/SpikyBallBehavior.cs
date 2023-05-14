using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBallBehavior : MonoBehaviour
{

    public float Speed = 2.5f;
    // public ProjectileBehavior ProjectilePrefab;
    // public Transform LaunchOffset;
    private int numHit = 0;

    private Rigidbody2D _ridgidbody;
    void Start()
    {
        _ridgidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //transform.position -= transform.right * Time.deltaTime * Speed;
        
    }


}
