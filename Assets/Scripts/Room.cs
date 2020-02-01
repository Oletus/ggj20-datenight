using UnityEngine;

[RequireComponent(typeof(StatefulFlower), typeof(StatefulElectricityBill))]
public class Room : MonoBehaviour
{
    public StatefulFlower Flower { get { return this.GetComponent<StatefulFlower>(); } }
    public StatefulElectricityBill ElectricityBill { get { return this.GetComponent<StatefulElectricityBill>(); } }

    public void ApplyState(RoomState roomState)
    {
        this.Flower.SetState(roomState.FlowerState);
        this.ElectricityBill.SetState(roomState.ElectricityBillState);
    }
}
