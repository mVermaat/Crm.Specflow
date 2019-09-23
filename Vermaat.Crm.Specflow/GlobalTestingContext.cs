using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;
using Vermaat.Crm.Specflow.Formulas;

namespace Vermaat.Crm.Specflow
{
    public static class GlobalTestingContext
    {
        public static ConnectionManager ConnectionManager { get; }
        public static MetadataCache Metadata { get; }
        public static ButtonTexts ButtonTexts { get; }
        public static ErrorCodes ErrorCodes { get; }
        public static FormulaParser FormulaParser { get; }

        internal static BrowserManager BrowserManager { get; }

        static GlobalTestingContext()
        {
            ConnectionManager = new ConnectionManager();
            Metadata = new MetadataCache(ConnectionManager);
            ButtonTexts = new ButtonTexts();
            BrowserManager = new BrowserManager(ButtonTexts);
            ErrorCodes = new ErrorCodes();
            FormulaParser = new FormulaParser();
        }

    }
}
