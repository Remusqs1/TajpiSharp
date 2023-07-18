
namespace TajpiSharp.Klasoj
{
    public class UzantAgordoj
    {
        public bool Aktiva { get; set; }
        public RektajKlavoj RektajKlavoj { get; set; }
        public Prefiksoj Prefiksoj { get; set; }
        public Sufiksoj Sufiksoj { get; set; }
        public bool UziAltGr { get; set; }
        public bool UziAutoAuhEh { get; set; }
        public bool UziW { get; set; }
        public int EnigoModo { get; set; }
        public bool Alglui { get; set; }
        public bool HTMLSurogatoj { get; set; }
        public bool StartiAktiva { get; set; }
        public bool StartiAuto { get; set; }
        public KlavoKomandoj KlavoKomandoj { get; set; }
    }


    public class RektajKlavoj
    {
        public bool UziRektajKlavoj { get; set; }
        public string[] Klavoj { get; set; }
    }

    public class Prefiksoj
    {
        public bool UziPrefiksoj { get; set; }
        public string Prefiksaro { get; set; }
        public bool Malvidebligi { get; set; }
    }

    public class Sufiksoj
    {
        public bool UziSufiksoj { get; set; }
        public string Sufiksaro { get; set; }
        public bool RipetoForigas { get; set; }
    }

    public class KlavoKomandoj
    {
        public bool UziCtrl { get; set; }
        public bool UziAlt { get; set; }
        public bool UziShift { get; set; }
        public string Klavo { get; set; }
    }

}
