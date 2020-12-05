namespace InstantMessenger.Shared.Messages.Queries
{
    public interface IPagedQuery<TResult> : IQuery<TResult>
    {
        int Page { get; }
        int Results { get; }
        string OrderBy { get; }
        string SortOrder { get; }
    }
}