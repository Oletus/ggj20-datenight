
public enum FlowerState
{
    Alive,
    Dry
}

public enum ElectricityBillState
{
    OnTable,
    InTrashBin,
    InAnEmptiedTrashBinWithCashOnTable,
    NoElectricity
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
