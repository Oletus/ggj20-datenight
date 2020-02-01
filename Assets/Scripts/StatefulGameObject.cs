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

public enum StatefulGameObjectId
{
    Flower,
    ElectricityBill,
    Money,
    Trash,
    WateringCan,
    Phone
}

public enum InteractionMode
{
    None,
    Picking,
    Clicking,
}

public class StatefulGameObject : MonoBehaviour
{
    [SerializeField] private StatefulGameObjectId _Id;
    public StatefulGameObjectId Id { get { return _Id; } }

    [ReorderableList]
    [SerializeField] private List<StateDefinition> States;

    [SerializeField] private InteractionMode _InteractionMode = InteractionMode.Clicking;
    public InteractionMode InteractionMode { get { return _InteractionMode; } }

    public Room ParentRoom
    {
        get { return this.GetComponentInParent<Room>(); }
    }

    public string ActiveState { get; private set; }
    public GameObject ActiveObject { get; private set; }

    private const float HILIGHT_PULSE_SPEED = 4.0f;

    private void Update()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(false);
        foreach ( Renderer r in renderers )
        {
            if ( Hilighted )
            {
                r.material.SetColor("_EmissionColor", Color.white*(Mathf.Sin((Time.time - LastHilightedTime) * HILIGHT_PULSE_SPEED) * 0.5f + 0.5f) * 0.8f);
                r.material.EnableKeyword("_EMISSION");
            }
            else
            {
                r.material.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    private bool _Hilighted;
    private float LastHilightedTime;
    public bool Hilighted
    {
        get => _Hilighted;
        set
        {
            if ( !_Hilighted && value )
            {
                LastHilightedTime = Time.time;
            }
            _Hilighted = value;
        }
    }

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

    public void PositionAsPicked(Camera pickingCamera, Pointer pointer)
    {
        if ( ActiveObject != null )
        {
            Ray ray = pointer.GetRay(pickingCamera);
            ActiveObject.transform.position = ray.origin + ray.direction * 2.5f;
        }
    }

    public void ResetPosition(Vector3 pos)
    {
        this.ActiveObject.transform.position = pos;
    }
}
