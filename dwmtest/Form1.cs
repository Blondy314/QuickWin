using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DwmTest
{
    public partial class Form1 : Form
    {
        #region Constants

        static readonly int GWL_STYLE = -16;
        
        static readonly int DWM_TNP_VISIBLE = 0x8;
        static readonly int DWM_TNP_OPACITY = 0x4;
        static readonly int DWM_TNP_RECTDESTINATION = 0x1;

        static readonly ulong WS_VISIBLE = 0x10000000L;
        static readonly ulong WS_BORDER = 0x00800000L;
        static readonly ulong TARGETWINDOW = WS_BORDER | WS_VISIBLE;

        #endregion

        #region DWM functions

        [DllImport("dwmapi.dll")]
        static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

        [DllImport("dwmapi.dll")]
        static extern int DwmUnregisterThumbnail(IntPtr thumb);

        [DllImport("dwmapi.dll")]
        static extern int DwmQueryThumbnailSourceSize(IntPtr thumb, out PSIZE size);

        [DllImport("dwmapi.dll")]
        static extern int DwmUpdateThumbnailProperties(IntPtr hThumb, ref DWM_THUMBNAIL_PROPERTIES props);

        #endregion

        #region Win32 helper functions

        [DllImport("user32.dll")]
        static extern ulong GetWindowLongA(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int EnumWindows(EnumWindowsCallback lpEnumFunc, int lParam);
        delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        #endregion

        private IntPtr thumb;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetWindows();
        }

        #region Retrieve list of windows

        private List<Window> windows;

        private void GetWindows()
        {
            windows = new List<Window>();
            EnumWindows(Callback, 0);

            lstWindows.Items.Clear();
            foreach (Window w in windows)
                lstWindows.Items.Add(w);
        }

        private bool Callback(IntPtr hwnd, int lParam)
        {
            if (this.Handle != hwnd && (GetWindowLongA(hwnd, GWL_STYLE) & TARGETWINDOW) == TARGETWINDOW)
            {
                StringBuilder sb = new StringBuilder(100);
                GetWindowText(hwnd, sb, sb.Capacity);

                Window t = new Window();
                t.Handle = hwnd;
                t.Title = sb.ToString();
                windows.Add(t);
            }

            return true; //continue enumeration
        }

        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (thumb != IntPtr.Zero)
                DwmUnregisterThumbnail(thumb);

            GetWindows();
        }

        private void lstWindows_SelectedIndexChanged(object sender, EventArgs e)
        {
            Window w = (Window)lstWindows.SelectedItem;

            if (thumb != IntPtr.Zero)
                DwmUnregisterThumbnail(thumb);

            int i = DwmRegisterThumbnail(this.Handle, w.Handle, out thumb);

            if (i == 0)
                UpdateThumb();
        }

        #region Update thumbnail properties

        private void UpdateThumb()
        {
            if (thumb != IntPtr.Zero)
            {
                PSIZE size;
                DwmQueryThumbnailSourceSize(thumb, out size);

                DWM_THUMBNAIL_PROPERTIES props = new DWM_THUMBNAIL_PROPERTIES();

                props.fVisible = true;
                props.dwFlags = DWM_TNP_VISIBLE | DWM_TNP_RECTDESTINATION | DWM_TNP_OPACITY;
                props.opacity = (byte)opacity.Value;
                props.rcDestination = new Rect(image.Left, image.Top, image.Right, image.Bottom);

                if (size.x < image.Width)
                    props.rcDestination.Right = props.rcDestination.Left + size.x;

                if (size.y < image.Height)
                    props.rcDestination.Bottom = props.rcDestination.Top + size.y;

                DwmUpdateThumbnailProperties(thumb, ref props);
            }
        }

        private void opacity_Scroll(object sender, EventArgs e)
        {
            UpdateThumb();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            UpdateThumb();
        }

        #endregion
    }

    internal class Window
    {
        public string Title;
        public IntPtr Handle;

        public override string ToString()
        {
            return Title;
        }
    }

    #region Interop structs

    [StructLayout(LayoutKind.Sequential)]
    internal struct DWM_THUMBNAIL_PROPERTIES
    {
        public int dwFlags;
        public Rect rcDestination;
        public Rect rcSource;
        public byte opacity;
        public bool fVisible;
        public bool fSourceClientAreaOnly;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Rect
    {
        internal Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PSIZE
    {
        public int x;
        public int y;
    }

    #endregion
}