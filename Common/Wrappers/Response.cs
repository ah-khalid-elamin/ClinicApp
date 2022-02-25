namespace Common.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data)
        {
            Status = 0;
            Message = string.Empty;
            Data = data;
        }
        public Response(int status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
        public Response(int status, string message)
        {
            Status = status;
            Message = message;
        }
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

    }
}
