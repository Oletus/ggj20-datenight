using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator Instance { get; private set; }
    const int ROOM_COUNT = 4;

    [ReorderableList]
    [SerializeField] private List<Room> Rooms;

    private RoomState[] roomStates = new RoomState[ROOM_COUNT];

    private void Awake()
    {
        Instance = this;
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

            var localIndex = i;
            nextRoomState.StateChanged += () => this.RegenerateRoomsFromIndex(localIndex);
            roomStates[i] = nextRoomState;
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

    public RoomState GetRoomStateByRoom(Room room)
    {
        var roomIndex = Rooms.IndexOf(room);
        return roomIndex >= 0 ? roomStates[roomIndex] : null;
    }
}
