using Assets.Scripts;
using UnityEngine;

public class LoadPlayers : MonoBehaviour {
    public GameObject player;
    public GameObject sumo;
    private int numberOfPlayers = 1;

	// Use this for initialization
	void Start () {
        numberOfPlayers = PlayerPrefs.GetInt("Players");

        for (int i = 1; i <= numberOfPlayers; i++)
        {
            var playerInstance = Instantiate(player, GetPlayerVector(i), Quaternion.identity);
            playerInstance.gameObject.name = "Player" + i;
            MonoBehaviour[] list = playerInstance.gameObject.GetComponents<MonoBehaviour>();

            var sumoInstance = Instantiate(sumo, GetPlayerVector(i), Quaternion.identity);

            foreach (var mb in list)
            {
                if (mb is PlayerController)
                {
                    (mb as PlayerController).PlayerNum = i;
                    (mb as PlayerController).jumpButton = "Jump" + i;
                    (mb as PlayerController).horizontalButton = "Horizontal" + i;
                    (mb as PlayerController).verticalButton = "Vertical" + i;
                    (mb as PlayerController).LinkSumo(sumoInstance);
                }
            }
        }
	}

    private Vector3 GetPlayerVector(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                return new Vector3(5, 1, 0);
            case 2:
                return new Vector3(-5, 1, 0);
            case 3:
                return new Vector3(0, 1, 5);
            case 4:
                return new Vector3(0, 1, -5);
            default:
                return new Vector3();
        }
    }
}
