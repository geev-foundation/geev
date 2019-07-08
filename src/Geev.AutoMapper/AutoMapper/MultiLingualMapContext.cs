using Geev.Configuration;

namespace Geev.AutoMapper
{
    public class MultiLingualMapContext
    {
        public ISettingManager SettingManager { get; set; }

        public MultiLingualMapContext(ISettingManager settingManager)
        {
            SettingManager = settingManager;
        }
    }
}