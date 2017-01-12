using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Events
{

    public class WeakEvent<TEventArgs>
    {
        private readonly List<WeakDelegate<TEventArgs>> _handlers = new List<WeakDelegate<TEventArgs>>();

        public void Raise(object sender, TEventArgs e)
        {
            lock (_handlers)
            {
                _handlers.RemoveAll(h => !h.Invoke(sender, e));
            }
        }

        public void Subscribe(EventHandler<TEventArgs> handler)
        {
            var weakHandlers = handler
                .GetInvocationList()
                .Select(d => new WeakDelegate<TEventArgs>(d))
                .ToList();

            lock (_handlers)
            {
                _handlers.AddRange(weakHandlers);
            }
        }

        public void Unsubscribe(EventHandler<TEventArgs> handler)
        {
            lock (_handlers)
            {
                int index = _handlers.FindIndex(h => h.IsMatch(handler));
                if (index >= 0)
                    _handlers.RemoveAt(index);
            }
        }

        public bool HasSubscribers
        {
            get
            {
                return _handlers.Count > 0;
            }
        }
    }
}

