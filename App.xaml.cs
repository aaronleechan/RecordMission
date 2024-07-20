using MauiApp1;

namespace RecordMission;

public partial class App : Application
{

	public static BaseRepository<Mission> MissionRepo { get; private set; }
	public static BaseRepository<Record> RecordRepo { get; private set; }
	public App(BaseRepository<Mission> _missionRepo, BaseRepository<Record> _recordRepo)
	{
		InitializeComponent();

		MissionRepo = _missionRepo;
		RecordRepo = _recordRepo;

		MainPage = new NavigationPage(new DashboardPage());
	}

}
