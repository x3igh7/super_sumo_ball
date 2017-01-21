using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{

    //Check every frame for player input
    //Apply input every frame as movement
    private Rigidbody rb;
    public float speed;  //should show up in the inspector
    public float jumpForce; //should show up in the inspector
    private Vector3 startPosition; //save the starting position of the player

    //Called on the first frame the script is active, often first frame of game
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = GetComponent<Rigidbody>().position;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump1"))
        {
            Jump();
        }
    }

    public void Jump()
    {
        if(GetComponent<Rigidbody>().position.y == startPosition.y)
        {
            Vector3 up = Vector3.up;
            rb.AddForce(up * jumpForce, ForceMode.Impulse);
        }
        
    }

    private void FixedUpdate()  //called before applying physics, movement code
    {
        float moveHorizontal = Input.GetAxis("Horizontal1");
        float moveVertical = Input.GetAxis("Vertical1");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);  //determine force added to ball

        rb.AddForce(movement * speed);
    }    
}
