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
    private int score = 0; // current gameplay score
    public Text scoreText;
    public bool win = false;
    private Vector2 startPosition;
    private float killOffset = 1f;
    private int lives = 3;
    private int keysToCollect = 3;
    private int collectedKeys = 0;
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
        scoreText.text = "Score: " + score.ToString();
        if(win)
        {
            return;
        }
        if(hasFallen())
        {
            lostLife();
            //scoreText.text = "Game over!";
        }
        isWalking = false;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        { // move avatar right
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            if (!isFacingRight)
                Flip();
            isWalking = true;
        }
        else
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        { // move avatar left
            if (isFacingRight)
                Flip();
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
        }
        else
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            Jump(); // jump
        animator.SetBool("isGrounded", isGrounded());
        animator.SetBool("isWalking", isWalking);
    }

    void lostLife()
    {
        this.transform.position = startPosition;
        lives--;
        Debug.Log("You lost a life!");
        if (lives <= 0)
        {
            scoreText.text = "Game over";
            Debug.Log("GameOver");
        }
    }

    bool hasFallen()
    {
        if (this.transform.position.y <= -10)
            return true;
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
    private void OnTriggerEnter2D(Collider2D other)
    { // avatar is just touching another object
        if (other.CompareTag("Key"))
        {
            score += 10;
            other.gameObject.SetActive(false);
            collectedKeys++;
            if (collectedKeys >= 3)
            {
                isDoorOpened = true;
            }
        }
        if (other.CompareTag("Gem"))
        {
            score += 10; // incease te score
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Meta"))
        {
            if(isDoorOpened)
            {
                Debug.Log("WIN!");
                scoreText.text = "You win!";
                win = true;
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
                score += 10;
                Debug.Log("Killed an enemy! Score: " + score);
            }
            else
            {
                lostLife();
            }
        }

        }
}

