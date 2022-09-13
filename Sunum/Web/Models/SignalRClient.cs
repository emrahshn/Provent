using Web.Framework.Kendoui;

namespace Web.Models
{
    public class SignalRClient
    {
        public string ConnectionID { get; private set; } // ConnectionId is a unique Id per connection
        public DataSourceSonucu KendoDataSourceRequest { get; set; }

        public SignalRClient(string connectionID)
        {
            ConnectionID = connectionID;
        }
    }
}
