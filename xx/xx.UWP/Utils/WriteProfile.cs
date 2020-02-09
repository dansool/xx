using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xx.UWP.Utils
{
    public class WriteProfile
    {
        public async Task<Tuple<bool, string>> Write(string profileName)
        {
            try
            {

                Windows.Storage.StorageFolder _folder = await Windows.Storage.KnownFolders.DocumentsLibrary.CreateFolderAsync("Profile", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var uri = new System.Uri("ms-appx:///Assets/" + profileName);
                Windows.Storage.StorageFile profileFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
                String profileContent = await Windows.Storage.FileIO.ReadTextAsync(profileFile);
                Windows.Storage.StorageFile myFile = await _folder.CreateFileAsync(profileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(myFile, profileContent);
                return new Tuple<bool, string>(true, null);

            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
        }
    }
}
