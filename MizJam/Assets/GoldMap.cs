using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMap : BasicMap
{

    public CameraResetterInteractable cameraResetter;
    public override void DoInitialize() {
        
        this.cameraResetter.Initialize(this.OnSkip);
    }
    protected virtual void OnSkip() {
        List<Transform> otherTransforms = new List<Transform>();
        this.CallGoToNextMap();
        GameManager.Instance.gameController.mapScroller.ResetPlayerCamera(GameManager.Instance.gameController.playerController.moveableObject, otherTransforms, true);
    }
}
