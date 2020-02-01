using LPUnityUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class Inventory : MonoBehaviour
{
    private StatefulGameObject CurrentPickedObject;
    private Pointer CurrentPointer;

    [SerializeField] private Camera PickingCamera;

    // Update is called once per frame
    void Update()
    {
        Pointer pointerDown = Pointer.CreateOnPointerDown();
        if (pointerDown != null)
        {
            bool raycasthit = Physics.Raycast(pointerDown.GetRay(PickingCamera), out RaycastHit hitInfo);
            if ( raycasthit )
            {
                StatefulGameObject hitObject = hitInfo.transform.GetComponentInParent<StatefulGameObject>();
                if ( hitObject == null )
                {
                    return;
                }
                if ( CurrentPickedObject != null )
                {
                    if ( CurrentPickedObject.CanBeUsedOn(hitObject) )
                    {
                        // TODO: Trigger interaction with the hit object somehow!
                        CurrentPickedObject.DisableAll();
                        CurrentPickedObject = null;
                    }
                }
                else
                {
                    if ( hitObject.CanPickUp() )
                    {
                        CurrentPickedObject = hitObject;
                        CurrentPointer = pointerDown;
                    }
                }
            }
        } 
        if ( CurrentPickedObject != null )
        {
            CurrentPickedObject.PositionAsPicked(PickingCamera, CurrentPointer);
        }
    }
}
