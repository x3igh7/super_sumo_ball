using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Check every frame for player input
    //Apply input every frame as movement
    private Rigidbody rb;
    public float speed;  //should show up in the inspector
    public float jumpForce; //should show up in the inspector
    public string jumpButton;
    public string horizontalButton;
    public string verticalButton;
    private AudioSource footSteps;
    private AudioSource impacts;
    private Vector3 startPosition; //save the starting position of the player

    //Called on the first frame the script is active, often first frame of game
    void Start()
    {
        footSteps = gameObject.AddComponent<AudioSource>();
        footSteps.clip = Resources.Load<AudioClip>("Sounds/Footsteps");
        impacts = gameObject.AddComponent<AudioSource>();
        impacts.clip = Resources.Load<AudioClip>("Sounds/BallLanding");
        rb = GetComponent<Rigidbody>();
        startPosition = GetComponent<Rigidbody>().position;
    }

    private void Update()
    {
        if(jumpButton == null)
        {
            return;
        }
        if (Input.GetButtonDown(jumpButton))
        {
            Jump();
        }
    }

    public void Jump()
    {
        if(GetComponent<Rigidbody>().position.y <= startPosition.y + 1)
        {
            if (impacts != null) { impacts.Play(); }
            Vector3 up = Vector3.up;
            rb.AddForce(up * jumpForce, ForceMode.Impulse);
        }
        
    }

    private void FixedUpdate()  //called before applying physics, movement code
    {
        float moveHorizontal = Input.GetAxis(horizontalButton);
        float moveVertical = Input.GetAxis(verticalButton);
        if( Mathf.Abs(moveHorizontal) > .75 || Mathf.Abs(moveVertical) > .75)
        {
            print("horiz = " + moveHorizontal + " and vert = " + moveVertical);
            if (footSteps != null && !footSteps.isPlaying)
            {
                footSteps.Play();
            }
        }
        else
        {
            if (footSteps != null && footSteps.isPlaying)
            {
                footSteps.Stop();
            }
        }
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);  //determine force added to ball

        rb.AddForce(movement * speed);
    }    
}
