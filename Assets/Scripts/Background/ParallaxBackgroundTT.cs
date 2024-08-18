using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundTT : MonoBehaviour
{
    private float startPos;

    [SerializeField]
    Transform mainCam;

    [SerializeField]
    Transform beginBG;

    [SerializeField]
    Transform middleBG;

    [SerializeField]
    Transform endBG;

    [SerializeField]
    private float parallaxEffect;

    [SerializeField]
    private float length;

   

    private void Start()
    {
        // Get the initial position
        startPos = transform.position.x;
    }

    private void Update()
    {
        
            MoveBackground();
        
        
    }

    private void MoveBackground()
    {
        // Calculate parallax effect
        
            float speedMove = mainCam.position.x * parallaxEffect;
            transform.position = new Vector3(startPos + speedMove, transform.position.y, transform.position.z);



        // If the camera has moved beyond the middle background, shift the background elements
        if (mainCam.position.x > middleBG.position.x + length )
        {
            // Move right
            ShiftBackground(Vector3.right);
        }
        else if (mainCam.position.x < middleBG.position.x - length )
        {
            // Move left
            ShiftBackground(Vector3.left);
        }
    }

    void ShiftBackground(Vector3 direction)
    {
        // Shift positions of the backgrounds based on direction
        if (direction == Vector3.right)
        {
            beginBG.position = endBG.position + Vector3.right * length;
            SwapPositionsWhenMoveRight();
        }
        else if (direction == Vector3.left)
        {
            endBG.position = beginBG.position + Vector3.left * length;
            SwapPositionsWhenMoveLeft();
        }
    }

    void SwapPositionsWhenMoveRight()
    {
        Transform temp = beginBG;
        beginBG = middleBG;
        middleBG = endBG;
        endBG = temp;
    }

    void SwapPositionsWhenMoveLeft()
    {
        Transform temp = endBG;
        endBG = middleBG;
        middleBG = beginBG;
        beginBG = temp;
    }
}
