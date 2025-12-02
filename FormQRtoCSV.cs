using System.Reflection;
using System.Text;

namespace QRtoCSVSystem
{
    public partial class FormQRtoCSV : Form
    {
        #region 定数
        private static readonly Encoding CsvEncoding = Encoding.GetEncoding("Shift-JIS");
        private const string TempFolderName = "TempCsv";
        #endregion

        #region 変数
        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static Logger LogBase = Logger.GetInstance;
        private string _csvData = string.Empty;
        private string _tempFilePath = string.Empty;

        #endregion

        public FormQRtoCSV()
        {
            InitializeComponent();
        }

        #region イベントハンドラ
        private void txtQRCode_KeyDown(object sender, KeyEventArgs e)
        {
            //キーコードがEnterキー以外であれば処理しない
            if (e.KeyCode != Keys.Enter) return;

            try
            {
                //CSVデータ作成
                CreateCsvData();

                //CSV作成
                CreateCsvFile();

                //CSVデータをフォルダに送る
                if (PostCsv())
                {
                    //完了メッセージ表示
                    lblResult.Text = MessageManager.GetStringValue(InfoMessage.INFO002);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageManager.GetStringValue(ErrorMessage.ERR999));
                LogBase.WriteLog(Logger.LogLevel.Fatal, MethodBase.GetCurrentMethod().Name, ex.Message + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                //処理終わりに入力データをクリア
                txtQRCode.Text = string.Empty;
            }
        }
        #endregion

        #region プライベートメソッド


        /// <summary>
        /// CSVデータ作成
        /// </summary>
        private void CreateCsvData()
        {
            // INIファイルの読み込み (IniFile.csを使用)
            IniFile ini = new IniFile(); // config.iniが見つからない場合、ここで例外 (ERR001) が発生

            // INIから必須データ項目を取得
            string shopNo = ini.ReadValue(IniFile.Sections.CsvData, IniFile.Keys.CsvData.ShopNo); // 事業所コード
            string posNo = ini.ReadValue(IniFile.Sections.CsvData, IniFile.Keys.CsvData.PosNo); // 作業場所NO
            string casherCode = ini.ReadValue(IniFile.Sections.CsvData, IniFile.Keys.CsvData.CasherCode); // 作業者ID
            string casherName = ini.ReadValue(IniFile.Sections.CsvData, IniFile.Keys.CsvData.CasherName); // 作業者名

            // 現在時刻の取得 (INDEX_DATE, INDEX_TIME用)
            DateTime now = DateTime.Now;

            // --- 必須7項目の生成 ---
            // 1. INDEX_DATE: 作業した日付 (例: 2023/07/30)
            string indexDate = now.ToString("yyyy/MM/dd");
            // 2. INDEX_TIME: 作業した時刻 (例: 12:00:33)
            string indexTime = now.ToString("HH:mm:ss");

            // 3. INDEX_TRANSACTIONNO: QRコードの値を使用
            
            string indexTransactionNo = txtQRCode.Text.Trim();
            if (string.IsNullOrEmpty(indexTransactionNo))
            {
                // QRコードが空の場合は処理を中断
                throw new Exception("QRコードの値が入力されていません。");
            }

            // --- CSVレコードの作成 ---
            // 1レコード1行、ダブルクォーテーションとカンマ区切りで整形
            _csvData = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"",
                indexDate,
                indexTime,
                indexTransactionNo,
                shopNo,
                posNo,
                casherCode,
                casherName
            );
        }

        /// <summary>
        /// CSV作成
        /// </summary>
        private void CreateCsvFile()
        {

            // 1. ファイル名決定: JYYYYMMDDHHMMSS.csv (ミリ秒を含む)
            string fileName = "J" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv";

            // 2. テンポラリフォルダのパス取得と作成
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string tempDirectory = Path.Combine(exePath, TempFolderName);

            // テンポラリフォルダが存在しない場合は作成
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            // 3. テンポラリファイルのフルパスを保持
            _tempFilePath = Path.Combine(tempDirectory, fileName);

            // 4. CSVデータをShift-JISでファイルに書き込み
            File.WriteAllText(_tempFilePath, _csvData, CsvEncoding);
        }

        /// <summary>
        /// CSVを指定フォルダに送る
        /// </summary>
        private bool PostCsv()
        {
            // 1. INIファイルからホスト名と規定フォルダのパスを取得
            // IniFile ini = new IniFile();
            
            // string destDirectory = ini.ReadValue(IniFile.Sections.Network, IniFile.Keys.Network.DestDirectory, "");
            string destDirectory = @"C:\CSV";

            // INIファイルの設定が存在するかチェック
            if (string.IsNullOrEmpty(destDirectory))
            {
                //エラーメッセージ表示
                MessageBox.Show(MessageManager.GetStringValue(ErrorMessage.ERR002));
                // 送信先設定がなければ、エラー
                return false;
            }

            //INIファイルの設定パスが正しいものかチェック
            bool isValid = IsValidFolderPath(destDirectory);

            //正しいものではなければ、エラー
            if (!isValid)
            {
                //エラーメッセージ表示
                MessageBox.Show(MessageManager.GetStringValue(ErrorMessage.ERR003));
                return false;
            }

            //設定フォルダがなければ作成
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }

            // 2. コピー先のフルパスを生成
            string destFilePath = Path.Combine(destDirectory, Path.GetFileName(_tempFilePath));

            // 3. 規定フォルダにファイルをコピー (上書きを許可: true)
            // ネットワークドライブ経由を想定し、File.Copyを使用
            File.Copy(_tempFilePath, destFilePath, true);

            // 4. コピーが成功したら、テンポラリフォルダのCSVを削除
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }

            return true;
        }

        /// <summary>
        /// フォルダパスが正しい形式かどうかをチェックする
        /// </summary>
        /// <param name="path">チェック対象のフォルダパス</param>
        /// <returns>妥当な形式ならtrue</returns>
        static bool IsValidFolderPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            // 無効な文字が含まれていないかチェック
            char[] invalidChars = Path.GetInvalidPathChars();
            if (path.IndexOfAny(invalidChars) >= 0)
                return false;

            try
            {
                // 正規化できるか（例外が出れば不正）
                string fullPath = Path.GetFullPath(path);

                // ルート（C:\ または \\server\share）が存在するか
                string root = Path.GetPathRoot(fullPath);
                if (string.IsNullOrEmpty(root))
                    return false;

                // 絶対パスであること
                if (!Path.IsPathRooted(path))
                    return false;

                // UNCパスかどうか
                bool isUNC = fullPath.StartsWith(@"\\");
                if (isUNC)
                {
                    // UNCパス形式チェック：最低限 \\server\share が必要
                    //   "\\server" → ×（share がない）
                    //   "\\server\share" → ○
                    var parts = fullPath.TrimStart('\\').Split('\\');

                    if (parts.Length < 2)
                        return false;  // server と share が必要

                    // 空フォルダ名を許容するのは UNC の先頭のみ
                    // それ以降は空であってはならない
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(parts[i]))
                            return false;
                    }
                }
                else
                {
                    // ローカルパスの場合：空のパス要素は不可
                    var parts = fullPath.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var p in parts)
                    {
                        if (string.IsNullOrWhiteSpace(p))
                            return false;
                    }
                }

                return true;
            }
            catch
            {
                // Path.GetFullPathで例外が出た場合は不正
                return false;
            }
        }
        #endregion
    }
}
