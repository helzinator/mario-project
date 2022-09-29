using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpAndFall : MonoBehaviour
{
    public float jumpForce;
    public float jumpGravity;
    public float fallGravity;
    public float jumpCancellingGravity;
    public bool wantToJump, isJumping, jumpButtonIsPressed, isOnGround;
    public Animator animator;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        ManageInput();

        ManageJumpAndFall();
    }

    void SyncAnimation()
    {
        animator.SetBool("isJumping", isJumping);
    }

    void ManageInput()
    {
        wantToJump = Input.GetKey(KeyCode.Space);
        jumpButtonIsPressed = Input.GetKey(KeyCode.Space);
    }

    void ManageJumpAndFall()
    {
        if(wantToJump && isOnGround)
        {
            isOnGround = false;
            isJumping = true;
            wantToJump = false;
            rb.gravityScale = jumpGravity;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if(rb.velocity.y < 0)
        {
            rb.gravityScale = fallGravity;
        }

        else if (isJumping && !jumpButtonIsPressed)
        {
            rb.gravityScale = jumpCancellingGravity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.GetContact(0).normal.y >= 0.5f)
        {
            isJumping = false;
            isOnGround = true;
        }
    }

    //use overlap box all to check if there is something beneath you (for homework)
}
