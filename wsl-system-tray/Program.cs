using System.Diagnostics;
using wsl_system_tray.SystemTray;
using Microsoft.Extensions.Configuration;

namespace wsl_system_tray
{
    internal static class Program
    {
        static NotifyIcon _notifyIcon = new NotifyIcon();
        static bool _visible = true;
        static Process _process = new Process();
        static IConfiguration _config;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _config = builder.Build();

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
            _notifyIcon.Icon = new Icon(_config["icon"]!);
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
                FileName = _config["wslLocation"]!
            };
            _process.StartInfo = startInfo;
            _process.Start();
        }
    }
}