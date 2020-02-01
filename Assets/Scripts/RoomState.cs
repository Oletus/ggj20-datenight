
public enum FlowerState
{
    Alive,
    Lack,
    Dead,
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

public enum WaterCanState
{
    FallenDown,
    Filled,
}

public delegate void RoomStateChanged();
public class RoomState
{ 
    public FlowerState FlowerState { get; private set; }
    public ElectricityBillState ElectricityBillState { get; private set; }
    public CashState CashState { get; private set; }
    public WaterPipeState WaterPipeState { get; private set; }
    public WaterCanState WaterCanState { get; private set; }

    private int _daysSincePlumberCalled = 0;
    private bool _plumberCalled = false;
    private bool _plantWatered = false;
    private int _daysPlantHasBeenAlive = 0;

    public RoomState()
    {
        FlowerState = FlowerState.Alive;
        ElectricityBillState = ElectricityBillState.OnTable;
        CashState = CashState.OnTable;
        WaterCanState = WaterCanState.FallenDown;
        WaterPipeState = WaterPipeState.PipeBroken;
    }

    private RoomState(RoomState other)
    {
        FlowerState = other.FlowerState;
        ElectricityBillState = other.ElectricityBillState;
        CashState = other.CashState;
        WaterPipeState = other.WaterPipeState;
        _daysSincePlumberCalled = other._daysSincePlumberCalled;
        _plumberCalled = other._plumberCalled;
        _plantWatered = other._plantWatered;
        _daysPlantHasBeenAlive = other._daysPlantHasBeenAlive;
    }

    public event RoomStateChanged StateChanged;

    public RoomState GenerateNextState()
    {
        RoomState nextState = new RoomState(this);

        switch(FlowerState)
        {
            case FlowerState.Alive:
                if(!_plantWatered)
                {
                    if(_daysPlantHasBeenAlive == 0)
                    {
                        nextState._daysPlantHasBeenAlive++;
                    }
                    else
                    {
                        nextState.FlowerState = FlowerState.Lack;
                    }
                }
                break;
            case FlowerState.Lack:
                if (!_plantWatered)
                {
                    nextState.FlowerState = FlowerState.Dead;
                }
                break;
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
        if ( FlowerState == FlowerState.Alive || FlowerState == FlowerState.Lack)
        {
            FlowerState = FlowerState.Alive;
            _plantWatered = true;
            _daysPlantHasBeenAlive = 0;
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

    public bool FillWateringCan()
    {
        if(WaterCanState == WaterCanState.FallenDown)
        {
            WaterCanState = WaterCanState.Filled;
            this.StateChanged();
            return true;
        }

        return false;
    }

    public bool FixPipe()
    {
        if(WaterPipeState == WaterPipeState.PipeBroken)
        {
            WaterPipeState = WaterPipeState.Fixed;
            this.StateChanged();
            GuideText.Instance.SetText("You fixed the pipe");
            return true;
        }

        return false;
    }
}
