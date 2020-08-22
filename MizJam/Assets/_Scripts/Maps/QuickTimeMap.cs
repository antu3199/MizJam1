using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeMap : BasicMap
{

    public Reward rewardForCompleting;
    public FallingObject fallingObject;
    public CameraResetterInteractable cameraResetter;

    public float moveAmount = 3f;

    private float playerDefaultPosition = -5.5f;

    private bool gotHit = false;
    

    public override void DoInitialize() {
        this.mapType = MapType.QUICKTIME;
        this.fallingObject.Initialize(this.reference, this.OnGetHit);
        this.cameraResetter.Initialize(this.OnSkip);
        this.playerDefaultPosition = GameManager.Instance.gameController.mapScroller.playerDefaultPosition.position.x;
    }

    private void OnDestroy() {
        if (this.fallingObject != null) {
            Destroy(this.fallingObject.gameObject);
        }
    }

    public void TriggerFallingObject() {
        this.fallingObject.StartFalling();
        this.OverrideControls();
        GameManager.Instance.gameController.logController.TypeAnimationFunc(new LogMessage("A large object flies towards you!", null));
    }

    private void OverrideControls() {
        GameManager.Instance.gameController.playerController.OverrideJumpButton(this.MoveToTheLeft, "Dodge");
    }

    private void MoveToTheLeft() {
        GameManager.Instance.gameController.playerController.ResetOverrides();
        GameManager.Instance.gameController.logController.TypeAnimationFunc(new LogMessage("Nice dodge!", null));
        StartCoroutine(this.MoveToTheLeftCor());
    }


    private void OnGetHit() {
        this.CancelQTE();
        this.gotHit = true;
    }

    private void CancelQTE() {
        GameManager.Instance.gameController.playerController.ResetOverrides();
    }

    private IEnumerator MoveToTheLeftCor() {
        PlayerController player = GameManager.Instance.gameController.playerController;
        float target = GameManager.Instance.gameController.mapScroller.playerDefaultPosition.position.x;

        float lerpTime = 0.25f;
        float lerpCounter = 0;
        float start = player.transform.position.x;
        float end = target - this.moveAmount;
       
        while (lerpCounter < lerpTime) {
            float t = lerpCounter / lerpTime;
            float x = Mathf.Lerp(start, end, t);

            player.transform.position = new Vector3(x, player.transform.position.y, player.transform.position.z );

            yield return null;

            lerpCounter += Time.deltaTime;
        }

        player.transform.position = new Vector3(end, player.transform.position.y, player.transform.position.z );
    }

    protected virtual void OnSkip() {
        if (!gotHit) {
            PlayerController controller = GameManager.Instance.gameController.playerController;
            GameManager.Instance.gameController.StartCoroutine(GameManager.Instance.gameController.InstantiateRewardCor(rewardForCompleting, controller.transform.position + Vector3.up, controller.transform));
        }

        this.CancelQTE();

        List<Transform> otherTransforms = new List<Transform>();
        this.CallGoToNextMap();
        GameManager.Instance.gameController.mapScroller.ResetPlayerCamera(GameManager.Instance.gameController.playerController.moveableObject, otherTransforms, false);
    }
}
