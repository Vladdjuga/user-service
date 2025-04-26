using Application.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public class ExceptionHandlingBehavior<TRequest,TResponse>:IPipelineBehavior<TRequest,TResponse> 
    where TRequest : notnull, IRequest<TResponse>
    where TResponse:Result
{
    private readonly ILogger _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Exception occured during request handling");
            _logger.LogInformation("Request: {@Request}", request);
            _logger.LogError(ex, ex.Message);
            return (TResponse)Result.Failure(ex.Message);
        }
    }
}