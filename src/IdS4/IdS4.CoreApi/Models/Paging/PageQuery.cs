namespace IdS4.CoreApi.Models.Paging
{
    public class PageQuery
    {
        public int Skip { get; set; }
        public int Limit { get; set; }
        public string Sort { get; set; }
    }

    public class PageQuery<T> : PageQuery
    {
        public T Query { get; set; }
    }
}
