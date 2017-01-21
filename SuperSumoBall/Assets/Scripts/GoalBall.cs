using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class GoalBall : MonoBehaviour
    {

        private Rigidbody rb;
        private Vector3 startPosition; //save the starting position of the player

        //Called on the first frame the script is active, often first frame of game
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            startPosition = GetComponent<Rigidbody>().position;
        }

        private void Update()
        {
            if(rb.position.y < -15)
            {
                rb.position = startPosition;
                rb.velocity = new Vector3();
            }
        }


    }
}
