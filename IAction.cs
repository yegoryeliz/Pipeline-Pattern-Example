namespace Pipeline_Example;

// Marker interface used for grouping only.
public interface IAction
{
}

// The below interfaces can realistically be run in any order, but why would you? Its a pipeline after all.

// First bookend interface takes no input.
// Intended for cases where it needs to generate its own data on kickoff.
public interface ISource<TOut> : IAction
{
    Task<TOut> ExecuteAsync();
}

// Most common interface. Can also serve as a start or end action.
// Takes an input and provides an output.
public interface IStep<in TIn, TOut> : IAction
{
    Task<TOut> ExecuteAsync(TIn input);
}

// Last Bookend interface provides no output.
// Intended for cases where the last action in the chain will perform a side effect such as writing to console.
public interface ISink<in TIn> : IAction
{
    Task ExecuteAsync(TIn input);
}