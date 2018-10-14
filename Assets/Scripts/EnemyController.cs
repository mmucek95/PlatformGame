using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Rigidbody2D rigidBody;
    public float moveSpeed = 2f;
    private bool isFacingRight = true;
    private bool isMovingRight = true;
    public Animator animator;
    public float XMin = 5.5f;
    public float XMax = 5.5f;
    public float killOffset = 1f;
    private bool isDead = false;

    private float startPositionX;
    // Use this for initialization
    void Awake () {
        startPositionX = this.transform.position.x;
        this.transform.position = new Vector2(Random.Range(startPositionX - XMin,startPositionX + XMax), this.transform.position.y);
        rigidBody = GetComponent<Rigidbody2D>();
        animator.SetBool("isDead", isDead);
        Flip();
    }

    // Update is called once per frame
    void Update () {
        if (isMovingRight)
        {
            if (this.transform.position.x < startPositionX + XMax)
                MoveRight();
            else
            {
                isMovingRight = false;
                MoveLeft();
                Flip();
            }
        }
        else
        {
            if (this.transform.position.x > startPositionX-XMin)
            MoveLeft();
        else
        {
                isMovingRight = true;
                MoveRight();
                Flip();
            }
        }

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

    private void Flip()
    { // flip avatar's facing direction
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.7f);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.transform.position.y >
            this.transform.position.y + killOffset)
            {
                Debug.Log("Enemy dead");
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

}
