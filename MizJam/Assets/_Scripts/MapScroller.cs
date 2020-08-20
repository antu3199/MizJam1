using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapScroller : MonoBehaviour
{

    public List<BasicMap> possibleMaps;

    public BasicMap currentMap; // Set this to starting map at the start

    public Grid grid; // Used for parent

    public float mapMoveSpeed = 1f;
    public List<BasicMap> activeMaps{get; set;}

    public bool moveMap = true;

    public void Initialize() {
        this.activeMaps = new List<BasicMap>();
        this.activeMaps.Add(this.currentMap);
        this.currentMap.Initialize(this.OnLoadNextMap, this.OnDestroyMap, this.SetAsCenterMap );
    }

    // Update is called once per frame
    void Update()
    {
        if (this.moveMap) {
            for (int i = 0; i < this.activeMaps.Count; i++) {
                BasicMap map = this.activeMaps[i];
                map.PerformUpdate();
            }
        }
    }


    public void SetMapMovable(bool moveable) {
        this.moveMap = moveable;
    }

    private void OnDestroyMap(BasicMap map) {
        this.activeMaps.Remove(map);
    }

    private void OnLoadNextMap(BasicMap map) {
        int randMapIndex = UnityEngine.Random.Range(0, possibleMaps.Count);
        randMapIndex = 2;
        //randMapIndex = 1;
        BasicMap newMap = Instantiate(possibleMaps[randMapIndex], grid.transform) as BasicMap;
        newMap.Initialize(this.OnLoadNextMap, this.OnDestroyMap, this.SetAsCenterMap, map.transform.position);
        this.activeMaps.Add(newMap);
    }

    private void SetAsCenterMap(BasicMap map) {
        this.currentMap = map;
        GameManager.Instance.gameState.SetLevel(GameManager.Instance.gameState.floorNumber + 1);
    }
}
