namespace QRtoCSVSystem
{
    /// <summary>
    /// ログ出力クラス
    /// </summary>
    /// ログ出力を行う。※インスタンス化不要。公開プロパティ[Instance]経由でアクセス
    public class Logger
    {
        #region "メンバー変数"
        // シングルトンクラスアクセス用変数
        private static readonly Logger instance = new Logger();
        #endregion

        #region "公開列挙型"
        /// <summary>ログ出力レベル</summary>
        public enum LogLevel
        {
            Debug = 1,
            Info = 2,
            Warn = 3,
            Error = 4,
            Fatal = 5,
            None = 9
        }

        /// <summary>ログ書込モード：Append=追記/Over=上書き</summary>
        public enum LogWriteModeType
        {
            Append,
            Over
        }

        /// <summary>ログファイル名フォーマット</summary>
        public enum LogFormatFileNameType
        {
            YYYYMMDD,
            YYYYMMDDHHMMSS,
            YYYYMMDDHHMMSSFFF,
            None
        }
        #endregion

        #region "公開プロパティ"
        /// クラスインスタンスアクセス用
        public static Logger GetInstance
        {
            get { return instance; }
        }

        /// ログ出力ディレクトリ
        public string LogFileDir { get; set; }

        /// ログファイル名
        public string LogFileName { get; set; }

        /// ログファイルの書込モード
        public LogWriteModeType LogWriteMode { get; set; }

        /// ログファイル名のフォーマット
        public LogFormatFileNameType LogFormatFileName { get; set; }

        /// ログファイルの出力レベル
        public LogLevel LogOutPutLevel { get; set; }

        /// エンコード
        public string Encode { get; set; }

        #endregion

        #region "コンストラクタ"
        private Logger()
        {
            // 各プロパティに初期値を設定
            // ログ出力先ディレクトリ
            LogFileDir = AppDomain.CurrentDomain.BaseDirectory + "\\log";
            // ログファイル名
            LogFileName = "TraceLog";
            // ログファイルの書込モード
            LogWriteMode = LogWriteModeType.Append;
            // ログファイル名のフォーマット
            LogFormatFileName = LogFormatFileNameType.YYYYMMDD;
            // ログファイルの出力レベル
            LogOutPutLevel = LogLevel.Info;
            // エンコード
            Encode = "shift-jis";
        }
        #endregion

        #region "公開メソッド"

        #region 処理ログ出力
        /// <summary>
        /// 処理ログ出力
        /// </summary>
        /// <param name="logProc">呼び出し元関数</param>
        /// <param name="writeLogLevel">書込ログレベル</param>
        /// <param name="action">処理</param>
        public static void DebugLogOutput(string logProc, LogLevel writeLogLevel, Action action)
        {
            try
            {
                instance.WriteLog(writeLogLevel, logProc, "START");
                action();
            }
            catch (Exception ex)
            {
                instance.WriteLog(LogLevel.Error, ex.TargetSite.Name, ex.Message + Environment.NewLine + ex.StackTrace);
                throw;
            }
            finally
            {
                instance.WriteLog(writeLogLevel, logProc, "END");
            }
        }
        #endregion

        #region "ログファイル書き込み"
        /// <summary>
        /// ログファイル書き込み
        /// </summary>
        /// <param name="writeLogLevel">書込ログレベル</param>
        /// <param name="logProc">呼び出し元関数</param>
        /// <param name="logMsg">ログ内容</param>
        public bool WriteLog(LogLevel writeLogLevel, string logProc, string logMsg)
        {
            try
            {
                // ログ出力文字列作成
                string LogString = CreateLogString(writeLogLevel, logProc, logMsg);

                // 書込ディレクトリが無ければ、作成
                if (!Directory.Exists(LogFileDir))
                {
                    Directory.CreateDirectory(LogFileDir);
                }

                // ログ書込モードによって[追記/上書]を行う
                using (StreamWriter Fs = new StreamWriter(Path.Combine(LogFileDir, CreateLogFilePath(LogFileName)), Convert.ToBoolean(LogWriteMode == LogWriteModeType.Append ? true : false), System.Text.Encoding.GetEncoding(Encode)))
                {
                    Fs.Write(LogString);
                }

                return true;
            }
            catch
            {
                // 握りつぶす
                return false;
            }
        }
        #endregion
        #endregion

        #region "内部メソッド"

        #region "ログ出力文字列作成"
        /// <summary>
        /// ログ出力文字列作成
        /// </summary>
        /// <param name="logMsg">ログ内容</param>
        /// <param name="writeLogLevel">書込ログレベル</param>
        /// <param name="exMsg">ログ区分</param>
        /// <param name="logProc">呼び出し元関数名</param>
        private string CreateLogString(LogLevel writeLogLevel, string logProc, string logMsg)
        {
            // ログ文字列（"日付","時刻","ログレベル","関数名","メッセージ"）

            string logString = string.Empty;
            string logTemplate = "\"" + DateTime.Now.ToString("yyyy/MM/dd") + "\",\"" + DateTime.Now.ToString("HH:mm:ss:fff") + "\",\"[" + writeLogLevel.ToString() + "]\"," + "{0}" + "\r\n";

            logString = string.Format(logTemplate, "\"" + logProc + "\",\"" + logMsg + "\"");
            return logString;
        }
        #endregion

        #region "ログファイルパス設定"
        /// <summary>
        /// ログファイルパス設定
        /// </summary>
        /// <param name="logFileName">ログファイル名</param>
        private string CreateLogFilePath(string logFileName)
        {
            switch (LogFormatFileName)
            {
                case LogFormatFileNameType.YYYYMMDD:
                    logFileName += "_" + DateTime.Now.ToString("yyyyMMdd");
                    break;
                case LogFormatFileNameType.YYYYMMDDHHMMSS:
                    logFileName += "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    break;
                case LogFormatFileNameType.YYYYMMDDHHMMSSFFF:
                    logFileName += "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    break;
                case LogFormatFileNameType.None:
                    break;
            }

            return logFileName + ".log";
        }
        #endregion

        #endregion
    }
}