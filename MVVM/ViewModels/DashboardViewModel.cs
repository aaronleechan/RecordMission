using System.Collections.ObjectModel;

namespace RecordMission;

public class DashboardViewModel
{

    public ObservableCollection<Record> Records { get; set; }
    public ObservableCollection<Mission> Missions { get; set; } 
    public DashboardViewModel()
    {
        FillData();
    }

    public void FillData()
    {
        var missions = App.MissionRepo.GetItems().ToList();
        Missions = new ObservableCollection<Mission>(missions);
    }

    // public async string PlayAllAction()
    // {
    //     foreach (var mission in Missions)
    //     {
    //         Console.WriteLine("Mission Name: " + mission.Name);
    //         Console.WriteLine("Mission Duration: " + mission.Duration);
    //         Console.WriteLine("___________________________________");
    //         return "Mission Name: " + mission.Name + "Mission Duration: " + mission.Duration;
    //     }
    // }

}
