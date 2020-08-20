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


    public int tmp_FixedMap = 0;
    private Queue<BasicMap> nextMapsToAdd = new Queue<BasicMap>();

    public const int NUM_MAPS_PER_MILE = 10;

    public void Initialize() {
        this.activeMaps = new List<BasicMap>();
        this.activeMaps.Add(this.currentMap);
        double reference = Currency.GetBaseCost(GameManager.Instance.gameState.floorNumber);
        this.currentMap.Initialize(reference, this.OnLoadNextMap, this.OnDestroyMap, this.SetAsCenterMap, -1 );
        this.GenerateMapsFromIndex(true);
        
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
        if (this.nextMapsToAdd.Count == 0) {
            this.GenerateMapsFromIndex(false);
            GameManager.Instance.gameState.SetLevel(GameManager.Instance.gameState.floorNumber + 1);
        }

        //randMapIndex = 1;
        BasicMap newMap = this.nextMapsToAdd.Dequeue();
        newMap.PositionRelativeTo(map.transform.position);
        newMap.gameObject.SetActive(true);
        this.activeMaps.Add(newMap);

        if (newMap.mapIndex != -1) {
            GameManager.Instance.gameController.topBar.SetLevelIconHighlight(newMap.mapIndex);
        }

    }

    private void SetAsCenterMap(BasicMap map) {
        this.currentMap = map;
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

    // Index = 1 if start, 0 otherwise
    private void GenerateMapsFromIndex(bool firstTime) {
        int startingIndex = firstTime ? 1 : 0;

        const int initialLevel = 1;

        double referenceA = Currency.GetBaseCost(initialLevel + GameManager.Instance.gameState.floorNumber);
        double referenceB = Currency.GetBaseCost(initialLevel + GameManager.Instance.gameState.floorNumber+1);

        int mapIndexCount = 0;

        for (int i = startingIndex; i < NUM_MAPS_PER_MILE; i++) {

            float t = (float)i/NUM_MAPS_PER_MILE;
            double reference = this.LerpDouble(referenceA, referenceB, t);
            //Debug.Log("Floor: " + GameManager.Instance.gameState.floorNumber + " Level: " + i + " Reference: " + reference );
            
            int randMapIndex = UnityEngine.Random.Range(0, possibleMaps.Count);
            int basicMapIndex = -1;
            if (i % 2 == 1 ) {
                randMapIndex = 0;
            } else {
                randMapIndex = tmp_FixedMap; // TMP
                basicMapIndex = mapIndexCount; 
            }

            BasicMap newMap = Instantiate(possibleMaps[randMapIndex], grid.transform) as BasicMap;
            newMap.Initialize(reference, this.OnLoadNextMap, this.OnDestroyMap, this.SetAsCenterMap, basicMapIndex);
            newMap.gameObject.SetActive(false);
            nextMapsToAdd.Enqueue(newMap);

            if (basicMapIndex != -1) {
                GameManager.Instance.gameController.topBar.SetLevelIconSprite(mapIndexCount, newMap.levelIconSprite);
                mapIndexCount++;
            }
        }
    }

    private double LerpDouble(double a, double b, float t) {
        return a * (1-t) + b * t;
    }


}
