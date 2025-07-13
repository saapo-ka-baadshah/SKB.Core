# SKB.Core.Policies

This provides the central repository for the project based core 
policies. These policies are leveraged by `Polly`.

For detailed specifications study [Polly](https://github.com/App-vNext/Polly).

The project structure should follow the abstraction structure of `Polly`.

## Resilience strategies

Polly provides a variety of resilience strategies. Alongside the comprehensive guides for each strategy, the wiki also includes an [overview of the role each strategy plays in resilience engineering](https://github.com/App-vNext/Polly/wiki/Transient-fault-handling-and-proactive-resilience-engineering).

Polly categorizes resilience strategies into two main groups:

### Reactive

These strategies handle specific exceptions that are thrown, or results that are returned, by the callbacks executed through the strategy.

| Strategy | Premise | AKA | Mitigation |
| ------------- | ------------- | -------------- | ------------ |
| [**Retry** family](#retry) | Many faults are transient and may self-correct after a short delay. | *Maybe it's just a blip* | Allows configuring automatic retries. |
| [**Circuit-breaker** family](#circuit-breaker) | When a system is seriously struggling, failing fast is better than making users/callers wait. <br/><br/>Protecting a faulting system from overload can help it recover. | *Stop doing it if it hurts* <br/><br/>*Give that system a break* | Breaks the circuit (blocks executions) for a period, when faults exceed some pre-configured threshold. |
| [**Fallback**](#fallback) | Things will still fail - plan what you will do when that happens. | *Degrade gracefully* | Defines an alternative value to be returned (or action to be executed) on failure. |
| [**Hedging**](#hedging) | Things can be slow sometimes, plan what you will do when that happens. | *Hedge your bets* | Executes parallel actions when things are slow and waits for the fastest one. |

### Proactive

Unlike reactive strategies, proactive strategies do not focus on handling errors, but the callbacks might throw or return.
They can make proactive decisions to cancel or reject the execution of callbacks.

| Strategy | Premise | AKA | Prevention |
| ----------- | ------------- | -------------- | ------------ |
| [**Timeout**](#timeout) | Beyond a certain wait, a success result is unlikely. | *Don't wait forever* | Guarantees the caller won't have to wait beyond the timeout. |
| [**Rate Limiter**](#rate-limiter) | Limiting the rate a system handles requests is another way to control load. <br/> <br/> This can apply to the way your system accepts incoming calls, and/or to the way you call downstream services. | *Slow down a bit, will you?* | Constrains executions to not exceed a certain rate. |

Visit [resilience strategies](https://www.pollydocs.org/strategies) docs to explore how to configure individual resilience strategies in more detail.
