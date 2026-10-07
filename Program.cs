using Pipeline_Example;
using System.Diagnostics.CodeAnalysis;

[RequiresUnreferencedCode("This pipeline uses dynamic runtime binding and may not be compatible with trimming/AOT.")]
[RequiresDynamicCode("This pipeline uses dynamic dispatch and may require runtime code generation.")]

public class Application
{
    public static async Task Main()
    {
        // Queue demonstrates a dynamic pipeline creation event.
        var queue = new Queue<object>();
        var pipeline = new PipelineBuilder();

        // See Actions.cs for actions
        queue.Enqueue(new DataGenerator());
        queue.Enqueue(new AddTen());
        queue.Enqueue(new IntoToStringConverter());
        queue.Enqueue(new TerminalOutput());


        // This layout demonstrates dynamic pipeline composition using DLR (Dynamic Language Runtime) code generation for runtime type inference.
        // This is still orders of magnitude faster than using reflection to invoke methods.
        // The Source, Step, and Sink interfaces can realistically be assembled in any order as long and input and output types are satisfied.
        object firstItem = queue.Peek();
        pipeline.AddSource((dynamic)firstItem);
        _ = queue.Dequeue();

        while (queue.Count > 1)
        {
            object currentItem = queue.Peek();
            pipeline.AddStep((dynamic)currentItem);
            _ = queue.Dequeue();
        }

        object lastItem = queue.Peek();
        pipeline.AddSink((dynamic)lastItem);


        // Wait until all the steps have been collected before running.
        // Realistically there would be some kind of error checking and/or validation before calling the RunAsync() Method.
        // This way the pipeline can be dynamically composed and called atomically after assembly/validation.
        await pipeline.RunAsync();
    }
}