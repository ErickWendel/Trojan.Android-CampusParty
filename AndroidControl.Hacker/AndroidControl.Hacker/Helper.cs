using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;

namespace AndroidControl.Hacker
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y) { }

        public static implicit operator System.Drawing.Point(POINT p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static implicit operator POINT(System.Drawing.Point p)
        {
            return new POINT(p.X, p.Y);
        }
    }

    public class Helper
    {
        const uint SPI_SETDESKWALLPAPER = 0x14;
        const uint SPIF_UPDATEINIFILE = 0x01;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #region Methods to acess unmanaged code of Windows OS (DLL buit in C or C++) PINVOKE

        /// <summary>
        /// This method is responsible for swap the mouse button
        /// </summary>
        /// <see cref="http://pinvoke.net/default.aspx/user32/SwapMouseButton.html"/>
        /// <param name="fSwap"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool SwapMouseButton(bool fSwap);

        /// <summary>
        ///  Method to hide the mouse pointer
        /// </summary>
        /// <see cref="http://pinvoke.net/default.aspx/user32/ShowCursor.html"/>
        /// <param name="bShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern int ShowCursor(bool bShow);

        /// <summary>
        /// Method to open/close the cd/dvd driver
        /// </summary>
        /// <see cref="http://pinvoke.net/default.aspx/winmm/mciSendString.html"/>
        /// <param name="command"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferSize"></param>
        /// <param name="hwndCallback"></param>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        /// <summary>
        /// Method to logout of Windows
        /// </summary>
        /// <see cref="http://pinvoke.net/default.aspx/user32/ExitWindowsEx.html"/>
        /// <param name="uFlags"></param>
        /// <param name="dwReason"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        /// <summary>
        /// Method to lock workstation
        /// <see cref="http://pinvoke.net/default.aspx/user32/LockWorkStation.html"/>
        /// </summary>
        [DllImport("user32")]
        public static extern void LockWorkStation();


        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(
            uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetCursorPos(int x, int y);

        #endregion
        public static void SetWallpaper(string path)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE);
        }
        public static void SwapMouse(bool swap)
        {
            SwapMouseButton(swap);
        }

         
        public static void MouseCursor( )
        {
            POINT point;
            GetCursorPos(out point);
            for (int i = 0; i < 4; i++)
            {
                
                var x = point.X + i * 100;
                var y = point.Y - i * 100;
                SetCursorPos(x, y);
                Thread.Sleep(500);
            }
            
        }

        public static void OpenCdDriver()
        {
            // "set CDAudio door open" is the command to open CD Driver
            mciSendString("set CDAudio door open", null, 127, IntPtr.Zero);
        }

        public static void CloseCdDriver()
        {
            // "set CDAudio door open" is the command to close CD Driver
            mciSendString("set CDAudio door closed", null, 127, IntPtr.Zero);
        }

        public static void Logoff()
        {
            ExitWindowsEx(0, 0);
        }

        public static void Lock()
        {
            LockWorkStation();
        }



        public static void TaskBar(Int32 visibility)
        {
            var hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, visibility);
        }


    }
}
