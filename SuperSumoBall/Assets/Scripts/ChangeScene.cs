using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class ChangeScene : MonoBehaviour
    {
        public void startScene1Players(int newScene)
        {
            SceneManager.LoadScene(newScene);
            PlayerPrefs.SetInt("Players", 1);
        }

        public void startScene2Players(int newScene)
        {
            SceneManager.LoadScene(newScene);
            PlayerPrefs.SetInt("Players", 2);
        }

        public void startScene3Players(int newScene)
        {
            SceneManager.LoadScene(newScene);
            PlayerPrefs.SetInt("Players", 3);
        }

        public void startScene4Players(int newScene)
        {
            SceneManager.LoadScene(newScene);
            PlayerPrefs.SetInt("Players", 4);
        }

}

     
