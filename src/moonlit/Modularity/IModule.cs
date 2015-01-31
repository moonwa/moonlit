using System.Collections.Generic;

namespace Moonlit.Modularity
{
    public interface IModule
    {
        void Init();
        IEnumerable<IModule> Dependencies { get; }
        Bootstrapper Bootstrapper { set; }
    }
}
