using System.Collections.ObjectModel;
using Plugin.Maui.Audio;

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

}
