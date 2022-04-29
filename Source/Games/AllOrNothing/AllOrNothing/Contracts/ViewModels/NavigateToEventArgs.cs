using System;

namespace AllOrNothing.Contracts.ViewModels
{
    public class NavigateToEventArgs
    {
        public Type PageVM { get; set; }
        public string PageName { get; set; }
    }
}