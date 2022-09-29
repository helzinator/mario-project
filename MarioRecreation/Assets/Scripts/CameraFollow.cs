using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform mario;
    public Transform leftBound;
    public Transform rightBound;
    public float smoothDampTime = 0.15f;

    private float cameraWidth;
    private float cameraHeight;
    private float posMinX;
    private float posMaxX;
    private Vector3 smoothDampVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        cameraHeight = Camera.main.orthographicSize * 2; // half of the vertical viewing volume
        cameraWidth = cameraHeight * Camera.main.aspect; // aspect ratio is the width divided by the height. we define it in game mode

        float leftBoundWidth = leftBound.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        float rightBoundWidth = rightBound.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;

        posMinX = leftBound.position.x + leftBoundWidth + (cameraWidth / 2); //camera only starts moving after mario's position added with the width of the left bound and half of the camera's width has passed
        posMaxX = rightBound.position.x - rightBoundWidth - (cameraWidth / 2); // opposite for the right side
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (mario) // pos of camera should be mario's location if he is before the maxBound and after the minBound
        {
            float marioPosX = Mathf.Max(posMinX, Mathf.Min(posMaxX, mario.position.x));

            float xSmooth = Mathf.SmoothDamp(transform.position.x, marioPosX, ref smoothDampVelocity.x, smoothDampTime); //method that slowly changes the cameras position over time
            transform.position = new Vector3(xSmooth, transform.position.y, transform.position.z); //apply it to the camera's transform
        }        
    }
}
