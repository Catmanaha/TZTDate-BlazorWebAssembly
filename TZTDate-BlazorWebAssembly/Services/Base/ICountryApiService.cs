using TZTDateBlazorWebAssembly.Models;

namespace TZTDateBlazorWebAssembly.Services.Base;

public interface ICountryApiService
{
    public Task<List<SelectListItem>> GetCountries();
    public Task<List<City>> GetCities(string countryCode, string stateCode);
    public Task<List<SelectListItem>> GetStates(string countryCode);
}
