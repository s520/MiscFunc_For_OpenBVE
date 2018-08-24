using IniParser;
using IniParser.Model;
using System.IO;

namespace MiscFunc {

    /// <summary>
    /// 設定を読み込むクラス
    /// </summary>
    internal class LoadConfig {

        // --- メンバ ---
        private static LoadConfig load_config_ = new LoadConfig();

        internal static int WiperRate { get; private set; }
        internal static int WiperHoldPosition { get; private set; }
        internal static int WiperDelay { get; private set; }
        internal static int WiperSoundBehaviour { get; private set; }
        internal static int WiperWet { get; private set; }


        // --- コンストラクタ ---
        /// <summary>
        /// 新しいインスタンスを作成する
        /// </summary>
        private LoadConfig() {
            WiperRate = 700;
            WiperHoldPosition = 0;
            WiperDelay = 700;
            WiperSoundBehaviour = 0;
            WiperWet = 0;
        }

        // --- 関数 ---
        /// <summary>
        /// インスタンスを取得するメソッド
        /// </summary>
        /// <returns>インスタンス</returns>
        internal static LoadConfig GetInstance() {
            return load_config_;
        }

        /// <summary>
        /// ファイルから設定を読み込む関数
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        internal void LoadCfgFile(string filePath) {
            if (File.Exists(filePath)) {
                //Create an instance of a ini file parser
                FileIniDataParser fileIniData = new FileIniDataParser();

                //Parse the ini file
                IniData parsedData = fileIniData.ReadFile(filePath);

                //Get concrete data from the ini file
                int value;
                if (int.TryParse(parsedData["Wiper"]["WiperRate"], out value)) {
                    WiperRate = value;
                }
                if (int.TryParse(parsedData["Wiper"]["WiperHoldPosition"], out value)) {
                    WiperHoldPosition = value;
                }
                if (int.TryParse(parsedData["Wiper"]["WiperDelay"], out value)) {
                    WiperDelay = value;
                }
                if (int.TryParse(parsedData["Wiper"]["WiperSoundBehaviour"], out value)) {
                    WiperSoundBehaviour = value;
                }
                if (int.TryParse(parsedData["Wiper"]["WiperWet"], out value)) {
                    WiperWet = value;
                }
            }
        }
    }
}