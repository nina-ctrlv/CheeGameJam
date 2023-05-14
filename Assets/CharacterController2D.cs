using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction 
{
    Up,
    Down,
    Left,
    Right
}

public class CharacterController2D : MonoBehaviour
{
       // Start is called before the first frame update
    Direction curDirection = Direction.Up;
    private int numHit = 0; 
    
    void Start()
    {
        this.transform.position = new Vector3(-6.6f, -3.2f, -0.5f);
        // this.transform.position = new Vector3(0f, 0f, -0.5f);
    }

    // Update is called once per frame
    void Update()
    {

        switch(curDirection) {
            case Direction.Up:
                if (this.transform.position.y < 3.8) {
                    this.transform.position += new Vector3(0f, 4 * Time.deltaTime, 0f);
                } else {
                    this.curDirection = Direction.Right;
                }
                break;
            case Direction.Right:
                if (this.transform.position.x < 6.65) {
                    this.transform.position += new Vector3(4 * Time.deltaTime, 0f, 0f);
                } else {
                    this.curDirection = Direction.Down;
                }
                break;
            case Direction.Down:
                if (this.transform.position.y > -3.4) {
                    this.transform.position -= new Vector3(0f, 4 * Time.deltaTime, 0f);
                } else {
                    this.curDirection = Direction.Left;
                }
                break;
            case Direction.Left:
                if (this.transform.position.x > -6.6) {
                    this.transform.position -= new Vector3(4 * Time.deltaTime, 0f, 0f);
                } else {
                    this.curDirection = Direction.Up;
                }
                break;

        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Spiky ball");
        // Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        numHit += 1;

        if (numHit == 1) {
            Destroy(gameObject);
        }
    }
}
