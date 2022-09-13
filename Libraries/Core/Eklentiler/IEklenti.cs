namespace Core.Eklentiler
{
    public interface IEklenti
    {
        EklentiTanımlayıcı EklentiTanımlayıcı { get; set; }
        void Yükle();
        void Sil();
    }
}