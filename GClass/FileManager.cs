using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace GClass
{

	/// <summary>
	/// ファイル操作に関するクラス
	/// </summary>
    public class FileManager
    {
       
//	    static string CnvFileExceptionCause( int cause )

        /// <summary>
        /// プロジェクト名（アセンブリ名）を取得
        /// </summary>
        /// <returns></returns>
        public static string GetProjectName()
        {
#if FRAME_WORK_4
            // アセンブリ名をフルパスで取得
            var fullAssemblyNmae = Application.ExecutablePath;// GetType().Assembly.Location;

            // アセンブリ名のみを取得
            var assemblyName = System.IO.Path.GetFileName(fullAssemblyNmae);

            var ret = GetFilenameWithoutExtention(assemblyName);

            return (ret);
#else
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            string path = assembly.Location;
            return (path); // will
#endif
        }

		/// <summary>
		/// フルパスからファイル名を抽出
		/// </summary>
		/// <param name="pFullPath"></param>
		/// <returns></returns>
        public static string GetFileName(string pFullPath)
        {
            // ファイル名 を取得する
            string stParentName = Path.GetFileName(pFullPath);

            return (stParentName);
        }

		/// <summary>
		/// フルパスからディレクトリだけ抜き出す
		/// </summary>
		/// <param name="pFullPath"></param>
		/// <returns></returns>
        public static string GetFilePath(string pFullPath)
        {
            // 親ディレクトリ名 (フォルダ名) を取得する
            string stParentName = Path.GetDirectoryName( pFullPath );

            return(stParentName);
  
        }

        public static int CreateDirectory( string folder )
        {
            int rc = CreateFolder(folder);
            return (rc);
        }

	    public static int CreateFolder( string sCreateFolderName )			// フォルダ作成
        {
            int ret = 0;

            try
            {
                System.IO.Directory.CreateDirectory(sCreateFolderName);
            }
            catch( Exception ex)
            {
                ret = -1;
            }

            return (ret);
        }

		/// <summary>
		/// フォルダ内のファイル数を数える
		/// </summary>
		/// <param name="foldername"></param>
		/// <returns></returns>
	    public static int GetFileCountInFolder( string foldername )			
		{
			DirectoryInfo info = new DirectoryInfo(foldername);

            int cnt = 0;
#if FRAME_WORK_4
            cnt = info.EnumerateFiles().Count();
#else
            cnt = info.GetFiles().Length; // will
#endif
			return (cnt);
		}

//	    static int MakeDirectoryAll( string filename )				    // ディレクトリを自動作成
        public static bool IsExistDirectory(string dirname)				// フォルダの存在確認
        {
            bool ret = System.IO.Directory.Exists(dirname);
            return (ret);
        }

		public static int CheckExistDirectoryAndCreate( string directory )
		{
			if ( !IsExistDirectory( directory))
			{
				int rc = CreateDirectory(directory);
				if ( rc != 0 )
				{
					return (-1);
				}
			}
			return (0);
		}

        public static bool IsExistFile(string filename)					// ファイルの存在確認
        {
            bool ret = System.IO.File.Exists(filename);

            return (ret);
        }

		public static bool IsFile( string path )
		{
			bool ret = File.Exists(path);
			return (ret);
		}

		public static bool IsDirectory( string path )
		{
			bool ret = Directory.Exists(path);
			return (ret);
		}

        public static int CopyFile(string src, string dst)				// ファイルをコピーする
        {
            int rc = 0;
            try
            {
                System.IO.File.Copy(src, dst);
            }
            catch( Exception ex)
            {
                rc = -1;
            }
            return (rc);
        }

        public static int DeleteFile(string filename)						// ファイルを消す
        {
            try
            {
                System.IO.File.Delete(filename);
            }
            catch (Exception ex)
            {
                return (-1);
            }

            return (0);
      
        }


	    static int MoveToTrash( string filename, bool bMessage 
#if FRAME_WORK_4
            = false
#endif
            )	// ゴミ箱へファイルを移動
		{
			// will

			File.Delete(filename ) ;

			return (0);
		}

        public static string GetCurrentDirectory() 
        {
#if FRAME_WORK_4
            return (Application.StartupPath); 
#else
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();

            string exeName = assembly.Location;

            string path = GetFilePath( exeName ) ;

            return (path); 
#endif
            //return (System.IO.Directory.GetCurrentDirectory());
        }
    // 2005.11.15
		// 拡張子（ピリオドを含む)を取得
        public static string GetExtension(string pFullPath) 
        {
            // 拡張子を取得する
            string stParentName = Path.GetExtension(pFullPath);

            return (stParentName);
         }

        // ピリオドなし拡張子取得
        public static string GetExtensionNotPiriod(string pFullPath)
        {
            // 拡張子を取得する
            string stParentName = Path.GetExtension(pFullPath);

            string stTrim = stParentName.TrimStart('.') ;

            return (stTrim);
        }

        // 拡張子なしファイル名取得
        public static string GetFilenameWithoutExtention(string pFullPath)
        {
            // 拡張子を取得する
            string stExtention = Path.GetExtension(pFullPath);

           // int sizeFull = pFullPath.Length ;

           int sizeExtention = stExtention.Length;

            string filename = GetFileName(pFullPath);

            int sizeFull = filename.Length;


         //   string stRet = pFullPath.Substring(0, sizeFull - sizeExtention);
            string stRet = filename.Substring(0, sizeFull - sizeExtention); 

            return (stRet);
        }



    // 2006.01.22
//	    static int Replace( string filename, string Pre, string Next ) 

        public static void CopyFileInFolder(string FolderSrc, string FolderDst/*, CTakanoThread* pThread = NULL*/)
        {
            CopyDirectory(FolderSrc, FolderDst);
        }


        /// <summary>
        /// ディレクトリをコピーする
        /// </summary>
        /// <param name="sourceDirName">コピーするディレクトリ</param>
        /// <param name="destDirName">コピー先のディレクトリ</param>
        public static void CopyDirectory(
            string sourceDirName, string destDirName)
        {
            //コピー先のディレクトリがないときは作る
            if (!System.IO.Directory.Exists(destDirName))
            {
                System.IO.Directory.CreateDirectory(destDirName);
                //属性もコピー
                System.IO.File.SetAttributes(destDirName,
                    System.IO.File.GetAttributes(sourceDirName));
            }

            //コピー先のディレクトリ名の末尾に"\"をつける
            if (destDirName[destDirName.Length - 1] !=
                    System.IO.Path.DirectorySeparatorChar)
                destDirName = destDirName + System.IO.Path.DirectorySeparatorChar;

            //コピー元のディレクトリにあるファイルをコピー
            string[] files = System.IO.Directory.GetFiles(sourceDirName);
            foreach (string file in files)
                System.IO.File.Copy(file,
                    destDirName + System.IO.Path.GetFileName(file), true);

            //コピー元のディレクトリにあるディレクトリについて、再帰的に呼び出す
            string[] dirs = System.IO.Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
                CopyDirectory(dir, destDirName + System.IO.Path.GetFileName(dir));
        }
        
        
//	    static bool IsSameFile( string FileSrc, string FileDst )

    // 2007.02.16
//	    static void CreateFile( string FileName ) 

    // 2007.02.22
	    public static DateTime GetFileMakeTime( string FileName )
		{
			DateTime ret = File.GetCreationTime(FileName);

			return (ret);

		}
		
		public static DateTime GetFileLastWriteTime( string FileName )
		{
			DateTime ret = File.GetLastWriteTime(FileName);

			return (ret);

		}

    // 2009.02.13
		public static bool CompFileLastWriteTime(string FileSrc, string FileDst)
		{
			DateTime retSrc = File.GetLastWriteTime(FileSrc);
			DateTime retDst = File.GetLastWriteTime(FileDst);

			bool ret = retSrc > retDst;

			return (ret);
		}

	    public static string GetAppName()
		{
			string str = "";

			
			string appPath; //アプリケーションのファイルパス
			string appFileName; //アプリケーションのファイル名
			string appName; //アプリケーションの拡張子を取り除いたアプリケーション名

			//パスの取得
#if FRAME_WORK_4
 			appPath = Application.ExecutablePath;
#else
			appPath = GetCurrentDirectory() ;//.ExecutablePath;
#endif
			//アプリケーションのファイル名
			appFileName = Path.GetFileName(appPath);


			//拡張子を除いたアプリケーションファイル名
			appName = Path.ChangeExtension(appFileName, null);

			return (appName);
		}

		public static string GetFileVersion()
		{
#if FRAME_WORK_4
 			string appPath = Application.ExecutablePath;
#else
            string appPath = GetCurrentDirectory();//.ExecutablePath;
#endif		
            //string appPath = Application.ExecutablePath; 

			// 指定したファイルのバージョン情報を取得する
			FileVersionInfo hVerInfo = FileVersionInfo.GetVersionInfo(appPath);

			return (hVerInfo.FileVersion);
		}


		

    }
}
