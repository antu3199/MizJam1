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
            Debug.Log("Showing health already: " + this.gameObject.name);
            this.healthCounter = 0;
            this.healthBar.gameObject.SetActive(true);
        } else {
            this.healthCounter = 0;
            this.isShowingHealth = true;
            StartCoroutine(ShowForSeconds(this.healthBar.gameObject, showTime));
        }
    }

    private IEnumerator ShowForSeconds(GameObject obj, float seconds) {
        obj.SetActive(true);
        while (this.healthCounter < this.showTime) {
            this.healthCounter += Time.deltaTime;
            Debug.Log("Showing for seconds");
            yield return null;
        }

        obj.SetActive(false);
        this.isShowingHealth = false;
    }

}
