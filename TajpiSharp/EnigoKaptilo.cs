using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TajpiSharp.Klasoj;

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

        public static AgordoKontrolo AgordoKontrolo = new AgordoKontrolo();
        public static EnigaKontrolo EnigaKontrolo = new EnigaKontrolo();
        private static List<Keys> PremitajKlavoj = new List<Keys>();
        private static List<Keys> MalAktivajKlavoj = new List<Keys>();
        private static UzantAgordoj Agordoj = new UzantAgordoj();
        private static bool Aktiveco = false;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var param1 = (IntPtr)WM_KEYDOWN;
            var param2 = (IntPtr)WM_KEYUP;

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                if (!PremitajKlavoj.Contains(key))
                {
                    key = MapiKlavo(key);
                    if (PremitajKlavoj.All(x => x != key))
                    {
                        PremitajKlavoj.Add(key);
                    }
                }
            }
            
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;
                key = MapiKlavo(key);
                PremitajKlavoj.Remove(key);
            }

            if (MalAktivajKlavoj.Count == PremitajKlavoj.Count)
            {
                if (KontroliKlavoj(MalAktivajKlavoj))
                {
                    bool nunaAktiveco = AgordoKontrolo.AkiriNunaAktiveco();
                    AgordoKontrolo.ModifiAgordoj("Aktiva", !nunaAktiveco);
                    PremitajKlavoj = new List<Keys>();
                }
            }


            if (AgordoKontrolo.AkiriNunaAktiveco())
            {
                Console.WriteLine("TODO - Anstatauhi vortojn");
            }

            return CallNextHookEx(hookHandle, nCode, wParam, lParam);
        }

        

        [STAThread]
        public static void Main(string[] args)
        {
            keyboardHookProc = KeyboardHookCallback;
            Agordoj = AgordoKontrolo.LegiAgordoj();
            Aktiveco = Agordoj.Aktiva;
            MalAktivajKlavoj = AkiriMalaktivigajklavoj(Agordoj.KlavoKomandoj);

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                hookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, keyboardHookProc, GetModuleHandle(curModule.ModuleName), 0);
                Application.Run();
                UnhookWindowsHookEx(hookHandle);
            }
        }

        private static Keys MapiKlavo(Keys klavo)
        {
            if (klavo.Equals(Keys.LControlKey) || klavo.Equals(Keys.RControlKey) || klavo.Equals(Keys.ControlKey))
            {
                klavo = Keys.Control;
            }
            else if (klavo.Equals(Keys.RShiftKey) || klavo.Equals(Keys.LShiftKey) || klavo.Equals(Keys.ShiftKey))
            {
                klavo = Keys.Shift;
            }
            else if (klavo.Equals(Keys.LMenu) || klavo.Equals(Keys.RMenu))
            {
                klavo = Keys.Alt;
            }

            return klavo;
        }

        private static bool KontroliKlavoj(List<Keys> agordKlavoj)
        {
            bool valida = PremitajKlavoj.All(klavo => agordKlavoj.Contains(klavo));
            return valida;
        }

        private static List<Keys> AkiriMalaktivigajklavoj(KlavoKomandoj klavoKomandoj)
        {
            List<Keys> klavoListo = new List<Keys>();

            if (klavoKomandoj.UziCtrl) klavoListo.Add(Keys.Control);
            if (klavoKomandoj.UziAlt) klavoListo.Add(Keys.Alt);
            if (klavoKomandoj.UziShift) klavoListo.Add(Keys.Shift);

            if (!string.IsNullOrEmpty(klavoKomandoj.Klavo))
            {
                Keys miaKlavo = (Keys)Enum.Parse(typeof(Keys), klavoKomandoj.Klavo);
                klavoListo.Add(miaKlavo);
            }

            return klavoListo;
        }


    }
}
