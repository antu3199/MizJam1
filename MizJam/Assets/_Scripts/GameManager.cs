using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// A Singleton bridge to access data from across the application
public class GameManager : Singleton<GameManager> {
    public GameController gameController{get; set;} // Only set on game
    public AudioManager audio;

    public GameStorage gameStorage;
    public GameDataManager gameData;

    public GameState gameState;

    void Awake() {
        if (FindObjectsOfType<GameManager>().Length >= 2) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        gameData.Initialize();
        gameState.Initialize();
        gameStorage.Initialize();
        audio.Initialize();

        // Initialize with the field for debugging purposes
        if (SceneManager.GetActiveScene().name != "_MainMenu") {
           
        }
    }

    public void AfterLoad() {
        gameState.AfterLoad();
    }
}