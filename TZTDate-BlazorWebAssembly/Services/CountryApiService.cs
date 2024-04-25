using System.Text.Json;
using TZTDateBlazorWebAssembly.Models;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Services;

public class CountryApiService : ICountryApiService
{
    private readonly HttpClient httpClient;

    public CountryApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<City>> GetCities(string countryCode, string stateCode)
    {
        try
        {
            var response = await httpClient.GetStringAsync($"countries/{countryCode}/states/{stateCode}/cities");
            var cities = JsonSerializer.Deserialize<List<City>>(response);

            return cities;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<City>();
        }
    }

    public async Task<List<SelectListItem>> GetCountries()
    {
        try
        {
            var response = await httpClient.GetStringAsync("countries");
            var countries = JsonSerializer.Deserialize<List<CountryAndState>>(response);

            return countries.Select(country => new SelectListItem
            {
                Value = country.Iso2,
                Text = country.Name
            }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<SelectListItem>();
        }
    }

    public async Task<List<SelectListItem>> GetStates(string countryCode)
    {
        try
        {
            var response = await httpClient.GetStringAsync($"countries/{countryCode}/states");
            var states = JsonSerializer.Deserialize<List<CountryAndState>>(response);

            return states.Select(state => new SelectListItem
            {
                Value = state.Iso2,
                Text = state.Name
            }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<SelectListItem>();
        }
    }
}

