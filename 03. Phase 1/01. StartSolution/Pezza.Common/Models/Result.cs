namespace Pezza.Common.Models
{
    using System.Collections.Generic;

    public class Result
    {
        internal Result(bool succeeded, string error)
        {
            this.Succeeded = succeeded;

            this.Errors = new List<string>
            {
                error
            };
        }

        internal Result(bool succeeded, List<string> errors)
        {
            this.Succeeded = succeeded;
            this.Errors = errors;
        }

        public bool Succeeded { get; set; }

        public List<string> Errors { get; set; }

        public static Result Success() => new Result(true, new List<string> { });

        public static Result Failure(List<string> errors) => new Result(false, errors);

        public static Result Failure(string error) => new Result(false, error);
    }

    public class Result<T>
    {
        internal Result(bool succeeded, List<string> errors)
        {
            this.Succeeded = succeeded;
            this.Errors = errors;
        }

        public bool Succeeded { get; set; }

        public T Data { get; set; }

        public List<string> Errors { get; set; }

        public static Result<T> Success() => new Result<T>(true, new List<string> { });

        public static Result<T> Failure(List<string> errors) => new Result<T>(false, errors);
    }
}
