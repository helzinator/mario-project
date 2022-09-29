using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WalkAndRun : MonoBehaviour
{
    public float maxWalkingSpeed;
    public float maxRunningSpeed;
    public float maxSpeed;
    public float accelForce;
    public bool wantToGoRight, wantToGoLeft, wantToRun, isSliding, crouch = false;
    public float dampingForce;
    public float desiredDampingForceModifier;
    public float accelForceModifier = 1f;

    private enum eDirection
    {
        None,
        Left,
        Right
    }

    private eDirection desiredDirection;
    private float currentSpeed;
    private float desiredDampingForce = 0.2f;
    private Animator anim;
    private SpriteRenderer SpriteRenderer;
    private float minSpeed = 0.007f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput();
        ManageMovement();
        SyncAnimation();
    }

    void ManageInput()
    {
        wantToGoLeft = Input.GetKey(KeyCode.LeftArrow);
        wantToGoRight = Input.GetKey(KeyCode.RightArrow);
        wantToRun = Input.GetKey(KeyCode.A);
        crouch = Input.GetKey(KeyCode.DownArrow);
    }

    void SyncAnimation()
    {
        anim.SetBool("isWalking", Mathf.Abs(currentSpeed) > minSpeed);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("isCrouching", crouch);
       
        if(currentSpeed < 0)
        {
            SpriteRenderer.flipX = true;
        }
        else if (currentSpeed > 0)
        {
            SpriteRenderer.flipX = false;
        }
    }

    void ManageMovement()
    {
        maxSpeed = maxWalkingSpeed;
        if(wantToRun)
        {
            maxSpeed = maxRunningSpeed;
        }
        if(wantToGoLeft)
        {
            if (currentSpeed >= -maxSpeed)
            {
                currentSpeed += -accelForce * accelForceModifier * Time.deltaTime;
            }
        }

        if(wantToGoRight)
        {
            if (currentSpeed <= maxSpeed)
            {
                currentSpeed += accelForce * accelForceModifier * Time.deltaTime;
            }
        }
        isSliding = (wantToGoLeft && currentSpeed > 0) || (wantToGoRight && currentSpeed < 0);

        if(isSliding)
        {
            desiredDampingForce = desiredDampingForceModifier;
        }

        currentSpeed = Mathf.Lerp(currentSpeed, 0, dampingForce * Time.deltaTime); //finds the middle betwen 2 values, and progressively goes towards it


        transform.position = transform.position + new Vector3(currentSpeed, 0, 0);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.GetContact(0).normal.x <= -0.5f) //makes sure the impact is coming from left and right, not the floor
        {
            currentSpeed = 0;
        }
    }
}
