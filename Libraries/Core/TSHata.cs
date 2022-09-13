using System;
using System.Runtime.Serialization;

namespace Core
{
    [Serializable]
    public class TSHata : Exception
    {
        public TSHata()
        {
        }
        public TSHata(string mesaj)
            : base(mesaj)
        {
        }
        public TSHata(string mesajFormatı, params object[] args)
            : base(string.Format(mesajFormatı, args))
        {
        }
        protected TSHata(SerializationInfo
            info, StreamingContext context)
            : base(info, context)
        {
        }
        public TSHata(string mesaj, Exception innerException)
            : base(mesaj, innerException)
        {
        }
    }
}
