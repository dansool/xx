using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xx.Helper.ListDefinitions
{
    public class ListOfUser
    {
        public string scannerID { get; set; }
        public int sessionID { get; set; }
        public string username { get; set; }
        public string errorMessage { get; set; }
        public byte[] byteData { get; set; }
        public int pEnv { get; set; }
        public int mustSelectPrinter { get; set; }
        public int manualInput { get; set; }
        public int stocktakeReader { get; set; }
        public int serialnumberIncreaser { get; set; }
        public string scannerSystemData { get; set; }
    }
}
