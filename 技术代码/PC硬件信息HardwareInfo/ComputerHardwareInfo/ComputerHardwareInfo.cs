using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Management;
using System.Collections.Generic;


namespace ComputerHardwareInfo
{
    public class ComputerHardwareInfo
    {
    //    // 硬件
    //Win32_Processor, // CPU 处理器
    //Win32_PhysicalMemory, // 物理内存条
    //Win32_Keyboard, // 键盘
    //Win32_PointingDevice, // 点输入设备，包括鼠标。
    //Win32_FloppyDrive, // 软盘驱动器
    //Win32_DiskDrive, // 硬盘驱动器
    //Win32_CDROMDrive, // 光盘驱动器
    //Win32_BaseBoard, // 主板
    //Win32_BIOS, // BIOS 芯片
    //Win32_ParallelPort, // 并口
    //Win32_SerialPort, // 串口
    //Win32_SerialPortConfiguration, // 串口配置
    //Win32_SoundDevice, // 多媒体设置，一般指声卡。
    //Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
    //Win32_USBController, // USB 控制器
    //Win32_NetworkAdapter, // 网络适配器
    //Win32_NetworkAdapterConfiguration, // 网络适配器设置
    //Win32_Printer, // 打印机
    //Win32_PrinterConfiguration, // 打印机设置
    //Win32_PrintJob, // 打印机任务
    //Win32_TCPIPPrinterPort, // 打印机端口
    //Win32_POTSModem, // MODEM
    //Win32_POTSModemToSerialPort, // MODEM 端口
    //Win32_DesktopMonitor, // 显示器
    //Win32_DisplayConfiguration, // 显卡
    //Win32_DisplayControllerConfiguration, // 显卡设置
    //Win32_VideoController, // 显卡细节。
    //Win32_VideoSettings, // 显卡支持的显示模式。

    //// 操作系统
    //Win32_TimeZone, // 时区
    //Win32_SystemDriver, // 驱动程序
    //Win32_DiskPartition, // 磁盘分区
    //Win32_LogicalDisk, // 逻辑磁盘
    //Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
    //Win32_LogicalMemoryConfiguration, // 逻辑内存配置
    //Win32_PageFile, // 系统页文件信息
    //Win32_PageFileSetting, // 页文件设置
    //Win32_BootConfiguration, // 系统启动配置
    //Win32_ComputerSystem, // 计算机信息简要
    //Win32_OperatingSystem, // 操作系统信息
    //Win32_StartupCommand, // 系统自动启动程序
    //Win32_Service, // 系统安装的服务
    //Win32_Group, // 系统管理组
    //Win32_GroupUser, // 系统组帐号
    //Win32_UserAccount, // 用户帐号
    //Win32_Process, // 系统进程
    //Win32_Thread, // 系统线程
    //Win32_Share, // 共享
    //Win32_NetworkClient, // 已安装的网络客户端
    //Win32_NetworkProtocol, // 已安装的网络协议



        /// <summary>
        /// Computer CPU 序号 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCPU_IDs()
        {
            List<string> cpuInfos = new List<string>();
            //WIN32_BaseBoard 主版
            //Win32_DisplayControllerConfiguration  显卡
            //Win32_NetworkAdapter 网卡
            //Win32_SoundDevice 声卡
            //Win32_OperatingSystem 系统信息
            ManagementClass cimobject = new ManagementClass("Win32_Processor");

            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                string cpuInfo = mo.Properties["Name"].Value.ToString();

                //foreach (var item in mo.Properties)
                //{
                //    var s = item;
                //}

                if (string.IsNullOrEmpty(cpuInfo) == false)
                {
                    cpuInfos.Add(cpuInfo);
                }
            }
            return cpuInfos;
        }

        /// <summary>
        /// 硬盘信息
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHD_Ids()
        {

            List<string> hd_Infos = new List<string>();
            //Win32_DiskDrive  磁盘硬件信息
            //ManagementClass cimobject = new ManagementClass("Win32_DiskDrive");

            //ManagementObjectCollection moc = cimobject.GetInstances();
            //foreach (ManagementObject mo in moc)
            //{
            //    string hd_Info = mo.Properties["Model"].Value.ToString();

            //    foreach (var item in mo.Properties)
            //    {
            //        var s = item;
            //    }

            //    if (string.IsNullOrEmpty(hd_Info) == false)
            //    {
            //        hd_Infos.Add(hd_Info);
            //    }
            //}



            ManagementObjectSearcher ms = new ManagementObjectSearcher("select * from Win32_LogicalDisk where DriveType=3");
            ManagementObjectCollection ReturnCollection = ms.Get();
            foreach (ManagementObject Return in ReturnCollection)
            {
                foreach (var item in Return.Properties)
                {
                    Console.WriteLine(item.Name);
                }

                string name = Return["Name"].ToString();
                string freelSpace = Return["FreeSpace"].ToString();
                string size = Return["Size"].ToString();

                hd_Infos.Add(string.Format("Name: {0},Frel: {1},Size: {2}", name, freelSpace, size));
            }

            return hd_Infos;
        }

        /// <summary>
        /// MAC
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMacAddresses()
        {

            List<string> hd_Infos = new List<string>();

            ManagementClass cimobject = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {

                foreach (var item in mo.Properties)
                {
                    var s = item;
                }

                if ((bool)mo["IPEnabled"] == true)
                {
                    string mac = mo["MacAddress"].ToString();
                    if (string.IsNullOrEmpty(mac) == false)
                    {
                        hd_Infos.Add(mac);
                    }
                }
            }
            return hd_Infos;
        }
    }


    public class IDE
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct IDSECTOR
        {
            public ushort wGenConfig;
            public ushort wNumCyls;
            public ushort wReserved;
            public ushort wNumHeads;
            public ushort wBytesPerTrack;
            public ushort wBytesPerSector;
            public ushort wSectorsPerTrack;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public ushort[] wVendorUnique;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string sSerialNumber;
            public ushort wBufferType;
            public ushort wBufferSize;
            public ushort wECCSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string sFirmwareRev;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string sModelNumber;
            public ushort wMoreVendorUnique;
            public ushort wDoubleWordIO;
            public ushort wCapabilities;
            public ushort wReserved1;
            public ushort wPIOTiming;
            public ushort wDMATiming;
            public ushort wBS;
            public ushort wNumCurrentCyls;
            public ushort wNumCurrentHeads;
            public ushort wNumCurrentSectorsPerTrack;
            public uint ulCurrentSectorCapacity;
            public ushort wMultSectorStuff;
            public uint ulTotalAddressableSectors;
            public ushort wSingleWordDMA;
            public ushort wMultiWordDMA;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] bReserved;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct DRIVERSTATUS
        {
            public byte bDriverError;
            public byte bIDEStatus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] bReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public uint[] dwReserved;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct SENDCMDOUTPARAMS
        {
            public uint cBufferSize;
            public DRIVERSTATUS DriverStatus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 513)]
            public byte[] bBuffer;
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct SRB_IO_CONTROL
        {
            public uint HeaderLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string Signature;
            public uint Timeout;
            public uint ControlCode;
            public uint ReturnCode;
            public uint Length;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IDEREGS
        {
            public byte bFeaturesReg;
            public byte bSectorCountReg;
            public byte bSectorNumberReg;
            public byte bCylLowReg;
            public byte bCylHighReg;
            public byte bDriveHeadReg;
            public byte bCommandReg;
            public byte bReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SENDCMDINPARAMS
        {
            public uint cBufferSize;
            public IDEREGS irDriveRegs;
            public byte bDriveNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] bReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] dwReserved;
            public byte bBuffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GETVERSIONOUTPARAMS
        {
            public byte bVersion;
            public byte bRevision;
            public byte bReserved;
            public byte bIDEDeviceMap;
            public uint fCapabilities;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] dwReserved; // For future use. 
        }


        [DllImport("kernel32.dll")]
        private static extern int CloseHandle(uint hObject);

        [DllImport("kernel32.dll")]
        private static extern int DeviceIoControl(uint hDevice,
                                                  uint dwIoControlCode,
                                                  ref SENDCMDINPARAMS lpInBuffer,
                                                  int nInBufferSize,
                                                  ref SENDCMDOUTPARAMS lpOutBuffer,
                                                  int nOutBufferSize,
                                                  ref uint lpbytesReturned,
                                                  int lpOverlapped);

        [DllImport("kernel32.dll")]
        private static extern int DeviceIoControl(uint hDevice,
                                                  uint dwIoControlCode,
                                                  int lpInBuffer,
                                                  int nInBufferSize,
                                                  ref GETVERSIONOUTPARAMS lpOutBuffer,
                                                  int nOutBufferSize,
                                                  ref uint lpbytesReturned,
                                                  int lpOverlapped);

        [DllImport("kernel32.dll")]
        private static extern uint CreateFile(string lpFileName,
                                              uint dwDesiredAccess,
                                              uint dwShareMode,
                                              int lpSecurityAttributes,
                                              uint dwCreationDisposition,
                                              uint dwFlagsAndAttributes,
                                              int hTemplateFile);
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint OPEN_EXISTING = 3;
        private const uint INVALID_HANDLE_VALUE = 0xffffffff;
        private const uint DFP_GET_VERSION = 0x00074080;
        private const int IDE_ATAPI_IDENTIFY = 0xA1; // Returns ID sector for ATAPI. 
        private const int IDE_ATA_IDENTIFY = 0xEC; // Returns ID sector for ATA. 
        private const int IDENTIFY_BUFFER_SIZE = 512;
        private const uint DFP_RECEIVE_DRIVE_DATA = 0x0007c088;


        public static string Read(byte drive)
        {
            OperatingSystem os = Environment.OSVersion;
            if (os.Platform != PlatformID.Win32NT) throw new NotSupportedException("仅支持WindowsNT/2000/XP");
            //if (os.Version.Major < 5) throw new NotSupportedException("仅支持WindowsNT/2000/XP"); 

            string driveName = "\\\\.\\PhysicalDrive" + drive.ToString();
            uint device = CreateFile(driveName,
                                     GENERIC_READ | GENERIC_WRITE,
                                     FILE_SHARE_READ | FILE_SHARE_WRITE,
                                     0, OPEN_EXISTING, 0, 0);
            if (device == INVALID_HANDLE_VALUE) return "";
            GETVERSIONOUTPARAMS verPara = new GETVERSIONOUTPARAMS();
            uint bytRv = 0;

            if (0 != DeviceIoControl(device, DFP_GET_VERSION,
                                     0, 0, ref verPara, Marshal.SizeOf(verPara),
                                     ref bytRv, 0))
            {
                if (verPara.bIDEDeviceMap > 0)
                {
                    byte bIDCmd = (byte)(((verPara.bIDEDeviceMap >> drive & 0x10) != 0) ? IDE_ATAPI_IDENTIFY : IDE_ATA_IDENTIFY);
                    SENDCMDINPARAMS scip = new SENDCMDINPARAMS();
                    SENDCMDOUTPARAMS scop = new SENDCMDOUTPARAMS();

                    scip.cBufferSize = IDENTIFY_BUFFER_SIZE;
                    scip.irDriveRegs.bFeaturesReg = 0;
                    scip.irDriveRegs.bSectorCountReg = 1;
                    scip.irDriveRegs.bCylLowReg = 0;
                    scip.irDriveRegs.bCylHighReg = 0;
                    scip.irDriveRegs.bDriveHeadReg = (byte)(0xA0 | ((drive & 1) << 4));
                    scip.irDriveRegs.bCommandReg = bIDCmd;
                    scip.bDriveNumber = drive;

                    if (0 != DeviceIoControl(device, DFP_RECEIVE_DRIVE_DATA,
                                             ref scip, Marshal.SizeOf(scip), ref scop,
                                             Marshal.SizeOf(scop), ref bytRv, 0))
                    {
                        StringBuilder s = new StringBuilder();
                        for (int i = 20; i < 40; i += 2)
                        {
                            s.Append((char)(scop.bBuffer[i + 1]));
                            s.Append((char)scop.bBuffer[i]);
                        }
                        CloseHandle(device);
                        return s.ToString().Trim();
                    }
                }
            }
            CloseHandle(device);
            return "";
        }

    }
}
