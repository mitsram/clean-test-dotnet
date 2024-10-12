using System.Threading.Tasks;

namespace JupiterPrime.Application.Interfaces;

public interface IWebElementAdapter
{
    Task ClickAsync();
    Task SendKeysAsync(string text);
    Task<string> GetTextAsync();
    Task<bool> IsDisplayedAsync();
    Task<bool> IsEnabledAsync();
}

