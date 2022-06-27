using MyApp.Framework.Helpers.Script;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var engine = new JintEngine(Core.Parameters.Script.Timeout, Core.Parameters.Script.MemoryLimit);

            engine.SetGlobalFunction("ftpDownloadCsv", new Func<string, string, string, string, object[], IList<dynamic>>(DummyFtpHelper.DownloadCsv));

            var script = @"
function after_action() {
    var columnNames = [
            'Region',
            'Country',
            'Item Type',
            'Sales Channel',
            'Order Priority',
            'Order Date',
            'Order ID',
            'Ship Date',
            'Units Sold',
            'Units Price',
            'Unit Cost',
            'Total Revenue',
            'Total Cost',
            'Total Profit'
    ];

    var result = ftpDownloadCsv('TEST-DATA.csv', ',', 'true', 'UTF-8', columnNames);
    return true;
}";

            engine.Execute(script);
            var result = engine.Evaluate("after_action()");
        }
    }
}