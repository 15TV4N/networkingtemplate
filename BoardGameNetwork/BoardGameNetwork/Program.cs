using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing;
using System.IO;

namespace BoardGameNetwork
{
    /// <summary>
    /// The program will run a 'console app' for the server and
    /// a 'chain' of forms for all the clients described below.
    /// TCP protocol is used to communicate through local ports.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //one server ONLY
            var serverMainThread = new Thread(() => new BGServerApp());
            serverMainThread.Priority = ThreadPriority.Highest;
            serverMainThread.Start();

            /* 
             * The points refer to points on screen
             * It is advised to have at least a screen resolution of 1024x768
             * (some forms' start location attributes are set to manual input
             * and will spawn based on these values)
             */
            (new Thread(() =>new BGClientApp(new Point(500,100)))).Start();
            (new Thread(() => new BGClientApp(new Point(200, 400)))).Start();
            (new Thread(() => new BGClientApp(new Point(500, 400)))).Start(); 
        }
    }
}
