using RecordMission;
using RecordMission.MVVM.ViewModels;

namespace MauiApp1;

public partial class MissionsPage : ContentPage
{
	public MissionsPage()
	{
		InitializeComponent();
		BindingContext = new MissionsViewModel();
	}

	private async void Save_Clicked(object sender, EventArgs e)
	{
		var currentVM = (MissionsViewModel)BindingContext;	
		var message = currentVM.SaveMission();
		await DisplayAlert("Save", message, "OK");
		await Navigation.PopToRootAsync();
	}

	private async void Cancel_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopToRootAsync();
	}
}