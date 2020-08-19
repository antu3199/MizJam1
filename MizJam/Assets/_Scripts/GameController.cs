using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    
    public MapScroller mapScroller;
    public LogController logController;
    public RewardObjectAnimator rewardObjectAnimatorPrefab;
    public TopBar topBar;


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

    public void InstantiateReward(Reward reward, Vector3 position, Transform parent = null) {
        RewardObjectAnimator rewardObject = Instantiate(this.rewardObjectAnimatorPrefab, parent) as RewardObjectAnimator;
        rewardObject.transform.position = position;
        rewardObject.Initialize(reward);
    }
}
