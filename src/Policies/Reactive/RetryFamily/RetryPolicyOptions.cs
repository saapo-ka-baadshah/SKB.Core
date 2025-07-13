using JetBrains.Annotations;

namespace SKB.Core.Policies.Reactive.RetryFamily;

/// <summary>
/// Options for the retry policy
/// </summary>
public class RetryPolicyOptions
{
    /// <summary>
    /// Retry policy options key name
    /// </summary>
    public const string RetryPolicyOptionsKey = "RetryPolicyOptions";
    
    /// <summary>
    /// Fixed delay if not set 
    /// </summary>
    [PublicAPI]
    public TimeSpan InitialDelay { get; set; } = TimeSpan.FromMilliseconds(100);
    
    /// <summary>
    /// Fixed count for retries in the policies
    /// </summary>
    [PublicAPI]
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Adds jitter by default to all retry policies
    /// </summary>
    [PublicAPI]
    public bool Jitter { get; set; } 
    
    /// <summary>
    /// By default, prevents the Exponential Backoff,
    ///     NOTE: This is to prevent network based retries flooding the physical network 
    /// </summary>
    [PublicAPI]
    public bool BackoffExponential { get; set; } = true;
    
    /// <summary>
    /// Fixed count for decorated median jitter
    /// </summary>
    [PublicAPI]
    public TimeSpan DecoratedJitterMedian { get; set; } = TimeSpan.FromSeconds(1);
    
    /// <summary>
    /// Fixed forever sleep duration
    /// </summary>
    [PublicAPI]
    public TimeSpan ForeverSleepDuration { get; set; } = TimeSpan.FromSeconds(30);
}