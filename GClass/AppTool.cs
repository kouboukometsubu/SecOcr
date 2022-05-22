using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace GClass
{
    static public class AppTool
    {
        // ２重起動チェック
        public static bool CheckDoubleBoot()
        {
            if (System.Diagnostics.Process.GetProcessesByName(
               System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                return (true);
            }

            return (false);
        }

        // アプリルートフォルダを取得
        public static string GetRootFolder()
        {
            string currentFolder = FileManager.GetCurrentDirectory();

            if (currentFolder.IndexOf("bin\\Debug") >= 0 
                || currentFolder.IndexOf("bin\\Release") >= 0
                || currentFolder.IndexOf("bin\\x64\\Debug") >= 0 
                || currentFolder.IndexOf("bin\\x64\\Release") >= 0
                )
            {
                int posBin = FileManager.GetCurrentDirectory().IndexOf("\\bin");
                currentFolder = FileManager.GetCurrentDirectory().Substring(0, posBin);
            }

            return (currentFolder);
        }

        /// <summary>
        /// 拡張子を登録 例： .000
        /// </summary>
        /// <param name="extension"></param>
        public static void RegistExtension(string extension)
        {

            //関連付ける拡張子
            //string extension = ".000";

            //実行するコマンドライン
            string commandline = "\"" + Application.ExecutablePath + "\" \"%1\"";
            //ファイルタイプ名
            string fileType = Application.ProductName + ".0";
            //説明（「ファイルの種類」として表示される）
            string description = "MyApplication File";
            //動詞
            string verb = "open";
            //動詞の説明（エクスプローラのコンテキストメニューに表示される。
            //　省略すると、「開く(&O)」となる。）
            string verbDescription = "MyApplicationで開く(&O)";
            //アイコンのパスとインデックス
            string iconPath = Application.ExecutablePath;
            int iconIndex = 0;

            Microsoft.Win32.RegistryKey rootkey =
                Microsoft.Win32.Registry.ClassesRoot;

            //拡張子のキーを作成し、そのファイルタイプを登録
            Microsoft.Win32.RegistryKey regkey =
                rootkey.CreateSubKey(extension);
            regkey.SetValue("", fileType);
            regkey.Close();

            //ファイルタイプのキーを作成し、その説明を登録
            Microsoft.Win32.RegistryKey typekey =
                rootkey.CreateSubKey(fileType);
            typekey.SetValue("", description);
            typekey.Close();

            //動詞のキーを作成し、その説明を登録
            Microsoft.Win32.RegistryKey verblkey =
                rootkey.CreateSubKey(fileType + "\\shell\\" + verb);
            verblkey.SetValue("", verbDescription);
            verblkey.Close();

            //コマンドのキーを作成し、実行するコマンドラインを登録
            Microsoft.Win32.RegistryKey cmdkey =
                rootkey.CreateSubKey(fileType + "\\shell\\" + verb + "\\command");
            cmdkey.SetValue("", commandline);
            cmdkey.Close();

            //アイコンのキーを作成し、アイコンのパスと位置を登録
            Microsoft.Win32.RegistryKey iconkey =
                rootkey.CreateSubKey(fileType + "\\DefaultIcon");
            iconkey.SetValue("", iconPath + "," + iconIndex.ToString());
            iconkey.Close();

        }

        public static void UnRegistExtention(string extension)
        {
            //拡張子
            //string extension = ".000";

            //ファイルタイプ名
            string fileType = Application.ProductName;

            //レジストリキーを削除
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(extension);
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(fileType);
        }


    }

}
