using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject RoomPrefab;
    public RoomState InitialRoomState = new RoomState
    {
        FlowerState = FlowerState.AliveNotWatered,
        ElectricityBillState = ElectricityBillState.OnTable
    };

    public void GenerateRoom()
    {
        var nextRoomState = this.InitialRoomState;
        const int RoomsToGenerate = 4;
        for (int i = 0; i < RoomsToGenerate; i++)
        {
            var instantiated = Instantiate(RoomPrefab);
            instantiated.transform.position = new Vector3(10, 0, 0) * i; // TODO: correct position and camera setup

            Room room = instantiated.GetComponent<Room>();
            room.ApplyState(nextRoomState);


            nextRoomState = nextRoomState.GenerateNextState();
        }
    }
}
