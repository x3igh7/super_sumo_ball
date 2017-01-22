using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GoalBall : MonoBehaviour
    {

        public int CurrentOwner = -1;
        public Text Leaderboard;
        public Text Timer;
        public float TimeLeft; 
        public List<int> ScoreList = new List<int>();

        private Rigidbody rb;
        private Vector3 startPosition; //save the starting position of the player
        private int playerCount;

        //Called on the first frame the script is active, often first frame of game
        void Start()
        {
            playerCount = PlayerPrefs.GetInt("Players");
            TimeLeft = 120;

            for (int i= 0; i<=playerCount; i++)
            {
                ScoreList.Add(0);
            }

            gameObject.GetComponent<Renderer>().material.color = Color.yellow;

            rb = GetComponent<Rigidbody>();
            startPosition = GetComponent<Rigidbody>().position;
            CurrentOwner = -1;
            UpdateLeaderboard();
        }

        private void Update()
        {
            TimeLeft -= Time.deltaTime;
            UpdateTimer();
            if (Mathf.RoundToInt(TimeLeft) == 0)
            {
                int winningPlayerNum = GetWinner();
                print("Player " + winningPlayerNum + "won the game.");
                // Gameover
                PlayerPrefs.SetInt("Winner", winningPlayerNum);
                SceneManager.LoadScene(2);
            }


            if (rb.position.y < -15)
            {
                if (CurrentOwner > -1)
                {
                    //TODO:  Score one!
                    ScoreList[CurrentOwner] += 3;
                    CurrentOwner = -1;
                    UpdateLeaderboard();
                }
                rb.position = startPosition;
                rb.velocity = new Vector3();
            }
        }

        public void UpdateTimer()
        {
            var timeRemaining = Mathf.RoundToInt(TimeLeft).ToString() + " seconds left!";
            Timer.text = timeRemaining;
        }

        public void UpdateLeaderboard()
        {
            List<int> order = new List<int>();
            for (int i = 1; i < ScoreList.Count; i++)
            {
                int listLength = order.Count;
                for (int j = order.Count - 1; j > -1; j--)
                {
                    if (ScoreList[i] <= ScoreList[order[j]])
                    {
                        order.Insert(j + 1, i);
                        break;
                    }
                }
                if (order.Count == listLength)
                {
                    order.Insert(0, i);
                }
            }
            string leaderText = "Leaderboard:";
            for (int i = 0; i < order.Count; i++)
            {
                leaderText += "\nPlayer " + order[i] + " (" + PlayerColor(order[i]) + ") -  " + ScoreList[order[i]].ToString();
            }
            Leaderboard.text = leaderText;
        }

        public int GetWinner()
        {
            var winner = 0;
            var highScore = 0;
            for (int i = 0; i < ScoreList.Count; i++)
            {
                if (ScoreList[i] > highScore)
                {
                    winner = i + 1;
                    highScore = ScoreList[i];
                } else if (ScoreList[i] == highScore) {
                    winner = 0;
                }
            }

            return winner;
        }

        private string PlayerColor(int PlayerNum)
        {
            
            if (PlayerNum == 1)
            {
                return "Blue";
            }
            else if (PlayerNum == 2)
            {
                return "Red";
            }
            else if (PlayerNum == 3)
            {
                return "White";
            }
            else if (PlayerNum == 4)
            {
                return "Black";
            }
            else
            {
                return "Yellow";
            }
        }
    }
}
