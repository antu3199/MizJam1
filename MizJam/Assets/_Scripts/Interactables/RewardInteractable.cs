using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardInteractable : PlayerInteractable
{

    public Reward reward;
    public float soundVolume = 1f;
    public AudioClip soundEffect;
    public override void Interact(MoveableObject controller) {
        GameManager.Instance.gameController.StartCoroutine(GameManager.Instance.gameController.InstantiateRewardCor(reward, controller.transform.position + Vector3.up, controller.transform));
        if (soundEffect != null) {
            GameManager.Instance.audio.PlayOneShot(soundEffect, soundVolume);
        }
        Destroy(this.gameObject);
    }
}
