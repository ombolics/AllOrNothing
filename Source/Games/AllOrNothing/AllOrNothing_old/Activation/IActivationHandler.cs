using System.Threading.Tasks;

namespace AllOrNothing.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle(object args);

        Task HandleAsync(object args);
    }
}
