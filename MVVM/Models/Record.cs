namespace RecordMission;

public class Record: TableData
{
    public string Name { get; set; } = string.Empty;
    public int Sets { get; set; } = 0;
    public double Weights { get; set; } = 0;
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public DateTime WorkoutDate { get; set; } = DateTime.Now;
}

