using System.Collections.ObjectModel;
using TaxiParkAppMobile1.Models;
using TaxiParkAppMobile1.Services;

namespace TaxiParkAppMobile1.Pages;

public partial class CarsPage : ContentPage
{
    private readonly TaxiDataService _dataService;
    private Car? _selectedCar;

    public ObservableCollection<Car> Cars { get; } = new();

    public CarsPage(TaxiDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCarsAsync();
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

    private void OnCarSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedCar = e.CurrentSelection.FirstOrDefault() as Car;
        if (_selectedCar is null)
        {
            DeleteCarButton.IsEnabled = false;
            return;
        }

        BrandEntry.Text = _selectedCar.Brand;
        ModelEntry.Text = _selectedCar.Model;
        PlateEntry.Text = _selectedCar.LicensePlate;
        YearEntry.Text = _selectedCar.Year.ToString();
        ColorEntry.Text = _selectedCar.Color;
        DeleteCarButton.IsEnabled = true;
    }

    private async void OnSaveCar(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(BrandEntry.Text) || string.IsNullOrWhiteSpace(ModelEntry.Text))
        {
            await DisplayAlert("Ошибка", "Марка и модель обязательны", "OK");
            return;
        }

        var car = _selectedCar ?? new Car();
        car.Brand = BrandEntry.Text.Trim();
        car.Model = ModelEntry.Text.Trim();
        car.LicensePlate = PlateEntry.Text?.Trim() ?? string.Empty;
        car.Color = ColorEntry.Text?.Trim() ?? string.Empty;
        car.Year = int.TryParse(YearEntry.Text, out var year) ? year : 0;

        await _dataService.SaveCarAsync(car);
        await LoadCarsAsync();
        ClearForm();
    }

    private async void OnCarDeleteRequested(object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipeItem || swipeItem.BindingContext is not Car car)
        {
            return;
        }

        var confirm = await DisplayAlert("Удалить автомобиль?", $"{car.Brand} {car.Model}", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        await _dataService.DeleteCarAsync(car.Id);
        await LoadCarsAsync();
        ClearForm();
    }

    private async void OnDeleteCar(object sender, EventArgs e)
    {
        if (_selectedCar is null)
        {
            return;
        }

        var confirm = await DisplayAlert("Удалить автомобиль?", $"{_selectedCar.Brand} {_selectedCar.Model}", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        await _dataService.DeleteCarAsync(_selectedCar.Id);
        await LoadCarsAsync();
        ClearForm();
    }

    private void OnClearCar(object sender, EventArgs e) => ClearForm();

    private void ClearForm()
    {
        _selectedCar = null;
        CarsCollection.SelectedItem = null;
        BrandEntry.Text = string.Empty;
        ModelEntry.Text = string.Empty;
        PlateEntry.Text = string.Empty;
        YearEntry.Text = string.Empty;
        ColorEntry.Text = string.Empty;
        DeleteCarButton.IsEnabled = false;
    }
}

