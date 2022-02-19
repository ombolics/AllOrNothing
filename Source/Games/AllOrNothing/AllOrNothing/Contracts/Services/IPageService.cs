using System;

namespace AllOrNothing.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);
        bool IsPageKey(string key);
    }
}
