namespace Common.Wrappers
{
    public class Pageable<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
     
        public Pageable(T data, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Status = 0;
            this.Message = null;
        }
        public Pageable(int pageNumber, int pageSize, int status,string message, T data)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Status = status;
            this.Message = message;
        }
    }
}
