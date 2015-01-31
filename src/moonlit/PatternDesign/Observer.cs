using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.PatternDesign
{
    /// <summary>
    /// 观察者
    /// </summary>
    /// <typeparam name="TArg">参数类型</typeparam>
    public interface IObserver<TArg>
    {
        /// <summary>
        /// 通知观察者更新
        /// </summary>
        /// <param name="arg">通知的参数.</param>
        void Update(TArg arg);
    }
    /// <summary>
    /// 可观察者的对象
    /// </summary>
    /// <typeparam name="TArg">通知时使用的参数类型</typeparam>
    public class Observable<TArg>
    {
        List<IObserver<TArg>> _observers = new List<IObserver<TArg>>();
        /// <summary>
        /// 注册一个观察者
        /// </summary>
        /// <param name="observer">The observer.</param>
        public void Register(IObserver<TArg> observer)
        {
            var observers = from o in _observers
                            where o == observer
                            select observer;
            if (observers.Count() == 0)
            {
                _observers.Add(observer);
            }
        }
        /// <summary>
        /// 取消一个观察者的注册
        /// </summary>
        /// <param name="observer">The observer.</param>
        public void Unregister(IObserver<TArg> observer)
        {
            _observers.RemoveAll(o => o == observer);
        }
        /// <summary>
        /// 通知观察者更新
        /// </summary>
        /// <param name="arg">The arg.</param>
        public void Notify(TArg arg)
        {
            foreach (var observer in _observers)
            {
                observer.Update(arg);
            }
        }
    }
}
