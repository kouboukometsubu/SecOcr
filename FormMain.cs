using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GClass;

namespace SecOcr
{
    public partial class FormMain : Form
    {

        string appPath = "";
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllowDrop = true;

            DragDrop += FormMain_DragDrop;
            DragEnter += FormMain_DragEnter;

           appPath = AppTool.GetRootFolder() + "\\Data";

        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            //ドラッグされているデータがstring型か調べ、
            //そうであればドロップ効果をMoveにする
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }

            else
            {
                //string型でなければ受け入れない
                e.Effect = DragDropEffects.None;
            }//throw new NotImplementedException();
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames =
            (string[])e.Data.GetData(DataFormats.FileDrop, false);

            OpenFile(fileNames[0]);
        }

        void OpenFile( string filename )
        {

            //言語ファイルの格納先
            string langPath = appPath ;

            //言語（日本語なら"jpn"）
            string lngStr = "jpn";
            lngStr = "eng";

            //画像ファイル
            //var img = new Bitmap(filename);
            Tesseract.Pix img = null;
            try
            {
                img = Tesseract.Pix.LoadFromFile(filename);
            }
            catch(ExecutionEngineException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            using (var tesseract = new Tesseract.TesseractEngine(langPath, lngStr))
            {
               
                //tesseract.SetVariable( "VAR_CHAR_WHITELIST", ".,0123456789");

                // OCRの実行
                Tesseract.Page page = tesseract.Process(img);

                //表示
                Console.WriteLine(page.GetText());
                Console.ReadLine();

                MessageBox.Show(page.GetText());

                Clipboard.SetText(page.GetText());
            }
        }
    }


    
}
