namespace IdS4.Abstraction.Events
{
    /// <summary>
    /// 表示实体具有时间管理者
    /// </summary>
    public interface IHasEventsManager
    {
        IEventsManager EventsManager { get; }
    }
}
