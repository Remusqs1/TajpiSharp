using System.Windows.Forms;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace TajpiSharp
{

    public partial class TajpiSharpForm : Form
    {
        int? klavoListoValoro = 0;
        private Dictionary<Keys, string> klavoMapigo;
        bool uziRektajKlavoj = false;
        bool uziPrefiksoj = false;
        bool uziIgiMalvidebla = false;
        bool uziSufiksoj = false;
        bool uziRipeto = false;
        bool uziAltGr = false;
        bool uziAutoAuh = false;
        bool uziW = false;
        public static bool estasAktiva = true;
        public static AgordoKontrolo agordoKontrolo = new AgordoKontrolo();


        List<Keys> klavKomandaro = new List<Keys>(); //savitaj klavoj
        private List<Keys> premitajKlavoj = new List<Keys>();

        public TajpiSharpForm()
        {
            InitializeComponent();
            EkKlavoMapigo();
            GetKomandoKlavoj();
            GetUzantajValoroj();
            EkiForm();
        }

        private void EkiForm()
        {
            this.akceptiBtn.Enabled = false;

            if (!uziRektajKlavoj)
            {
                this.tekstoKadroCh.Enabled = false;
                this.tekstoKadroGh.Enabled = false;
                this.tekstoKadroHh.Enabled = false;
                this.tekstoKadroJh.Enabled = false;
                this.tekstoKadroSh.Enabled = false;
                this.tekstoKadroUh.Enabled = false;
            }

            if (!uziPrefiksoj)
            {
                this.prefiksojTekstoKadro.Enabled = false;
                this.malvidebligiElektoBtn.Enabled = false;
            }

            if (!uziSufiksoj)
            {
                this.sufiksojTekstoKadro.Enabled = false;
                this.ripetiElektoBtn.Enabled = false;
            }

            this.aktivaBtn.BackColor = estasAktiva ? Color.Green : Color.Red;
        }

        private void EkKlavoMapigo()
        {
            klavoMapigo = new Dictionary<Keys, string>
            {
                { Keys.Add, "+" },
                { Keys.Subtract, "-" },
                { Keys.Multiply, "*" },
                { Keys.Divide, "/" },
                { Keys.Decimal, "." },
                { Keys.Separator, "," },
            };
        }

        private void GetKomandoKlavoj()
        {
            var klavoj = Enum.GetValues(typeof(Keys))
                                   .Cast<Keys>()
                                   .Where(key =>
                                       (key >= Keys.A && key <= Keys.Z) ||
                                       (key >= Keys.D0 && key <= Keys.D9) ||
                                       key == Keys.Space ||
                                       key == Keys.Tab ||
                                       key == Keys.Enter ||
                                       (key >= Keys.Add && key <= Keys.Divide))
                                   .ToList();


            var mapitajKlavoj = klavoj.Select(key => klavoMapigo.ContainsKey(key) ? klavoMapigo[key] : key.ToString()).ToList();

            klavaroListo.DataSource = mapitajKlavoj;
        }

        private void GetUzantajValoroj()
        {
            if (klavoListoValoro == 0 || klavoListoValoro == null) this.klavaroListo.SelectedItem = Keys.Space;

            //TODO
            klavKomandaro.Append(Keys.Control);
            klavKomandaro.Append(Keys.Alt);
            klavKomandaro.Append(Keys.Space);
        }

        private void rektajKlavojElektoBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.tekstoKadroCh.Enabled = this.rektajKlavojElektoBtn.Checked;
            this.tekstoKadroGh.Enabled = this.rektajKlavojElektoBtn.Checked;
            this.tekstoKadroHh.Enabled = this.rektajKlavojElektoBtn.Checked;
            this.tekstoKadroJh.Enabled = this.rektajKlavojElektoBtn.Checked;
            this.tekstoKadroSh.Enabled = this.rektajKlavojElektoBtn.Checked;
            this.tekstoKadroUh.Enabled = this.rektajKlavojElektoBtn.Checked;
        }

        private void prefiksojElektoBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.prefiksojTekstoKadro.Enabled = this.prefiksojElektoBtn.Checked;
            this.malvidebligiElektoBtn.Enabled = this.prefiksojElektoBtn.Checked;

        }

        private void sufiksoElektoBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.sufiksojTekstoKadro.Enabled = this.sufiksoElektoBtn.Checked;
            this.ripetiElektoBtn.Enabled = this.sufiksoElektoBtn.Checked;
        }

        private void TajpiSharpForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!premitajKlavoj.Contains(e.KeyCode)) premitajKlavoj.Add(e.KeyCode);

            if (ChiujPremitaj(premitajKlavoj)) this.ShanghiAktiveco();

            if (estasAktiva)
            {

            }
        }

        private bool ChiujPremitaj(List<Keys> klavoj)
        {
            if (klavoj.Count != klavKomandaro.Count)
            {
                return false;
            }

            foreach (Keys klavo in klavoj)
            {
                if (!klavKomandaro.Contains(klavo))
                {
                    return false;

                }
            }

            return true;
        }

        private void ShanghiAktiveco()
        {
            estasAktiva = !estasAktiva;
        }
    }
}
