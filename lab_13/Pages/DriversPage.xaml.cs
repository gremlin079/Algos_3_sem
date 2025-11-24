using System.Collections.ObjectModel;
using TaxiParkAppMobile1.Models;
using TaxiParkAppMobile1.Services;

namespace TaxiParkAppMobile1.Pages;

public partial class DriversPage : ContentPage
{
    private readonly TaxiDataService _dataService;
    private Driver? _selectedDriver;

    public ObservableCollection<Driver> Drivers { get; } = new();

    public DriversPage(TaxiDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDriversAsync();
    }

    private async Task LoadDriversAsync()
    {
        Drivers.Clear();
        var drivers = await _dataService.GetDriversAsync();
        foreach (var driver in drivers)
        {
            Drivers.Add(driver);
        }
    }

    private void OnDriverSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedDriver = e.CurrentSelection.FirstOrDefault() as Driver;
        if (_selectedDriver is null)
        {
            DeleteDriverButton.IsEnabled = false;
            return;
        }

        FirstNameEntry.Text = _selectedDriver.FirstName;
        LastNameEntry.Text = _selectedDriver.LastName;
        PhoneEntry.Text = _selectedDriver.Phone;
        LicenseEntry.Text = _selectedDriver.LicenseNumber;
        ExperienceEntry.Text = _selectedDriver.Experience.ToString();
        DeleteDriverButton.IsEnabled = true;
    }

    private async void OnSaveDriver(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) || string.IsNullOrWhiteSpace(LastNameEntry.Text))
        {
            await DisplayAlert("Ошибка", "Имя и фамилия обязательны", "OK");
            return;
        }

        var driver = _selectedDriver ?? new Driver();
        driver.FirstName = FirstNameEntry.Text.Trim();
        driver.LastName = LastNameEntry.Text.Trim();
        driver.Phone = PhoneEntry.Text?.Trim() ?? string.Empty;
        driver.LicenseNumber = LicenseEntry.Text?.Trim() ?? string.Empty;
        driver.Experience = int.TryParse(ExperienceEntry.Text, out var exp) ? exp : 0;

        await _dataService.SaveDriverAsync(driver);
        await LoadDriversAsync();
        ClearForm();
    }

    private async void OnDriverDeleteRequested(object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem || swipeItem.BindingContext is not Driver driver)
        {
            return;
        }

        var confirm = await DisplayAlert("Удалить водителя?", $"{driver.FirstName} {driver.LastName}", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        await _dataService.DeleteDriverAsync(driver.Id);
        await LoadDriversAsync();
        ClearForm();
    }

    private async void OnDeleteDriver(object sender, EventArgs e)
    {
        if (_selectedDriver is null)
        {
            return;
        }

        var confirm = await DisplayAlert("Удалить водителя?", $"{_selectedDriver.FirstName} {_selectedDriver.LastName}", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        await _dataService.DeleteDriverAsync(_selectedDriver.Id);
        await LoadDriversAsync();
        ClearForm();
    }

    private void OnClearDriver(object sender, EventArgs e) => ClearForm();

    private void ClearForm()
    {
        _selectedDriver = null;
        DriversCollection.SelectedItem = null;
        FirstNameEntry.Text = string.Empty;
        LastNameEntry.Text = string.Empty;
        PhoneEntry.Text = string.Empty;
        LicenseEntry.Text = string.Empty;
        ExperienceEntry.Text = string.Empty;
        DeleteDriverButton.IsEnabled = false;
    }
}

