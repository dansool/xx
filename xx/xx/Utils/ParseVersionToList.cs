using System;
using System.Collections.Generic;
using System.Text;
using xx.Helper.ListDefinitions;
using System.Diagnostics;

namespace xx.Utils
{
    public class ParseVersionToList
    {
        public List<ListOfVersion> Get(string version)
        {
            try
            {
                if (!string.IsNullOrEmpty(version))
                {
                    var lstResult = new List<ListOfVersion>();
                    var resultSplitted = version.Split(new string[] { "." }, StringSplitOptions.None);
                    var Build = Convert.ToInt32(resultSplitted[0].ToString());
                    var Major = Convert.ToInt32(resultSplitted[1].ToString());
                    var Minor = Convert.ToInt32(resultSplitted[2].ToString());
                    var Revision = Convert.ToInt32(resultSplitted[3].ToString());
                    var row = new ListOfVersion
                    {
                        Build = Build,
                        Major = Major,
                        Minor = Minor,
                        Revision = Revision

                    };
                    lstResult.Add(row);
                    return lstResult;
                }
                else
                {
                    return new List<ListOfVersion>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ParseVersionToList " + ex.Message);
                return new List<ListOfVersion>();
            }
        }
    }
}
