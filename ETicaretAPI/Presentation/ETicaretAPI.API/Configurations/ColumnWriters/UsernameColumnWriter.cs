using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace ETicaretAPI.API.Configurations.ColumnWriters
{
    public class UsernameColumnWriter : ColumnWriterBase
    {
        public UsernameColumnWriter(): base(NpgsqlDbType.Varchar) { }
        public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
        {
            var (userName, value) = logEvent.Properties.FirstOrDefault( data => data.Key == "user_name");
            return value?.ToString() ?? null;
        }
    }
}
