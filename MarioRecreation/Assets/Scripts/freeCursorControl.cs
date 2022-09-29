using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeCursorControl : MonoBehaviour
{
    public BoxCollider2D FloorSensor;
    public float AccelForce = 1.0f;
    public float FrictionForce = 1.0f;
    public float MaxSpeed = 10.0f;
    public float JumpImpulseForce = 1f;
    public float JumpingGravityScale = 1f;
    public float FallingGravityScale = 1f;

    bool IsOnGround = true;

    Rigidbody2D rb;

    bool WantToGoUp, WantToGoDown, WantToGoLeft, WantToGoRight;

    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            Destroy(this);
        }
    }

    void Update()
    {
        rb.drag = FrictionForce; //adds a force of friction equal to the preset frictional force
        WantToGoRight = Input.GetKey(KeyCode.RightArrow); // held down
        WantToGoLeft = Input.GetKey(KeyCode.LeftArrow);
        
        Collider2D[] Hits = Physics2D.OverlapBoxAll(FloorSensor.transform.position, FloorSensor.bounds.extents, 0);
        for (int i = 0; i < Hits.Length; ++i)
            {
                 IsOnGround = true;
            }

        if (IsOnGround)
        {
            rb.gravityScale = 1;
            if (Input.GetKeyDown(KeyCode.Space)) //pressed quickly
            {
                Jump();
            }
        }
        else
        {
            if (rb.velocity.y <= 0f)
            {
                rb.gravityScale = FallingGravityScale;
            }
        }

    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * JumpImpulseForce, ForceMode2D.Impulse);
        rb.gravityScale = JumpingGravityScale;
        IsOnGround = false;
    }

    private void FixedUpdate()
    {
        
        if (WantToGoRight)
        {

            rb.AddForce(Vector2.right * AccelForce * Time.deltaTime, ForceMode2D.Force);
        }
        if (WantToGoLeft)
        {

            rb.AddForce(Vector2.left * AccelForce * Time.deltaTime, ForceMode2D.Force);
        }

        if (rb.velocity.sqrMagnitude > Mathf.Pow(MaxSpeed, 2))  // must normalize vector to have a length 1, so we can make it exactly the same size. 
        {
            rb.velocity = rb.velocity.normalized * MaxSpeed;
        }
    }
}
