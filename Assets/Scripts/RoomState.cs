
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

public enum CouchState
{
    Ok,
    Broken,
}

public enum DogState
{
    Sleeping,
    Angry,
    Happy
}

public enum WindowState
{
    Closed,
    Broken,
    Open
}

public enum BallState
{
    Hidden,
    OnGround
}

public delegate void RoomStateChanged();
public class RoomState
{ 
    public FlowerState FlowerState { get; private set; }
    public ElectricityBillState ElectricityBillState { get; private set; }
    public CashState CashState { get; private set; }
    public WaterPipeState WaterPipeState { get; private set; }
    public WaterCanState WaterCanState { get; private set; }
    public CouchState CouchState { get; private set; }
    public DogState DogState { get; private set; }
    public WindowState WindowState { get; private set; }
    public BallState BallState { get; private set; }

    private int _daysSincePlumberCalled = 0;
    private bool _plumberCalled = false;
    private bool _plantWatered = false;
    private int _daysPlantHasBeenAlive = 0;
    private bool _hasGivenBallToAngryDog = false;


    //  note: doesn work in constructor
    public Room Room
    {
        get { return RoomGenerator.Instance.GetRoomByRoomState(this); }
    }

    public int RoomIndex
    {
        get { return Room.RoomIndex; }
    } 


    public RoomState()
    {
        FlowerState = FlowerState.Alive;
        ElectricityBillState = ElectricityBillState.OnTable;
        CashState = CashState.OnTable;
        WaterCanState = WaterCanState.FallenDown;
        WaterPipeState = WaterPipeState.PipeBroken;
        WindowState = WindowState.Closed;
        BallState = BallState.Hidden;
        CouchState = CouchState.Ok;
        DogState = DogState.Sleeping;
    }

    private RoomState(RoomState other)
    {
        FlowerState = other.FlowerState;
        ElectricityBillState = other.ElectricityBillState;
        CashState = other.CashState;
        WaterPipeState = other.WaterPipeState;
        CouchState = other.CouchState;
        DogState = other.DogState;
        WindowState = other.WindowState;
        BallState = other.BallState;
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


        int NextRoomIndex = this.RoomIndex + 1;

        // third day
        if(NextRoomIndex ==  2)
        {
            if(DogState == DogState.Angry && !_hasGivenBallToAngryDog)
            {
                nextState.CouchState = CouchState.Broken;
            }

            nextState.DogState = DogState.Sleeping;
        }

        // 4th day
        if(NextRoomIndex == 3)
        {
            if (WindowState == WindowState.Closed)
            {
                nextState.WindowState = WindowState.Broken;
            }
            else if(WindowState == WindowState.Open)
            {
                nextState.BallState = BallState.OnGround;
            }

            nextState.DogState = DogState.Sleeping;
        }

        // second day
        if (NextRoomIndex == 1 && DogState != DogState.Happy)
        {
            nextState.DogState = DogState.Angry;
        }

        if (DateNightGameState.Instance.PipeFixedIndex == NextRoomIndex)
        {
            nextState._plumberCalled = false;
            nextState._daysSincePlumberCalled = 0;
            nextState.WaterPipeState = WaterPipeState.Fixed;
        }

        if(DateNightGameState.Instance.FlowerWateredIndex == NextRoomIndex)
        {
            nextState.FlowerState = FlowerState.Alive;
            nextState._daysPlantHasBeenAlive = 0;
            nextState._plantWatered = true;
        }

        if (DateNightGameState.Instance.WindowOpenedIndex == NextRoomIndex)
        {
            nextState.WindowState = WindowState.Open;
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
            DateNightGameState.Instance.FlowerWateredIndex = this.RoomIndex;
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
        if (WaterPipeState == WaterPipeState.PipeBroken)
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

        this.StateChanged();
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
            GuideText.Instance.SetText("You fixed the pipe");
            DateNightGameState.Instance.PipeFixedIndex = this.RoomIndex;
            this.StateChanged();
            return true;
        }

        return false;
    }

    public bool OpenWindow()
    {
        if(WindowState == WindowState.Closed)
        {
            WindowState = WindowState.Open;
            DateNightGameState.Instance.WindowOpenedIndex = this.RoomIndex;
            this.StateChanged();
            return true;
        }

        return false;
    }

    public bool AddBallToDog()
    {
        if (DogState == DogState.Angry)
        {
            DogState = DogState.Happy;
            DateNightGameState.Instance.DogBallIndex = this.RoomIndex;
            GuideText.Instance.SetText("The dog is happy with the ball!");
            this.StateChanged();
            return true;
        }

        return false;
    }
    
}
