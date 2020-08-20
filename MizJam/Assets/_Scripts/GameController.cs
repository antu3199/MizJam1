﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    const float REWARD_RECEIVE_DELAY = 0.5f;
    
    public MapScroller mapScroller;
    public LogController logController;
    public RewardObjectAnimator rewardObjectAnimatorPrefab;
    public TopBar topBar;

    public PlayerController playerController;


    public void Start() {

        GameManager.Instance.gameController = this;
        this.Initialize();
    }

    public void Initialize() {
        this.mapScroller.Initialize();
        this.playerController.Initialize();
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

    public IEnumerator InstantiateRewardCor(List<Reward> rewards, Vector3 position, Transform parent = null) {
        foreach (Reward reward in rewards) {
            this.InstantiateReward(reward, position, parent);
            yield return new WaitForSeconds(REWARD_RECEIVE_DELAY);
        }
    }

    private void InstantiateReward(Reward reward, Vector3 position, Transform parent = null) {
        RewardObjectAnimator rewardObject = Instantiate(this.rewardObjectAnimatorPrefab, parent) as RewardObjectAnimator;
        rewardObject.transform.position = position;
        rewardObject.Initialize(reward);
    }
}
