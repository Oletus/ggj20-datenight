using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateObjectPair
{
    [SerializeField] public string State; // generic T didnt work with Unity editing -_- ....
    [SerializeField] public GameObject Object; // TODO: do we actually want this to be a list?
}

public class StatefulGameObject : MonoBehaviour
{
    // TODO: Add NaughtyAttributes library
    //[ReorderableList]
    [SerializeField] public List<StateObjectPair> States;

    public void SetState<T>(T activeState) where T : System.Enum
    {
        foreach(StateObjectPair pair in States)
        {
            pair.Object.SetActive(pair.State.ToLower().Trim() == activeState.ToString().ToLower());
        }
    }
}
