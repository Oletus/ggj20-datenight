using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomState
{

    public enum FlowerStateType
    {
        Alive,
        Dry
    }

    private FlowerStateType FlowerState;

    public void ApplyState(Room room)
    {
        if (FlowerState == FlowerStateType.Alive)
        {
            // Room.Flower.SetState(FlowerStateType.Alive);
        }
        else
        {
            // Room.Flower.SetState(FlowerStateType.Dry);
        }
    }

    public RoomState GenerateNextState()
    {
        // TODO: This should generate the state for the next day
        return null;
    }
    
}
