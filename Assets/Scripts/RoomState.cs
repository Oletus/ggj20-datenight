
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

    public RoomState GenerateNextState()
    {
        // TODO: This should generate the state for the next day
        return this;
    }
    
}
