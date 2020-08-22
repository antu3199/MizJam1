using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TutorialResObj {
    [TextArea]
    public List<string> messages;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TutorialRes", order = 1)]
public class TutorialRes : ScriptableObject
{
    public List<TutorialResObj> tutorialObj;

}
