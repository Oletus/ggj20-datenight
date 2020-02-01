using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    const int ROOM_COUNT = 4;
    private Room[] generatedRooms = new Room[ROOM_COUNT];
    private RoomState[] roomStates = new RoomState[ROOM_COUNT];

    [SerializeField] private GameObject RoomPrefab;

    private void Awake()
    {
        this.GenerateInitialRooms();  
    }

    private void GenerateInitialRooms()
    {
        var roomContainerObject = new GameObject("Generated Rooms");

        var nextRoomState = new RoomState();
        for (int i = 0; i < ROOM_COUNT; i++)
        {
            var instantiated = Instantiate(RoomPrefab, roomContainerObject.transform);
            instantiated.name = "Room " + i;

            instantiated.transform.position = new Vector3(10, 0, 0) * i; // TODO: correct position and camera setup

            Room room = instantiated.GetComponent<Room>();
            room.RoomIndex = i;
            room.ApplyState(nextRoomState);

            nextRoomState.StateChanged += () => this.RegenerateRoomsFromIndex(i);

            generatedRooms[i] = room;
            nextRoomState = nextRoomState.GenerateNextState();
        }
    }

    private void RegenerateRoomsFromIndex(int startIndex)
    {
        var nextRoomState = roomStates[startIndex];
        for (int i = startIndex; i < ROOM_COUNT; i++)
        {
            generatedRooms[i].ApplyState(nextRoomState);
            if(i != startIndex)
            {
                nextRoomState.StateChanged += () => RegenerateRoomsFromIndex(i);
            }

            roomStates[i] = nextRoomState;
            nextRoomState = nextRoomState.GenerateNextState();
        }
    }
}
