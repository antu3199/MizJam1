using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public DealDamageInteractableObject dealDamageInteractableObject;
    public SpriteRenderer sprite;
    public float fallSpeed = 0.5f;

    public float rotationSpeed = 5f;

    private bool startFalling = false;

    private float xMoveSpeed = 1f;
    private float timeToFallToPlayer = 1f;

    public void Initialize(double reference, Action onHit) {
        this.dealDamageInteractableObject.Initialize(reference, onHit);
        this.transform.SetParent(null);
        Vector3 playerPos = GameManager.Instance.gameController.mapScroller.playerDefaultPosition.position;
        this.timeToFallToPlayer = (this.transform.position.y - playerPos.y) / this.fallSpeed;
        this.xMoveSpeed = (this.transform.position.x - playerPos.x) / this.timeToFallToPlayer;
    }

    public void StartFalling() {
        this.startFalling = true;
        //this.StartCoroutine(this.MoveToTheLeftCor());
    }


    // Update is called once per frame
    void Update()
    {
        if (this.startFalling) {
            float fallDelta = -fallSpeed * Time.deltaTime;
            float horizontalDelta = -this.xMoveSpeed * Time.deltaTime;
            this.transform.position = new Vector3(this.transform.position.x + horizontalDelta, this.transform.position.y + fallDelta, this.transform.position.z);
        }

        this.sprite.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

/*
    private IEnumerator MoveToTheLeftCor() {
        Debug.Log("Time to fall to player: " + this.timeToFallToPlayer);
        float lerpTime = this.timeToFallToPlayer;
        float lerpCounter = 0;
        float start = this.transform.position.x;
        float end = GameManager.Instance.gameController.mapScroller.playerDefaultPosition.position.x;
       
        while (lerpCounter < lerpTime) {
            float t = lerpCounter / lerpTime;
            float x = Mathf.Lerp(start, end, t);

            this.transform.position = new Vector3(x, this.transform.position.y, this.transform.position.z );

            yield return null;

            lerpCounter += Time.deltaTime;
        }

        this.transform.position = new Vector3(end, this.transform.position.y, this.transform.position.z );
    }
    */

}
