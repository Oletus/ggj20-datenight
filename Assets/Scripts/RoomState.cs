
public enum FlowerState
{
    AliveNotWatered,
    Dry,

    // "Completed"
    AliveWatered,
}

public enum ElectricityBillState
{
    OnTable,
    InTrashBin,
    InAnEmptiedTrashBinWithCashOnTable,
    NoElectricity,

    // "Completed"
    Paid
}


public class RoomState
{
    public FlowerState FlowerState;
    public ElectricityBillState ElectricityBillState;

    private RoomState(RoomState other)
    {
        FlowerState = other.FlowerState;
        ElectricityBillState = other.ElectricityBillState;
    }

    public RoomState GenerateNextState()
    {
        RoomState nextState = new RoomState(this);

        if ( FlowerState == FlowerState.AliveNotWatered )
        {
            nextState.FlowerState = FlowerState.Dry;
        }
        return nextState;
    }
    
}
