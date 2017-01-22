using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayWinner : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int winningPlayer;
        winningPlayer= PlayerPrefs.GetInt("Winner");

        //if (winningPlayer == 0)
        //{
        //    var winnerInstance = Instantiate(PLAYER1WIN, Vector3(5, 1, 0), Quaternion.identity);
        //}
        //else if (winningPlayer == 1)
        //{
        //    var winnerInstance = Instantiate(PLAYER2WIN, Vector3(5, 1, 0), Quaternion.identity);
        //}
        //else if (winningPlayer == 2)
        //{
        //    var winnerInstance = Instantiate(PLAYER3WIN, Vector3(5, 1, 0), Quaternion.identity);
        //}
        //else if (winningPlayer == 3)
        //{
        //    var winnerInstance = Instantiate(PLAYER4WIN, Vector3(5, 1, 0), Quaternion.identity);
        //}
    }
}
