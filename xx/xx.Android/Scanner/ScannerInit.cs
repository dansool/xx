using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using xx.Droid;
using Android;
using Honeywell.AIDC.CrossPlatform;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;

namespace xx.Droid.Scanner
{
    public class ScannerInit
    {
        private const string DEFAULT_READER_KEY = "default";
        private Dictionary<string, BarcodeReader> mBarcodeReaders;
        private bool mContinuousScan = false, mOpenReader = false;
        private BarcodeReader mSelectedReader = null;
        private SynchronizationContext mUIContext = SynchronizationContext.Current;
        private int mTotalContinuousScanCount = 0;
        private bool mSoftContinuousScanStarted = false;
        private bool mSoftOneShotScanStarted = false;
        public IList<BarcodeReaderInfo> readerList = null;
        public async void OpenBarcodeReader()
        {
            mBarcodeReaders = new Dictionary<string, BarcodeReader>();
            readerList = await BarcodeReader.GetConnectedBarcodeReaders();
            mSelectedReader = GetBarcodeReader(readerList.First().ScannerName);
            if (!mSelectedReader.IsReaderOpened)
            {
                BarcodeReader.Result result = await mSelectedReader.OpenAsync();
                if (result.Code == BarcodeReader.Result.Codes.SUCCESS || result.Code == BarcodeReader.Result.Codes.READER_ALREADY_OPENED)
                {
                    SetScannerAndSymbologySettings();
                }
                else
                {
                    MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "exception", "ScannerInit/OpenBarcodeReader: Code:" + result.Code + " Message:" + result.Message);
                }
            }
        }

        public BarcodeReader GetBarcodeReader(string readerName)
        {
            BarcodeReader reader = null;

            if (readerName == DEFAULT_READER_KEY)
            {
                readerName = null;
            }

            if (null == readerName)
            {
                if (mBarcodeReaders.ContainsKey(DEFAULT_READER_KEY))
                {
                    reader = mBarcodeReaders[DEFAULT_READER_KEY];
                }
            }
            else
            {
                if (mBarcodeReaders.ContainsKey(readerName))
                {
                    reader = mBarcodeReaders[readerName];
                }
            }

            if (null == reader)
            {
                reader = new BarcodeReader(readerName);
                reader.BarcodeDataReady += MBarcodeReader_BarcodeDataReady;
                if (null == readerName)
                {
                    mBarcodeReaders.Add(DEFAULT_READER_KEY, reader);
                }
                else
                {
                    mBarcodeReaders.Add(readerName, reader);
                }
            }

            return reader;
        }

        private async void MBarcodeReader_BarcodeDataReady(object sender, BarcodeDataArgs e)
        {
            mUIContext.Post(_ => { UpdateBarcodeInfo(e.Data, e.SymbologyName, e.TimeStamp); }, null);
            if (mContinuousScan)
            {
                mTotalContinuousScanCount++;
            }
            else if (mSoftOneShotScanStarted)
            {
                await mSelectedReader.SoftwareTriggerAsync(false);
                mSoftOneShotScanStarted = false;
            }
        }

        private void UpdateBarcodeInfo(string data, string symbology, DateTime timestamp)
        {
            MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "scannedValue", data);
        }

        public async void CloseBarcodeReader()
        {
            if (mSelectedReader != null && mSelectedReader.IsReaderOpened)
            {
                if (mSoftOneShotScanStarted || mSoftContinuousScanStarted)
                {
                    await mSelectedReader.SoftwareTriggerAsync(false);
                    mSoftOneShotScanStarted = false;
                    mSoftContinuousScanStarted = false;
                }

                BarcodeReader.Result result = await mSelectedReader.CloseAsync();
                if (result.Code == BarcodeReader.Result.Codes.SUCCESS)
                {
                }
                else
                {
                    MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "exception", "ScannerInit/UpdateBarcodeInfo: Code:" + result.Code + " Message:" + result.Message);
                }
            }
        }

        private async void SetScannerAndSymbologySettings()
        {
            try
            {
                if (mSelectedReader.IsReaderOpened)
                {
                    Dictionary<string, object> settings = new Dictionary<string, object>()
                    {
                        {mSelectedReader.SettingKeys.TriggerScanMode, mSelectedReader.SettingValues.TriggerScanMode_OneShot },
                        {mSelectedReader.SettingKeys.Code128Enabled, true },
                        {mSelectedReader.SettingKeys.Code39Enabled, true },
                        {mSelectedReader.SettingKeys.Ean8Enabled, true },
                        {mSelectedReader.SettingKeys.Ean8CheckDigitTransmitEnabled, true },
                        {mSelectedReader.SettingKeys.Ean13Enabled, true },
                        {mSelectedReader.SettingKeys.Ean13CheckDigitTransmitEnabled, true },
                        {mSelectedReader.SettingKeys.Interleaved25Enabled, true },
                        {mSelectedReader.SettingKeys.DatamatrixEnabled, true },
                        {mSelectedReader.SettingKeys.QrCodeEnabled, true },
                        {mSelectedReader.SettingKeys.Interleaved25MaximumLength, 100 },
                        {mSelectedReader.SettingKeys.Postal2DMode, mSelectedReader.SettingValues.Postal2DMode_Usps }
                    };

                    BarcodeReader.Result result = await mSelectedReader.SetAsync(settings);
                    if (result.Code == BarcodeReader.Result.Codes.SUCCESS)
                    {

                        MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "scannerInitStatus", "true");
                    }
                    else
                    {
                        MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "exception", "ScannerInit/UpdateBarcodeInfo: Code:" + result.Code + " Message:" + result.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "exception", "ScannerInit/UpdateBarcodeInfo: " + ex.Message);
            }
        }
    }
}