using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 心電図縦断面横断面図自動作成常駐アプリケーション
{
    public partial class Coronal : Form
    {
        
        //フォームのCreateParamsプロパティをオーバーライドする
        protected override CreateParams CreateParams
        {
            [System.Security.Permissions.SecurityPermission(
                System.Security.Permissions.SecurityAction.LinkDemand,
                Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                const int WS_EX_TOOLWINDOW = 0x80;
                const long WS_POPUP = 0x80000000L;
                const int WS_VISIBLE = 0x10000000;
                const int WS_SYSMENU = 0x80000;
                const int WS_MAXIMIZEBOX = 0x10000;

                CreateParams cp = base.CreateParams;
                cp.ExStyle = WS_EX_TOOLWINDOW;
                cp.Style = unchecked((int)WS_POPUP) |
                    WS_VISIBLE | WS_SYSMENU | WS_MAXIMIZEBOX;
                cp.Width = 0;
                cp.Height = 0;

                return cp;
            }
        }
        
        Graphics canvasG;
        


        public Coronal()
        {
            
            InitializeComponent();

            iniFileReader inifile = new iniFileReader(@"ECG.ini");
            String sImgpath_source;
            String sImgpath_output;
            String sJpglist = string.Empty;
            string sSourceImageFileName;
            string sAddFileName;
            int iPaste_h;
            int iPaste_w;
            int c_img_x;
            int c_img_y;
            int c_img_w;
            int c_img_h;
            int p_img_x;
            int p_img_y;
            int p_img_w;
            int p_img_h;
            int p_img_to_x;
            int p_img_to_y;



            //元データ格納フォルダパス取得
            sImgpath_source = inifile.getIniDataString("FOLDER", "imgpath_source");
            //出力データ格納先フォルダパス取得
            sImgpath_output = inifile.getIniDataString("FOLDER", "imgpath_output");
            //出力画像末尾に不可する文字列を取得
            sAddFileName = inifile.getIniDataString("SETTINGS", "Coronal_name");

            //元データ格納フォルダからjpegファイルの一覧を取得(sJpglistへ格納)
            foreach (string sFilePath in System.IO.Directory.GetFiles(@sImgpath_source, "*.jpg"))
            {
                sJpglist += sFilePath + System.Environment.NewLine;
            }
            //取得したjpegファイルの一覧を配列saJpglistへ格納
            sJpglist = sJpglist.Replace("\r", "");
            string[] saJpglist = sJpglist.Split('\n');

            //画像切り取り⇒貼り付け時の幅、高さ定義取得
            iPaste_h = inifile.getIniDataInt("C_IMGPASTE", "h");
            iPaste_w = inifile.getIniDataInt("C_IMGPASTE", "w");

            try
            {
                //定義フォルダ存在するJPEG画像の数だけ処理繰り返し
                foreach (string sJpgFile in saJpglist)
                {
                    Bitmap bmp = new Bitmap(pictureBox1.Image);

                    canvasG = Graphics.FromImage(bmp);

                    sSourceImageFileName = Path.GetFileNameWithoutExtension(sJpgFile);


                    //pictureBox1の元画像をBitmapクラスimageとして取得
                    Bitmap image = new Bitmap(pictureBox1.Image);
                    //元データからjpegファイルを取得

                    //取得したjpegファイルをBitmapクラスsource_imageとして取得
                    Bitmap jpg_image = new Bitmap(@sJpgFile);

                    //出力画像生成処理（ループ）へ
                    for (int i = 1; i <= 6; ++i)
                    {

                        //配置座標x、y、切り取りの幅、高さのw、ｈをiniファイルより取得
                        c_img_x = inifile.getIniDataInt("C_IMGCUT", "x" + i);
                        c_img_y = inifile.getIniDataInt("C_IMGCUT", "y" + i);
                        c_img_w = inifile.getIniDataInt("C_IMGCUT", "w" + i);
                        c_img_h = inifile.getIniDataInt("C_IMGCUT", "h" + i);

                        //貼り付け先配置座標x,y、切り取りの幅、高さのw,hをiniファイルより取得
                        p_img_x = inifile.getIniDataInt("C_IMGPASTE", "x" + i);
                        p_img_y = inifile.getIniDataInt("C_IMGPASTE", "y" + i);
                        p_img_to_x = inifile.getIniDataInt("C_IMGPASTE", "to_x" + i);
                        p_img_to_y = inifile.getIniDataInt("C_IMGPASTE", "to_y" + i);

                        
                        //source_imageから切り出す座標、高さ、幅を定義
                        Rectangle rctSrcCutImg = new Rectangle(c_img_x, c_img_y, c_img_w, c_img_h);
                        //PictureBoxに貼り付ける座標、高さ、幅を定義
                        Rectangle rctPcsPstImg = new Rectangle(p_img_x, p_img_y, iPaste_w, iPaste_h);
                        //PictureBoxに反転させて貼り付ける座標、高さ、幅を定義
                        Rectangle rctPcsPstToImg = new Rectangle(p_img_to_x, p_img_to_y + iPaste_h, iPaste_w, -iPaste_h);

                        //描画モード指定。縮小貼り付けによる画像劣化防止
                        canvasG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        //PictureBoxにjpg画像から切り出した画像を指定定義で貼付け
                        canvasG.DrawImage(jpg_image, rctPcsPstImg, rctSrcCutImg, GraphicsUnit.Pixel);

                        //PictureBoxにjpg画像から切り出した画像を反転させた上で指定定義で貼付け
                        canvasG.DrawImage(jpg_image, rctPcsPstToImg, rctSrcCutImg, GraphicsUnit.Pixel);

                        pictureBox1.Image = bmp;
                    }
                    string sCompleteFolder = @sImgpath_output + "\\" + sSourceImageFileName;
                    //出力先定義フォルダにデータ出力用フォルダPatientFolderを生成
                    DirectoryInfo di = Directory.CreateDirectory(sCompleteFolder);
                    //iniファイルをPatientFolderにコピー
                    //File.Copy(@"ECG.ini", sCompleteFolder + "\\ECG.ini");
                    //元データ（画像）をPatientFolderに移動
                    //File.Copy(@sJpgFile, sCompleteFolder + "\\" + sSourceImageFileName + ".jpg",true);

                    //File.Copy(@sJpgFile, sCompleteFolder + "\\test.jpg");
                    //pictureBox画像をJPEGファイルとして書き出し、PatientFolderへ出力
                    pictureBox1.Image.Save(sCompleteFolder + "\\" + sSourceImageFileName + sAddFileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                    bmp.Dispose();
                    image.Dispose();
                    jpg_image.Dispose();
                    //元データ（画像）を作業用フォルダから削除
                    //※Tranverse画像作成時に患者データ生成フォルダにコピーされている※
                    DeleteFile(@sJpgFile);
                }

            }
            catch
            {
                //エラー内容をログに出力

            }
            finally
            {
                
                Dispose();
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
        }
        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     指定したファイルを削除します。</summary>
        /// <param name="stFilePath">
        ///     削除するファイルまでのパス。</param>
        /// -----------------------------------------------------------------------------
        public static void DeleteFile(string stFilePath)
        {
            System.IO.FileInfo cFileInfo = new System.IO.FileInfo(stFilePath);

            // ファイルが存在しているか判断する
            if (cFileInfo.Exists)
            {
                // 読み取り専用属性がある場合は、読み取り専用属性を解除する
                if ((cFileInfo.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                {
                    cFileInfo.Attributes = System.IO.FileAttributes.Normal;
                }

                // ファイルを削除する
                //cFileInfo.Delete();
                System.IO.File.Delete(@stFilePath);
                
            }
        }


    }
}
