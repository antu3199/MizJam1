using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public Text levelText;

    public ImageIcon levelIconPrefab;
    public Transform levelIconContainer;
    
    private List<ImageIcon> levelIcons = new List<ImageIcon>();

    private int numUniqueMaps;
    public void Initialize() {
        numUniqueMaps = MapScroller.NUM_MAPS_PER_MILE/2;
        for (int i = 0; i < numUniqueMaps; i++) {
            ImageIcon imageIcon = Instantiate(levelIconPrefab, levelIconContainer) as ImageIcon;
            // remember to set sprite later!
            this.levelIcons.Add(levelIconPrefab);
        }

        this.SetLevelIconHighlight(-1);
    }

    public void SetLevelIconSprite(int index, Sprite sprite) {
        Debug.Log("Set sprite: " + index);
        this.levelIcons[index].SetSprite(sprite);
    }

    public void SetLevelIconHighlight(int index) {
        for (int i = 0; i < levelIcons.Count; i++) {
            this.levelIcons[i].SetHighlight(i == index);
        }
    }

    public void UpdateLevelText() {
        this.levelText.text = string.Format("Miles travelled {0}", GameManager.Instance.gameState.floorNumber);
    }
}
