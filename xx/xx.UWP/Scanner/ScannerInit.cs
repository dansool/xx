using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.PointOfService;

using System.Diagnostics;
using Xamarin.Forms;

namespace xx.UWP.Scanner
{
    public class ScannerInit
    {
        private App obj = App.Current as App;
        public string ProfileFileName = "HoneywellDecoderSettingsV2.exm";
        public bool scannerInitComplete;

        public async Task<Tuple<bool, string>> ReloadScannerAsync()
        {
            try
            {
                await ReleaseScannerAsync(false);
                var p = await InitializeScannerAsync();
                if (p.Item1)
                {
                    return new Tuple<bool, string>(true, null);
                }
                else
                {
                    return new Tuple<bool, string>(false, "ReloadScannerAsync " + p.Item2);
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "exception", "ScrannerInit/ReloadScannerAsync: " + ex.Message);
                return new Tuple<bool, string>(false, "ReloadScannerAsync " + ex.Message);
            }
        }

        public async Task Scanned(Windows.Devices.PointOfService.BarcodeScannerDataReceivedEventArgs args)
        {
            try
            {
                string scannedCode = await GetData(args.Report.ScanDataLabel, args.Report.ScanDataType);
                string scannedSymbology = Windows.Devices.PointOfService.BarcodeSymbologies.GetName(args.Report.ScanDataType);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "exception", "ScrannerInit/Scanned: " + ex.Message);
            }
        }

        public async Task<bool> ReleaseScannerAsync(bool exit)
        {
            try
            {
                if (App.claimedScanner != null)
                {
                    App.claimedScanner.DataReceived -= ClaimedScanner_DataReceivedAsync;
                    App.claimedScanner.ReleaseDeviceRequested -= OnClaimedScannerReleaseDeviceRequested;
                    App.claimedScanner.IsDecodeDataEnabled = false;
                    await App.claimedScanner.DisableAsync();
                    App.claimedScanner.Dispose();
                }
                App.scanner = null;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "exception", "ScrannerInit/ReleaseScannerAsync: " + ex.Message);
                return false;
            }
            return true;
        }

        private async Task<Tuple<bool, string>> InitializeScannerAsync()
        {
            try
            {
                var wp = new xx.UWP.Utils.WriteProfile();
                var x = await wp.Write(ProfileFileName);


                if (App.scanner == null)
                {
                    App.scanner = await Windows.Devices.PointOfService.BarcodeScanner.GetDefaultAsync();
                    if (App.scanner == null)
                    {
                        Windows.Devices.Enumeration.DeviceInformationCollection deviceCollection = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(Windows.Devices.PointOfService.BarcodeScanner.GetDeviceSelector());
                        if (deviceCollection != null && deviceCollection.Count > 0)
                        {
                            scannerInitComplete = false;
                            foreach (Windows.Devices.Enumeration.DeviceInformation D in deviceCollection)
                            {
                                if (D.Id.Contains("POSBarcodeScanner"))
                                {
                                    App.scanner = await Windows.Devices.PointOfService.BarcodeScanner.FromIdAsync(D.Id);
                                }
                            }
                        }
                    }
                }
                if (App.scanner != null)
                {

                    App.claimedScanner = await App.scanner.ClaimScannerAsync();
                }
                if (App.claimedScanner != null)
                {
                    App.claimedScanner.DataReceived += ClaimedScanner_DataReceivedAsync;
                    App.claimedScanner.ReleaseDeviceRequested += OnClaimedScannerReleaseDeviceRequested;
                    App.claimedScanner.IsDecodeDataEnabled = true;

                    await App.claimedScanner.EnableAsync();
                    //await obj.claimedScanner.SetActiveSymbologiesAsync(new List<uint> { 0 });

                    foreach (string ProfileName in App.scanner.GetSupportedProfiles())
                    {
                        if (ProfileName == "Develok Profile") { await App.claimedScanner.SetActiveProfileAsync(ProfileName); break; }
                    }
                    scannerInitComplete = true; 
                    MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "scannerInitStatus", "true");
                    
                    return new Tuple<bool, string>(true, null);
                }
                else
                {
                    return new Tuple<bool, string>(false, "InitializeScannerAsync 1");
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "exception", "ScrannerInit/InitializeScannerAsync: " + ex.Message);
                return new Tuple<bool, string>(false, "InitializeScannerAsync " + ex.Message);
            }
        }

        public async void ClaimedScanner_DataReceivedAsync(Windows.Devices.PointOfService.ClaimedBarcodeScanner sender, Windows.Devices.PointOfService.BarcodeScannerDataReceivedEventArgs args)
        {
            try
            {
                await Scanned(args);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "exception", "ScrannerInit/ClaimedScanner_DataReceivedAsync: " + ex.Message);
            }
        }

        public async Task<string> GetData(Windows.Storage.Streams.IBuffer data, uint scanDataType)
        {
            try
            {
                string result = null;
                if (data == null)
                {
                    result = "No data";
                }
                else
                {
                    switch (Windows.Devices.PointOfService.BarcodeSymbologies.GetName(scanDataType))
                    {
                        case "Ean13":
                        case "Ean8":
                        case "Code128":
                        case "Qr":
                        case "Code93":
                        case "Code39":
                        case "Gs1128":
                        case "DataMatrix":
                        case "Gs1128Coupon":
                        case "Gs1DatabarType1":
                        case "Gs1DatabarType2":
                        case "Gs1DatabarType3":
                        case "Upca":
                        case "Upce":
                        case "TfInd":
                        case "TfInt":
                        case "TfStd":
                        case "UccEan128":
                        case "Ean13Add2":
                        case "Ean13Add5":
                            Windows.Storage.Streams.DataReader reader = Windows.Storage.Streams.DataReader.FromBuffer(data);
                            result = reader.ReadString(data.Length).ToString();
                            byte[] bytes = Encoding.ASCII.GetBytes(result);
                            result = Encoding.UTF8.GetString(bytes);

                            break;
                        default:
                            result = string.Format("Decoded data unavailable. Raw label data: {0}", GetData(data));
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "exception", "ScrannerInit/GetData: " + ex.Message);
            }
            return "";
        }

        public async void OnClaimedScannerReleaseDeviceRequested(object sender, Windows.Devices.PointOfService.ClaimedBarcodeScanner e)
        {
            try
            {
                if (e != null)
                {
                    e.RetainDevice();
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "exception", "ScrannerInit/OnClaimedScannerReleaseDeviceRequested: " + ex.Message);
            }
        }

        private async Task<bool> LastTryAsync()
        {
            try
            {
                await ReleaseScannerAsync(false);
                await InitializeScannerAsync();
                return false;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "exception", "ScrannerInit/LastTryAsync: " + ex.Message);
                return false;
            }
        }

        public async Task<string> GetData(Windows.Storage.Streams.IBuffer data)
        {
            try
            {
                StringBuilder result = new StringBuilder();

                if (data == null)
                {
                    result.Append("No data");
                }
                else
                {
                    const uint MAX_BYTES_TO_PRINT = 20;
                    uint bytesToPrint = Math.Min(data.Length, MAX_BYTES_TO_PRINT);

                    Windows.Storage.Streams.DataReader reader = Windows.Storage.Streams.DataReader.FromBuffer(data);
                    byte[] dataBytes = new byte[bytesToPrint];
                    reader.ReadBytes(dataBytes);

                    for (uint byteIndex = 0; byteIndex < bytesToPrint; ++byteIndex)
                    {
                        result.AppendFormat("{0:X2} ", dataBytes[byteIndex]);
                    }

                    if (bytesToPrint < data.Length)
                    {
                        result.Append("...");
                    }
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<xx.App, string>((xx.App)obj.xxApp, "exception", "ScrannerInit/GetData IBuffer: " + ex.Message);
                return "";
            }
        }
    }
}
