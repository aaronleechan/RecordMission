using RecordMission;

namespace RecordMission.MVVM.ViewModels;

public class MissionsViewModel
{
    public Mission Mission { get; set; } = new Mission{

    };
    public string SaveMission()
    {
        try
        {
            App.MissionRepo.SaveItem(Mission);
            return App.MissionRepo.StatusMessage;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}