using LPUnityUtils;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

#pragma warning disable 0649

[System.Serializable]
public class StateDefinition
{
    [FormerlySerializedAs("State")]
    [SerializeField] private string _State; // generic T didnt work with Unity editing -_- ....
    public string State => _State.ToLower().Trim();

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

    [SerializeField] private bool _CanBePicked = true;

    private void Awake()
    {
        for( int i = 0; i < IdsThisCanBeUsedOn.Count; ++i )
        {
            IdsThisCanBeUsedOn[i] = IdsThisCanBeUsedOn[i].ToLower().Trim();
        }
    }

    public string ActiveState { get; private set; }
    public GameObject ActiveObject { get; private set; }

    private HashSet<GameObject> ActiveObjectsFromAllStates
    {
        get
        {
            HashSet<GameObject> activeObjectsFromAllStates = new HashSet<GameObject>();
            foreach ( StateDefinition state in States )
            {
                if ( state.ActiveObject != null )
                {
                    activeObjectsFromAllStates.Add(state.ActiveObject);
                }
            }
            return activeObjectsFromAllStates;
        }
    }

    public void SetState<T>(T activeState) where T : System.Enum
    {
        GameObject activeObject = null;
        foreach(StateDefinition state in States)
        {
            if ( state.State == activeState.ToString().ToLower() )
            {
                ActiveState = state.State;
                activeObject = state.ActiveObject;
            }
        }
        if ( activeObject != null )
        {
            activeObject.SetActive(true);
        }
        ActiveObject = activeObject;
        foreach (GameObject obj in ActiveObjectsFromAllStates)
        {
            if (obj != activeObject)
            {
                obj.SetActive(false);
            }
        }
    }

    public void DisableAll()
    {
        foreach ( GameObject obj in ActiveObjectsFromAllStates )
        {
            obj.SetActive(false);
        }
    }

    public bool CanBeUsedOn(StatefulGameObject other)
    {
        return IdsThisCanBeUsedOn.Contains(other.Id);
    }

    public bool CanPickUp()
    {
        return _CanBePicked;
    }

    public void PositionAsPicked(Camera pickingCamera, Pointer pointer)
    {
        if ( ActiveObject != null )
        {
            ActiveObject.transform.position = pickingCamera.transform.position + pickingCamera.transform.forward * 10.0f;
        }
    }
}
