namespace InstantMessenger.Shared.Messages.Queries
{
    public abstract class PagedQueryBase<TResult> : IPagedQuery<TResult>
    {
        public int Page { get; set; }
        public int Results { get; set; }
        public string OrderBy { get; set; }
        public string SortOrder { get; set; }
    }
}