using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;
    private float length;
    private float xPosition;
    [SerializeField] private float parallaxEffect;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;

    }
    void Update()
    {
        float distanceMoved =cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position=new Vector3(xPosition + distanceToMove, transform.position.y);
        if(distanceMoved > xPosition + length)
        {
            xPosition = xPosition + length;
        }
        
    }
}
