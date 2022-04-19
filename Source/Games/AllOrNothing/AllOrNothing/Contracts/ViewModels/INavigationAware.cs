namespace AllOrNothing.Contracts.ViewModels
{
    public interface INavigationAware
    {
        void OnNavigatedTo();

        void OnNavigatedFrom();
        bool IsReachable();
    }
}
