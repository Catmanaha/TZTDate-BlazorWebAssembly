using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Tewr.Blazor.FileReader;
using TZTDateBlazorWebAssembly.Models;

namespace TZTDate_BlazorWebAssembly.Pages.Auth;

public partial class RegisterBase : ComponentBase
{
    public string cUrl = "https://api.countrystatecity.in/v1/countries";
    public string ckey = "NHhvOEcyWk50N2Vna3VFTE00bFp3MjFKR0ZEOUhkZlg4RTk1MlJlaA==";
    public List<SelectListItem> CountryOptions { get; set; } = new();
    public List<SelectListItem> CityOptions { get; set; } = new();
    public List<SelectListItem> StateOptions { get; set; } = new();
    public int CurrentStep { get; set; } = 1;
    public List<string> StepLabels { get; set; } = new List<string> { "User Details", "Location", "Preferences", "Photos" };

    public string selectedCountryCode;
    public UserRegisterDto userRegisterDto { get; set; } = new();

    public List<string> interests = new List<string>
        {
            "Sport", "Games", "Traveling", "Reading", "Cooking", "Hiking", "Photography", "Music",
            "Painting", "Writing",
            "Gardening", "Yoga", "Meditation", "Astronomy", "Dancing", "Film-making", "Bird-watching",
            "Knitting", "Surfing",
            "Scuba diving", "Chess", "Calligraphy"
        };
    public List<CheckboxListItem> InterestsList { get; set; }
    public StringBuilder interestsStringBuilder = new(100);

    public RegisterBase()
    {
        InterestsList = interests.Select(interest => new CheckboxListItem
        {
            Label = interest
        }).ToList();
    }

    public void UpdateInterests(string interest, bool isChecked)
    {
        InterestsList.FirstOrDefault(o => o.Label == interest).IsChecked = isChecked;

        if (isChecked)
        {
            interestsStringBuilder.Append(interest + " ");
            userRegisterDto.Interests = interestsStringBuilder.ToString();
        }
        else
        {
            interestsStringBuilder.Remove(interestsStringBuilder.ToString().IndexOf(interest + " "), interest.Length + 1);
            userRegisterDto.Interests = interestsStringBuilder.ToString();
        }
    }

    public void NavigateToFormStep(int stepNumber)
    {
        CurrentStep = stepNumber;
    }

    public async Task LoadStatesAsync(ChangeEventArgs e)
    {
        selectedCountryCode = e.Value.ToString();

        try
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Add("X-CSCAPI-KEY", ckey);
            var response = await httpclient.GetStringAsync($"{cUrl}/{selectedCountryCode}/states");
            var states = JsonSerializer.Deserialize<List<State>>(response);

            StateOptions = states.Select(state => new SelectListItem
            {
                Value = state.Iso2,
                Text = state.Name
            }).ToList();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading states: {ex.Message}");
        }
    }

    public async Task LoadCitiesAsync(ChangeEventArgs e)
    {
        var selectedStateCode = e.Value.ToString();

        try
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Add("X-CSCAPI-KEY", ckey);
            var response = await httpclient.GetStringAsync($"{cUrl}/{selectedCountryCode}/states/{selectedStateCode}/cities");
            var cities = JsonSerializer.Deserialize<List<City>>(response);

            CityOptions = cities.Select(city => new SelectListItem
            {
                Value = city.Iso2,
                Text = city.Name
            }).ToList();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading cities: {ex.Message}");
        }
    }

    public async Task LoadCountries()
    {
        try
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Add("X-CSCAPI-KEY", ckey);
            var response = await httpclient.GetStringAsync(cUrl);
            var countries = JsonSerializer.Deserialize<List<Country>>(response);

            CountryOptions = countries.Select(country => new SelectListItem
            {
                Value = country.Iso2,
                Text = country.Name
            }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading countries: {ex.Message}");
        }
        
    }

    
}
