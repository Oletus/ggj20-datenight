using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

[System.Serializable]
public class StateObjectPair
{
    [SerializeField] public string State; // generic T didnt work with Unity editing -_- ....
    [SerializeField] public GameObject Object;
}

public class StatefulGameObject : MonoBehaviour
{
    [SerializeField] private string Id;

    // TODO: Add NaughtyAttributes library
    //[ReorderableList]
    [SerializeField] private List<StateObjectPair> States;

    [SerializeField] private List<string> IdsThisCanBeUsedOn;

    public void SetState<T>(T activeState) where T : System.Enum
    {
        foreach(StateObjectPair pair in States)
        {
            if (pair.Object == null)
            {
                Debug.LogError("null object in StatefulGameObject");
                continue;
            }
            pair.Object.SetActive(pair.State.ToLower().Trim() == activeState.ToString().ToLower());
        }
    }
}
