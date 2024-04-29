using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TZTDate_BlazorWebAssembly.Dtos;
using TZTDate_BlazorWebAssembly.Models;
using TZTDateBlazorWebAssembly.Services.Base;
using TZTDateBlazorWebAssembly.Models.Zodiac.Entities;
using Microsoft.AspNetCore.SignalR.Client;


namespace TZTDateBlazorWebAssembly.Components;

public class DetailsBase : ComponentBase
{
    public IJSRuntime JSRuntime { get; set; }
    public UserDetailsDto UserDetails { get; set; }
    public RelationshipData ZodiacCompatibility { get; set; }

    [Parameter] public int ViewedUserId { get; set; }
    [Parameter] public User CurrentUser { get; set; }
    [Parameter] public User ViewedUser { get; set; }
    [Parameter] public List<string> ProfilePicPaths { get; set; }

    protected bool LikeButtonState;
    protected bool ReplicateMembership;

    [Inject]
    private IWebApiService webApiService { get; set; }
    [Inject]
    private NavigationManager Navigation { get; set; }

    protected override async Task OnInitializedAsync()
    {


        this.UserDetails = await GetDetails();
        this.CurrentUser = this.UserDetails.CurrentUser;
        this.ViewedUser = this.UserDetails.ViewedUser;
        this.ProfilePicPaths = this.UserDetails.ImageUris ?? new List<string>();
        this.ZodiacCompatibility = await CurrentUser.CompatibilityByZodiacSign(ViewedUser);
        if (CurrentUser.Followed is not null) LikeButtonState = CurrentUser.Followed.Any(user => user.Id == ViewedUser.Id);
        ReplicateMembership = ViewedUser.Followed != null && CurrentUser.Followed != null
                      && CurrentUser.Followers.Any(user => user.Id == ViewedUser.Id)
                      && CurrentUser.Followed.Any(user => user.Id == ViewedUser.Id);
        System.Console.WriteLine(ReplicateMembership);
    }

    private async Task<UserDetailsDto> GetDetails()
    {
        var currentUserIdString = await webApiService.GetUserIdFromJwt();
        var a = int.TryParse(currentUserIdString, out var currentUserId);
        return await webApiService.GetDetails(currentUserId, this.ViewedUserId);
    }


    protected async Task LikeButtonClick()
    {
        LikeButtonState = !LikeButtonState;
        if (LikeButtonState)
        {
            await webApiService.SetMembership(CurrentUser.Id, ViewedUser.Id);

        }
        else
        {
            await webApiService.SetMembership(CurrentUser.Id, ViewedUser.Id);

        }

        ReplicateMembership = ViewedUser.Followed != null && CurrentUser.Followed != null
                          && CurrentUser.Followers.Any(user => user.Id == ViewedUser.Id)
                          && CurrentUser.Followed.Any(user => user.Id == ViewedUser.Id);

        StateHasChanged();
    }

    protected async Task ChatButtonClick()
    {
        var url = $"/PrivateChat/{ViewedUser.Id}/{CurrentUser.Id}";
        Navigation.NavigateTo(url);
    }
}