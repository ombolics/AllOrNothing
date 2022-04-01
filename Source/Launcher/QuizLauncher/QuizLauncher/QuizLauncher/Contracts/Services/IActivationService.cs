using System.Threading.Tasks;

namespace QuizLauncher.Contracts.Services
{
    public interface IActivationService
    {
        Task ActivateAsync(object activationArgs);
    }
}
