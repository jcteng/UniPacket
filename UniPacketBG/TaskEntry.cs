using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using System.Diagnostics;
namespace UniPacketBG
{
    public sealed class TaskEntry:IBackgroundTask
    {
        const String UniPackPropPluginKey = "UniPackPropPluginKey";
        //We don't need async call there yet
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //make this plugin unqiue 
            object plugin;
            if (!CoreApplication.Properties.TryGetValue(UniPackPropPluginKey, out plugin))
            {
                plugin = new UniPacketPlugin();
                CoreApplication.Properties[UniPackPropPluginKey] = plugin;
                Debug.WriteLine("BG run with new Plugin");
            }
            else {
                Debug.WriteLine("BG run with existed Plugin");
            }
                        
            Windows.Networking.Vpn.VpnChannel.ProcessEventAsync(plugin, taskInstance.TriggerDetails);

        }
    }
}
