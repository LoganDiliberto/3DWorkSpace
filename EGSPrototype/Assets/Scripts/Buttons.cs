using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    private Scene currentScene;

    public void NextNight(){
        currentScene = SceneManager.GetActiveScene();
		Time.timeScale = 1f;
		SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
    }
}
