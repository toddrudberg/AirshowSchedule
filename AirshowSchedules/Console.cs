using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static AirshowSchedules.frmAirshowScheduleTool;

namespace AirshowSchedules;
public partial class frmAirshowScheduleTool
{
    #region Console Output
    [DllImport("kernel32.dll")]
    static extern bool AllocConsole();
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, COORD size);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleWindowInfo(IntPtr hConsoleOutput, bool absolute, ref SMALL_RECT consoleWindow);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    private const int STD_OUTPUT_HANDLE = -11;
    private static readonly IntPtr HWND_TOP = IntPtr.Zero;
    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_NOSIZE = 0x0001;

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;

        public COORD(short x, short y)
        {
            X = x;
            Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }
    #endregion
    private void SetConsoleSize(int width, int height)
    {
        IntPtr consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);

        // Set the buffer size
        COORD bufferSize = new COORD((short)width, (short)height);
        SetConsoleScreenBufferSize(consoleHandle, bufferSize);

        // Set the window size
        SMALL_RECT windowSize = new SMALL_RECT();
        windowSize.Left = 0;
        windowSize.Top = 0;
        windowSize.Right = (short)(width - 1);
        windowSize.Bottom = (short)(height - 1);
        SetConsoleWindowInfo(consoleHandle, true, ref windowSize);
        // Find the console window handle
        IntPtr consoleWindowHandle = FindWindow(null, Console.Title);
        // Position the console in the upper-left corner
        SetWindowPos(consoleWindowHandle, HWND_TOP, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOSIZE);
    }
}

