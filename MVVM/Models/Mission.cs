namespace RecordMission;

public class Mission : TableData
{
    public string Name { get; set; } = string.Empty;
    public int Sets { get; set; } = 0;
    public int Reps { get; set; } = 0;
    public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(1);

}
