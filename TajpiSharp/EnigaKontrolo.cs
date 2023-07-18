using System.Collections.Generic;
using System.Windows.Forms;

namespace TajpiSharp
{
    public class EnigaKontrolo
    {
        private static List<Keys> premitajKlavoj = new List<Keys>();
        private static List<Keys> agordKlavoj = new List<Keys>();
        public static AgordoKontrolo agordoKontrolo = new AgordoKontrolo();

        public EnigaKontrolo()
        {
            Ek();
        }

        private void Ek()
        {
            var agordoj = agordoKontrolo.LegiAgordoj();
            //agordKlavoj = agordoj.KlavoListo;
        }

        private void AkiriPremitajKlavoj(List<Keys> klavoj)
        {
            premitajKlavoj = klavoj;
        }
    }
}
