using System.Threading.Tasks;

namespace AllOrNothing.Activation
{
    // For more information on understanding activation flow see
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/WinUI/activation.md
    //
    // Extend this class to implement new ActivationHandlers
    public abstract class ActivationHandler<T> : IActivationHandler
        where T : class
    {
        // Override this method to add the activation logic in your activation handler
        protected abstract Task HandleInternalAsync(T args);

        public async Task HandleAsync(object args)
        {
            await HandleInternalAsync(args as T);
        }

        public bool CanHandle(object args)
        {
            // checks if args has the requiered type
            return args is T && CanHandleInternal(args as T);
        }

        /// <summary>
        /// Potential override  method to add extra validation on activation args to determine if ActivationHandler should handle this activation args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual bool CanHandleInternal(T args)
        {
            return true;
        }
    }
}
