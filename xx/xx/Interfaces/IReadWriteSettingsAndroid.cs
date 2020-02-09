using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xx
{
    public interface IReadWriteSettingsAndroid
    {
        Task <string> SaveSettingsAsync(string settings);
        Task <string> ReadSettingsAsync();
    }
}
