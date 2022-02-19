using System.Threading.Tasks;

namespace AllOrNothing.Contracts.Services
{
    public interface IActivationService
    {
        Task ActivateAsync(object activationArgs);
    }
}
