using UnityEngine;

#pragma warning disable 0649

public class Room : MonoBehaviour
{
    [SerializeField] private StatefulGameObject Flower;
    [SerializeField] private StatefulGameObject ElectricityBill;
    [SerializeField] private StatefulGameObject WaterPipe;
    [SerializeField] private StatefulGameObject WaterCan;
    [SerializeField] private StatefulGameObject Couch;
    [SerializeField] private StatefulGameObject Window;
    [SerializeField] private StatefulGameObject Dog;
    [SerializeField] private StatefulGameObject Ball;
    [System.NonSerialized] public int RoomIndex;

    public void ApplyState(RoomState roomState)
    {
        this.Flower?.SetState(roomState.FlowerState);
        this.WaterPipe.SetState(roomState.WaterPipeState);
        this.WaterCan.SetState(roomState.WaterCanState);
        this.Flower.SetState(roomState.FlowerState);
        this.ElectricityBill?.SetState(roomState.ElectricityBillState);
        this.Couch.SetState(roomState.CouchState);
        this.Window.SetState(roomState.WindowState);
        this.Dog.SetState(roomState.DogState);
        this.Ball.SetState(roomState.BallState);
    }
}
