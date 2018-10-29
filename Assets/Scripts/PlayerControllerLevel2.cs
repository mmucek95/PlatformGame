using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerLevel2 : MonoBehaviour
{

    public float moveSpeed = 5f; // max speed parameter
    public float jumpForce = 2f; // max jump parameter
    private Rigidbody2D rigidBody; // rigid body reference
    public LayerMask groundLayer; // groundLayer reference
    public Animator animator; // animator reference
    private bool isWalking = true; // is the object walking now?
    private bool isFacingRight = true; // is the object facing right direction now
    private Vector2 startPosition;
    private float killOffset = 1f;
    private bool isDoorOpened = false;
    private int killedEnemies = 0;
    public AudioClip coinSound;
    private AudioSource source;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
        source = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.insance.currentGameState == GameManager.GameState.GS_GAME)
        {
            if (rigidBody.velocity.x < moveSpeed)
            {
                rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            }
            if ((Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space)) && isGrounded())
            {
                if (transform.parent != null)
                    Unlock();
                Jump();
            }
            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isGrounded", isGrounded());
        }
        else
        {
            if (rigidBody.velocity.x > 0)
                rigidBody.velocity = new Vector2(Mathf.Max(0f, rigidBody.velocity.x -
               rigidBody.velocity.x / 20f), rigidBody.velocity.y);
            else if (rigidBody.velocity.x < 0)
                rigidBody.velocity = new Vector2(Mathf.Min(0f, rigidBody.velocity.x -
               rigidBody.velocity.x / 20f), rigidBody.velocity.y);
        }
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
        if (other.CompareTag("MovingPlatform"))
        {
            Unlock();
        }
        else if (other.CompareTag("FallLevel"))
        {
            GameManager.insance.lostLife();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    { // avatar is just touching another object
        if (other.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            GameManager.insance.addKeys();
            if (GameManager.insance.keys >= 3)
            {
                isDoorOpened = true;
            }
        }
        if (other.CompareTag("Gem"))
        {
            GameManager.insance.addCoins();
            other.gameObject.SetActive(false);
            source.PlayOneShot(coinSound, AudioListener.volume);
        }
        if (other.CompareTag("Meta"))
        {
                GameManager.insance.LevelCompleted();
        }

        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.transform.position.y + killOffset <
            this.transform.position.y)
            {
                killedEnemies += 1;
            }
            else
            {
                GameManager.insance.lostLife();
            }
        }
    }
}

