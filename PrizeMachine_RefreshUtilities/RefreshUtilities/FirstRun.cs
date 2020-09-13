using System.Collections;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace RefreshUtilities
{
    public class FirstRun
    {
        public bool FirstTimeAppHasRun = true;

        public FirstRun()
        {

        }

        public bool IsThisFirstRunOnThisPC()
        {
            var cpu = new ManagementObjectSearcher("select * from Win32_Processor").Get().Cast<ManagementObject>().First();

            string processorID = (string)cpu["ProcessorId"];
            string configPath = Application.StartupPath + "/config/config.cel";
            string[] configs;
            string configProcessorID = "";
            ArrayList configsList = new ArrayList();

            if (File.Exists(configPath))
            {
                configs = Encryption.Decrypt(File.ReadAllLines(configPath));

                foreach (string config in configs)
                {
                    configsList.Add(config);

                    if (config.Contains("processorID"))
                    {
                        configProcessorID = config.Replace("processorID=", "");

                        if (configProcessorID == processorID)
                            return false;
                        else
                        {
                            configsList[configsList.IndexOf(config)] = "processorID=" + processorID;
                            FirstTimeAppHasRun = false;
                        }
                    }
                }

                if (configsList.Count == 0)
                    configsList.Add("processorID=" + processorID);

                configs = configsList.ToArray(typeof(string)) as string[];
                configs = Encryption.Encrypt(configs);

                if (!FirstTimeAppHasRun)
                    File.WriteAllLines(configPath, configs);
            }
            else
            {
                System.IO.Directory.CreateDirectory(configPath.Replace("/config.cel", ""));
                File.WriteAllText(configPath, "");
            }



            //CPU.ID = (string)cpu["ProcessorId"];
            //CPU.Socket = (string)cpu["SocketDesignation"];
            //CPU.Name = (string)cpu["Name"];
            //CPU.Description = (string)cpu["Caption"];
            //CPU.AddressWidth = (ushort)cpu["AddressWidth"];
            //CPU.DataWidth = (ushort)cpu["DataWidth"];
            //CPU.Architecture = (CPU.CpuArchitecture)(ushort)cpu["Architecture"];
            //CPU.SpeedMHz = (uint)cpu["MaxClockSpeed"];
            //CPU.BusSpeedMHz = (uint)cpu["ExtClock"];
            //CPU.L2Cache = (uint)cpu["L2CacheSize"] * (ulong)1024;
            //CPU.L3Cache = (uint)cpu["L3CacheSize"] * (ulong)1024;
            //CPU.Cores = (uint)cpu["NumberOfCores"];
            //CPU.Threads = (uint)cpu["NumberOfLogicalProcessors"];

            //CPU.Name =
            //   CPU.Name
            //   .Replace("(TM)", "™")
            //   .Replace("(tm)", "™")
            //   .Replace("(R)", "®")
            //   .Replace("(r)", "®")
            //   .Replace("(C)", "©")
            //   .Replace("(c)", "©")
            //   .Replace("    ", " ")
            //   .Replace("  ", " ");

            return true;
        }
    }
}
