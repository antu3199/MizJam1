using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    
    public MapScroller mapScroller;
    public LogController logController;


    public void Start() {

        GameManager.Instance.gameController = this;
        this.Initialize();
    }

    public void Initialize() {
        this.mapScroller.Initialize();
    }

    public void Update() {
        if (Input.GetKeyDown("1")) {
            GameManager.Instance.gameStorage.SaveGame();
        }

        if (Input.GetKeyDown("2")) {
            GameManager.Instance.gameStorage.DeleteSaveFile();
        }

        GameManager.Instance.gameState.AddGold(GameManager.Instance.gameState.GPS * Time.deltaTime);

        GameManager.Instance.gameStorage.timeSinceLastSave += Time.deltaTime;
        if (GameManager.Instance.gameStorage.timeSinceLastSave >= GameManager.Instance.gameStorage.autoSaveTime) {
            GameManager.Instance.gameStorage.SaveGame();
        }
    }
}
