using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace lab3_v4
{
    static class foxMain
    {
        [STAThread]
        static void Main()
        {
            GuidAttribute attribute = (GuidAttribute)typeof(foxMain).Assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            using (Mutex mutex = new Mutex(false, @"Global\" + attribute.Value))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("An instance of fox is already running");
                    return;
                }
                GC.Collect();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new fox());
            }
        }
    }
}
