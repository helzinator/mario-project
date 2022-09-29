using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 marioVelocity;
    public float sprintMultiplier;
    public LayerMask wallMask;
    public float jumpVelocityY;

    private bool walk, walkLeft, walkRight, jump, sprint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        UpdateMario();
    }

    void CheckInput()
    {
        bool inputLeft = Input.GetKey(KeyCode.LeftArrow);
        bool inputRight = Input.GetKey(KeyCode.RightArrow);
        bool inputSpace = Input.GetKeyDown(KeyCode.Space);
        bool inputShift = Input.GetKey(KeyCode.LeftShift);

        walk = inputLeft || inputRight;
        walkLeft = inputLeft && ! inputRight;
        walkRight = inputRight && ! inputLeft;
        sprint = inputShift;
        jump = inputSpace;
    }

    void UpdateMario()
    {
        //fetch reference to mario's transform and scale
        Vector3 marioPosition = transform.localPosition;
        Vector3 marioScale = transform.localScale;
    
        if (walk)
        {
            GetComponent<Animator>().SetBool("isRunning", true);
            if(walkLeft & ! sprint)
            {
                marioPosition -= marioVelocity * Time.deltaTime; //move to the left
                marioScale.x = -6.25f; // orient the mario to the left
            }

            else if(walkLeft && sprint)
            {
                marioPosition -= new Vector3(marioVelocity.x * sprintMultiplier * Time.deltaTime, marioVelocity.y, marioVelocity.z); //move to the left
                marioScale.x = -6.25f; // orient the mario to the left
            }

            else if(walkRight & ! sprint)
            {
                marioPosition += marioVelocity * Time.deltaTime;
                marioScale.x = 6.25f;
            }

            else if(walkRight && sprint)
            {
                marioPosition += new Vector3(marioVelocity.x * sprintMultiplier * Time.deltaTime, marioVelocity.y, marioVelocity.z);
                marioScale.x = 6.25f;
            }

            marioPosition = CheckWallRays(marioPosition, marioScale.x);
        }

        if(! walk)
        {
            GetComponent<Animator>().SetBool("isRunning", false);
        }

        //apply precreated reference to the actual transform and scale
        transform.localPosition = marioPosition;
        transform.localScale = marioScale;
    }

    Vector3 CheckWallRays(Vector3 pos, float direction)
    {
        //mario is 2 units tall and 1 unit wide, consier this when emitting raycasts
        Vector2 originTop = new Vector2(pos.x + direction * 0.4f, pos.y + 1f - 0.2f); //takes the raycast from the y coord of the top of Mario's head. 
        Vector2 originMiddle = new Vector2(pos.x + direction * 0.4f, pos.y); //middle of mario -- no need to change the y coord
        Vector2 originBottom = new Vector2(pos.x + direction * 0.4f, pos.y - 1f + 0.2f); // takess the raycast from the y coord of the bottom

        //method Physics2d.Raycast takes in 4 parameters: a Vector2 origin, a Vector2 direction, a float distance, and a layer mask for our ray.
        RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), marioVelocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(direction, 0), marioVelocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(direction, 0), marioVelocity.x * Time.deltaTime, wallMask);

        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
        {
            pos.x -= marioVelocity.x * Time.deltaTime * direction;
        }

        return pos;
    }

    void UpdateAnimationState()
    {

    }

}
