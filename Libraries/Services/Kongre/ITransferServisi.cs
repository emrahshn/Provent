using Core.Domain.Kongre;
using System.Collections.Generic;

namespace Services.Kongre
{
    public partial interface ITransferServisi
    {
        void TransferSil(Transfer transfer);
        Transfer TransferAlId(int transferId);
        IList<Transfer> TümTransferAl();
        IList<Transfer> TransferAlKongreId(int kongreId);
        void TransferEkle(Transfer transfer);
        void TransferGüncelle(Transfer transfer);
    }

}
