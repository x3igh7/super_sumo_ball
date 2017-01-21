using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Check every frame for player input
    //Apply input every frame as movement

    private Rigidbody rb;
    public float speed;  //should show up in the inspector

    //Called on the first frame the script is active, often first frame of game
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()  //called before applying physics, movement code
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);  //determine force added to ball

        rb.AddForce(movement * speed);
    }    
}
