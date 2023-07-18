using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TajpiSharp.Klasoj;

namespace TajpiSharp
{
    public class AgordoKontrolo
    {
        public AgordoKontrolo()
        {
            Ek();
        }

        string dosierindiko = "agordoj.json";
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
                Aktiva = true,
                KlavoListo = new List<Keys>()
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
    }
}
