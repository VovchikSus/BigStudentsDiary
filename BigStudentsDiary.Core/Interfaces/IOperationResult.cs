namespace BigStudentsDiary.Core.Interfaces;

public interface IOperationResult
{
    /// <summary>
    /// Является ли результат успешным
    /// </summary>
    public bool Successful { get; }

    /// <summary>
    /// Сообщение об ошибке, если результат неуспешный. Иначе оставить пустым
    /// </summary>
    public string ErrorMessage { get; }
}

public interface IOperationResult<T> : IOperationResult
{
    /// <summary>
    /// Результат выполнения операции, если она подразумевает таковой
    /// </summary>
    public T Result { get; }
}