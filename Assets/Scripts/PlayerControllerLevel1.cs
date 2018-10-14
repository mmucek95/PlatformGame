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
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    // Use this for initialization
    void Start()
    {
        setScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        if(win)
        {
            return;
        }
        if(hasFallen())
        {
            scoreText.text = "Game over!";
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
        if (other.CompareTag("Gem"))
        {
            score += 10; // incease te score
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
            setScoreText();
        }
        if (other.CompareTag("Meta"))
        {
            Debug.Log("WIN!");
            scoreText.text = "You win!";
            win = true;
        }
    }

    void setScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}

