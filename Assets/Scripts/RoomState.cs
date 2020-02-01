
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
    TakenOutOfTrash,
    NoElectricity,

    // "Completed"
    Paid
}

public enum CashState
{
    OnTable,
    InTrashBin,
    TakenOutOfTrash,
    Used
}

public class RoomState
{
    public FlowerState FlowerState { get; private set; }
    public ElectricityBillState ElectricityBillState { get; private set; }
    public CashState CashState { get; private set; }

    public RoomState()
    {
        FlowerState = FlowerState.AliveNotWatered;
        ElectricityBillState = ElectricityBillState.OnTable;
        CashState = CashState.OnTable;
    }

    private RoomState(RoomState other)
    {
        FlowerState = other.FlowerState;
        ElectricityBillState = other.ElectricityBillState;
        CashState = other.CashState;
    }

    public RoomState GenerateNextState()
    {
        RoomState nextState = new RoomState(this);

        if ( FlowerState == FlowerState.AliveNotWatered )
        {
            nextState.FlowerState = FlowerState.Dry;
        }
        switch ( ElectricityBillState )
        {
            case ElectricityBillState.InTrashBin:
                nextState.ElectricityBillState = ElectricityBillState.TakenOutOfTrash;
                break;
            case ElectricityBillState.TakenOutOfTrash:
                nextState.ElectricityBillState = ElectricityBillState.NoElectricity;
                break;
        }
        switch ( CashState )
        {
            case CashState.InTrashBin:
                nextState.CashState = CashState.TakenOutOfTrash;
                break;
        }
        return nextState;
    }

    public bool WaterPlant()
    {
        if ( FlowerState == FlowerState.AliveNotWatered || FlowerState == FlowerState.AliveWatered )
        {
            FlowerState = FlowerState.AliveWatered;
            // TODO: Play sound?
            return true;
        }
        return false;
    }

    public bool PayElectricityBill()
    {
        if ( ElectricityBillState == ElectricityBillState.OnTable )
        {
            ElectricityBillState = ElectricityBillState.Paid;
            // TODO: Play sound?
            return true;
        }
        return false;
    }
    
}
