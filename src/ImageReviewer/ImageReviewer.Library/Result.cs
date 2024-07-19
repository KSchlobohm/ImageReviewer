using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageReviewer
{
    public class Result<T> where T : class
    {
        public T? Value { get; }
        public bool IsSuccess { get; }
        public string Error { get; }

        private Result(T value)
        {
            Value = value;
            IsSuccess = true;
            Error = string.Empty;
        }

        private Result(string error)
        {
            Value = null;
            IsSuccess = false;
            Error = error;
        }

        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(string error) => new Result<T>(error);
    }
}
