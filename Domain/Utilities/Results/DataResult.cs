namespace SocialOffice.Domain.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool isSuccess) : base(isSuccess)
        {
            Data = data;
        }

        public DataResult(T data, bool isSuccess, string key, string message) : base(isSuccess, key, message)
        {
            Data = data;
        }

        public DataResult(bool isSuccess, string key, string message) : base(isSuccess, key, message)
        {

        }

        public DataResult(bool isSuccess, string message) : base(isSuccess, message)
        {

        }

        public DataResult(T data, bool isSuccess, int totalCount) : base(isSuccess)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public DataResult(T data, bool isSuccess, List<KeyValuePair<string, string>> messages) : base(isSuccess, messages)
        {
            Messages.AddRange(messages);
        }

        public DataResult(bool isSuccess, List<KeyValuePair<string, string>> messages) : base(isSuccess, messages)
        {
            Messages.AddRange(messages);
        }

        public DataResult(T data)
        {

        }

        public DataResult()
        {

        }

        public T Data { get; set; }
        public int TotalCount { get; set; }
    }
}
