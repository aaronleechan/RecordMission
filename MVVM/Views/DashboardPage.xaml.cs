using RecordMission;
using System.Timers;
using Microsoft.Maui.Dispatching;
namespace MauiApp1;

public partial class DashboardPage : ContentPage
{

	private System.Timers.Timer countdownTimer;
	private TimeSpan timeLeft;

	public Mission currentMission { get; set; }

	public DashboardPage()
	{
		InitializeComponent();
		BindingContext = new DashboardViewModel();
	}

	private async void AddMission_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new MissionsPage());
		//await Navigation.PushAsync(new TransactionsPage());
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (DashboardViewModel)BindingContext;
		vm.FillData();
		listView.ItemsSource = vm.Missions;
	}


	private async void PlayAction(object sender, EventArgs e)
	{
		DisplayAlert("abc test", "This is Testing", "OK");
	}

	private async void MissionAction(object sender, EventArgs e)
	{
		var button = sender as Button;
		if (button == null) return;

		var mission = button.CommandParameter as Mission;
		if (mission == null) return;

		currentMission = mission;

		if (button.Text == "Start")
		{
			button.Text = "Stop";
			exerciseName.Text = mission.Name;
			timeLeft = mission.Duration;

			// Initialize the countdown timer
			if (countdownTimer == null)
			{
				countdownTimer = new System.Timers.Timer(1000); // Set the interval to 1 second
				countdownTimer.Elapsed += OnTimedEvent;
			}

			countdownTimer.Start();
		}
		else
		{

			button.Text = "Start";
			exerciseName.Text = string.Empty;
			TimerLap.Text = "00:00:00";

			if (countdownTimer != null)
			{

				countdownTimer.Stop();
			}
		}
	}

	private void OnTimedEvent(object sender, ElapsedEventArgs e)
	{
		if (timeLeft > TimeSpan.Zero)
		{
			timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));

			// Update the UI on the main thread
			MainThread.BeginInvokeOnMainThread(() =>
			{
				TimerLap.Text = timeLeft.ToString(@"hh\:mm\:ss"); // Update UI with time left
			});
		}
		else
		{
			countdownTimer.Stop();
			MainThread.BeginInvokeOnMainThread(() =>
			{
				// Reset button text and show alert
				var button = sender as Button;
				if (button != null)
				{
					button.Text = "Start";
				}
				//When completd update the mission data
				UpdateMissionData();
				DisplayAlert("Mission Completed", "The mission duration has ended.", "OK");
			});
		}
	}

	private void UpdateMissionData()
	{
		if (currentMission != null)
		{
			currentMission.Sets += 1;
			App.MissionRepo.SaveItem(currentMission);
			var items = App.MissionRepo.GetItems();
			listView.ItemsSource = items;
		}
	
	}

	private async void onMissionSelected(object sender, SelectionChangedEventArgs e)
	{
		var selectedMission = e.CurrentSelection.FirstOrDefault() as Mission;
		if(selectedMission != null)
		{
			string action = await DisplayActionSheet("Mission", "Cancel", null, "Delete");

			switch(action)
			{
				case "Edit":

					break;
				case "Delete":
					App.MissionRepo.DeleteItem(selectedMission);
					var items = App.MissionRepo.GetItems();
					listView.ItemsSource = items;
					break;
			}
		}
	}
}