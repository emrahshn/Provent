using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using Core.BileşenModeli;
using Core.Eklentiler;

[assembly: PreApplicationStartMethod(typeof(EklentiYönetici), "Initialize")]
namespace Core.Eklentiler
{
    public class EklentiYönetici
    {
        #region Const

        private const string KuruluEklentilerYolu = "~/App_Data/YüklüEklentiler.txt";
        private const string EklentiYolu = "~/Eklentiler";
        private const string GölgeKopyaYolu = "~/Eklentiler/bin";

        #endregion

        #region Fields

        private static readonly ReaderWriterLockSlim Kilitleyici = new ReaderWriterLockSlim();
        private static DirectoryInfo _gölgeKopyaKlasörü;
        private static bool _başlangıçtaGölgeKopyaKlasörünüTemizle;

        #endregion

        #region Methods
        public static IEnumerable<EklentiTanımlayıcı> ReferenslıEklentiler { get; set; }
        public static IEnumerable<string> UyumsuzEklentiler { get; set; }
        public static void Initialize()
        {
            using (new TekKullanımlıkKilit(Kilitleyici))
            {
                var eklentiKlasörü = new DirectoryInfo(GenelYardımcı.MapPath(EklentiYolu));
                _gölgeKopyaKlasörü = new DirectoryInfo(GenelYardımcı.MapPath(GölgeKopyaYolu));

                var referenslıEklentiler = new List<EklentiTanımlayıcı>();
                var uyumsuzEklentiler = new List<string>();

                _başlangıçtaGölgeKopyaKlasörünüTemizle = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["BaşlangıçtaGölgeKopyaKlasörünüTemizle"]) &&
                   Convert.ToBoolean(ConfigurationManager.AppSettings["BaşlangıçtaGölgeKopyaKlasörünüTemizle"]);

                try
                {
                    var kuruluEklentiSistemAdları = EklentiDosyasıAyrıştırıcı.KuruluEklentiDosyalarınıAyrıştır(KuruluEklentilerDosyaYolunuAl());

                    Debug.WriteLine("Gölge kopya klasörü oluşturma ve dll sorgulama");
                    //klasörlerin oluşturulduğunu doğrula
                    Directory.CreateDirectory(eklentiKlasörü.FullName);
                    Directory.CreateDirectory(_gölgeKopyaKlasörü.FullName);

                    //bin klasöründeki tüm dosyaların listesini al
                    var binDosyaları = _gölgeKopyaKlasörü.GetFiles("*", SearchOption.AllDirectories);
                    if (_başlangıçtaGölgeKopyaKlasörünüTemizle)
                    {
                        //Gölge kopyalanan eklentileri temizle
                        foreach (var f in binDosyaları)
                        {
                            Debug.WriteLine("Siliniyor " + f.Name);
                            try
                            {
                                File.Delete(f.FullName);
                            }
                            catch (Exception exc)
                            {
                                Debug.WriteLine("Silinirken hata oluştu " + f.Name + ". Hata: " + exc);
                            }
                        }
                    }

                    //açıklama dosyalarını kur
                    foreach (var dfd in AçıklamaDosyalarıveTanımlayıcılarıAl(eklentiKlasörü))
                    {
                        var açıklamaDosyası = dfd.Key;
                        var eklentiTanımlayıcı = dfd.Value;

                        //Eklenti sürümünün geçerli olduğundan emin olun
                        if (!eklentiTanımlayıcı.DesteklenenSürümler.Contains(TSSürüm.MevcutSürüm, StringComparer.InvariantCultureIgnoreCase))
                        {
                            uyumsuzEklentiler.Add(eklentiTanımlayıcı.SistemAdı);
                            continue;
                        }

                        //doğrulamalar
                        if (String.IsNullOrWhiteSpace(eklentiTanımlayıcı.SistemAdı))
                            throw new Exception(string.Format("'{0}' eklentisinin sistem adı yoktur . Eklentiye benzersiz bir ad atamayı ve yeniden derlemeyi deneyin.", açıklamaDosyası.FullName));
                        if (referenslıEklentiler.Contains(eklentiTanımlayıcı))
                            throw new Exception(string.Format("'{0}' eklenti adı daha önce kullanılmıştır", eklentiTanımlayıcı.SistemAdı));

                        //Kuruldu olarak ayarla
                        eklentiTanımlayıcı.Kuruldu = kuruluEklentiSistemAdları
                            .FirstOrDefault(x => x.Equals(eklentiTanımlayıcı.SistemAdı, StringComparison.InvariantCultureIgnoreCase)) != null;

                        try
                        {
                            if (açıklamaDosyası.Directory == null)
                                throw new Exception(string.Format("'{0}' açıklama dosyası için dizin çözümlenemiyor", açıklamaDosyası.Name));
                            //Eklentilerdeki tüm DLL'lerin listesini alın (bin klasörü içindekiler değil!)
                            var eklentiDosyaları = açıklamaDosyası.Directory.GetFiles("*.dll", SearchOption.AllDirectories)
                                //gölge kopyalanan eklentileri kaydettirmediğinden emin olun
                                .Where(x => !binDosyaları.Select(q => q.FullName).Contains(x.FullName))
                                .Where(x => PaketEklentiDosyası(x.Directory))
                                .ToList();

                            //Diğer eklenti açıklama bilgileri
                            var anaEklentiDosyası = eklentiDosyaları
                                .FirstOrDefault(x => x.Name.Equals(eklentiTanımlayıcı.EklentiDosyaAdı, StringComparison.InvariantCultureIgnoreCase));
                            eklentiTanımlayıcı.OrijinalAssemblyDosyası = anaEklentiDosyası;

                            //Gölge kopya ana eklenti dosyası
                            eklentiTanımlayıcı.ReferanslıAssembly = DosyaÇözücü(anaEklentiDosyası);

                            //Başvurulan tüm derlemeleri şimdi kur
                            foreach (var eklenti in eklentiDosyaları
                                .Where(x => !x.Name.Equals(anaEklentiDosyası.Name, StringComparison.InvariantCultureIgnoreCase))
                                .Where(x => !ZatenYüklendi(x)))
                                DosyaÇözücü(eklenti);

                            //Eklenti türünü başlat (assembly başına yalnızca bir eklentiye izin verilir)
                            foreach (var t in eklentiTanımlayıcı.ReferanslıAssembly.GetTypes())
                                if (typeof(IEklenti).IsAssignableFrom(t))
                                    if (!t.IsInterface)
                                        if (t.IsClass && !t.IsAbstract)
                                        {
                                            eklentiTanımlayıcı.EklentiTipi = t;
                                            break;
                                        }

                            referenslıEklentiler.Add(eklentiTanımlayıcı);
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            //Bir eklenti adı ekleyin. Bu şekilde problemli bir eklenti kolayca tespit edebiliriz
                            var msg = string.Format("Eklenti '{0}'. ", eklentiTanımlayıcı.KısaAd);
                            foreach (var e in ex.LoaderExceptions)
                                msg += e.Message + Environment.NewLine;

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                        catch (Exception ex)
                        {
                            //Bir eklenti adı ekleyin. Bu şekilde problemli bir eklenti kolayca tespit edebiliriz
                            var msg = string.Format("Eklenti '{0}'. {1}", eklentiTanımlayıcı.KısaAd, ex.Message);

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    for (var e = ex; e != null; e = e.InnerException)
                        msg += e.Message + Environment.NewLine;

                    var fail = new Exception(msg, ex);
                    throw fail;
                }


                ReferenslıEklentiler = referenslıEklentiler;
                UyumsuzEklentiler = uyumsuzEklentiler;

            }
        }
        public static void EklentileriKurulduOlarakİşaretle(string sistemAdı)
        {
            if (String.IsNullOrEmpty(sistemAdı))
                throw new ArgumentNullException("sistemAdı");

            var doysaYolu = GenelYardımcı.MapPath(KuruluEklentilerYolu);
            if (!File.Exists(doysaYolu))
                using (File.Create(doysaYolu))
                {
                    //Dosyayı oluşturduktan sonra kapatmak için 'using' özelliğini kullanıyoruz
                }


            var kuruluEklentiSistemAdları = EklentiDosyasıAyrıştırıcı.KuruluEklentiDosyalarınıAyrıştır(KuruluEklentilerDosyaYolunuAl());
            bool zatenKuruluOlarakİşaretlendi = kuruluEklentiSistemAdları
                                .FirstOrDefault(x => x.Equals(sistemAdı, StringComparison.InvariantCultureIgnoreCase)) != null;
            if (!zatenKuruluOlarakİşaretlendi)
                kuruluEklentiSistemAdları.Add(sistemAdı);
            EklentiDosyasıAyrıştırıcı.KuruluEklentiDosyasınıKaydet(kuruluEklentiSistemAdları, doysaYolu);
        }
        public static void EklentileriKaldırıldıOlarakİşaretle(string sistemAdı)
        {
            if (String.IsNullOrEmpty(sistemAdı))
                throw new ArgumentNullException("sistemAdı");

            var dosyaYolu = GenelYardımcı.MapPath(KuruluEklentilerYolu);
            if (!File.Exists(dosyaYolu))
                using (File.Create(dosyaYolu))
                {
                    //Dosyayı oluşturduktan sonra kapatmak için 'using' özelliğini kullanıyoruz
                }


            var kuruluEklentiSistemAdları = EklentiDosyasıAyrıştırıcı.KuruluEklentiDosyalarınıAyrıştır(KuruluEklentilerDosyaYolunuAl());
            bool zatenKuruluOlarakİşaretlendi = kuruluEklentiSistemAdları
                                .FirstOrDefault(x => x.Equals(sistemAdı, StringComparison.InvariantCultureIgnoreCase)) != null;
            if (zatenKuruluOlarakİşaretlendi)
                kuruluEklentiSistemAdları.Remove(sistemAdı);
            EklentiDosyasıAyrıştırıcı.KuruluEklentiDosyasınıKaydet(kuruluEklentiSistemAdları, dosyaYolu);
        }
        public static void EklentilerinTümüKaldırıldıOlarakİşaretle()
        {
            var dosyaYolu = GenelYardımcı.MapPath(KuruluEklentilerYolu);
            if (File.Exists(dosyaYolu))
                File.Delete(dosyaYolu);
        }

        #endregion

        #region Utilities
        private static IEnumerable<KeyValuePair<FileInfo, EklentiTanımlayıcı>> AçıklamaDosyalarıveTanımlayıcılarıAl(DirectoryInfo eklentiKlasörü)
        {
            if (eklentiKlasörü == null)
                throw new ArgumentNullException("eklentiKlasörü");

            //Liste oluştur (<dosya bilgisi, ayrıştırılmış eklenti tanımlayıcı>)
            var sonuç = new List<KeyValuePair<FileInfo, EklentiTanımlayıcı>>();
            //Listeye görüntülenme sırası ve  yol ekle
            foreach (var açıklamaDosyası in eklentiKlasörü.GetFiles("Açıklama.txt", SearchOption.AllDirectories))
            {
                if (!PaketEklentiDosyası(açıklamaDosyası.Directory))
                    continue;

                //dosyayı ayrıştır
                var eklentiTanımlayıcı = EklentiDosyasıAyrıştırıcı.EklentiAçıklamaDosyasıAyrıştırıcı(açıklamaDosyası.FullName);

                //Listeyi doldur
                sonuç.Add(new KeyValuePair<FileInfo, EklentiTanımlayıcı>(açıklamaDosyası, eklentiTanımlayıcı));
            }

            //Görüntüleme sırasına göre listeyi sıralayın. NOT: En düşük DisplayOrder ilk olacaktır, yani 0, 1, 1, 1, 5, 10
            sonuç.Sort((firstPair, nextPair) => firstPair.Value.GörüntülemeSırası.CompareTo(nextPair.Value.GörüntülemeSırası));
            return sonuç;
        }
        private static bool ZatenYüklendi(FileInfo dosyaBilgisi)
        {
            try
            {
                string uzantısızDosyaAdı = Path.GetFileNameWithoutExtension(dosyaBilgisi.FullName);
                if (uzantısızDosyaAdı == null)
                    throw new Exception(string.Format("{0} Dosya uzantısı bulunamadı.", dosyaBilgisi.Name));
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    string assemblyAdı = a.FullName.Split(new[] { ',' }).FirstOrDefault();
                    if (uzantısızDosyaAdı.Equals(assemblyAdı, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Bir derlemenin yüklü olup olmadığını doğrulanamadı. " + exc);
            }
            return false;
        }
        private static Assembly DosyaÇözücü(FileInfo dosya)
        {
            if (dosya.Directory == null || dosya.Directory.Parent == null)
                throw new InvalidOperationException(dosya.Name+" dosyası için eklenti dizini, izin verilen uygulama klasör hiyerarşisinin dışındaki bir klasördedir.");

            FileInfo gölgeKopyaDosyası;

            if (GenelYardımcı.GüvenSeviyesiAl() != AspNetHostingPermissionLevel.Unrestricted)
            {
                //Tüm eklentilerin ~/Eklentiler/bin/ dizinine kopyalanması gereklidir.
                //Bunların hepsi web.config'de statik olarak probingPaths'in ayarlanmasına bağlıdır

                //Orta güvenilirlikte çalışıyorken, özel bin klasörüne kopyalanır
                var gölgeKopyaDosyaKlasörü = Directory.CreateDirectory(_gölgeKopyaKlasörü.FullName);
                gölgeKopyaDosyası = OrtaGüveniBaşlat(dosya, gölgeKopyaDosyaKlasörü);
            }
            else
            {
                var klasör = AppDomain.CurrentDomain.DynamicDirectory;
                Debug.WriteLine(dosya.FullName + " to " + klasör);
                //Tam güven içerisinde çalışıyorken, standart dinamik klasöre kopyalanır
                gölgeKopyaDosyası = TamGüveniBaşlat(dosya, new DirectoryInfo(klasör));
            }

            //Şimdi eklenti tanımını kaydedebiliriz
            var gölgeKopyaAssembly = Assembly.Load(AssemblyName.GetAssemblyName(gölgeKopyaDosyası.FullName));

            //BuildManager'e referans ekleniyor
            Debug.WriteLine("BuildManager'e ekleniyor: '{0}'", gölgeKopyaAssembly.FullName);
            BuildManager.AddReferencedAssembly(gölgeKopyaAssembly);

            return gölgeKopyaAssembly;
        }
        private static FileInfo TamGüveniBaşlat(FileInfo dosya, DirectoryInfo gölgeKopyaDosyaKlasörü)
        {
            var gölgeKopyaDosyası = new FileInfo(Path.Combine(gölgeKopyaDosyaKlasörü.FullName, dosya.Name));
            try
            {
                File.Copy(dosya.FullName, gölgeKopyaDosyası.FullName, true);
            }
            catch (IOException)
            {
                Debug.WriteLine(gölgeKopyaDosyası.FullName + " kilitlendi, yeniden adlandırmaya çalışılıyor");
                //Bu dosyalar kilitlendiğinde oluşur,
                //Nedense devenv eklenti dosyalarını bir kaç kez kilitler ve başka bir sebeple bunları yeniden adlandırmanıza izin verilir
                //Yeniden gölge kopyası oluşturularak bu sorun çözülür.
                try
                {
                    var eskiDosya = gölgeKopyaDosyası.FullName + Guid.NewGuid().ToString("N") + ".old";
                    File.Move(gölgeKopyaDosyası.FullName, eskiDosya);
                }
                catch (IOException exc)
                {
                    throw new IOException(gölgeKopyaDosyası.FullName + " yeniden adlandırma başarısız oldu, eklenti başlatılamıyor.", exc);
                }
                //Şimdi gölge kopyayı tekrar deneyelim
                File.Copy(dosya.FullName, gölgeKopyaDosyası.FullName, true);
            }
            return gölgeKopyaDosyası;
        }
        private static FileInfo OrtaGüveniBaşlat(FileInfo dosya, DirectoryInfo gölgeKopyaDosyaKlasörü)
        {
            var kopyalanmalı = true;
            var gölgeKopyaDosyası = new FileInfo(Path.Combine(gölgeKopyaDosyaKlasörü.FullName, dosya.Name));

            //Gölge kopyalanan dosyanın zaten olup olmadığını kontrol edin ve eğer varsa, güncellenmiş olup olmadığını kontrol eder, değilse kopyalar.
            if (gölgeKopyaDosyası.Exists)
            {
                //Dosya hash'ini karşılaştırmak daha iyi olabilir.
                var dosyalarÖzdeşmi = gölgeKopyaDosyası.CreationTimeUtc.Ticks >= dosya.CreationTimeUtc.Ticks;
                if (dosyalarÖzdeşmi)
                {
                    Debug.WriteLine("Kopyalanamıyor; Dosyalar aynı görünüyor: '{0}'", gölgeKopyaDosyası.Name);
                    kopyalanmalı = false;
                }
                else
                {
                    //Mevcut dosyayı sil

                    Debug.WriteLine("Yeni eklenti bulundu; Eski dosya siliniyor: '{0}'", gölgeKopyaDosyası.Name);
                    File.Delete(gölgeKopyaDosyası.FullName);
                }
            }

            if (kopyalanmalı)
            {
                try
                {
                    File.Copy(dosya.FullName, gölgeKopyaDosyası.FullName, true);
                }
                catch (IOException)
                {
                    Debug.WriteLine(gölgeKopyaDosyası.FullName + " kilitlendi, yeniden adlandırmaya çalışılıyor");
                    //Bu dosyalar kilitlendiğinde oluşur,
                    //Nedense devenv eklenti dosyalarını bir kaç kez kilitler ve başka bir sebeple bunları yeniden adlandırmanıza izin verilir
                    //Yeniden gölge kopyası oluşturularak bu sorun çözülür.
                    try
                    {
                        var eskiDosya = gölgeKopyaDosyası.FullName + Guid.NewGuid().ToString("N") + ".old";
                        File.Move(gölgeKopyaDosyası.FullName, eskiDosya);
                    }
                    catch (IOException exc)
                    {
                        throw new IOException(gölgeKopyaDosyası.FullName + " yeniden adlandırma başarısız oldu, eklenti başlatılamıyor.", exc);
                    }
                    //Şimdi gölge kopyayı tekrar deneyelim
                    File.Copy(dosya.FullName, gölgeKopyaDosyası.FullName, true);
                }
            }

            return gölgeKopyaDosyası;
        }
        private static bool PaketEklentiDosyası(DirectoryInfo klasör)
        {
            if (klasör == null) return false;
            if (klasör.Parent == null) return false;
            if (!klasör.Parent.Name.Equals("Eklentiler", StringComparison.InvariantCultureIgnoreCase)) return false;
            return true;
        }
        private static string KuruluEklentilerDosyaYolunuAl()
        {
            return GenelYardımcı.MapPath(KuruluEklentilerYolu);
        }

        #endregion

    }
}