using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

#pragma warning disable 0649

[System.Serializable]
public class StateDefinition
{
    [SerializeField] public string State; // generic T didnt work with Unity editing -_- ....

    [FormerlySerializedAs("Object")]
    [SerializeField] public GameObject ActiveObject;
}

public class StatefulGameObject : MonoBehaviour
{
    [SerializeField] private string _Id;
    private string Id => _Id.ToLower().Trim();

    [ReorderableList]
    [SerializeField] private List<StateDefinition> States;

    [ReorderableList]
    [SerializeField] private List<string> IdsThisCanBeUsedOn;

    private void Awake()
    {
        for( int i = 0; i < IdsThisCanBeUsedOn.Count; ++i )
        {
            IdsThisCanBeUsedOn[i] = IdsThisCanBeUsedOn[i].ToLower().Trim();
        }
    }

    public void SetState<T>(T activeState) where T : System.Enum
    {
        GameObject activeObject = null;
        HashSet<GameObject> activeObjectsFromAllStates = new HashSet<GameObject>();
        foreach(StateDefinition state in States)
        {
            if ( state.State.ToLower().Trim() == activeState.ToString().ToLower() )
            {
                activeObject = state.ActiveObject;
            }
            if ( state.ActiveObject != null )
            {
                activeObjectsFromAllStates.Add(state.ActiveObject);
            }
        }
        if ( activeObject != null )
        {
            activeObject.SetActive(true);
        }
        foreach (GameObject obj in activeObjectsFromAllStates)
        {
            if (obj != activeObject)
            {
                obj.SetActive(false);
            }
        }
    }

    public bool CanBeUsedOn(StatefulGameObject other)
    {
        return IdsThisCanBeUsedOn.Contains(other.Id);
    }
}
