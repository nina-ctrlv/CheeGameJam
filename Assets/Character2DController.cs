using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    public float MovementSpeed = 1;
    public float JumpForce = 1;

    public ProjectileBehavior ProjectilePrefab;
    public Transform LaunchOffset;

    public Rigidbody2D _ridgidbody;

    // Start is called before the first frame update
    void Start()
    {
        _ridgidbody = GetComponent<Rigidbody2D>();
    }



    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("OnTriggeredEnter");
        Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);

    }
}
