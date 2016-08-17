namespace NzzApp.Providers.Helpers
{
    public enum ErrorTypes
    {
        NoInternet,
        Unspecific
    }

    public class TaskResult
    {
        protected TaskResult(bool success)
        {
            Success = success;
        }

        protected TaskResult(bool success, ErrorTypes errorType)
        {
            Success = success;
            ErrorType = errorType;
        }

        public static TaskResult CreateSuccess()
        {
            return new TaskResult(true);
        }

        public static TaskResult CreateFail(ErrorTypes errorType)
        {
            return new TaskResult(false, errorType);
        }

        public bool Success { get; protected set; }
        public ErrorTypes ErrorType { get; protected set; }
    }

    public class TaskResult<T> : TaskResult
    {
        protected TaskResult(bool success, T value):
            base(success)
        {
            Value = value;
        }
        protected TaskResult(bool success, ErrorTypes errorType, T value):
            base(success, errorType)
        {
            Value = value;
        }

        public static TaskResult<T> CreateSuccess(T value)
        {
            return new TaskResult<T>(true, value);
        }

        public static TaskResult<T> CreateFail(ErrorTypes errorType, T value)
        {
            return new TaskResult<T>(false, errorType, value);
        }

        public T Value { get; private set; }
    }
}
