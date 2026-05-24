using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    internal class RichTextBoxSemSnap : RichTextBox
    {
        private const int EM_GETOPTIONS = 0x044E;
        private const int EM_SETOPTIONS = 0x044D;
        private const int ECOOP_SET = 0x0001;
        private const int ECO_AUTOWORDSELECTION = 0x00000001;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            DisableNativeAutoWordSelection();
        }

        private void DisableNativeAutoWordSelection()
        {
            AutoWordSelection = false;

            int options = SendMessage(Handle, EM_GETOPTIONS, IntPtr.Zero, IntPtr.Zero).ToInt32();
            options &= ~ECO_AUTOWORDSELECTION;

            SendMessage(Handle, EM_SETOPTIONS, new IntPtr(ECOOP_SET), new IntPtr(options));
        }
    }
}
