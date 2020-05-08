using System.Collections.Generic;

namespace WebsiteManagement.Application.Common
{
    public class OperationResult<TResult>
    {
        private OperationResult(bool isSuccessful, TResult result)
        {
            IsSuccessful = isSuccessful;
            Result = result;
        }

        private OperationResult(bool isSuccessful, Dictionary<string, string> errors)
        {
            IsSuccessful = isSuccessful;
            Errors = errors;
        }

        public bool IsSuccessful { get; }

        public Dictionary<string, string> Errors { get; }

        public TResult Result { get; }

        public static OperationResult<TResult> Success(TResult result)
        {
            return new OperationResult<TResult>(true, result);
        }

        public static OperationResult<TResult> Failure(Dictionary<string, string> errors)
        {
            return new OperationResult<TResult>(false, errors);
        }
    }
}
