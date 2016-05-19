using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace PasswordSaver
{
    public enum SaveType
    {
        LocalState,
        LocalFile,
        RoamingData,
        OneDrive
    }

    public class FileManager
    {
        //将对象存到jsonstring中去
        public static string GetJsonString<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        //从jsonstring中得到想要的对象
        public static T ReadFromJson<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        //通过储存的字符串得到密码的MD5校验
        public static string GetCode(string str)
        {
            return str.Substring(0, 32);
        }
             

        /// <summary>
        /// 将str做备份
        /// </summary>
        /// <param name="str">需要备份的string</param>
        /// <param name="st">备份的类型</param>
        /// <returns>返回成功与否</returns>
        public async static Task<string> BackupAsync(string str, SaveType st)
        {
            try
            {
                switch (st)
                {
                    case SaveType.LocalState:
                        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                        StorageFile LocalStateFile = await localFolder.CreateFileAsync("data.pwsv", CreationCollisionOption.ReplaceExisting);
                        await FileIO.WriteTextAsync(LocalStateFile, str);
                        break;
                    case SaveType.LocalFile:
                        FileSavePicker picker = new FileSavePicker();
                        picker.DefaultFileExtension = ".pwsv";
                        picker.FileTypeChoices.Add("密码计算器文件", new List<string>() { ".pwsv" });
                        picker.SuggestedFileName = "backup";
                        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                        StorageFile LocalFileFile = await picker.PickSaveFileAsync();
                        if (LocalFileFile == null)
                        { return "-未选择文件！"; }
                        await FileIO.WriteTextAsync(LocalFileFile, str);
                        break;
                    case SaveType.RoamingData:
                        StorageFolder RoamingFolder = ApplicationData.Current.RoamingFolder;
                        //Debug.WriteLine(ApplicationData.Current.RoamingStorageQuota);
                        StorageFile RoamingDataFile = await RoamingFolder.CreateFileAsync("data.pwsv", CreationCollisionOption.ReplaceExisting);
                        await FileIO.WriteTextAsync(RoamingDataFile, str);
                        break;
                    case SaveType.OneDrive:
                        try { 
                        //string[] scopes = new string[] { "wl.signin", "wl.offline_access", "onedrive.readwrite" };
                            string[] scopes = new string[] { "wl.signin",  "onedrive.readwrite" };
                            var oneDriveClient = OneDriveClientExtensions.GetClientUsingOnlineIdAuthenticator(scopes);
                        await oneDriveClient.AuthenticateAsync();
                        byte[] array = Encoding.UTF8.GetBytes(str);
                        MemoryStream stream = new MemoryStream(array);
                        var uploadedItem = await oneDriveClient
                                                       .Drive
                                                       .Root
                                                       .ItemWithPath("Documents/backup.pwsv")
                                                       .Content
                                                       .Request()
                                                       .PutAsync<Item>(stream);
                        }
                        catch(Exception exc)
                        {
                            return "-" + exc.Message + "\n详细信息：" + exc.InnerException.Message;
                        }
                        break;
                    default: return "-备份时遇到未知参数";
                }
                return "1";
            }
            catch (Exception ex)
            { return "-" + ex.Message; }
        }

        /// <summary>
        /// 还原数据
        /// </summary>
        /// <param name="st">还原的类型</param>
        /// <returns>还原回的string，开头为-则失败</returns>
        public async static Task<string> RecoverAsync(SaveType st)
        {
            try {
                switch(st)
                {
                    case SaveType.LocalState:
                        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                        StorageFile LocalStateFile = await localFolder.GetFileAsync("data.pwsv");
                        if(LocalStateFile==null)
                        { return "-本地不存在此文件"; }
                        return await FileIO.ReadTextAsync(LocalStateFile);
                    case SaveType.LocalFile:
                        FileOpenPicker openFile = new FileOpenPicker();
                        openFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                        openFile.FileTypeFilter.Add(".pwsv");
                        StorageFile LocalFileFile = await openFile.PickSingleFileAsync();
                        if (LocalFileFile == null)
                        { return "-未选择文件"; }
                        return await FileIO.ReadTextAsync(LocalFileFile);
                    case SaveType.RoamingData:
                        StorageFolder RoamingFolder = ApplicationData.Current.RoamingFolder;
                        StorageFile RoamingDataFile = await RoamingFolder.GetFileAsync("data.pwsv");
                        return await FileIO.ReadTextAsync(RoamingDataFile);
                    case SaveType.OneDrive:
                        try { 
                        //string[] scopes = new string[] { "wl.signin", "wl.offline_access", "onedrive.readwrite" };
                            string[] scopes = new string[] { "wl.signin", "onedrive.readwrite" };
                            var oneDriveClient = OneDriveClientExtensions.GetClientUsingOnlineIdAuthenticator(scopes);
                        await oneDriveClient.AuthenticateAsync();
                        var item = await oneDriveClient
                                 .Drive
                                 .Root
                                 .ItemWithPath("Documents/backup.pwsv")
                                 .Request()
                                 .GetAsync();

                        using (var contentStream = await oneDriveClient.Drive.Items[item.Id].Content.Request().GetAsync())
                        {
                            StreamReader reader = new StreamReader(contentStream);
                            return reader.ReadToEnd();
                        }
                        }
                        catch(Exception exc)
                        {
                            return "-" + exc.Message + "\n详细信息：" + exc.InnerException.Message;
                        }
                    default: return "-还原时遇到未知参数";
                }
            }
            catch(Exception ex)
            { return "-" + ex.Message; }
        }
    }

}
