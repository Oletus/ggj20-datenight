using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateObjectPair
{
    [SerializeField] public string State; // generic T didnt work with Unity editing -_- ....
    [SerializeField] public GameObject Object; // TODO: do we actually want this to be a list?
}

public abstract class StatefulGameObject<T> : MonoBehaviour
    where T : System.Enum
{
    // TODO: Add NaughtyAttributes library
    //[ReorderableList]
    [SerializeField] public List<StateObjectPair> States;

    public void SetState(T activeState)
    {
        foreach(StateObjectPair pair in States)
        {
            pair.Object.SetActive(pair.State == activeState.ToString());
        }
    }
}
