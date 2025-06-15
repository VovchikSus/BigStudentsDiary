using BigStudentsDiary.Core.Interfaces;

namespace BigStudentsDiary.Core.Implementations;

public class Failure<T> : IOperationResult<T>
{
    public bool Successful => false;
    public string ErrorMessage { get; }
    public T Result => default;

    public Failure(string error) => ErrorMessage = error;
}

public class Failure : IOperationResult
{
    public bool Successful => false;
    public string ErrorMessage { get; }

    public Failure(string error) => ErrorMessage = error;
}