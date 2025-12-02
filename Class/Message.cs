using System.Reflection;

namespace QRtoCSVSystem
{
    #region 列挙体

    /// <summary>
    /// インフォメーションメッセージ
    /// 
    /// </summary>
    public enum InfoMessage
    {
        /// <summary>
        /// CSVデータ作成中...
        /// </summary>
        [Message("CSVデータ作成中...")]
        INFO001,
        /// <summary>
        /// CSV出力が完了しました。
        /// </summary>
        [Message("CSV出力が完了しました。")]
        INFO002,
    }

    /// <summary>
    /// 質問メッセージ
    /// </summary>
    public enum QuestionMessage
    {

    }

    /// <summary>
    /// 警告メッセージ
    /// </summary>
    public enum WarnMessage
    {

    }

    /// <summary>
    /// エラーメッセージ
    /// </summary>
    public enum ErrorMessage
    {
        /// <summary>
        /// INIファイルが見つかりません。システム管理者までご連絡お願いします。
        /// </summary>
        [Message("INIファイルが見つかりません。システム管理者までご連絡お願いします。")]
        ERR001,
        /// <summary>
        /// CSV送信先の設定がありません。設定ファイルを確認して下さい。
        /// </summary>
        [Message("CSV送信先の設定がありません。設定ファイルを確認して下さい。")]
        ERR002,
        /// <summary>
        /// CSV送信先の設定のフォルダパスが正しくありません。設定ファイルを確認して下さい。
        /// </summary>
        [Message("CSV送信先の設定のフォルダパスが正しくありません。設定ファイルを確認して下さい。")]
        ERR003,
        /// <summary>
        /// 異常なエラーが発生しました。システム管理者までご連絡お願いします。
        /// </summary>
        [Message("異常なエラーが発生しました。システム管理者までご連絡お願いします。")]
        ERR999,
    }
    

    #endregion

    /// <summary>
    /// Enumに文字列を付加するためのAttributeクラス
    /// </summary>
    public class MessageAttribute : Attribute
    {
        /// <summary>
        /// 列挙型の値を保持するコンストラクタ
        /// </summary>
        public string StringValue { get; protected set; }

        public MessageAttribute(string value)
        {
            this.StringValue = value;
        }
    }

    public static class MessageManager
    {
        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static Logger LogBase = Logger.GetInstance;

        /// <summary>
        /// 列挙型の値に対するメッセージ内容を返す
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            try
            {
                Type type = value.GetType();

                //フィールド情報を取得
                FieldInfo fieldInfo = type.GetField(value.ToString());

                //範囲外の値チェック
                if (fieldInfo == null) return null;

                MessageAttribute[] attribute = fieldInfo.GetCustomAttributes(typeof(MessageAttribute), false) as MessageAttribute[];

                //マッチしたメッセージを返す
                return attribute.Length > 0 ? attribute[0].StringValue : null;
            }
            catch (Exception ex)
            {
                LogBase.WriteLog(Logger.LogLevel.Fatal, MethodBase.GetCurrentMethod().Name, ex.Message + Environment.NewLine + ex.StackTrace);
                return string.Empty;
            }


        }

        /// <summary>
        /// 列挙型の値に対するメッセージ内容を返す
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value, string sParam)
        {
            try
            {
                Type type = value.GetType();

                //フィールド情報を取得
                FieldInfo fieldInfo = type.GetField(value.ToString());

                //範囲外の値チェック
                if (fieldInfo == null) return null;

                MessageAttribute[] attribute = fieldInfo.GetCustomAttributes(typeof(MessageAttribute), false) as MessageAttribute[];

                //マッチしたメッセージを返す
                return attribute.Length > 0 ? string.Format(attribute[0].StringValue, sParam) : null;
            }
            catch (Exception ex)
            {
                LogBase.WriteLog(Logger.LogLevel.Fatal, MethodBase.GetCurrentMethod().Name, ex.Message + Environment.NewLine + ex.StackTrace);
                return string.Empty;
            }

        }
    }
}