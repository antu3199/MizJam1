using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// A Singleton bridge to access data from across the application
public class GameManager : Singleton<GameManager> {
    public GameController gameController{get; set;} // Only set on game
    public AudioManager audio;

    void Awake() {
        if (FindObjectsOfType<GameManager>().Length >= 2) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        audio.Initialize();

        // Initialize with the field for debugging purposes
        if (SceneManager.GetActiveScene().name != "_MainMenu") {
           
        }
    }
}