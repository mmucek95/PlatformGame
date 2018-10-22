using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerLevel1 : MonoBehaviour {

    public float moveSpeed = 5f; // max speed parameter
    public float jumpForce = 6f; // max jump parameter
    private Rigidbody2D rigidBody; // rigid body reference
    public LayerMask groundLayer; // groundLayer reference
    public Animator animator; // animator reference
    private bool isWalking = false; // is the object walking now?
    private bool isFacingRight = true; // is the object facing right direction now
    private Vector2 startPosition;
    private float killOffset = 1f;
    private bool isDoorOpened = false;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }
    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        hasFallen();
        if (GameManager.insance.currentGameState == GameManager.GameState.GS_GAME)
        {
            isWalking = false;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            { // move avatar right
                if (transform.parent != null)
                    Unlock();
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                if (!isFacingRight)
                    Flip();
                isWalking = true;
            }
            else
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            { // move avatar left
                if (transform.parent != null)
                    Unlock();
                if (isFacingRight)
                    Flip();
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isWalking = true;
            }
            else
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (transform.parent != null)
                    Unlock();
                Jump(); // jump
            }
            animator.SetBool("isGrounded", isGrounded());
            animator.SetBool("isWalking", isWalking);
        }
    }

    void lostLife()
    {
        this.transform.position = startPosition;
    }

    bool hasFallen()
    {
        if (this.transform.position.y <= -10)
        {
            lostLife();
            GameManager.insance.lostLife();
            return true;
        }
        return false;
    }

    bool isGrounded()
    { // is avatar on the ground?
        return Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, groundLayer.value);

    }
    void Jump()
    { // do jump
        if (isGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jumping");
        }
    }
    private void Flip()
    { // flip avatar's facing direction
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            rigidBody.isKinematic = true;
            transform.parent = other.transform;
        }
    }
    private void Unlock()
    {
        rigidBody.isKinematic = false;
        transform.parent = null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("MovingPlatform"))
        {
            Unlock();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    { // avatar is just touching another object
        if (other.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            GameManager.insance.addKeys();
            if(GameManager.insance.keys >= 3)
            {
                isDoorOpened = true;
            }
        }
        if (other.CompareTag("Gem"))
        {
            GameManager.insance.addCoins();
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Meta"))
        {
            if(isDoorOpened)
            {
                GameManager.insance.LevelCompleted();
            }
            else
            {
                Debug.Log("You need all stars to win!");
            }
        }

        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.transform.position.y + killOffset <
            this.transform.position.y)
            {
                Debug.Log("Killed an enemy!");
            }
            else
            {
                lostLife();
                GameManager.insance.lostLife();
            }
        }
        }
}

