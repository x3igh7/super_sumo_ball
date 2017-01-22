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

        public int CurrentOwner = -1;

        private Rigidbody rb;
        private Vector3 startPosition; //save the starting position of the player
        public List<int> ScoreList;

        //Called on the first frame the script is active, often first frame of game
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            startPosition = GetComponent<Rigidbody>().position;
            CurrentOwner = -1;
        }

        private void Update()
        {
            if (rb.position.y < -15)
            {
                if (CurrentOwner > -1)
                {
                    //TODO:  Score one!
                    ScoreList[CurrentOwner] += 1;
                    print("Scoring for team: " + CurrentOwner + "and now its score is: " + ScoreList[CurrentOwner]);
                    CurrentOwner = -1;
                }
                rb.position = startPosition;
                rb.velocity = new Vector3();
            }
        }


    }
}
