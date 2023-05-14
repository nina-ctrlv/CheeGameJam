using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float Speed = 2.5f;

    void Update()
    {
        transform.position -= transform.right * Time.deltaTime * Speed;
    }


    // private void OnCollisionEnter2D(CircleCollider2D collission) {
    //     Destroy(gameObject);
    // }
}
