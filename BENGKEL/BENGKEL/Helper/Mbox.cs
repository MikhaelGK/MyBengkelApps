using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BENGKEL.Helper
{
    public static class Mbox
    {
        public static void Warning(string title, string msg)
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string title, string msg)
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Information(string title, string msg)
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Stop(string title, string msg)
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        public static void Exclamation(string title, string msg)
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
