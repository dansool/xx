using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Net.Http;
using System.Threading.Tasks;
using xx.Helper.ListDefinitions;
using xx.Helper.Utils;
using System.Threading;
using System.Diagnostics;


namespace xx.Helper.WCF
{
    public class Communicator
    {

        private RestAddressSuffix RestAddressSuffix = new RestAddressSuffix();
        public async Task<Tuple<bool, string, string>> Communicate(string methodRequestType, string methodName, string input, string webServiceAddress, int pEnv)
        {
            try
            {
                string returnString = null;
                    if (!string.IsNullOrEmpty(webServiceAddress))
                    {
                        string URI = "";
                        URI = webServiceAddress + RestAddressSuffix.Get(pEnv);
                        Uri ServiceURI = new Uri(URI + methodName);

                        string responseBody = null;
                        HttpClient httpClient = new HttpClient();

                        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        HttpRequestMessage request = new HttpRequestMessage(methodRequestType == "GET" ? HttpMethod.Get : HttpMethod.Post, ServiceURI);
                        request.Content = new StringContent(input, System.Text.Encoding.UTF8, "application/json");
                        CancellationTokenSource cts = new CancellationTokenSource(40000);
                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        responseBody = await response.Content.ReadAsStringAsync();
                        returnString = responseBody;
                        if (responseBody.ToUpper().Contains("ÜLDINE"))
                        {
                            Debug.WriteLine(returnString);
                        }

                        if (!string.IsNullOrEmpty(returnString))
                        {
                            return new Tuple<bool, string, string>(true, returnString, "");
                        }
                        return new Tuple<bool, string, string>(false, null, "Vastus oli tühi");
                    }
                    else
                    {
                        return new Tuple<bool, string, string>(false, null, "Aadress oli tühi");
                    }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("HttpRequestException " + ex.Message);
                string errorMessage = ex.Message;
                return new Tuple<bool, string, string>(false, null, errorMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = "";
                if (ex.Message.Contains("0x80072EE7"))
                {
                    errorMessage = "Serverit ei leitud";
                }
                if (ex.Message.Contains("0x80072EFD"))
                {
                    errorMessage = "Võrguprobleem. Wifiga ei saadud ühendust";
                }
                else
                {
                    errorMessage = ex.Message;
                }
                return new Tuple<bool, string, string>(false, null, errorMessage);
            }
        }
    }
}
