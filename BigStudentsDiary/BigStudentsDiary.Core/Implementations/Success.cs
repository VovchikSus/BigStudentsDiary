using BigStudentsDiary.Core.Interfaces;

namespace BigStudentsDiary.Core.Implementations;

// public class Success<T> : BaseResult<T>
// {
//     public Success(T element) :
//         base(element, true, string.Empty)
//     {
//     }
// }

// public class Success : BaseResult
// {
//     public Success() :
//         base(true, string.Empty)
//     {
//     }
// }
public class Success<T> : IOperationResult<T>
{
    public bool Successful => true;
    public string ErrorMessage => null;
    public T Result { get; }

    public Success(T result) => Result = result;
}
public class Success : IOperationResult
{
    public bool Successful => true;
    public string ErrorMessage => null;
}