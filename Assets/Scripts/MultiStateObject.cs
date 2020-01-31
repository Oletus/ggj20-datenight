using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Inherit Flower from this.
public abstract class MultiStateObject<T> : MonoBehaviour
{
    [System.Serializable]
    public class StateObjectPair
    {
        T StateId;
        GameObject Object;
    }

    // TODO: Add NaughtyAttributes library
    //[ReorderableList]
    [SerializeField] private List<StateObjectPair> States;

    public void SetState(T activeState)
    {
        // This should set just one object active.
    }
}
