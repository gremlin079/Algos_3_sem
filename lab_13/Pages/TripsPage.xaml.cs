using System.Collections.ObjectModel;
using TaxiParkAppMobile1.Models;
using TaxiParkAppMobile1.Services;

namespace TaxiParkAppMobile1.Pages;

public partial class TripsPage : ContentPage
{
    private readonly TaxiDataService _dataService;
    private Trip? _selectedTrip;

    public ObservableCollection<Trip> Trips { get; } = new();
    public ObservableCollection<Driver> Drivers { get; } = new();
    public ObservableCollection<Car> Cars { get; } = new();

    public TripsPage(TaxiDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        await LoadDriversAsync();
        await LoadCarsAsync();
        await LoadTripsAsync();
        TripDatePicker.Date = DateTime.Today;
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

    private async Task LoadCarsAsync()
    {
        Cars.Clear();
        var cars = await _dataService.GetCarsAsync();
        foreach (var car in cars)
        {
            Cars.Add(car);
        }
    }

    private async Task LoadTripsAsync()
    {
        Trips.Clear();
        var trips = await _dataService.GetTripsAsync();
        foreach (var trip in trips)
        {
            Trips.Add(trip);
        }
    }

    private void OnTripSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedTrip = e.CurrentSelection.FirstOrDefault() as Trip;
        if (_selectedTrip is null)
        {
            DeleteTripButton.IsEnabled = false;
            return;
        }

        FromEntry.Text = _selectedTrip.FromAddress;
        ToEntry.Text = _selectedTrip.ToAddress;
        TripDatePicker.Date = _selectedTrip.TripDate;
        PriceEntry.Text = _selectedTrip.Price.ToString("0.##");
        DriverPicker.SelectedItem = Drivers.FirstOrDefault(d => d.Id == _selectedTrip.DriverId);
        CarPicker.SelectedItem = Cars.FirstOrDefault(c => c.Id == _selectedTrip.CarId);
        DeleteTripButton.IsEnabled = true;
    }

    private async void OnSaveTrip(object sender, EventArgs e)
    {
        if (DriverPicker.SelectedItem is not Driver driver || CarPicker.SelectedItem is not Car car)
        {
            await DisplayAlert("Ошибка", "Выберите водителя и автомобиль", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(FromEntry.Text) || string.IsNullOrWhiteSpace(ToEntry.Text))
        {
            await DisplayAlert("Ошибка", "Заполните адреса", "OK");
            return;
        }

        var trip = _selectedTrip ?? new Trip();
        trip.FromAddress = FromEntry.Text.Trim();
        trip.ToAddress = ToEntry.Text.Trim();
        trip.TripDate = TripDatePicker.Date;
        trip.Price = decimal.TryParse(PriceEntry.Text, out var price) ? price : 0;
        trip.DriverId = driver.Id;
        trip.CarId = car.Id;

        await _dataService.SaveTripAsync(trip);
        await LoadTripsAsync();
        ClearForm();
    }

    private async void OnTripDeleteRequested(object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem || swipeItem.BindingContext is not Trip trip)
        {
            return;
        }

        var confirm = await DisplayAlert("Удалить поездку?", $"{trip.FromAddress} → {trip.ToAddress}", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        await _dataService.DeleteTripAsync(trip.Id);
        await LoadTripsAsync();
        ClearForm();
    }

    private async void OnDeleteTrip(object sender, EventArgs e)
    {
        if (_selectedTrip is null)
        {
            return;
        }

        var confirm = await DisplayAlert("Удалить поездку?", $"{_selectedTrip.FromAddress} → {_selectedTrip.ToAddress}", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        await _dataService.DeleteTripAsync(_selectedTrip.Id);
        await LoadTripsAsync();
        ClearForm();
    }

    private void OnClearTrip(object sender, EventArgs e) => ClearForm();

    private void ClearForm()
    {
        _selectedTrip = null;
        TripsCollection.SelectedItem = null;
        FromEntry.Text = string.Empty;
        ToEntry.Text = string.Empty;
        PriceEntry.Text = string.Empty;
        DriverPicker.SelectedItem = null;
        CarPicker.SelectedItem = null;
        TripDatePicker.Date = DateTime.Today;
        DeleteTripButton.IsEnabled = false;
    }
}

