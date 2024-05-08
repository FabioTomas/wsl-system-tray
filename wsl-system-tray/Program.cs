using System.Diagnostics;
using wsl_system_tray.SystemTray;

namespace wsl_system_tray
{
    internal static class Program
    {
        static NotifyIcon _notifyIcon = new NotifyIcon();
        static bool _visible = true;
        static Process _process = new Process();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            StartSystemTray();

            StartWsl();

            Application.Run();

            _notifyIcon.Visible = false;            
        }

        public static void StartSystemTray()
        {
            _notifyIcon.DoubleClick += (s, e) =>
            {
                _visible = !_visible;
                SystemTrayHelper.SetConsoleWindowVisibility(_visible);
            };
            _notifyIcon.Icon = new Icon("Icon/app.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Wsl";

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Exit", null, (s, e) => { 
                _process.Kill();
                Application.Exit();
            });
            _notifyIcon.ContextMenuStrip = contextMenu;

            Console.WriteLine("Running!");
        }

        public static void StartWsl()
        {
            ProcessStartInfo startInfo = new()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = @"C:\Program Files\WindowsApps\MicrosoftCorporationII.WindowsSubsystemForLinux_2.1.5.0_x64__8wekyb3d8bbwe\wsl.exe"
            };
            _process.StartInfo = startInfo;
            _process.Start();
        }
    }
}