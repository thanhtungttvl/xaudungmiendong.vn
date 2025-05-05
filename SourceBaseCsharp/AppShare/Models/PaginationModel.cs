namespace AppShare.Models
{
    public class PaginationModel<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public T? Data { get; set; }
    }
}
