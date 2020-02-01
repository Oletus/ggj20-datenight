using System;
using LPUnityUtils;
using UnityEngine;

#pragma warning disable 0649

public class Inventory : MonoBehaviour
{
    private Vector3? _returnCurrentPickedObjectPosition = null;
    private StatefulGameObject CurrentPickedObject;
    private Pointer CurrentPointer;

    [SerializeField] private Camera PickingCamera;

    private StatefulGameObject _CurrentHoverObject;
    private StatefulGameObject CurrentHoverObject
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
            if(_returnCurrentPickedObjectPosition.HasValue)
            {
                CurrentPickedObject.ResetPosition(_returnCurrentPickedObjectPosition.Value);
                _returnCurrentPickedObjectPosition = null;

            }
            this.CurrentPickedObject = null;
        }

        // TODO: Update hilight
    }

    private StatefulGameObject GetObjectFromRay(Ray ray)
    {
        RaycastHit[] raycastHits = Physics.RaycastAll(ray);
        if ( raycastHits.Length > 0 )
        {
            foreach ( RaycastHit hitInfo in raycastHits )
            {
                StatefulGameObject hitObject = hitInfo.transform.GetComponentInParent<StatefulGameObject>();
                if ( hitObject == null || hitObject == this.CurrentPickedObject )
                {
                    continue;
                }
                return hitObject;
            }
        }
        return null;
    }

    private void UpdatePicking()
    {
        Pointer pointerDown = Pointer.CreateOnPointerDown();
        if (pointerDown != null)
        {
            StatefulGameObject hitObject = GetObjectFromRay(pointerDown.GetRay(PickingCamera));
            if (hitObject != null)
            {
                // pick up the item if no item is currently picked up
                if (CurrentPickedObject == null)
                {
                    if (hitObject.CanPickUp())
                    {
                        CurrentPickedObject = hitObject;
                        CurrentPointer = pointerDown;
                        _returnCurrentPickedObjectPosition = CurrentPickedObject.transform.position;
                    }
                }
                // try to use the picked object on another item
                else
                {
                    var useItemAction = TryGetUseItemAction(CurrentPickedObject, hitObject, RoomGenerator.Instance.GetRoomStateByRoom(hitObject.ParentRoom));
                    if (useItemAction != null)
                    {
                        bool success = useItemAction();
                        if(success)
                        {

                            CurrentPickedObject.DisableAll();
                            CurrentPickedObject = null;
                        }
                    }
                }
            } 
        }
    }

    private Func<bool> TryGetUseItemAction(StatefulGameObject item, StatefulGameObject target, RoomState roomState)
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

        return null;
    }
}
