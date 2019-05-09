using MediatR;
using System.Collections.Generic;

namespace IdS4.Abstraction.Events
{
    /// <summary>
    /// 事件管理者
    /// </summary>
    public interface IEventsManager
    {
        /// <summary>
        /// 事件合集
        /// </summary>
        IReadOnlyCollection<INotification> Events { get; }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="event"></param>
        void Add(INotification @event);

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="event"></param>
        void Remove(INotification @event);

        /// <summary>
        /// 清空事件
        /// </summary>
        void Clear();

        /// <summary>
        /// 派遣事件
        /// </summary>
        void Dispatch();
    }
}
