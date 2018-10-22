using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour {
    private Rigidbody2D rigidBody;
    public float moveSpeed = 2f;
    private bool isMovingRight = true;
    public float XMin = 4.5f;
    public float XMax = 4.5f;
    private float startPositionX;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if (this.transform.position.x < startPositionX + XMax)
                MoveRight();
            else
            {
                isMovingRight = false;
                MoveLeft();
            }
        }
        else
        {
            if (this.transform.position.x > startPositionX - XMin)
                MoveLeft();
            else
            {
                isMovingRight = true;
                MoveRight();
            }
        }

    }

    void Awake()
    {
        startPositionX = this.transform.position.x;
        this.transform.position = new Vector2(Random.Range(startPositionX - XMin, startPositionX + XMax), this.transform.position.y);
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void MoveRight()
    {
        if (rigidBody.velocity.x < moveSpeed)
        {
            rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.right * 0.6f, ForceMode2D.Impulse);
        }
    }

    void MoveLeft()
    {
        if (rigidBody.velocity.x > -moveSpeed)
        {
            rigidBody.velocity = new Vector2(-moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.left * 0.6f, ForceMode2D.Impulse);
        }
    }
}
