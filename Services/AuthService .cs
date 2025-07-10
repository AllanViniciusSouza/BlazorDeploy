using Microsoft.JSInterop;

namespace BlazorDeploy.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _js;

        public AuthService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _js.InvokeAsync<string>("sessionStorage.getItem", "accesstoken");
            return !string.IsNullOrEmpty(token);
        }

        public async Task<string> GetUsuarioNomeAsync()
        {
            return await _js.InvokeAsync<string>("sessionStorage.getItem", "usuarionome");
        }

        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("sessionStorage.clear");
        }
    }
}
