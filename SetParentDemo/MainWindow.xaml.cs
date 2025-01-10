using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SetParentDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using var windowsIdentity = WindowsIdentity.GetCurrent();
            var isAdmin = new WindowsPrincipal(windowsIdentity).IsInRole(WindowsBuiltInRole.Administrator);
            CurrentRole.IsChecked = isAdmin;
            CurrentHandleTextBox.Text = new WindowInteropHelper(this).Handle.ToString("X");
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        const int GWL_EXSTYLE = -20;
        const int WS_EX_TOOLWINDOW = 0x00000080; // 不在任务栏显示

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOACTIVATE = 0x0010;
        const uint SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        /// <summary>窗体属性</summary>
        [Flags]
        public enum WindowStyles : uint
        {
            WS_OVERLAPPED = 0,
            WS_POPUP = 2147483648, // 0x80000000
            WS_CHILD = 1073741824, // 0x40000000
            WS_MINIMIZE = 536870912, // 0x20000000
            WS_VISIBLE = 268435456, // 0x10000000
            WS_DISABLED = 134217728, // 0x08000000
            WS_CLIPSIBLINGS = 67108864, // 0x04000000
            WS_CLIPCHILDREN = 33554432, // 0x02000000
            WS_MAXIMIZE = 16777216, // 0x01000000
            WS_CAPTION = 12582912, // 0x00C00000
            WS_BORDER = 8388608, // 0x00800000
            WS_DLGFRAME = 4194304, // 0x00400000
            WS_VSCROLL = 2097152, // 0x00200000
            WS_HSCROLL = 1048576, // 0x00100000
            WS_SYSMENU = 524288, // 0x00080000
            WS_THICKFRAME = 262144, // 0x00040000
            WS_GROUP = 131072, // 0x00020000
            WS_TABSTOP = 65536, // 0x00010000
            WS_MINIMIZEBOX = WS_GROUP, // 0x00020000
            WS_MAXIMIZEBOX = WS_TABSTOP, // 0x00010000
            WS_TILED = 0,
            WS_ICONIC = WS_MINIMIZE, // 0x20000000
            WS_SIZEBOX = WS_THICKFRAME, // 0x00040000
            WS_TILEDWINDOW = WS_SIZEBOX | WS_MAXIMIZEBOX | WS_MINIMIZEBOX | WS_SYSMENU | WS_DLGFRAME | WS_BORDER, // 0x00CF0000
            WS_OVERLAPPEDWINDOW = WS_TILEDWINDOW, // 0x00CF0000
            WS_POPUPWINDOW = WS_SYSMENU | WS_BORDER | WS_POPUP, // 0x80880000
            WS_CHILDWINDOW = WS_CHILD, // 0x40000000
        }

        private void AddChildWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var inputInt32 = Convert.ToInt32(InputTextBox.Text, 16);
            IntPtr inputPtr = (IntPtr)inputInt32;
            var helper = new WindowInteropHelper(this);
            var currentInPtr = helper.Handle;

            //设置当前窗口。有UIACCESS的窗口，要去除WS_CHILD
            //int exStyle = GetWindowLong(currentInPtr, GWL_EXSTYLE);
            //exStyle &= ~(int)WindowStyles.WS_CHILD;
            //SetWindowLong(currentInPtr, GWL_EXSTYLE, exStyle);

            var aaa = SetParent(inputPtr, currentInPtr);
            MessageBox.Show($"SetParent Result:{aaa}");
        }

        private void AddParentWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var inputInt32 = Convert.ToInt32(InputTextBox.Text, 16);
            IntPtr inputPtr = (IntPtr)inputInt32;
            var helper = new WindowInteropHelper(this);
            var currentInPtr = helper.Handle;

            //int exStyle = GetWindowLong(currentInPtr, GWL_EXSTYLE);
            //exStyle &= ~(int)WindowStyles.WS_CHILD;
            //SetWindowLong(currentInPtr, GWL_EXSTYLE, exStyle);

            var aaa = SetParent(currentInPtr, inputPtr);
            MessageBox.Show($"SetParent Result:{aaa}");
        }
    }
}