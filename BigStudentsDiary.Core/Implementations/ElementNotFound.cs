namespace BigStudentsDiary.Core.Implementations;

public class ElementNotFound<T> : BaseResult<T> where T : class
{
    public ElementNotFound(T element, string errorMessage)
        : base(element, false, errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
            this.ErrorMessage = $"Элемент типа {typeof(T)} не найден";
    }
}

public class ElementNotFound : BaseResult
{
    public ElementNotFound(string errorMessage)
        : base(false, errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
            this.ErrorMessage = $"Элемент не найден";
    }
}