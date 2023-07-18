using Newtonsoft.Json;
using System;
using System.IO;
using TajpiSharp.Klasoj;

namespace TajpiSharp
{
    public class AgordoKontrolo
    {
        public AgordoKontrolo()
        {
            Ek();
        }

        string dosierindiko = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "/agordoj.json");
        public UzantAgordoj Ek()
        {
            if (!EkzistasAgordoj())
            {
                return Kreado();
            }
            else
            {
                return LegiAgordoj();
            }
        }

        public UzantAgordoj Kreado()
        {
            File.Create(dosierindiko).Close();

            UzantAgordoj agordoj = new UzantAgordoj()
            {
                Aktiva =  true,
                RektajKlavoj = new RektajKlavoj()
                {
                    UziRektajKlavoj = false,
                    Klavoj = new string[6]
                },
                Prefiksoj = new Prefiksoj()
                {
                    UziPrefiksoj = false,
                    Malvidebligi = false,
                    Prefiksaro = string.Empty
                },
                Sufiksoj = new Sufiksoj()
                {
                    UziSufiksoj = false,
                    RipetoForigas = false,
                    Sufiksaro = string.Empty
                },
                UziW = false,
                UziAltGr = false,
                UziAutoAuhEh = false,
                EnigoModo = 1,
                Alglui = false,
                HTMLSurogatoj = false,
                StartiAktiva = false,
                StartiAuto = false,
                KlavoKomandoj = new KlavoKomandoj()
                {
                    UziCtrl = false,
                    UziAlt = false,
                    UziShift = false,
                    Klavo = null
                }
            };

            string json = JsonConvert.SerializeObject(agordoj, Formatting.Indented);
            File.WriteAllText(dosierindiko, json);
            return agordoj;
        }

        public UzantAgordoj LegiAgordoj()
        {
            if (EkzistasAgordoj())
            {
                string json = File.ReadAllText(dosierindiko);
                return JsonConvert.DeserializeObject<UzantAgordoj>(json);
            }
            else return null;
            
        }

        public bool EkzistasAgordoj()
        {
            return File.Exists(dosierindiko);
        }

        public void ModifiAgordoj(string kampo, object valoro)
        {
            string json = File.ReadAllText(dosierindiko);
            UzantAgordoj agordoj = JsonConvert.DeserializeObject<UzantAgordoj>(json);

            var property = typeof(UzantAgordoj).GetProperty(kampo);
            if (property != null)
            {
                property.SetValue(agordoj, valoro);
            }
            else
            {
                throw new ArgumentException("Ne valida kampo");
            }

            string novaDosiero = JsonConvert.SerializeObject(agordoj, Formatting.Indented);
            File.WriteAllText(dosierindiko, novaDosiero);
        }

        public bool ModifiChiujnAgordojn(UzantAgordoj agordoj)
        {
            try
            {
                string novajAgordoj = JsonConvert.SerializeObject(agordoj, Formatting.Indented);
                File.WriteAllText(dosierindiko, novajAgordoj);

                return true;
            }
            catch (Exception ex)
            {
                string novajAgordoj = JsonConvert.SerializeObject(agordoj, Formatting.Indented);
                File.WriteAllText(dosierindiko, novajAgordoj);

                throw ex;
            }
        }

        public bool AkiriNunaAktiveco()
        {
            var agordo = LegiAgordoj();
            return agordo.Aktiva;
        }
    }
}
