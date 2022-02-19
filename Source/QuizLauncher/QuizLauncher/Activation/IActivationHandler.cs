using System.Threading.Tasks;

namespace QuizLauncher.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle(object args);

        Task HandleAsync(object args);
    }
}
