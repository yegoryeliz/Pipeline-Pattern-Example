namespace Pipeline_Example;

public class PipelineBuilder
{
    private readonly List<Func<object, Task<object>>> _executionList = [];
    private Type? _lastType = null;

    public PipelineBuilder AddSource<TOut>(ISource<TOut> source)
    {
        _lastType = typeof(TOut);

        _executionList.Add(async (previousOutput) =>
        {
            TOut? result = await source.ExecuteAsync();

            if (result == null)
            {
                throw new InvalidOperationException($"Source {source.GetType().Name} returned null.");
            }
            return result;
        });

        return this;
    }

    public PipelineBuilder AddStep<TIn, TOut>(IStep<TIn, TOut> step)
    {
        if (_lastType != null && _lastType != typeof(TIn))
        {
            throw new InvalidOperationException
                (
                    $"Type mismatch: Expected input type {typeof(TIn).Name}, but previous step output type was {_lastType.Name}."
                );
        }

        _lastType = typeof(TOut);

        _executionList.Add(async (previousOutput) =>
        {
            var typedInput = (TIn)previousOutput;
            TOut? result = await step.ExecuteAsync(typedInput);
            if (result == null)
            {
                throw new InvalidOperationException($"Step {step.GetType().Name} returned null.");
            }
            return result;
        });

        return this;
    }

    public PipelineBuilder AddSink<TIn>(ISink<TIn> sink)
    {
        _lastType = typeof(void);

        _executionList.Add(async (previousOutput) =>
        {
            var typedInput = (TIn)previousOutput;
            await sink.ExecuteAsync(typedInput);

            // This method does not return a value, explicitly returning null! to satisfy the delegate signature.
            return null!;
        });

        return this;
    }

    public async Task RunAsync()
    {
        object? currentState = null;

        foreach (Func<object, Task<object>> actionDelegate in _executionList)
        {

            // Source does not take an input, explicitly passing null! for the first execution.
            currentState = await actionDelegate(currentState!);
        }
    }
}