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
	/// <param name="config">Provided application settings configuration</param>
	/// <typeparam name="TError">Type of error to be handled</typeparam>
	/// <returns>Limited Count retry policy</returns>
	[PublicAPI]
	public static Policy CountLimitedRetryPolicy<TError>(
			IConfiguration? config = null
		)
	where TError : Exception
	{
		// load the correct retry policy options
		RetryPolicyOptions options = GetRetryPolicyOptions(config);

		if (options.Jitter)
		{
			return Policy
				.Handle<TError>()
				.WaitAndRetry(
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
				.WaitAndRetry(
					Backoff.ExponentialBackoff(
						options.InitialDelay,
						options.MaxRetries
					)
				);
		}

		// default case should always be the linear backoff, but will be overriden by the configuration
		return Policy
			.Handle<TError>()
			.WaitAndRetry(
				Backoff.LinearBackoff(
					options.InitialDelay,
					options.MaxRetries
				)
			);
	}

    /// <summary>
    /// Adds a count limited retry policy
    ///     NOTE: Defaulted to Exponential backoff
    /// </summary>
    /// <param name="config">Provided application settings configuration</param>
    /// <typeparam name="TError">Type of error to be handled</typeparam>
    /// <returns>Limited Count retry policy</returns>
    [PublicAPI]
    public static IAsyncPolicy CountLimitedRetryPolicyAsync<TError>(
            IConfiguration? config = null
        )
    where TError : Exception
    {
        // load the correct retry policy options
        RetryPolicyOptions options = GetRetryPolicyOptions(config);

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
    [PublicAPI]
    public static Policy ForeverRetryPolicy<TError>(
	    IConfiguration? config = null
    )
	    where TError: Exception
    {
	    // load the correct retry policy options
	    RetryPolicyOptions options = GetRetryPolicyOptions(config);

	    return Policy
		    .Handle<TError>()
		    .WaitAndRetryForever(_ =>
			    options.ForeverSleepDuration
		    );
    }

    /// <summary>
    /// Adds a forever retry policy
    ///     NOTE: Defaulted to Exponential backoff
    /// </summary>
    /// <param name="config">Provided application settings configuration</param>
    /// <typeparam name="TError">Type of error to be handled</typeparam>
    /// <returns>forever retry policy</returns>
    [PublicAPI]
    public static IAsyncPolicy ForeverRetryPolicyAsync<TError>(
        IConfiguration? config = null
        )
    where TError: Exception
    {
        // load the correct retry policy options
        RetryPolicyOptions options = GetRetryPolicyOptions(config);

        return Policy
            .Handle<TError>()
            .WaitAndRetryForeverAsync(_ =>
                    options.ForeverSleepDuration
                );
    }

    private static RetryPolicyOptions GetRetryPolicyOptions(IConfiguration? config = null)
    {
	    return config is null
		    ? new RetryPolicyOptions()
		    : config!
			      .GetSection(RetryPolicyOptions.RetryPolicyOptionsKey)
			      .Get<RetryPolicyOptions>()
		      ?? new RetryPolicyOptions();
    }
}
