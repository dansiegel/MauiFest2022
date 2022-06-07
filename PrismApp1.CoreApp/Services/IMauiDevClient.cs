using PrismApp1.CoreApp.Models;
using Refit;

namespace PrismApp1.CoreApp.Services;

public interface IMauiDevClient
{
    [Get("/media/maui-devs.json")]
    Task<ApiResponse<IEnumerable<MauiInfluencer>>> GetInfluencers();
}
