using LPUnityUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionMode
{
    None,
    Picking,
    Clicking,
    TargetForPickedObjects
}

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private StatefulGameObjectId _Id;
    public StatefulGameObjectId Id { get { return _Id; } }

    [SerializeField] private InteractionMode _InteractionMode = InteractionMode.Clicking;
    public InteractionMode InteractionMode { get { return _InteractionMode; } }

    [SerializeField] private float ScaleWhenPicked = 1.0f;

    public Room ParentRoom
    {
        get { return this.GetComponentInParent<Room>(); }
    }

    private const float HILIGHT_PULSE_SPEED = 4.0f;

    private void Update()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(false);
        foreach ( Renderer r in renderers )
        {
            if ( Hilighted )
            {
                foreach ( Material mat in r.materials )
                {
                    mat.SetColor("_EmissionColor", Color.white * (Mathf.Sin((Time.time - LastHilightedTime) * HILIGHT_PULSE_SPEED) * 0.5f + 0.5f) * 0.8f);
                    mat.EnableKeyword("_EMISSION");
                }
            }
            else
            {
                foreach ( Material mat in r.materials )
                {
                    mat.SetColor("_EmissionColor", Color.black);
                }
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

    private Vector3 PosOnPick;
    private Vector3 ScaleOnPick;
    private float PickedTime;

    private const float OBJECT_PICK_TRANSITION_SPEED = 6.0f;
    private const float PICKED_OBJECT_OFFSET_FROM_CURSOR = 0.15f;

    public void OnPick()
    {
        PosOnPick = transform.position;
        ScaleOnPick = transform.localScale;
        PickedTime = Time.time;
    }

    public void PositionAsPicked(Camera pickingCamera, Pointer pointer)
    {
        Ray ray = pointer.GetRay(pickingCamera);

        float offsetAlongRay = 1.0f / ray.direction.z;
        Vector3 hoverPos = ray.origin + ray.direction * offsetAlongRay + (Vector3.right + Vector3.down) * PICKED_OBJECT_OFFSET_FROM_CURSOR;

        float pickedT = Mathf.Clamp01((Time.time - PickedTime) * OBJECT_PICK_TRANSITION_SPEED);

        transform.position = Vector3.Lerp(PosOnPick, hoverPos, pickedT);
        transform.localScale = Vector3.Lerp(ScaleOnPick, Vector3.one * ScaleWhenPicked, pickedT);
    }

    public void ResetPosition()
    {
        transform.position = PosOnPick;
        transform.localScale = ScaleOnPick;
    }
}
