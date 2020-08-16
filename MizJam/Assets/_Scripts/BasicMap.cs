using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MapEvent {
    public string identifier;
    
    public float positionToTrigger = 0f;

    public bool hasTriggered = false;
    public bool playerMoving = true;
    public UnityAction triggerEvent = null;
};

public class BasicMap : MonoBehaviour
{
    public List<MapEvent> mapEvents;

    const int VISIBLE_X = 36;

    public bool moveMap = true;
    public float moveMapSpeed = 1f;

    protected Action<BasicMap> onLoadNextMap = null;
    protected Action<BasicMap> onDestroy = null;
    protected Action<BasicMap> onSetAsCenterMap = null;
    private bool onInitializeCalled = false;
    

    public void Initialize(Action<BasicMap> onLoadNextMap, Action<BasicMap> onDestroy, Action<BasicMap> setAsCenterMap, Vector3 referencePosition) {
        this.transform.position = referencePosition + new Vector3(VISIBLE_X, 0, 0);
        this.CommonInitialization(onLoadNextMap, onDestroy, setAsCenterMap);
    }
    public void Initialize(Action<BasicMap> onLoadNextMap, Action<BasicMap> onDestroy, Action<BasicMap> setAsCenterMap) {
        this.CommonInitialization(onLoadNextMap, onDestroy, setAsCenterMap);
    }

    private void CommonInitialization(Action<BasicMap> onLoadNextMap, Action<BasicMap> onDestroy, Action<BasicMap> setAsCenterMap) {
        this.onLoadNextMap = onLoadNextMap;
        this.onDestroy = onDestroy;
        this.onSetAsCenterMap = setAsCenterMap;

        MapEvent destroyEvent = new MapEvent();
        destroyEvent.identifier = "Destroy event";
        destroyEvent.positionToTrigger = -VISIBLE_X;
        destroyEvent.triggerEvent = DestroyMap;
        this.mapEvents.Add(destroyEvent);

        MapEvent loadNextMapEvent = new MapEvent();
        loadNextMapEvent.identifier = "Load next map event";
        loadNextMapEvent.positionToTrigger = VISIBLE_X/2;
        loadNextMapEvent.triggerEvent = this.LoadNextMap;
        this.mapEvents.Add(loadNextMapEvent);

        MapEvent setCenterMapEvent = new MapEvent();
        setCenterMapEvent.identifier = "Load next map event";
        setCenterMapEvent.positionToTrigger = VISIBLE_X/2;
        setCenterMapEvent.triggerEvent = this.LoadNextMap;
        this.mapEvents.Add(loadNextMapEvent);

        this.DoInitialize();
    }

    // Update is called once per frame
    public void PerformUpdate()
    {
        bool hasTriggered = false;
        for (int i = 0; i < mapEvents.Count; i++) {
            MapEvent mapEvent = mapEvents[i];
            if (!mapEvent.hasTriggered && this.GetPosition() <= mapEvent.positionToTrigger) {
                if (mapEvent.triggerEvent != null) {
                    Debug.Log(this.name + " Trigger event: " + mapEvent.identifier);
                    mapEvent.triggerEvent();
                } else {
                    Debug.LogError("ERROR: Trigger event was null!: " + mapEvent.identifier);
                }

                mapEvent.hasTriggered = true;
                hasTriggered = true;
            }
        }


        if (hasTriggered) {
            this.mapEvents = this.mapEvents.Filter(mapEvent => !mapEvent.hasTriggered);
        }

        if (this.moveMap) {
            this.MoveMap( GameManager.Instance.gameController.mapScroller.mapMoveSpeed );
        }
    }
    
    public virtual void DoInitialize() {
    }

    public virtual void DestroyMap() {
        this.onDestroy(this);
        Destroy(this.gameObject);
    }

    protected void MoveMap(float delta) {
        this.transform.position = this.transform.position + new Vector3(-delta * Time.deltaTime, 0, 0);
    }

    protected void SetMapMovable(bool moveable) {
        this.moveMap = moveable;
    }

    protected float GetPosition() {
        return this.transform.position.x;
    }

    protected void LoadNextMap() {
        this.onLoadNextMap(this);
    }

    protected void SetAsCenterMap() {
        this.onSetAsCenterMap(this);
    }

}
