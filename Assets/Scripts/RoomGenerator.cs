using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    const int ROOM_COUNT = 4;

    [ReorderableList]
    [SerializeField] private List<Room> Rooms;

    private RoomState[] roomStates = new RoomState[ROOM_COUNT];

    private void Awake()
    {
        GenerateInitialRoomStates();
    }

    private void GenerateInitialRoomStates()
    {
        var nextRoomState = new RoomState();
        for (int i = 0; i < ROOM_COUNT; i++)
        {
            Room room = Rooms[i];
            room.RoomIndex = i;
            room.ApplyState(nextRoomState);

            nextRoomState.StateChanged += () => this.RegenerateRoomsFromIndex(i);
            nextRoomState = nextRoomState.GenerateNextState();
        }
    }

    private void RegenerateRoomsFromIndex(int startIndex)
    {
        var nextRoomState = roomStates[startIndex];
        for (int i = startIndex; i < ROOM_COUNT; i++)
        {
            Rooms[i].ApplyState(nextRoomState);
            if(i != startIndex)
            {
                nextRoomState.StateChanged += () => RegenerateRoomsFromIndex(i);
            }

            roomStates[i] = nextRoomState;
            nextRoomState = nextRoomState.GenerateNextState();
        }
    }
}
