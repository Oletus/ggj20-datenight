using UnityEngine;

#pragma warning disable 0649

public class Room : MonoBehaviour
{
    [SerializeField] private StatefulGameObject Flower;
    [SerializeField] private StatefulGameObject ElectricityBill;
    public int RoomIndex;

    public void ApplyState(RoomState roomState)
    {
        Debug.Log(roomState.FlowerState.ToString());
        this.Flower.SetState(roomState.FlowerState);
        this.ElectricityBill.SetState(roomState.ElectricityBillState);
    }
}
