namespace Pipeline_Example;

// These actions demonstrate the ability to take a varied set of Input and Output Types
// While you can use a single type throughout the whole pipeline you are not restricted to a single in/out Type
// as long as you have subsequent actions that can match previous output to their input.

public class DataGenerator : ISource<int>
{
    public Task<int> ExecuteAsync()
    {
        int variable = 10;
        Type type = variable.GetType();
        Console.WriteLine($"[Source Subroutine] Initializing raw integer data {type}.");
        return Task.FromResult(variable);
    }
}

public class IntoToStringConverter : IStep<int, string>
{
    public Task<string> ExecuteAsync(int input)
    {
        Type type = input.GetType();
        Console.WriteLine($"[Step Subroutine] Casting input: {input} of type {type} to string");
        return Task.FromResult(input.ToString());
    }
}

public class AddTen : IStep<int, int>
{
    public Task<int> ExecuteAsync(int input)
    {
        Type type = input.GetType();
        Console.WriteLine($"[Step Subroutine] Adding '10' to input: {input} of type {type}");
        return Task.FromResult(input + 10);
    }
}

public class TerminalOutput : ISink<string>
{
    public Task ExecuteAsync(string input)
    {
        Type type = input.GetType();
        Console.WriteLine($"[Sink Subroutine] Terminating pipeline. Final payload: {input} of type {type}");
        return Task.CompletedTask;
    }
}