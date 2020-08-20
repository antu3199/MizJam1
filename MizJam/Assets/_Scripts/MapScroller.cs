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

    public Transform playerDefaultPosition;

    private int basicAfterEveryOther = 0;

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

    public void ResetPlayerCamera(MoveableObject player, List<Transform> otherObjects) {
        this.moveMap = true;
        player.LockHorizontalMovement();
        StartCoroutine(this.movePlayerBackToPosition(player, otherObjects));
    }

    private void OnDestroyMap(BasicMap map) {
        this.activeMaps.Remove(map);
    }

    private void OnLoadNextMap(BasicMap map) {
        int randMapIndex = UnityEngine.Random.Range(0, possibleMaps.Count);
        if (this.basicAfterEveryOther++ % 2 == 1 ) {
            randMapIndex = 0;
        } else {
            randMapIndex = 2; // TMP
        }

        //randMapIndex = 1;
        BasicMap newMap = Instantiate(possibleMaps[randMapIndex], grid.transform) as BasicMap;
        newMap.Initialize(this.OnLoadNextMap, this.OnDestroyMap, this.SetAsCenterMap, map.transform.position);
        this.activeMaps.Add(newMap);
    }

    private void SetAsCenterMap(BasicMap map) {
        this.currentMap = map;
        GameManager.Instance.gameState.SetLevel(GameManager.Instance.gameState.floorNumber + 1);
    }

    private IEnumerator movePlayerBackToPosition(MoveableObject player, List<Transform> otherObjects) {
        float target = this.playerDefaultPosition.position.x;

        while (player.transform.position.x > target) {
            float delta =  -(this.mapMoveSpeed * Time.deltaTime);

            foreach (Transform trans in otherObjects) {
                trans.position += Vector3.right * delta;
            }

            player.transform.position += Vector3.right * delta;
            yield return null;
        }

        player.transform.position = new Vector3(target, player.transform.position.y, player.transform.position.z );

        
    }
}
