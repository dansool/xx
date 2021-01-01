using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Diagnostics;

namespace xx.UWP.Scanner
{
    public class WriteProfile
    {
        private App obj = App.Current as App;

        public async Task<bool> Write(string profileName)
        {
            try
            {
                if (obj.isHoneyWell)
                {
                    StorageFolder _folder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("Profile", CreationCollisionOption.OpenIfExists);
                    var uri = new System.Uri("ms-appx:///Assets/" + profileName);
                    StorageFile profileFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
                    String profileContent = await FileIO.ReadTextAsync(profileFile);
                    StorageFile myFile = await _folder.CreateFileAsync(profileName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(myFile, profileContent);
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
