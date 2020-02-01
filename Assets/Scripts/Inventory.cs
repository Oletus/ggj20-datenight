using System;
using LPUnityUtils;
using UnityEngine;

#pragma warning disable 0649

public class Inventory : MonoBehaviour
{
    private InteractableObject _CurrentPickedObject;
    private InteractableObject CurrentPickedObject
    {
        get
        {
            return _CurrentPickedObject;
        }
        set
        {
            if (_CurrentPickedObject == value)
            {
                return;
            }
            _CurrentPickedObject = value;
            if (_CurrentPickedObject)
            {
                _CurrentPickedObject.OnPick();
            }
        }
    }
    private Pointer CurrentPointer;

    [SerializeField] private Camera PickingCamera;

    private InteractableObject _CurrentHoverObject;
    private InteractableObject CurrentHoverObject
    {
        get
        {
            return _CurrentHoverObject;
        }
        set
        {
            if ( _CurrentHoverObject == value )
            {
                return;
            }
            if ( _CurrentHoverObject )
            {
                _CurrentHoverObject.Hilighted = false;
            }
            _CurrentHoverObject = value;
            if ( _CurrentHoverObject )
            {
                _CurrentHoverObject.Hilighted = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdatePicking();

        if (CurrentPickedObject != null &&  CurrentPointer != null)
        {
            CurrentPickedObject.PositionAsPicked(PickingCamera, CurrentPointer);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if( CurrentPickedObject != null)
            {
                CurrentPickedObject.ResetPosition();

            }
            this.CurrentPickedObject = null;
        }

        this.UpdateHover();
    }

    private InteractableObject GetObjectFromRay(Ray ray)
    {
        RaycastHit[] raycastHits = Physics.RaycastAll(ray);
        if ( raycastHits.Length > 0 )
        {
            foreach ( RaycastHit hitInfo in raycastHits )
            {
                InteractableObject hitObject = hitInfo.transform.GetComponentInParent<InteractableObject>();
                if ( hitObject == null || hitObject == this.CurrentPickedObject )
                {
                    continue;
                }
                return hitObject;
            }
        }
        return null;
    }

    private void UpdateHover()
    {
        Pointer hoverPointer = new Pointer();
        CurrentHoverObject = GetObjectFromRay(hoverPointer.GetRay(PickingCamera));
    }

    private void UpdatePicking()
    {
        Pointer pointerDown = Pointer.CreateOnPointerDown();
        if (pointerDown != null)
        {
            InteractableObject hitObject = GetObjectFromRay(pointerDown.GetRay(PickingCamera));
            if (hitObject != null)
            {
                // pick up the item if no item is currently picked up
                if (CurrentPickedObject == null && hitObject.InteractionMode != InteractionMode.None)
                {
                    if (hitObject.InteractionMode == InteractionMode.Clicking)
                    {
                        this.OnClickItem(hitObject, RoomGenerator.Instance.GetRoomStateByRoom(hitObject.ParentRoom));
                    }
                    else if(hitObject.InteractionMode == InteractionMode.Picking)
                    {
                        CurrentPickedObject = hitObject;
                        CurrentPointer = pointerDown;
                    }
                }
                // try to use the picked object on another item
                else if (CurrentPickedObject != null)
                {
                    var useItemAction = TryGetUseItemAction(CurrentPickedObject, hitObject, RoomGenerator.Instance.GetRoomStateByRoom(hitObject.ParentRoom));
                    if (useItemAction != null)
                    {
                        bool success = useItemAction();
                        if(success)
                        {

                            CurrentPickedObject.gameObject.SetActive(false);
                            CurrentPickedObject = null;
                        }
                    }
                }
            } 
        }
    }

    private bool OnClickItem(InteractableObject item, RoomState roomState)
    {
        switch(item.Id)
        {
            case StatefulGameObjectId.Phone:
                return roomState.CallPlumber();

        }

        return false;
    }

    private Func<bool> TryGetUseItemAction(InteractableObject item, InteractableObject target, RoomState roomState)
    {
        if(roomState == null)
        {
            Debug.LogError("ROOM STATE IS NULL IN INVENTORY");
        }
        
        if(item.Id == StatefulGameObjectId.Money)
        {
            switch(target.Id)
            {
                case StatefulGameObjectId.ElectricityBill:
                    return () => roomState.PayElectricityBill();

                case StatefulGameObjectId.Trash:
                    return () => { GuideText.Instance.SetText("You tried putting money in the trash"); return false; };
            }
        }
        else if(item.Id == StatefulGameObjectId.WateringCan)
        {
            switch(target.Id)
            {
                case StatefulGameObjectId.Flower:
                    return () => roomState.WaterPlant();
            }
        }
        else if(item.Id == StatefulGameObjectId.Flower)
        {
            switch(target.Id)
            {
                case StatefulGameObjectId.Flower:
                    return () => { GuideText.Instance.SetText("You tried putting a flower on a flower"); return false; };
            }
        }
         else if(item.Id == StatefulGameObjectId.Tape)
        {
            switch(target.Id)
            {
                case StatefulGameObjectId.WaterPipe:
                    if(roomState.WaterPipeState == WaterPipeState.PipeBroken)
                    {
                        return () => roomState.FixPipe();
                    }

                    break;
            }
        }

        return null;
    }
}
