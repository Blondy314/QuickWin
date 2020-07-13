using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BrightIdeasSoftware;
using MetroFramework;
using MetroFramework.Forms;

namespace QuickWin
{
    // TODO -
    // add run time + more details on process
    // Not responding
    // Replace task manager
    // Show CPU
    // Suspend
    // cmdline

    public partial class WinForm : Form
    {
        private IntPtr _thumb;
        private bool _notified;
        private ProcInfo[] _procs;
        private Timer _refreshTimer;
        private readonly NotifyIcon _notifyIcon;

        public WinForm()
        {
            Icon = Icon.FromHandle(new Bitmap(Properties.Resources.QuickWin).GetHicon());
            
            _notifyIcon = new NotifyIcon
            {
                Icon = Icon,
                ContextMenu = new ContextMenu(new[]
                {
                    new MenuItem("Show", (e, s) => ShowThis()), 
                    new MenuItem("Exit", (e, s) => Close()),
                }),

                BalloonTipText = @"Still Running (Ctrl + Shift + W)",
                BalloonTipTitle = @"Quick Win"
            };

            InitializeComponent();
        }

        private void RegisterHotKey()
        {
            try
            {
                var key = new HotKey(
                    Keys.W,
                    HotKey.KeyModifiers.Control | HotKey.KeyModifiers.Shift,
                    HotKeyPressed
                );
            }
            catch
            {
                // ignored
            }
        }

        private async void WinForm_Load(object sender, EventArgs e)
        {
            try
            {
                RegisterHotKey();

                lstView.Columns.AddRange(new ColumnHeader[]
                {
                    new OLVColumn("Id", "Id"),
                    new OLVColumn("Name", "Name"),
                    new OLVColumn("Title", "Title"),
              //      new OLVColumn("Start Time", "StartTime"),
                });

                lstView.ContextMenu = new ContextMenu(new[]
                {
                    new MenuItem("Copy", OnCopy),
                    new MenuItem("Kill", KillProc),
                    new MenuItem("Suspend", SuspendProc),
                    new MenuItem("Resume", ResumeProc),
                });

                lstView.ItemsChanged += LstViewOnItemsChanged;
              //  lstView.GridLines = true;
                lstView.KeyDown += LstViewOnKeyDown;

                lstLog.GridLines = true;
                
                lstLog.RowFormatter = LogRowFormat;

                lstLog.Columns.AddRange(new ColumnHeader[]
                {
                    new OLVColumn("Time", "Time"),
                    new OLVColumn("Log", "Log"),
                });

                _refreshTimer = new Timer { Interval = 10 * 1000, Enabled = true };
                _refreshTimer.Tick += async (s, _) => await GetProcs();
                _refreshTimer.Start();

                Log("Getting processes..");

                await GetProcs();
            }
            catch
            {
                // ignored
            }
        }

        private void LogRowFormat(OLVListItem item)
        {
            try
            {
                var log = (LogInfo) item.RowObject;
                if (log == null)
                {
                    return;
                }

                if (log.Color.HasValue)
                {
                    item.ForeColor = log.Color.Value;
                }

                if (log.Error)
                {
                    item.ForeColor = Color.DarkRed;
                }
            }
            catch
            {
                // ignored
            }
        }

        private void OnCopy(object sender, EventArgs e)
        {
            try
            {
                var hit = lstView.HitTest(_lastHit);

                if (hit.Item == null)
                {
                    return;
                }

                Clipboard.SetText(hit.SubItem.Text);
            }
            catch
            {
                // ignored
            }
        }

        private string GetStartTime(Process proc)
        {
            try
            {
                return proc.StartTime.ToString("dd/MM/yy HH:mm:ss");
            }
            catch
            {
                return null;
            }
        }

        private ProcInfo GetProcInfo(Process proc)
        {
            try
            {
                return new ProcInfo
                {
                    Id = proc.Id,
                    Name = proc.ProcessName,
                    Title = proc.MainWindowTitle,
                    //StartTime = GetStartTime(proc),
                    Proc = proc
                };
            }
            catch
            {
                return null;
            }
        }

        private static  string[] excludeProcs = new[]
        {
            "ApplicationFrameHost",
            "ScriptedSandbox32",
            "ScriptedSandbox",
            "ScriptedSandbox64"
        };

        private async Task GetProcs()
        {
            try
            {
                ProcInfo[] procs = null;

                await Task.Run(() =>
                {
                    var allProcs = Process.GetProcesses();
                    procs = allProcs.
                        Select(GetProcInfo).
                        Where(p => p != null).
                        Where(p => !excludeProcs.Contains(p.Name)).
                        OrderBy(p => p.Name).
                        ThenBy(proc => proc.StartTime).ToArray();
                });

                if (!chkAllProcs.Checked)
                {
                    procs = procs.Where(p => p.Proc.MainWindowHandle != IntPtr.Zero).ToArray();
                    _procs = _procs?.Where(p => p.Proc.MainWindowHandle != IntPtr.Zero).ToArray();
                }

                if (_procs == null)
                {
                    _procs = procs;

                    Log($"Found {procs.Length} processes.");

                    lstView.SetObjects(procs);
                    lstView.AutoResizeColumns();
                    lstView.CalculateReasonableTileSize();
                    lstView.Columns[2].Width = 200;
                    return;
                }

                var closedProcs = _procs.Where(p => procs.All(p1 => p1.Id != p.Id)).ToArray();
                var newProcs = procs.Where(p => _procs.All(p1 => p1.Id != p.Id)).ToArray();

                if (newProcs.Length > 0)
                {
                    var exist = lstView.Objects.Cast<ProcInfo>().Where(p => newProcs.Any(p1 => p1.Id == p.Id)).ToArray();
                    if (exist.Length == 0)
                    {
                        Log($"{newProcs.Length} processes created", color: Color.DarkBlue);
                        lstView.AddObjects(newProcs);
                        lstView.Sort("Name");
                    }
                }

                if (closedProcs.Length > 0)
                {
                    var closedObjects = lstView.Objects.Cast<ProcInfo>().Where(p => closedProcs.Any(p1 => p1.Id == p.Id)).ToArray();
                   
                    Log($"{closedProcs.Length} processes closed", color: Color.Gray);
                    lstView.RemoveObjects(closedObjects);
                }

                _procs = procs;
            }
            catch(Exception ex)
            {
                Log("Failed to get processes", ex);
            }
        }

        private string ProcStr(ProcInfo proc)
        {
            return $"{proc.Name} ({proc.Id})";
        }

        private void SuspendProc(object sender, EventArgs e)
        {
            foreach (var process in lstView.SelectedObjects.Cast<ProcInfo>().ToArray())
            {
                try
                {
                    Log("Suspending " + ProcStr(process));
                    NtSuspendProcess(process.Proc.Handle);
                }
                catch
                {
                    // ignored
                }
            }
        }


        private void ResumeProc(object sender, EventArgs e)
        {
            foreach (var process in lstView.SelectedObjects.Cast<ProcInfo>().ToArray())
            {
                try
                {
                    Log("Resuming " + ProcStr(process));
                    NtResumeProcess(process.Proc.Handle);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void KillProc(object sender, EventArgs e)
        {
            try
            {
                var processes = lstView.SelectedObjects.Cast<ProcInfo>().ToArray();
                var procStrs = string.Join("\n", processes.Select(ProcStr));

                if (MessageBox.Show($@"Kill {procStrs}?", 
                    @"Quick Win", 
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.ServiceNotification) != DialogResult.Yes)
                {
                    return;
                }

                foreach (var process in processes)
                {
                    var procStr = ProcStr(process);

                    try
                    {
                        Log($"Killing {procStr}");

                        NtSuspendProcess(process.Proc.Handle);
                        process.Proc.Kill();
                    }
                    catch(Exception ex)
                    {
                        Log("Failed to kill process " + procStr, ex);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        private void LstViewOnItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            try
            {
                SetItemCount();

                for (var i = 0; i < lstView.Items.Count; ++i)
                {
                    var tag = lstView.Items[i].Tag;
                    if (tag == null)
                    {
                        continue;
                    }

                    var proc = ((ProcInfo)tag).Proc;
                    if (!proc.Responding)
                    {
                        lstView.Items[i].BackColor = Color.LightGray;
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        private void LstViewOnKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
                {
                    txtFilter.Focus();
                    if (string.IsNullOrEmpty(txtFilter.Text))
                    {
                        txtFilter.Text = new KeysConverter().ConvertToString(e.KeyCode)?.ToLower();
                        txtFilter.SelectionStart = 1;
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        private void UpdateThumb()
        {
            if (_thumb != IntPtr.Zero)
            {
                DwmQueryThumbnailSourceSize(_thumb, out var size);

                var top = splitLeft.Top + splitLeft.Panel2.Top;
                var bottom = splitLeft.Top + picBox.Bottom;
                var right = splitLeft.Panel2.Right;

                var props = new DWM_THUMBNAIL_PROPERTIES
                {
                    fVisible = true,
                    dwFlags = DWM_TNP_VISIBLE | DWM_TNP_RECTDESTINATION | DWM_TNP_OPACITY,
                    opacity = 255,
                    rcDestination = new Rect(splitLeft.Panel2.Left,
                        top,
                        right, 
                        bottom)
                };

                if (size.x < picBox.Width)
                {
                    props.rcDestination.Right = props.rcDestination.Left + size.x;
                }

                if (size.y < picBox.Height)
                {
                    props.rcDestination.Bottom = props.rcDestination.Top + size.y;
                }

                DwmUpdateThumbnailProperties(_thumb, ref props);
            }
        }

        private void HotKeyPressed(object sender, EventArgs e)
        {
            try
            {
                ShowThis();
            }
            catch
            {
                // ignored
            }
        }

        private void ShowThis()
        {
            BringToFront();
            Activate();
            Show();
            lstView.SelectedIndex = -1;

            txtFilter.Text = null;
            WindowState = FormWindowState.Normal;
        }

        private void textBoxFilterSimple_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TimedFilter(lstView, txtFilter.Text, 2);
            }
            catch
            {
                // ignored
            }
        }

        private void TimedFilter(ObjectListView olv, string txt, int matchKind)
        {
            TextMatchFilter filter = null;
            if (!string.IsNullOrEmpty(txt))
            {
                switch (matchKind)
                {
                    default:
                        filter = TextMatchFilter.Contains(olv, txt);
                        break;
                    case 1:
                        filter = TextMatchFilter.Prefix(olv, txt);
                        break;
                    case 2:
                        filter = TextMatchFilter.Regex(olv, txt);
                        break;
                }
            }
            // Setup a default renderer to draw the filter matches
            olv.DefaultRenderer = filter == null ? null : new HighlightTextRenderer(filter);

            // Some lists have renderers already installed
            if (olv.GetColumn(0).Renderer is HighlightTextRenderer highlightingRenderer)
            {
                highlightingRenderer.Filter = filter;
            }

            olv.ModelFilter = filter;
            SetItemCount();
        }

        private void SetItemCount()
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke((Action)(SetItemCount));
                    return;
                }
            
                txtNum.Text = $@"{lstView.Items.Count} Results. ({lstView.SelectedObjects.Count} Selected)";
            }
            catch
            {
                // ignored
            }
        }

        private void WinForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                {
                    txtFilter.Focus();
                    return;
                }

                if (e.KeyCode == Keys.F5)
                {
                    btnRefresh_Click(null, null);
                    return;
                }

                if (e.KeyCode == Keys.Escape)
                {
                    WindowState = FormWindowState.Minimized;

                    e.Handled = e.SuppressKeyPress = true;

                    if (_notified)
                    {
                        return;
                    }

                    _notified = true;
                    _notifyIcon.ShowBalloonTip(1000);
                    _notifyIcon.Visible = true;
                    return;
                }

                if (e.KeyCode == Keys.Down)
                {
                    lstView.Focus();
                    if (lstView.SelectedIndex == -1)
                    {
                        lstView.SelectedIndex = 0;
                    }
                    return;
                }

                if (e.KeyCode == Keys.Up)
                {
                    if (lstView.SelectedIndex == 0)
                    {
                        txtFilter.Focus();
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        // ReSharper disable once InconsistentNaming
        private const int SW_RESTORE = 9;

        [DllImport("ntdll.dll", PreserveSig = false)]
        public static extern void NtSuspendProcess(IntPtr processHandle);

        [DllImport("ntdll.dll", PreserveSig = false, SetLastError = true)]
        public static extern void NtResumeProcess(IntPtr processHandle);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);
        // is used to set window in front
        [DllImport("User32.dll", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        private void ShowWindow(Process proc)
        {
            var handle = proc.MainWindowHandle;
            if (IsIconic(handle))
            {
                ShowWindow(handle, SW_RESTORE);
            }

            SwitchToThisWindow(handle, true);
        }

        private void lstView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = e.SuppressKeyPress = true;
                    ProcSelected();
                    return;
                }

                if (e.KeyCode == Keys.Delete)
                {
                    KillProc(sender, e);
                }
            }
            catch
            {
                // ignored
            }
        }

        private void ProcSelected()
        {
            if (lstView.SelectedItem == null)
            {
                return;
            }

            var proc = (ProcInfo) lstView.SelectedObject;
            if (proc.Proc.MainWindowHandle == IntPtr.Zero)
            {
                return;
            }

            SetForegroundWindow(proc.Proc.MainWindowHandle);
            ShowWindow(proc.Proc);
            WindowState = FormWindowState.Minimized;
        }

        private void lstView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ProcSelected();
            }
            catch
            {
                // ignored
            }
        }

        private void WinForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                _notifyIcon.Visible = true;
                Hide();
            }
            else if (FormWindowState.Normal == WindowState)
            {
                _notifyIcon.Visible = true;
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

        #region Constants

        static readonly int DWM_TNP_VISIBLE = 0x8;
        static readonly int DWM_TNP_OPACITY = 0x4;
        static readonly int DWM_TNP_RECTDESTINATION = 0x1;

        static readonly ulong WS_VISIBLE = 0x10000000L;
        static readonly ulong WS_BORDER = 0x00800000L;
        static readonly ulong TARGETWINDOW = WS_BORDER | WS_VISIBLE;

        #endregion

        private void lstView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetItemCount();

                if (lstView.SelectedObject == null)
                {
                    return;
                }

                var window = ((ProcInfo) lstView.SelectedObject).Proc.MainWindowHandle;
                if (window == IntPtr.Zero)
                {
                    return;
                }

                if (_thumb != IntPtr.Zero)
                {
                    DwmUnregisterThumbnail(_thumb);
                }

                var i = DwmRegisterThumbnail(Handle, window, out _thumb);

                if (i == 0)
                {
                    UpdateThumb();
                }
            }
            catch (Exception ex)
            {
                Log("Failed to select process", ex);
            }
        }

        private void Log(string log, Exception ex)
        {
            Log($"{log} - {ex.Message}", true);
        }

        private void Log(string log, bool error = false, Color? color = null)
        {
            try
            {
                lstLog.AddObject(new LogInfo
                {
                    Time = DateTime.Now.ToString("dd/MM/yy HH:mm:ss"),
                    Log = log,
                    Error = error,
                    Color = color
                });

                lstLog.AutoResizeColumns();
                lstLog.CalculateReasonableTileSize();
                lstLog.EnsureVisible(lstLog.Items.Count - 1);
            }
            catch
            {
                // ~ignored~
            }
        }

        private void WinForm_SizeChanged(object sender, EventArgs e)
        {
            lstView_SelectedIndexChanged(sender, e);
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Log("Refreshing..");

                _procs = null;
                Clear();

                await GetProcs();
            }
            catch (Exception ex)
            {
                Log("Failed to refresh", ex);
            }
        }

        public class ProcInfo
        {
            public int Id;
            public string Name;
            public string Title;
            public string StartTime;
            public string Args;

            public Process Proc;
        }

        public class LogInfo
        {
            public string Time;
            public string Log;
            public bool Error;
            public Color? Color;
        }

        private void Clear()
        {
            lstView.ClearObjects();
            SetItemCount();
        }

        private void chkAllProcs_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Clear();

                var procs = _procs;
               
                if (chkAllProcs.Checked)
                {
                    Log("Showing all processes");
                }
                else
                {
                    Log("Showing only processes with window");
                    procs = procs.Where(p => p.Proc.MainWindowHandle != IntPtr.Zero).ToArray();
                }
            
                lstView.SetObjects(procs);
                SetItemCount();
            }
            catch
            {
                // ignored
            }
        }

        private Point _lastHit;

        private void lstView_MouseUp(object sender, MouseEventArgs e)
        {
            _lastHit = e.Location;
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            metroStyleManager.Theme = metroStyleManager.Theme == MetroThemeStyle.Light ? MetroThemeStyle.Dark : MetroThemeStyle.Light;
        }

        private void WinForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _notifyIcon.Dispose();
        }

        private void picBox_SizeChanged(object sender, EventArgs e)
        {
            UpdateThumb();
        }
    }

    public static class ListExtensions
    {
        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
 