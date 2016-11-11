using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins
{
    public class DetachedPlugin : IDetachedPlugin
    {
        public DetachedPlugin()
        {
            Name = GetType().Name.Replace("Plugin", "");
        }

        public virtual string Name { get; set; }

        bool _enabled;
        public virtual bool IsEnabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (value)
                {
                    if (!_enabled)
                        OnEnabled();
                }
                else
                {
                    if (_enabled)
                        OnDisabled();
                }
                _enabled = value;
            }
        }

        public virtual int Priority { get; set; }

        protected virtual void OnEnabled()
        {
           
        }

        protected virtual void OnDisabled()
        {

        }

        public void Dispose()
        {
            OnDisabled();
        }
    }
}
