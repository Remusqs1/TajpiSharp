using System.Windows.Forms;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using TajpiSharp.Klasoj;
using System.Windows.Input;

namespace TajpiSharp
{

    public partial class TajpiSharpForm : Form
    {

        public static AgordoKontrolo AgordoKontrolo = new AgordoKontrolo();
        public UzantAgordoj Agordoj = null;
        private Dictionary<Keys, string> klavoMapigo;

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
            if (!Agordoj.RektajKlavoj.UziRektajKlavoj)
            {
                this.tekstoKadroCh.Enabled = false;
                this.tekstoKadroGh.Enabled = false;
                this.tekstoKadroHh.Enabled = false;
                this.tekstoKadroJh.Enabled = false;
                this.tekstoKadroSh.Enabled = false;
                this.tekstoKadroUh.Enabled = false;
            }

            if (!Agordoj.Prefiksoj.UziPrefiksoj)
            {
                this.prefiksojTekstoKadro.Enabled = false;
                this.malvidebligiElektoBtn.Enabled = false;
            }

            if (!Agordoj.Sufiksoj.UziSufiksoj)
            {
                this.sufiksojTekstoKadro.Enabled = false;
                this.ripetiElektoBtn.Enabled = false;
            }

            this.aktivaBtn.BackColor = Agordoj.Aktiva? Color.Green : Color.Red;
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
                { Keys.D0, "0" },
                { Keys.D1, "1" },
                { Keys.D2, "2" },
                { Keys.D3, "3" },
                { Keys.D4, "4" },
                { Keys.D5, "5" },
                { Keys.D6, "6" },
                { Keys.D7, "7" },
                { Keys.D8, "8" },
                { Keys.D9, "9" },
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
            bool ekzistasAgordoj = AgordoKontrolo.EkzistasAgordoj();

            if (ekzistasAgordoj)
            {
                Agordoj = AgordoKontrolo.LegiAgordoj();

                this.rektajKlavojElektoBtn.Checked = Agordoj.RektajKlavoj.UziRektajKlavoj;
                if (this.rektajKlavojElektoBtn.Checked)
                {
                    this.tekstoKadroCh.Text = Agordoj.RektajKlavoj.Klavoj[0];
                    this.tekstoKadroGh.Text = Agordoj.RektajKlavoj.Klavoj[1];
                    this.tekstoKadroHh.Text = Agordoj.RektajKlavoj.Klavoj[2];
                    this.tekstoKadroJh.Text = Agordoj.RektajKlavoj.Klavoj[3];
                    this.tekstoKadroSh.Text = Agordoj.RektajKlavoj.Klavoj[4];
                    this.tekstoKadroUh.Text = Agordoj.RektajKlavoj.Klavoj[5];
                }

                this.prefiksojElektoBtn.Checked = Agordoj.Prefiksoj.UziPrefiksoj;
                if (this.prefiksojElektoBtn.Checked)
                {
                    this.prefiksojTekstoKadro.Text = Agordoj.Prefiksoj.Prefiksaro;
                    this.malvidebligiElektoBtn.Checked = Agordoj.Prefiksoj.Malvidebligi;
                }

                this.sufiksoElektoBtn.Checked = Agordoj.Sufiksoj.UziSufiksoj;
                if (this.sufiksoElektoBtn.Checked)
                {
                    this.sufiksojTekstoKadro.Text = Agordoj.Sufiksoj.Sufiksaro;
                    this.ripetiElektoBtn.Checked = Agordoj.Sufiksoj.RipetoForigas;
                }

                this.AltGrSupersigniElektoBtn.Checked = Agordoj.UziAltGr;
                this.AutoAuhEuhElektoBtn.Checked = Agordoj.UziAutoAuhEh;
                this.wUhElektoButono.Checked = Agordoj.UziW;
                this.algluiElektoBtn.Checked = Agordoj.Alglui;
                this.htmlElektoBtn.Checked = Agordoj.HTMLSurogatoj;
                this.startiAktivaElektoBtn.Checked = Agordoj.StartiAktiva;
                this.startiAutoElektoBtn.Checked = Agordoj.StartiAuto;

                this.ctrlElektoBtn.Checked = Agordoj.KlavoKomandoj.UziCtrl;
                this.altElektoBtn.Checked = Agordoj.KlavoKomandoj.UziAlt;
                this.shiftElektoBtn.Checked = Agordoj.KlavoKomandoj.UziShift;
                this.klavaroListo.SelectedItem = Agordoj.KlavoKomandoj.Klavo;

                if (Agordoj.EnigoModo == 1) this.unikodoRadBtn.Checked = true;
                else this.latina3RadBtn.Checked = true;
            }
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

        private void akceptiBtn_Click(object sender, EventArgs e)
        {

            string[] klavoj = new string[6];
            klavoj.Append(this.tekstoKadroCh.Text);
            klavoj.Append(this.tekstoKadroGh.Text);
            klavoj.Append(this.tekstoKadroHh.Text);
            klavoj.Append(this.tekstoKadroJh.Text);
            klavoj.Append(this.tekstoKadroSh.Text);
            klavoj.Append(this.tekstoKadroUh.Text);

            UzantAgordoj agordoj = new UzantAgordoj() {

                RektajKlavoj = new RektajKlavoj()
                {
                    UziRektajKlavoj = this.rektajKlavojElektoBtn.Checked,
                    Klavoj = klavoj
                },
                Prefiksoj = new Prefiksoj()
                {
                    UziPrefiksoj = this.prefiksojElektoBtn.Checked,
                    Prefiksaro = this.prefiksojTekstoKadro.Text,
                    Malvidebligi = this.malvidebligiElektoBtn.Checked
                },
                Sufiksoj = new Sufiksoj()
                {
                    UziSufiksoj = this.sufiksoElektoBtn.Checked,
                    Sufiksaro = this.sufiksojTekstoKadro.Text,
                    RipetoForigas = this.ripetiElektoBtn.Checked
                },
                UziAltGr = this.AltGrSupersigniElektoBtn.Checked,
                UziAutoAuhEh = this.AutoAuhEuhElektoBtn.Checked,
                UziW = this.wUhElektoButono.Checked,
                EnigoModo = this.unikodoRadBtn.Checked ? 1 : 2,
                Alglui = this.algluiElektoBtn.Checked,
                HTMLSurogatoj = this.htmlElektoBtn.Checked,
                StartiAktiva = this.startiAktivaElektoBtn.Checked,
                StartiAuto = this.startiAutoElektoBtn.Checked,
                KlavoKomandoj = new KlavoKomandoj()
                {
                    UziAlt = this.altElektoBtn.Checked,
                    UziCtrl = this.ctrlElektoBtn.Checked,
                    UziShift = this.shiftElektoBtn.Checked,
                    Klavo = this.klavaroListo.SelectedItem != null ? this.klavaroListo.SelectedItem.ToString() : null,
                }
            };

            AgordoKontrolo.ModifiChiujnAgordojn(agordoj);
        }

        private void nuligiBtn_Click(object sender, EventArgs e)
        {
            this.GetUzantajValoroj();
        }

        private void Majuskligo(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.ToUpper();
            textBox.SelectionStart = textBox.Text.Length;
        }

        private void NurLiteroj(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.V && ModifierKeys == Keys.Control)
            {
                e.Handled = true;
            }

            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsPunctuation(e.KeyChar) &&
                e.KeyChar != '^' && e.KeyChar != '\'' && e.KeyChar != '"' &&
                e.KeyChar != '*' && e.KeyChar != '<' && e.KeyChar != '>')
            {
                e.Handled = true;
            }
        }
    }
}
