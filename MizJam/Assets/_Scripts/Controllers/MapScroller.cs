using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapScroller : MonoBehaviour
{

    public List<BasicMap> possibleMaps;

    public BasicMap startingMap; // Set this to starting map at the start

    public Grid grid; // Used for parent

    public float mapMoveSpeed = 1f;
    public List<BasicMap> activeMaps{get; set;}

    public bool moveMap = true;

    public Transform playerDefaultPosition;


    public int tmp_FixedMap = 0;
    private Queue<BasicMap> nextMapsToAdd = new Queue<BasicMap>();

    public const int NUM_MAPS_PER_MILE = 10;
    //public const int NUM_MAPS_PER_MILE = 4;
    //public const int NUM_UNIQUE_MAPS_PER_MILE = NUM_MAPS_PER_MILE/2 - 1;

    public int curMapIndex{get; set;}

    public float combatMapChance = 0.5f;

    public void Initialize() {
        this.activeMaps = new List<BasicMap>();
        this.activeMaps.Add(this.startingMap);
        double reference = Currency.GetBaseCost(GameManager.Instance.gameState.floorNumber);
        this.startingMap.Initialize(reference, this.OnLoadNextMap, this.OnDestroyMap, this.GoToNextMap, -1 );

        GameManager.Instance.gameState.hasPlayedTutorial = true;

        if (!GameManager.Instance.gameState.hasPlayedTutorial) {
            this.GenerateTutorialMaps();
        } else {
            this.GenerateMapsFromIndex(true);
        }

        GameManager.Instance.gameController.topBar.SetLevelIconHighlight(this.curMapIndex);
        this.SetTopBarFromMaps();
    }

    public void Reset() {
        this.curMapIndex = 0;
        foreach(BasicMap map in this.activeMaps) {
            Destroy(map.gameObject);
        }

        foreach (BasicMap map in this.nextMapsToAdd) {
            Destroy(map.gameObject);
        }


        this.activeMaps.Clear();
        this.nextMapsToAdd.Clear();

        this.GenerateMapsFromIndex(false);
        BasicMap nextMap = this.nextMapsToAdd.Dequeue();
        nextMap.transform.position = Vector3.zero;
        nextMap.gameObject.SetActive(true);
        this.activeMaps.Add(nextMap);
        this.curMapIndex = 0;
        this.SetTopBarFromMaps();
        GameManager.Instance.gameController.topBar.SetLevelIconHighlight(curMapIndex);
        this.moveMap = true;
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

    public void ResetPlayerCamera(MoveableObject player, List<Transform> otherObjects, bool disableCollider = false) {
        this.moveMap = true;
        player.LockHorizontalMovement();
        StartCoroutine(this.movePlayerBackToPosition(player, otherObjects, disableCollider));
    }

    private void OnDestroyMap(BasicMap map) {
        this.activeMaps.Remove(map);
    }

    private void OnLoadNextMap(BasicMap map) {
        if (this.nextMapsToAdd.Count == 0) {
            return;
        }

        //randMapIndex = 1;
        BasicMap newMap = this.nextMapsToAdd.Dequeue();
        newMap.PositionRelativeTo(map.transform.position);
        newMap.gameObject.SetActive(true);
        this.activeMaps.Add(newMap);
    }

    private void GoToNextMap(BasicMap map) {
        // Only unique maps matter
        if (map.mapIndex != -1) {
            if (curMapIndex == NUM_MAPS_PER_MILE/2 - 1) {
                Debug.Log("NO MORE MAPS");
                // Need to regenerate maps!
                this.RegenerateMaps();
                return;
            }


            curMapIndex++;
            GameManager.Instance.gameController.topBar.SetLevelIconHighlight(curMapIndex);
        }
    }

    private void RegenerateMaps() {
        GameManager.Instance.gameState.SetLevel(GameManager.Instance.gameState.floorNumber + 1);

        this.GenerateMapsFromIndex(false);
        BasicMap nextMap = this.nextMapsToAdd.Dequeue();
        nextMap.PositionRelativeTo( this.activeMaps[this.activeMaps.Count-1].transform.position );
        nextMap.gameObject.SetActive(true);
        this.activeMaps.Add(nextMap);
        this.curMapIndex = 0;
        this.SetTopBarFromMaps();
        GameManager.Instance.gameController.topBar.SetLevelIconHighlight(curMapIndex);
    }

    private IEnumerator movePlayerBackToPosition(MoveableObject player, List<Transform> otherObjects, bool disableCollider) {

        if (disableCollider) {
            player.GetComponent<PlayerController>().canMove = false;
            player.GetComponent<CharacterController>().enabled = false;
        }

        float target = this.playerDefaultPosition.position.x;

        float lerpTime = 1f;
        float lerpCounter = 0;
        float start = player.transform.position.x;
        float end = target;

        float startY = player.transform.position.y;
        float endY = this.playerDefaultPosition.position.y;
        
        while (lerpCounter < lerpTime) {
            float t = lerpCounter / lerpTime;
            float x = Mathf.Lerp(start, end, t);
            float y = player.transform.position.y;

            if (disableCollider) {
                y = Mathf.Lerp(startY, endY, t);
            }

            player.transform.position = new Vector3(x, y, player.transform.position.z );

            yield return null;

            lerpCounter += Time.deltaTime;
        }

        player.transform.position = new Vector3(target, player.transform.position.y, player.transform.position.z );

        if (disableCollider) {
            player.GetComponent<PlayerController>().canMove = true;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }

    // Index = 1 if start, 0 otherwise
    private void GenerateMapsFromIndex(bool firstTime) {
        int startingIndex = firstTime ? 1 : 0;

        const int initialLevel = 1;

        double referenceA = Currency.GetBaseRate(initialLevel + GameManager.Instance.gameState.floorNumber);
        double referenceB = Currency.GetBaseRate(initialLevel + GameManager.Instance.gameState.floorNumber+1);

        int mapIndexCount = 0;

        MapType specialMap = MapType.BASIC; // Stays basic unless need to do something special...

        for (int i = startingIndex; i < NUM_MAPS_PER_MILE; i++) {

            float t = (float)i/NUM_MAPS_PER_MILE;
            double reference = this.LerpDouble(referenceA, referenceB, t);
            int mapIndex = UnityEngine.Random.Range(1, possibleMaps.Count);
            int basicMapIndex = -1;
            if (i % 2 == 0 ) {
                mapIndex = 0;
            } else if (i == NUM_MAPS_PER_MILE - 1) {
                mapIndex = 2;
            } else if (UnityEngine.Random.Range(0.0f, 1.0f) <= this.combatMapChance) {
                mapIndex = 2;
            } else {
                
            }

            if (i % 2 != 0 && i != NUM_MAPS_PER_MILE - 1 ) {
                mapIndex = tmp_FixedMap; // TMP
            }

            if (i % 2 != 0) {
                basicMapIndex = mapIndexCount++; 
            }

            bool isBossMap = i == NUM_MAPS_PER_MILE-1;
            if (isBossMap) {
                specialMap = MapType.COMBAT_BOSS;
            }

            this.InstantiateMap(mapIndex, reference, basicMapIndex, specialMap);
        }
    }

    private void GenerateTutorialMaps() {
        int startingIndex = 1;
        int mapIndexCount = 0;
        double reference = 1;


        for (int i = startingIndex; i < NUM_MAPS_PER_MILE; i++) {
            int mapIndex = 1;
            int basicMapIndex = -1;
            MapType specialMap = MapType.BASIC;

            if (i % 2 == 0 ) {
                mapIndex = 0;
            } else if (i == NUM_MAPS_PER_MILE - 1) {
                mapIndex = 2;
            } else {
                specialMap = MapType.TALKING_TUTORIAL;
            }

            /*
            if (i % 2 != 0 && i != NUM_MAPS_PER_MILE - 1 ) {
                mapIndex = tmp_FixedMap; // TMP
            }
            */

            if (i % 2 != 0) {
                basicMapIndex = mapIndexCount++; 
            }



            this.InstantiateMap(mapIndex, reference, basicMapIndex, specialMap);
        }
    }

    private void InstantiateMap(int mapIndex, double reference, int basicMapIndex, MapType specialMapType) {
        BasicMap newMap = Instantiate(possibleMaps[mapIndex], grid.transform) as BasicMap;
        newMap.Initialize(reference, this.OnLoadNextMap, this.OnDestroyMap, this.GoToNextMap, basicMapIndex);

        switch (specialMapType) {
            case MapType.COMBAT_BOSS:
                CombatMap combatMap = (CombatMap)newMap;
                combatMap.SetAsBossMap();
                break;
            case MapType.TALKING_TUTORIAL:
                TalkingMap talkingMap = (TalkingMap)newMap;
                talkingMap.SetAsTutorial(basicMapIndex);
                break;
        }

        newMap.gameObject.SetActive(false);
        nextMapsToAdd.Enqueue(newMap);
    }

    private void SetTopBarFromMaps() {
        foreach (BasicMap map in this.nextMapsToAdd ) {
            if (map.mapIndex != -1) {
                GameManager.Instance.gameController.topBar.SetLevelIconSprite(map.mapIndex, map.levelIconSprite);
            }
        }
    }

    private double LerpDouble(double a, double b, float t) {
        return a * (1-t) + b * t;
    }


}
