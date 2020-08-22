using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageIcon : MonoBehaviour
{

    public Image icon;

    public Image highlight;

    public void SetSprite(Sprite iconSprite) {
        icon.sprite = iconSprite;
    }

    public void SetHighlight(bool active) {
        this.highlight.gameObject.SetActive(active);
    }
    
}
