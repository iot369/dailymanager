using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
namespace daily
{
    static class Program
    {
        /// <summary>
        /// 设置由不同线程产生的窗口的显示状态。
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">0不可见但仍然运行,1居中,2最小化,3最大化 </param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        /// 将窗体置于最顶端
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process[] processes = Process.GetProcessesByName("daily");
            if (processes != null && processes.Length == 2)
            {
                processes[1].CloseMainWindow();
                ShowWindowAsync(processes[0].MainWindowHandle, 1);
                SetForegroundWindow(processes[0].MainWindowHandle);
            }
            else
            {
                Global.GlobalVariable.InitSysParameter();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new UI.FrmMainThread());
            }
        }
    }
}