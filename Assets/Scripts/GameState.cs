using System;
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
}
