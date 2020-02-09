using System;
using System.Collections.Generic;
using System.Text;

namespace xx
{
    public interface IAppVersion
    {
        string GetVersion();
        int GetBuild();
    }
}
