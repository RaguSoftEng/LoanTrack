namespace LoanTrack.Web.Shared.Common;

public class AppSettingState
{
    private string _currentPageName = "Home";

    public string CurrentPageName {
        get => _currentPageName; set
        {
            _currentPageName = value;
            OnSettingsChanged?.Invoke(_currentPageName);
        }
    }
    public event Action<string> OnSettingsChanged; 
}
