using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TalkingMapResObj {
    [TextArea]
    public List<string> messages;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TalkingMapRes", order = 1)]
public class TalkingMapRes : ScriptableObject
{
    public List<TalkingMapResObj> possibleTalkingMapObj;

    public TalkingMapResObj GetRandomResObject() {
        int index = Random.Range(0, possibleTalkingMapObj.Count);
        return this.possibleTalkingMapObj[index];
    }
}
