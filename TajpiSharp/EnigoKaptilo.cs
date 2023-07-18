using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TajpiSharp
{
    public static class EnigoKaptilo
    {
        #region eksteraj libraroj kaj importadoj

        private static IntPtr hookHandle = IntPtr.Zero;
        private static LowLevelKeyboardProc keyboardHookProc;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion

        public static AgordoKontrolo agordoKontrolo = new AgordoKontrolo();
        public static EnigaKontrolo enigaKontrolo = new EnigaKontrolo();
        private static List<Keys> premitajKlavoj = new List<Keys>();
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {

            var agordoj = agordoKontrolo.LegiAgordoj();


            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                if (!premitajKlavoj.Contains(key))
                {
                    premitajKlavoj.Add(key);
                }

            }
            
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;
                premitajKlavoj.Remove(key);
            }



            if (agordoj.KlavoListo.Count == premitajKlavoj.Count && KontroliKlavoj(agordoj.KlavoListo))
            {
                agordoKontrolo.ModifiAgordoj("Aktiva", !agordoj.Aktiva);
            }

            if (agordoj.Aktiva)
            {
                //TODO
            }

            return CallNextHookEx(hookHandle, nCode, wParam, lParam);
        }

        

        [STAThread]
        public static void Main(string[] args)
        {
            keyboardHookProc = KeyboardHookCallback;

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                hookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, keyboardHookProc, GetModuleHandle(curModule.ModuleName), 0);
                Application.Run();
                UnhookWindowsHookEx(hookHandle);
            }
        }

        private static bool KontroliKlavoj(List<Keys> agordKlavoj)
        {
            agordoKontrolo.ModifiAgordoj("Premitaj", premitajKlavoj);
            return premitajKlavoj.All(klavo => agordKlavoj.Contains(klavo));
        }


    }
}
