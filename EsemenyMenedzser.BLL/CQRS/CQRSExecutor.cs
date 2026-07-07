using EsemenyMenedzser.BLL.CQRS.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EsemenyMenedzser.BLL.CQRS
{
    public class CQRSExecutor : ICQRSExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public CQRSExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            // Find the appropriate Query Handler from DI based on the request type
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetRequiredService(handlerType);

            // Dynamically invoke the HandleAsync method
            var method = handlerType.GetMethod("HandleAsync");
            return await (Task<TResult>)method!.Invoke(handler, [query])!;
        }

        public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
        {
            // 1. Check if there is a registered validator for this Command.
            var validatorType = typeof(IValidator<>).MakeGenericType(command.GetType());
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator != null)
            {
                // 2. Execute the validation.
                var context = new ValidationContext<object>(command);
                var validationResult = await validator.ValidateAsync(context);

                // 3. Throwing an exception with the error messages if validation fails.
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            // Find the appropriate Command Handler from DI based on the request type
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetRequiredService(handlerType);

            var method = handlerType.GetMethod("HandleAsync");
            return await (Task<TResult>)method!.Invoke(handler, [command])!;
        }
    }
}
