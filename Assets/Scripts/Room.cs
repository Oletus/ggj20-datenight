using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private StatefulFlower Flower;
    [SerializeField] private StatefulElectricityBill ElectricityBill;

    public void ApplyState(RoomState roomState)
    {
        this.Flower.SetState(roomState.FlowerState);
        this.ElectricityBill.SetState(roomState.ElectricityBillState);
    }
}
