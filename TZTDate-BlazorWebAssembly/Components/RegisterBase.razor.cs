using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TZTDateBlazorWebAssembly.Dtos;
using TZTDateBlazorWebAssembly.Models;
using TZTDateBlazorWebAssembly.Services.Base;

namespace TZTDateBlazorWebAssembly.Components;

public partial class RegisterBase : ComponentBase
{
    public List<SelectListItem> CountryOptions { get; set; } = new();
    public List<City> CityOptions { get; set; } = new();
    public List<SelectListItem> StateOptions { get; set; } = new();
    public UserRegisterDto UserRegisterDto { get; set; } = new();
    public int CurrentStep { get; set; } = 1;
    public List<string> StepLabels { get; set; } = new List<string> { "User Details", "Location", "Preferences", "Photos" };
    private string SelectedCountryCode { get; set; }
    public List<string> interests = new List<string>
        {
            "Sport", "Games", "Traveling", "Reading", "Cooking", "Hiking", "Photography", "Music",
            "Painting", "Writing",
            "Gardening", "Yoga", "Meditation", "Astronomy", "Dancing", "Film-making", "Bird-watching",
            "Knitting", "Surfing",
            "Scuba diving", "Chess", "Calligraphy"
        };
    public List<CheckboxListItem> InterestsList { get; set; }
    private StringBuilder InterestsStringBuilder { get; set; } = new(100);

    [Inject]
    private ICountryApiService CountryApiService { get; set; }

    [Inject]
    private IWebApiService WebApiService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }


    override protected async Task OnInitializedAsync()
    {
        InterestsList = interests.Select(interest => new CheckboxListItem
        {
            Label = interest
        }).ToList();

        await LoadCountries();
    }

    public async Task OnRegisterFormSubmitAsync(EditContext editContext)
    {
        await HandleValidFormSubmission();
    }

    public async Task HandleValidFormSubmission()
    {
        MapCountryCityState();
        await RegisterUserAsync();
        NavigationManager.NavigateTo("/Auth/Login");
    }

    private void MapCountryCityState()
    {
        UserRegisterDto.Country = CountryOptions.FirstOrDefault(x => x.Value == UserRegisterDto.Country)?.Text;
        UserRegisterDto.State = StateOptions.FirstOrDefault(x => x.Value == UserRegisterDto.State)?.Text;
    }

    private async Task RegisterUserAsync()
    {
        await WebApiService.Register(UserRegisterDto);
    }

    public void UpdateInterests(string interest, bool isChecked)
    {
        InterestsList.FirstOrDefault(o => o.Label == interest).IsChecked = isChecked;

        if (isChecked)
        {
            InterestsStringBuilder.Append(interest + " ");
            UserRegisterDto.Interests = InterestsStringBuilder.ToString();
        }
        else
        {
            InterestsStringBuilder.Remove(InterestsStringBuilder.ToString().IndexOf(interest + " "), interest.Length + 1);
            UserRegisterDto.Interests = InterestsStringBuilder.ToString();
        }
    }

    public void NavigateToFormStep(int stepNumber)
    {
        CurrentStep = stepNumber;
    }

    public async Task LoadStatesAsync(ChangeEventArgs e)
    {
        SelectedCountryCode = e.Value.ToString();
        StateOptions = await CountryApiService.GetStates(SelectedCountryCode);
    }

    public async Task LoadCitiesAsync(ChangeEventArgs e)
    {
        var selectedStateCode = e.Value.ToString();
        CityOptions = await CountryApiService.GetCities(SelectedCountryCode, selectedStateCode);
    }

    public async Task LoadCountries()
    {
        CountryOptions = await CountryApiService.GetCountries();
    }

    public async Task UploadFile(InputFileChangeEventArgs e, string imageName)
    {
        try
        {
            var file = e.File;
            var maxSize = 10000 * 1024;
            using var memoryStream = new MemoryStream();
            await file.OpenReadStream(maxSize).CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            var fileName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.Name);
            var targetFilePath = Path.ChangeExtension(fileName, extension);

            UserRegisterDto.GetType().GetProperty(imageName).SetValue(UserRegisterDto, fileBytes);
            UserRegisterDto.GetType().GetProperty(imageName + "Name").SetValue(UserRegisterDto, targetFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
