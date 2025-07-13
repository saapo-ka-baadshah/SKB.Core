using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace SKB.Core.Policies.Reactive.RetryFamily;

/// <summary>
/// Specifies different retry policies
/// </summary>
[PublicAPI]
public static class RetryPolicy
{
    /// <summary>
    /// Adds a count limited retry policy
    ///     NOTE: Defaulted to Exponential backoff
    /// </summary>
    /// <returns>Limited Count retry policy</returns>
    [PublicAPI]
    public static IAsyncPolicy CountLimitedRetryPolicyAsync<TError>(
            IConfiguration? config
        )
    where TError : Exception
    {
        // load the correct retry policy options
        RetryPolicyOptions options = config!
            .GetSection(RetryPolicyOptions.RetryPolicyOptionsKey)
            .Get<RetryPolicyOptions>() 
            ?? new RetryPolicyOptions();

        if (options.Jitter)
        {
            return Policy
                .Handle<TError>()
                .WaitAndRetryAsync(
                        Backoff.DecorrelatedJitterBackoffV2(
                                options.DecoratedJitterMedian,
                                options.MaxRetries
                            )
                    );
        }

        if (options.BackoffExponential)
        {
            // Defualt Retry to prevent the physical network flooding with exponential backoff policy
            return Policy
                .Handle<TError>()
                .WaitAndRetryAsync(
                    Backoff.ExponentialBackoff(
                        options.InitialDelay,
                        options.MaxRetries
                    )
                );
        }
        
        // default case should always be the linear backoff, but will be overriden by the configuration
        return Policy
            .Handle<TError>()
            .WaitAndRetryAsync(
                    Backoff.LinearBackoff(
                            options.InitialDelay,
                            options.MaxRetries
                        )
                );
    }

    /// <summary>
    /// Adds a forever retry policy
    ///     NOTE: Defaulted to Exponential backoff
    /// </summary>
    /// <param name="config">Provided application settings configuration</param>
    /// <typeparam name="TError">Type of error to be handled</typeparam>
    /// <returns>forever retry policy</returns>
    public static IAsyncPolicy ForeverRetryPolicyAsync<TError>(
        IConfiguration? config
        )
    where TError: Exception
    {
        // load the correct retry policy options
        RetryPolicyOptions options = config!
                                         .GetSection(RetryPolicyOptions.RetryPolicyOptionsKey)
                                         .Get<RetryPolicyOptions>() 
                                     ?? new RetryPolicyOptions();
        
        return Policy
            .Handle<TError>()
            .WaitAndRetryForeverAsync(_ => 
                    options.ForeverSleepDuration
                );
    }
    
}