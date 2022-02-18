using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Tren_Rezervasyonu.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrenRezervasyonController : ControllerBase
    {
        private readonly ILogger<TrenRezervasyonController> _logger;

        public TrenRezervasyonController(ILogger<TrenRezervasyonController> logger)
        {
            _logger = logger;
        }

        [HttpPost("GetRezervasyonBilgileri")]
        public DonusGenel Get([FromBody] Rezervasyon model)
        {
            var islemYapilacakModel = model;
            List<VagonBilgisi> yerlesilebilirVagonListesi = new List<VagonBilgisi>();

            DonusGenel donusModel = new DonusGenel();

            foreach (var item in islemYapilacakModel.Tren.Vagonlar)
            {
                if (Convert.ToDouble((item.DoluKoltukAdet / item.Kapasite)) < 0.7)
                    yerlesilebilirVagonListesi.Add(item);
            }

            if(yerlesilebilirVagonListesi.Count == 0)
            {
                donusModel.RezervasyonYapilabilir = false;
                return donusModel;
            }

            if(islemYapilacakModel.KisilerFarkliVagonlaraYerlestirilebilir == false)
            {
                Random ran = new Random();

               var yerlestirilebilirVagon = yerlesilebilirVagonListesi[ran.Next(0, yerlesilebilirVagonListesi.Count)];

                DonusVagonBilgisi donVagBilgi = new DonusVagonBilgisi();

                donVagBilgi.VagonAdi = yerlestirilebilirVagon.Ad;
                donVagBilgi.KisiSayisi = islemYapilacakModel.RezervasyonYapilacakKisiSayisi;

                donusModel.YerlesimAyrinti = new List<DonusVagonBilgisi>();
                donusModel.RezervasyonYapilabilir = true;
                donusModel.YerlesimAyrinti.Add(donVagBilgi);
                return donusModel;
            }
            if(islemYapilacakModel.KisilerFarkliVagonlaraYerlestirilebilir == true)
            {
                Random ran = new Random();

                if (yerlesilebilirVagonListesi.Count > 1)
                {
                    donusModel.YerlesimAyrinti = new List<DonusVagonBilgisi>();
                    for (int i = 0; i < islemYapilacakModel.RezervasyonYapilacakKisiSayisi; i++)
                    {
                        var yerlestirilebilirVagon = yerlesilebilirVagonListesi[ran.Next(0, yerlesilebilirVagonListesi.Count)];

                        DonusVagonBilgisi donVagBilgi = new DonusVagonBilgisi();

                        if (donusModel.YerlesimAyrinti.Where(x => x.VagonAdi == yerlestirilebilirVagon.Ad).ToList().Count > 0)
                            donusModel.YerlesimAyrinti.Find(y => y.VagonAdi == yerlestirilebilirVagon.Ad).KisiSayisi += 1;
                        else
                        {
                            donVagBilgi.VagonAdi = yerlestirilebilirVagon.Ad;
                            donVagBilgi.KisiSayisi += 1;
                            donusModel.YerlesimAyrinti.Add(donVagBilgi);
                        }
                    }

                }
                donusModel.RezervasyonYapilabilir = true;
                return donusModel;
            }
            return donusModel;
        }
    }
}
