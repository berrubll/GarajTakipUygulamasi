using System;
using System.Collections.Generic;

namespace AracBakimTakip
{
    class Program
    {
        static void Main(string[] args)
        {
            // Kullanıcı doğrulama
            if (KullaniciDogrulama())
            {
                Console.Clear();

                // Kullanıcı doğrulandıysa garaj yönetim sistemini başlat
                GarajYonetimSistemi garajYonetim = new GarajYonetimSistemi();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Garaj Yönetim Sistemi");
                    Console.WriteLine("1. Araç Ekle");
                    Console.WriteLine("2. Araç Bilgilerini Görüntüle");
                    Console.WriteLine("3. Bakım Zamanlarını Kontrol Et");
                    Console.WriteLine("4. Muayene Zamanlarını Kontol Et");
                    Console.WriteLine("5. Oturumu Kapat");
                    Console.WriteLine("6. Çıkış");
                    Console.Write("Seçiniz: ");
                    int secenek = Convert.ToInt32(Console.ReadLine());

                    switch (secenek)
                    {
                        case 1:
                            Console.Clear();
                            Console.Write("Araç Plakası Giriniz: ");
                            string aracPlaka = Console.ReadLine();
                            Console.Write("Araç Markasını Giriniz: ");
                            string aracMarka = Console.ReadLine();
                            Console.Write("Araç Rengini Giriniz: ");
                            string aracRenk = Console.ReadLine();
                            Console.Write("Bakım Tarihi Giriniz(dd.MM.yyyy): ");
                            DateTime bakimTarihi = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
                            Console.Write("Muayene Tarihi Giriniz(dd.MM.yyyy): ");
                            DateTime muayeneTarihi = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
                            garajYonetim.AracEkleme(aracPlaka, bakimTarihi, aracMarka, aracRenk, muayeneTarihi);
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("Bakım Kayıtları:");
                            Console.Clear();
                            foreach (var record in garajYonetim.BakimKayitlariKontrol())
                            {
                                Console.WriteLine($"{record.AracPlaka} plakalı {record.AracMarka} markalı {record.AracRenk} renkli aracın son bakım tarihi: {record.BakimTarihi.ToShortDateString()} son muayene tarihi: {record.MuayeneTarihi.ToShortDateString()}");
                            }
                            Console.WriteLine("Devam etmek için herhangi bir tuşa basınız.");
                            Console.ReadLine();
                            break;
                        case 3:
                            Console.Clear();
                            Console.Write("Araç Plakası: ");
                            string girilenPlaka = Console.ReadLine();
                            DateTime ilerdekiBakimTarihi = garajYonetim.BakimZamaniKontrolEt(girilenPlaka);

                            if (ilerdekiBakimTarihi == DateTime.MinValue)
                            {
                                Console.Clear();
                                Console.WriteLine("Araç bulunamadı veya bakım zamanı henüz gelmedi.");
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine($"{girilenPlaka} plakalı aracın bir sonraki bakım zamanı gelmiştir, lütfen bakıma götürün.");
                                Console.ReadLine();
                            }
                            break;
                        case 4:
                            Console.Clear();
                            Console.Write("Araç Plakası: ");
                            string girilenPlakaM = Console.ReadLine();
                            DateTime ilerdekiMuayeneTarihi = garajYonetim.MuayeneZamaniKontrolEt(girilenPlakaM);

                            if (ilerdekiMuayeneTarihi == DateTime.MinValue)
                            {
                                Console.Clear();
                                Console.WriteLine("Araç bulunamadı veya muayene zamanı henüz gelmedi.");
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine($"{girilenPlakaM} plakalı aracın bir sonraki muayene zamanı gelmiştir, lütfen araç muayenisi yaptırınız.");
                                Console.ReadLine();
                            }
                            break;
                        case 5:
                            KullaniciDogrulama();
                           
                            break;
                        case 6:
                            Console.WriteLine("Çıkmak istediğinize emin misiniz (e/h)");
                            string cikis = Console.ReadLine();
                            switch (cikis)
                            {
                                case "e":
                                    Environment.Exit(0);
                                    break;
                                case "h":
                                    Console.WriteLine("Ana menüye yönlendiriliyorsunuz.");
                                    break;
                                default:
                                    Console.WriteLine("Lütfen Evet(e) veya Hayır(h) seçeneklerinden biriniz seçiniz.");
                                    break;
                            }
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Kullanıcı doğrulama başarısız. Program sonlandırılıyor.");
            }
        }

        // Kullanıcı doğrulama fonksiyonu
        static bool KullaniciDogrulama()
        {
            Console.Write("Kullanıcı Adı: ");
            string kullaniciAdi = Console.ReadLine();

            Console.Write("Şifre: ");
            string sifre = Console.ReadLine();

            // Kullanıcı adı ve şifre doğrulama
            return (kullaniciAdi == "admin" && sifre == "*****");
        }
    }

    class AracKayit
    {
        public string AracPlaka { get; set; }
        public string AracMarka { get; set; }
        public string AracRenk { get; set; }
        public DateTime BakimTarihi { get; set; }
        public DateTime MuayeneTarihi { get; set; }
    }

    class GarajYonetimSistemi
    {
        private List<AracKayit> aracKayitListesi = new List<AracKayit>();

        public void AracEkleme(string aracPlaka, DateTime bakimTarihi, string aracMarka, string aracRenk, DateTime muayeneTarihi)
        {
            AracKayit record = new AracKayit
            {
                AracPlaka = aracPlaka,
                BakimTarihi = bakimTarihi,
                AracMarka = aracMarka,
                AracRenk = aracRenk,
                MuayeneTarihi = muayeneTarihi
            };

            AracKayit kontrol = aracKayitListesi.Find(r => r.AracPlaka.Equals(aracPlaka, StringComparison.OrdinalIgnoreCase));

            if (kontrol == null)
            {
                aracKayitListesi.Add(record);
                Console.Clear();
                Console.WriteLine($"{aracPlaka} Plakalı Aracınız Eklenmiştir.");
                Console.WriteLine("Devam etmek için herhangi bir tuşa basınız.");
                Console.ReadLine();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"{aracPlaka} Plakalı Araç Zaten Sistemde Mevcut.");
                Console.WriteLine("Devam etmek için herhangi bir tuşa basınız.");
                Console.ReadLine();
            }
        }

        public List<AracKayit> BakimKayitlariKontrol()
        {
            return aracKayitListesi;
        }

        public DateTime BakimZamaniKontrolEt(string aracPlaka)
        {
            AracKayit record = aracKayitListesi.Find(r => r.AracPlaka.Equals(aracPlaka, StringComparison.OrdinalIgnoreCase));

            if (record != null)
            {
                if (DateTime.Now >= record.BakimTarihi)
                {
                    Console.WriteLine($"{aracPlaka} Plakalı aracınızın bakım zamanı gelmiştir.");
                    return record.BakimTarihi.AddMonths(12);
                }
                else
                {
                    Console.WriteLine($"{aracPlaka} Plakalı aracınızın bakım zamanı gelmemiştir.");
                    return DateTime.MinValue;
                }
            }
            else
            {
                Console.WriteLine("Araç bulunamadı.");
                return DateTime.MinValue;
            }
        }

        public DateTime MuayeneZamaniKontrolEt(string aracPlaka)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            AracKayit record = aracKayitListesi.Find(r => r.AracPlaka.Equals(aracPlaka, StringComparison.OrdinalIgnoreCase));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (record != null)
            {
                if (DateTime.Now >= record.BakimTarihi)
                {
                    Console.WriteLine($"{aracPlaka} Plakalı aracınızın muayene zamanı gelmiştir.");
                    return record.BakimTarihi;
                }
                else
                {
                    Console.WriteLine($"{aracPlaka} Plakalı aracınızın bakım zamanı gelmemiştir.");
                    return DateTime.MinValue;
                }
            }
            else
            {
                Console.WriteLine("Araç bulunamadı.");
                return DateTime.MinValue;
            }
        }
    }
}