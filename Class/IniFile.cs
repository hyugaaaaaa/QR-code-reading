using System.Runtime.InteropServices;
using System.Text;

namespace QRtoCSVSystem
{
    internal class IniFile
    {
        private readonly string _filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "config.ini") ;

        /// <summary>
        /// DLLインポート
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="returnValue"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(
            string section,
            string key,
            string defaultValue,
            StringBuilder returnValue,
            int size,
            string filePath);

        /// <summary>
        /// INIクラスインスタンス
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        public IniFile()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException(MessageManager.GetStringValue(ErrorMessage.ERR001));

        }

        /// <summary>
        /// INIファイルのセクション
        /// </summary>
        public static class Sections
        {
            public const string Network = "Network";
            public const string CsvData = "CsvData";
        }

        /// <summary>
        /// INIファイルの値取得キー
        /// </summary>
        public static class Keys
        {
            public static class Network
            {
                public const string DestDirectory = "DestDirectory";
            }

            public static class CsvData
            {
                public const string ShopNo = "ShopNo";
                public const string PosNo = "PosNo";
                public const string CasherCode = "CasherCode";
                public const string CasherName = "CasherName";
            }
        }

        /// <summary>
        /// INIファイルの値読み込み
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string ReadValue(string section, string key, string defaultValue = "")
        {
            var buffer = new StringBuilder(1024);
            GetPrivateProfileString(section, key, defaultValue, buffer, buffer.Capacity, _filePath);
            return buffer.ToString();
        }
    }
}
