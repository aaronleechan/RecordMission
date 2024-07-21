using RecordMission;
using System.Timers;
using Microsoft.Maui.Dispatching;
using Plugin.Maui.Audio;
namespace MauiApp1;

public partial class DashboardPage : ContentPage
{

	private System.Timers.Timer countdownTimer;
	private TimeSpan timeLeft;

	public Mission currentMission { get; set; }

	private CancellationTokenSource _cancellationTokenSource;

	public DashboardPage()
	{
		InitializeComponent();
		BindingContext = new DashboardViewModel();
	}

	private async void AddMission_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new MissionsPage());
	}

	public async void PlayAudio()
    {
        var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("countDown.wav"));

        audioPlayer.Play();
    }

    public async void StopAudio()
    {
        var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("countDown.wav"));
        audioPlayer.Stop();
    }
	

	private async void onPlayAllAction(object sender, EventArgs e)
	{
		if (_cancellationTokenSource != null)
		{
			var cancelAction = await DisplayActionSheet("Do you want to Cancel the mission?", "Cancel", null, "Yes", "No");
			if (cancelAction == "Yes")
			{
				_cancellationTokenSource.Cancel();
				_cancellationTokenSource = null;
				DefaultText();
				return;
			}
		}
		else
		{
			_cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = _cancellationTokenSource.Token;

			var action = await DisplayActionSheet("Do you want to Play All Missions?", "Cancel", null, "Yes", "No");

			if (action == "No")
			{
				_cancellationTokenSource = null;
				return;
			}

			var missions = App.MissionRepo.GetItems();

			foreach (var mission in missions)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}

				currentMission = mission;

				TimeSpan duration = currentMission.Duration;
				int totalSeconds = (int)duration.TotalSeconds;
				exerciseName.Text = currentMission.Name;

				while (totalSeconds >= 0)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						break;
					}

					if(totalSeconds == 3)
					{
						PlayAudio();
					}

					if(totalSeconds == 0){
						UpdateMissionData();
						StopAudio();
					}

					int minutes = totalSeconds / 60;
					int seconds = totalSeconds % 60;
					Device.BeginInvokeOnMainThread(() =>
					{
						TimerLap.Text = $"{minutes}:{seconds:D2}";
					});

					try
					{
						await Task.Delay(1000, cancellationToken);
					}
					catch (TaskCanceledException)
					{
						StopAudio();
						break;
					}
					totalSeconds--;
				}

				

				if (cancellationToken.IsCancellationRequested)
				{
					StopAudio();
					break;
				}

				int restTotalSeconds = (int)TimeSpan.FromSeconds(10).TotalSeconds;
				exerciseName.Text = "Resting...";

				while (restTotalSeconds >= 0)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						StopAudio();
						break;
					}

					if(restTotalSeconds == 3)
					{
						PlayAudio();
					}

					if(restTotalSeconds == 0){
						StopAudio();
					}


					int restMinutes = restTotalSeconds / 60;
					int restSeconds = restTotalSeconds % 60;
					Device.BeginInvokeOnMainThread(() =>
					{
						TimerLap.Text = $"{restMinutes}:{restSeconds:D2}";
					});

					try
					{
						await Task.Delay(1000, cancellationToken);
					}
					catch (TaskCanceledException)
					{
						break;
					}
					restTotalSeconds--;
				}

				

				if (cancellationToken.IsCancellationRequested)
				{
					StopAudio();
					break;
				}

				// try
				// {
				// 	await Task.Delay(5000, cancellationToken);
				// }
				// catch (TaskCanceledException)
				// {
				// 	StopAudio();
				// 	break;
				// }
			}

			if (!cancellationToken.IsCancellationRequested)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					StopAudio();
					exerciseName.Text = string.Empty;
					TimerLap.Text = "00:00:00";
				});
			}
			else
			{
				DefaultText();
			}

			_cancellationTokenSource = null;
		}
	}

	private void DefaultText()
	{
		Device.BeginInvokeOnMainThread(() =>
		{
			StopAudio();
			exerciseName.Text = string.Empty;
			TimerLap.Text = "00:00:00";
		});
	}

	private async void onMissionReset(object sender, EventArgs e)
	{
		string action = await DisplayActionSheet("Do you want to Reset All Mission", "Cancel", null, "Yes","No");

		switch(action)
		{
			case "Yes":
				var items = App.MissionRepo.GetItems();
				foreach(var item in items)
				{
					item.Sets = 0;
					App.MissionRepo.SaveItem(item);
				}
				var allItems = App.MissionRepo.GetItems();
				listView.ItemsSource = allItems;
				break;
			case "No":
				break;
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (DashboardViewModel)BindingContext;
		vm.FillData();
		listView.ItemsSource = vm.Missions;
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

			if(timeLeft == TimeSpan.FromSeconds(3))
			{
				PlayAudio();
			}

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
				StopAudio();
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

	private void DurationEntry_Completed(object sender, EventArgs e)
	{
		if (sender is Entry entry && entry.BindingContext is Mission mission)
		{
			// Assuming mission.Duration is already updated due to TwoWay binding
			App.MissionRepo.SaveItem(mission);
			var items = App.MissionRepo.GetItems();
			listView.ItemsSource = items;
		}
	}
}