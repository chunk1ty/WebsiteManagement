namespace WebsiteManagement.Application.Common
{
    public class OperationResult<TResult>
    {
        private OperationResult(bool isSuccessful, TResult result)
        {
            IsSuccessful = isSuccessful;
            Result = result;
        }

        private OperationResult(bool isSuccessful, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccessful { get; }

        public string ErrorMessage { get; }

        public TResult Result { get; }

        public static OperationResult<TResult> Success(TResult result)
        {
            return new OperationResult<TResult>(true, result);
        }

        public static OperationResult<TResult> Failure(string errorMessage)
        {
            return new OperationResult<TResult>(false, errorMessage);
        }
    }
}
