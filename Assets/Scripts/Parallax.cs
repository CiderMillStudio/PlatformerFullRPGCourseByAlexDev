using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] float parallaxEffect; //this will determine the speed of parallax movement.
                                           //(the LOWER the value here, the faster it moves)
    float xPosition;
    float length; // we'll obtain this value from the sprite renderer of our parallax image

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        xPosition = transform.position.x;
    }

    private void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3 (xPosition + distanceToMove, 
            transform.position.y, transform.position.z);

        if (distanceMoved > xPosition + length)
        {
            xPosition = xPosition + length;
        }
        else if (distanceMoved < xPosition - length)
        {
            xPosition = xPosition - length;
        }
    }


}
