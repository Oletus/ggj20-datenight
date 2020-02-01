
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

public enum WaterPipeState
{
    PipeBroken, // Initial state
    PlumberFixing, // after plumber is called, it takes 3 days for him to come. So must be called on day for plumber to appear on day 4
    Fixed, //  Fixed with own tools
}

public delegate void RoomStateChanged();
public class RoomState
{ 
    public FlowerState FlowerState { get; private set; }
    public ElectricityBillState ElectricityBillState { get; private set; }
    public CashState CashState { get; private set; }
    public WaterPipeState WaterPipeState { get; private set; }

    private int _daysSincePlumberCalled = 0;
    private bool _plumberCalled = false;

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
        _daysSincePlumberCalled = other._daysSincePlumberCalled;
        _plumberCalled = other._plumberCalled;
    }

    public event RoomStateChanged StateChanged;

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

        if(_plumberCalled && WaterPipeState == WaterPipeState.PipeBroken)
        {
            if(++nextState._daysSincePlumberCalled == 3)
            {
                nextState.WaterPipeState = WaterPipeState.PlumberFixing;
            }
        }

        return nextState;
    }

    public bool WaterPlant()
    {
        if ( FlowerState == FlowerState.AliveNotWatered || FlowerState == FlowerState.AliveWatered )
        {
            FlowerState = FlowerState.AliveWatered;
            // TODO: Play sound?
            this.StateChanged();
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
            this.StateChanged();
            return true;
        }
        return false;
    }

    public bool CallPlumber()
    {
        if(WaterPipeState == WaterPipeState.PipeBroken)
        {
            if(!_plumberCalled)
            {
                _plumberCalled = true;
                GuideText.Instance.SetText("\"Hello? Yes, this is plumber. You need to fix a leaking pipe? Okay, I will come in three days from  now\"");
                this.StateChanged();
                return true;
            }
            else
            {
                GuideText.Instance.SetText("You have already called the plumber");
            }
        }

        return false;
    }
    
}
