using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionItemUI : MonoBehaviour
{
    public Text label;
    public Slider progressBar;

    public void SetLabel(string label) {
        this.label.text = label;
    }

    public void SetProgressBarVisible(bool visible) {
        if (!this.progressBar) {
            return;
        }

        this.progressBar.gameObject.SetActive(visible);
    }

    public bool IsProgressBarVisible() {
        return this.progressBar.gameObject.activeInHierarchy;
    }

    public void SetProgressBarValue(float value) {
        this.progressBar.value = value;
    }

    public float GetProgressBarValue() {
        return this.progressBar.value;
    }
    
}
