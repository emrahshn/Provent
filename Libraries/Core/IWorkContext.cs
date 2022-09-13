using Core.Domain.Kullanıcılar;

namespace Core
{
    public interface IWorkContext
    {
        Kullanıcı MevcutKullanıcı { get; set; }
        Kullanıcı OrijinalKullanıcıyıTaklitEt { get; }

        bool Yönetici { get; set; }
    }
}
