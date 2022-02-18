namespace Tren_Rezervasyonu
{
    public class Rezervasyon
    {
        public TrenBilgisi Tren { get; set; }

        public int RezervasyonYapilacakKisiSayisi { get; set; }

        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }

    }
}