namespace IdS4.Application.Models.Paging
{
    public class PagingQuery
    {
        public int Skip { get; set; }
        public int Limit { get; set; }
        public string Sort { get; set; }
    }

    public class PagingQuery<T> : PagingQuery
    {
        public T Query { get; set; }
    }
}
