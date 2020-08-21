using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{

    public float showTime = 1f;
    public Slider healthBar;

    private bool isShowingHealth = false;
    private float healthCounter = 0;

    void OnDisable() {
        this.StopAllCoroutines();
    }

    public void SetHPProgress(float value) {
        this.healthBar.value = value;
        if (this.isShowingHealth == true) {
            this.healthCounter = 0;
        } else {
            this.isShowingHealth = true;
            StartCoroutine(ShowForSeconds(this.healthBar.gameObject, showTime));
        }
    }

    private IEnumerator ShowForSeconds(GameObject obj, float seconds) {
        obj.SetActive(true);
        while (this.healthCounter < this.showTime) {
            this.healthCounter += Time.deltaTime;
            yield return null;
        }

        obj.SetActive(false);
        this.isShowingHealth = false;
    }

}
