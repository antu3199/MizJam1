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


    private bool onInitializeCalled = false;
    

    public void Initialize() {
        MapEvent destroyEvent = new MapEvent();
        destroyEvent.positionToTrigger = -VISIBLE_X;
        destroyEvent.triggerEvent = DestroyMap;
        this.mapEvents.Add(destroyEvent);

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
                    mapEvent.triggerEvent();
                } else {
                    Debug.LogError("ERROR: Trigger event was null!");
                }

                mapEvent.hasTriggered = true;
                hasTriggered = true;
            }
        }


        if (hasTriggered) {
            this.mapEvents = this.mapEvents.Filter(mapEvent => !mapEvent.hasTriggered);
        }

        if (this.moveMap) {
            this.MoveMap(this.moveMapSpeed);
        }
    }
    
    public virtual void DoInitialize() {

    }

    public virtual void DestroyMap() {
        Debug.Log("Destroy Map: " + this.gameObject.name);
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

}
