using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace AllOrNothing.Contracts.Services
{
    public interface IThemeSelectorService
    {
        UITheme Theme { get; }

        Task InitializeAsync();

        Task SetThemeAsync(UITheme theme);

        Task SetRequestedThemeAsync();
    }
}
