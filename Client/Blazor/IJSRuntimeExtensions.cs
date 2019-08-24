namespace todoweb.Client.Blazor
{
    using Microsoft.JSInterop;

    public static class IJSRuntimeExtensions
    {
        public static async void ShowAlert(this IJSRuntime jsRuntime, string message)
        {
            // ShowAlert is registered in index.html
            await jsRuntime.InvokeAsync<object>("ShowAlert", message);
        }
    }
}
