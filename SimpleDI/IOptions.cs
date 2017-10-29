namespace SimpleDI
{
    public interface IOptions<TOptions> where TOptions : class, new()
    {
        TOptions Value { get; }
        dynamic this[string key] { get; }
    }
}
