using Microsoft.JSInterop;
using Plot.Data.Models.Env;

namespace Plot.Services;

public interface ICookie
{
    public Task SetValue(string key, string value, int? minuets = null);
    public Task<string> GetValue(string key, string def = "");
}

public class Cookie : ICookie
{
    private const int _DEFAULT_EXPIRATION_TIME = 30;
    readonly IJSRuntime JSRuntime;
    string expires = "";
    

    public Cookie(IJSRuntime jsRuntime,EnvironmentSettings envSettings)
    {
        JSRuntime = jsRuntime;
        
        if (int.TryParse(envSettings.auth_expiration_time, out int auth_expiration_time))
        {
            ExpireMinutes = auth_expiration_time;
        }
        else
        {
            ExpireMinutes = _DEFAULT_EXPIRATION_TIME;
        }
    }

    public async Task SetValue(string key, string value, int? minutes = null)
    {
        var curExp = (minutes != null) ? (minutes > 0 ? DateToUTC(minutes.Value) : "") : expires;
        await SetCookie($"{key}={value}; expires={curExp}; path=/");
    }

    public async Task<string> GetValue(string key, string def = "")
    {
        var cValue = await GetCookie();
        if (string.IsNullOrEmpty(cValue)) return def;

        var vals = cValue.Split(';');
        foreach (var val in vals)
            if (!string.IsNullOrEmpty(val) && val.IndexOf('=') > 0)
                if (val.Substring(0, val.IndexOf('=')).Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                    return val.Substring(val.IndexOf('=') + 1);
        return def;
    }

    private async Task SetCookie(string value)
    {
        await JSRuntime.InvokeVoidAsync("eval", $"document.cookie = \"{value}\"");
    }

    private async Task<string> GetCookie()
    {
        return await JSRuntime.InvokeAsync<string>("eval", $"document.cookie");
    }

    public int ExpireMinutes
    {
        set => expires = DateToUTC(value);
    }

    private static string DateToUTC(int minuets) => DateTime.Now.AddMinutes(minuets).ToUniversalTime().ToString("R");
}
