using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xx.Helper.ListDefinitions;
using xx.Helper.Utils;
using System.Diagnostics;
using Newtonsoft.Json;


namespace xx.Helper.WCF
{
    public class WCFSC_ValidateUser
    {
        Communicator Communicator = new Communicator();
        DecompressData DecompressData = new DecompressData();

        public async Task<Tuple<bool, List<ListOfUser>, string>> Query(List<ListOfUser> lstQuery, string wmsAddress, bool completeDebug)
        {
            try
            {
                JsonSerializerSettings jSONsettings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat, Formatting = Formatting.Indented };

                string inputlst = JsonConvert.SerializeObject(lstQuery, jSONsettings);
                List<ListOfUser> lstOutput = new List<ListOfUser>();

                Debug.WriteLine(completeDebug ? "###### input " + this.GetType().Name.Replace("WCF", "") + " " + inputlst : "###### input " + this.GetType().Name.Replace("WCF", "") + " launched");
                var result = await Communicator.Communicate("POST", this.GetType().Name.Replace("WCF",""), inputlst, wmsAddress, lstQuery.First().pEnv);
                if (result.Item1)
                {
                    if (result.Item2.Contains('"' + "byteData" + '"' + ":null,"))
                    {
                        lstOutput = JsonConvert.DeserializeObject<List<ListOfUser>>(result.Item2, jSONsettings);
                        Debug.WriteLine(completeDebug ? "###### output " + this.GetType().Name.Replace("WCF", "") + " " + JsonConvert.SerializeObject(lstOutput, jSONsettings) : "###### output " + this.GetType().Name.Replace("WCF", "") + " received OK " + lstOutput.Count() + " rows");
                    }
                    else
                    {
                        var lstItemsA = JsonConvert.DeserializeObject<List<ListOfUser>>(result.Item2, jSONsettings);
                        string decompressData = DecompressData.DecompressString(lstItemsA.First().byteData);
                        lstOutput = JsonConvert.DeserializeObject<List<ListOfUser>>(decompressData, jSONsettings);
                        Debug.WriteLine(completeDebug ? "###### outputByte " + this.GetType().Name.Replace("WCF", "") + " " + JsonConvert.SerializeObject(lstOutput, jSONsettings) : "###### outputByte " + this.GetType().Name.Replace("WCF", "") + " recived OK " + lstOutput.Count() + " rows");
                    }
                }
                else
                {
                    return new Tuple<bool, List<ListOfUser>, string>(false, new List<ListOfUser>(), this.GetType().Name + "  " + result.Item3);
                }
                return new Tuple<bool, List<ListOfUser>, string>(true, lstOutput, null);
            }
            catch(Exception ex)
            {
                return new Tuple<bool, List<ListOfUser>, string>(false, new List<ListOfUser>(), this.GetType().Name + "  " + ex.Message);
            }
        }
    }
}
