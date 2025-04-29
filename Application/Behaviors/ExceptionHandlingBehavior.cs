using Application.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public class ExceptionHandlingBehavior<TRequest,TResponse>:IPipelineBehavior<TRequest,TResponse> 
    where TRequest : notnull, IRequest<TResponse>
    where TResponse:IResult
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
            _logger.LogError(ex, "Exception occured during request handling");
            _logger.LogError("Request: {@Request}", request);
            _logger.LogError(ex, ex.Message);
            var responseType = typeof(TResponse);
            if (responseType == typeof(Result))
                return (TResponse)Result.Failure(ex.Message);
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var failureMethod = responseType.GetMethod("Failure", [typeof(string)]);
                if (failureMethod is not null)
                {
                    var failureResult = failureMethod.Invoke(null, [ex.Message]);
                    return (TResponse)failureResult!;
                }
            }
            throw;
        }
    }
}