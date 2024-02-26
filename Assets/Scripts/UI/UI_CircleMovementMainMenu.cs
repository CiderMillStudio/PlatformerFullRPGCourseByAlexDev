using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CircleMovementMainMenu : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float parallaxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(parallaxSpeed, 0, 0);
    }


}
