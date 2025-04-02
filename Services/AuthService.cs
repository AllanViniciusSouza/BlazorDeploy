using Microsoft.JSInterop;
using System.Threading.Tasks;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;

    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> IsAuthenticated()
    {
        var token = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "accesstoken");
        return !string.IsNullOrEmpty(token);
    }

    public async Task Logout()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "accesstoken");
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "usuarioid");
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "usuarionome");
    }
}
