using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.PointOfService;

namespace ScannerInit4.Scanner
{
    public class ScannerInit
    {
        public bool scannerInitComplete = false;
        public  ClaimedBarcodeScanner claimedScanner = null;
        public  BarcodeScanner scanner = null;
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
                return new Tuple<bool, string>(false, "ReloadScannerAsync " + ex.Message);
            }
        }

        public async Task<bool> ReleaseScannerAsync(bool exit)
        {
            try
            {
                if (claimedScanner != null)
                {
                    claimedScanner.DataReceived -= ClaimedScanner_DataReceivedAsync;
                    claimedScanner.ReleaseDeviceRequested -= OnClaimedScannerReleaseDeviceRequested;
                    claimedScanner.IsDecodeDataEnabled = false;
                    await claimedScanner.DisableAsync();
                    claimedScanner.Dispose();
                }
                scanner = null;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }



        private async Task<Tuple<bool, string>> InitializeScannerAsync()
        {
            try
            {
                //var wp = new xx.UWP.WriteProfile();
                //var x = await wp.Write(ProfileFileName);


                if (scanner == null)
                {
                    scanner = await Windows.Devices.PointOfService.BarcodeScanner.GetDefaultAsync();
                    if (scanner == null)
                    {
                        Windows.Devices.Enumeration.DeviceInformationCollection deviceCollection = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(Windows.Devices.PointOfService.BarcodeScanner.GetDeviceSelector());
                        if (deviceCollection != null && deviceCollection.Count > 0)
                        {
                            scannerInitComplete = false;
                            foreach (Windows.Devices.Enumeration.DeviceInformation D in deviceCollection)
                            {
                                if (D.Id.Contains("POSBarcodeScanner"))
                                {
                                    scanner = await Windows.Devices.PointOfService.BarcodeScanner.FromIdAsync(D.Id);
                                }
                            }
                        }
                    }
                }
                if (scanner != null)
                {

                    claimedScanner = await scanner.ClaimScannerAsync();
                }
                if (claimedScanner != null)
                {
                    claimedScanner.DataReceived += ClaimedScanner_DataReceivedAsync;
                    claimedScanner.ReleaseDeviceRequested += OnClaimedScannerReleaseDeviceRequested;
                    claimedScanner.IsDecodeDataEnabled = true;

                    await claimedScanner.EnableAsync();
                    //await obj.claimedScanner.SetActiveSymbologiesAsync(new List<uint> { 0 });

                    foreach (string ProfileName in scanner.GetSupportedProfiles())
                    {
                        if (ProfileName == "Develok Profile") { await claimedScanner.SetActiveProfileAsync(ProfileName); break; }
                    }
                    scannerInitComplete = true;
                    //mp.BtnSettingsBackground(new SolidColorBrush(Color.FromArgb(120, 0, 255, 0)));
                    //if (!mp.skipUpdate)
                    //{
                    //    mp.successOfScanner = true;
                    //    if (mp.lstLocalScannerSettings.First().restAddress == "olemas")
                    //    {
                    //        if (await mp.DisplayMessageYesNoAsync("Serveri aadress on seadistamata. Kas soovid seda teha?"))
                    //        {
                    //            mp.PrepareSettings();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        CheckNewVersionAvailabilityAsync.Check(mp);
                    //        mp.TxtBkScannedValue.Focus(FocusState.Programmatic);
                    //    }
                    //}
                    return new Tuple<bool, string>(true, null);
                }
                else
                {
                    //mp.BtnSettingsBackground(new SolidColorBrush(Colors.Red));

                    return new Tuple<bool, string>(false, "InitializeScannerAsync 1");
                }
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "InitializeScannerAsync " + ex.Message);
                //mp.SendDebugErrorMessage(this.GetType().Name, "InitializeScannerAsync", ex);
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
                //await DisplayAlert("Error", "ClaimedScanner_DataReceivedAsync failed, Code:" + " Message:" + ex.Message, "OK");
            }
        }

        public async Task Scanned(Windows.Devices.PointOfService.BarcodeScannerDataReceivedEventArgs args)
        {
            try
            {
                //var scannedValue = null;
                string scannedCode = await GetData(args.Report.ScanDataLabel, args.Report.ScanDataType);
                string scannedSymbology = Windows.Devices.PointOfService.BarcodeSymbologies.GetName(args.Report.ScanDataType);
                //Debug.WriteLine(scannedCode + "\r\n" + scannedSymbology);
                //await DisplayAlert("Skänn", scannedCode + "\r\n" + scannedSymbology, "OK");
                //await Received(scannedCode, scannedSymbology, mp);
                //if (!obj.isDeviceHandheld)
                //{
                //    mp.Focus(FocusState.Programmatic);
                //}
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Error", "Scanned failed, Code:" + " Message:" + ex.Message, "OK");
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
                //await DisplayAlert("Error", "GetData failed, Code:" + " Message:" + ex.Message, "OK");
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
                //await DisplayAlert("Error", "OnClaimedScannerReleaseDeviceRequested failed, Code:" + " Message:" + ex.Message, "OK");
            }
        }

        private async Task<bool> LastTryAsync()
        {
            try
            {
                //mp.BtnSettingsBackground(new SolidColorBrush(Color.FromArgb(120, 255, 0, 0)));
                await ReleaseScannerAsync(false);
                await InitializeScannerAsync();
                return false;
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Error", "LastTryAsync failed, Code:" + " Message:" + ex.Message, "OK");
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
                //await DisplayAlert("Error", "GetData failed, Code:" + " Message:" + ex.Message, "OK");
                return "";
            }
        }
    }
}
