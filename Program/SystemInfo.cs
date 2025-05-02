using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InsightEngine.Program
{
    public class SystemInfo
    {
        public string[] getOperatingSystemInfo()
        {
            string[] resp = new string[3];

            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject managementObject in mos.Get())
            {
                if (managementObject["Caption"] != null)
                {
                    resp[0] = managementObject["Caption"].ToString();
                }
                if (managementObject["OSArchitecture"] != null)
                {
                    resp[1] = managementObject["OSArchitecture"].ToString();
                }
                if (managementObject["CSDVersion"] != null)
                {
                    resp[2] = managementObject["CSDVersion"].ToString();
                }
            }

            return resp;
        }

        public string getProcessorInfo()
        {
            string resp = "";
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    resp += processor_name.GetValue("ProcessorNameString").ToString();
                }
            }
            return resp;
        }

        public string[] GetGPUInfo()
        {
            List<string> gpuInfoList = new List<string>();

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}"))
            {
                if (key != null)
                {
                    foreach (string subKeyName in key.GetSubKeyNames())
                    {
                        if (subKeyName.StartsWith("0"))
                        {
                            using (RegistryKey subKey = key.OpenSubKey(subKeyName))
                            {
                                string? gpuName = subKey?.GetValue("DriverDesc")?.ToString();
                                string? vramSize = subKey?.GetValue("HardwareInformation.qwMemorySize")?.ToString();

                                if (!string.IsNullOrEmpty(gpuName))
                                {
                                    gpuInfoList.Add(gpuName);

                                    if (!string.IsNullOrEmpty(vramSize) && long.TryParse(vramSize, out long vramBytes))
                                    {
                                        gpuInfoList.Add($"{vramBytes / (1024 * 1024)} MB");
                                    }
                                    else
                                    {
                                        gpuInfoList.Add("N/A");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return gpuInfoList.ToArray();
        }

        public List<string[]> GetRAMInfoFromWMI()
        {
            List<string[]> ramSticks = new List<string[]>();

            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");

                foreach (ManagementObject obj in searcher.Get())
                {
                    string[] stickInfo = new string[3];

                    stickInfo[0] = $"{(Convert.ToUInt64(obj["Capacity"]) / (1024 * 1024))} MB";

                    stickInfo[1] = GetDDRVersion(obj["SMBIOSMemoryType"]);

                    stickInfo[2] = $"{obj["Speed"]} MHz";

                    ramSticks.Add(stickInfo);
                }
            }
            catch
            {
                
            }

            return ramSticks;
        }

        private string GetDDRVersion(object smbiosType)
        {
            if (smbiosType == null) return "Unknown";

            int type = Convert.ToInt32(smbiosType);

            return type switch
            {
                20 => "DDR",
                21 => "DDR2",
                22 => "DDR2 FB-DIMM",
                24 => "DDR3",
                26 => "DDR4",
                30 => "DDR5",
                _ => $"Unknown (Type {type})"
            };
        }

        public string GetDiskInfo()
        {
            try
            {
                var drive = new DriveInfo("C");
                if (drive.IsReady)
                {
                    return $"{drive.AvailableFreeSpace / (1024 * 1024 * 1024)} / {drive.TotalSize / (1024 * 1024 * 1024)} GB";
                }
                else
                {
                    return "-";
                }
            }
            catch (Exception ex)
            {
                return "N/A";    
            }
        }
    }
}