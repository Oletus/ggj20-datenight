using UnityEngine;

#pragma warning disable 0649

public class Room : MonoBehaviour
{
    [SerializeField] private StatefulGameObject Flower;
    [SerializeField] private StatefulGameObject ElectricityBill;
    [SerializeField] private StatefulGameObject WaterPipe;
    [System.NonSerialized] public int RoomIndex;

    public void ApplyState(RoomState roomState)
    {
        this.Flower?.SetState(roomState.FlowerState);
        this.WaterPipe.SetState(roomState.WaterPipeState);
        this.ElectricityBill?.SetState(roomState.ElectricityBillState);
    }
}
