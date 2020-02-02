using System;
using System.Collections.Generic;

public class DateNightGameState 
{
    private static DateNightGameState _instance;
    public static DateNightGameState Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new DateNightGameState();
            }

            return _instance;
        }
    }

    public int? FlowerWateredIndex { get; set; } = null;
    public int? PipeFixedIndex { get; set; } = null;
    public int? WindowOpenedIndex { get; set; } = null;
    public int? DogBallIndex { get; set; } = null;
    public HashSet<int> FilledWaterIndicies { get; private set; } = new HashSet<int>();

    public static void Reset()  {
        _instance = new DateNightGameState();
    }
}
