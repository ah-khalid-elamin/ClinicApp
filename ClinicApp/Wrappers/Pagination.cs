namespace ClinicApp.Wrappers
{
    public class Pagination
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Pagination()
        {
            this.Page = 1;
            this.PageSize = 10;
        }
        public Pagination(int page, int pageSize)
        {
            this.Page = page < 1 ? 1 : page;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
