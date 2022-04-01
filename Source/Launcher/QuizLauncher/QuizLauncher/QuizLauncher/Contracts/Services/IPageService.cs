using System;

namespace QuizLauncher.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);
    }
}
