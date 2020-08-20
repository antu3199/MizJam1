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
    public UnityEvent triggerEvent = null;
};

public class BasicMap : MonoBehaviour
{
    public List<MapEvent> mapEvents;
    public double reference = 1;

    const int VISIBLE_X = 36;

    protected Action<BasicMap> onLoadNextMap = null;
    protected Action<BasicMap> onDestroy = null;
    protected Action<BasicMap> onSetAsCenterMap = null;
    private bool onInitializeCalled = false;
    
    
    public void Initialize(double reference, Action<BasicMap> onLoadNextMap, Action<BasicMap> onDestroy, Action<BasicMap> setAsCenterMap) {
        this.CommonInitialization(reference, onLoadNextMap, onDestroy, setAsCenterMap);
    }

    private void CommonInitialization(double reference, Action<BasicMap> onLoadNextMap, Action<BasicMap> onDestroy, Action<BasicMap> setAsCenterMap) {
        this.onLoadNextMap = onLoadNextMap;
        this.onDestroy = onDestroy;
        this.onSetAsCenterMap = setAsCenterMap;
        this.reference = reference;

        MapEvent destroyEvent = new MapEvent();
        destroyEvent.identifier = "Destroy event";
        destroyEvent.positionToTrigger = -VISIBLE_X;
        destroyEvent.triggerEvent = new UnityEvent();
        destroyEvent.triggerEvent.AddListener(this.DestroyMap);
        this.mapEvents.Add(destroyEvent);

        MapEvent loadNextMapEvent = new MapEvent();
        loadNextMapEvent.identifier = "Load next map event";
        loadNextMapEvent.positionToTrigger = VISIBLE_X/2;
        loadNextMapEvent.triggerEvent = new UnityEvent();
        loadNextMapEvent.triggerEvent.AddListener(this.LoadNextMap);
        this.mapEvents.Add(loadNextMapEvent);

        MapEvent setCenterMapEvent = new MapEvent();
        setCenterMapEvent.identifier = "Set center position";
        setCenterMapEvent.positionToTrigger = 0;
        setCenterMapEvent.triggerEvent = new UnityEvent();
        setCenterMapEvent.triggerEvent.AddListener(this.SetAsCenterMap);
        this.mapEvents.Add(setCenterMapEvent);

        this.DoInitialize();
    }

    public void PositionRelativeTo(Vector3 referencePosition) {
        this.transform.position = referencePosition + new Vector3(VISIBLE_X, 0, 0);
    }

    // Update is called once per frame
    public void PerformUpdate()
    {
        bool hasTriggered = false;
        for (int i = 0; i < mapEvents.Count; i++) {
            MapEvent mapEvent = mapEvents[i];
            if (!mapEvent.hasTriggered && this.GetPosition() <= mapEvent.positionToTrigger) {
                if (mapEvent.triggerEvent != null) {
                    //Debug.Log(this.name + " Trigger event: " + mapEvent.identifier);
                    mapEvent.triggerEvent.Invoke();
                } else {
                    Debug.LogError("ERROR: Trigger event was null!: " + mapEvent.identifier);
                }
                if (mapEvent.playerMoving == false) {
                    GameManager.Instance.gameController.mapScroller.SetMapMovable(false);
                }

                mapEvent.hasTriggered = true;
                hasTriggered = true;
            }
        }


        if (hasTriggered) {
            this.mapEvents = this.mapEvents.Filter(mapEvent => !mapEvent.hasTriggered);
        }

        this.MoveMap( GameManager.Instance.gameController.mapScroller.mapMoveSpeed );
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
