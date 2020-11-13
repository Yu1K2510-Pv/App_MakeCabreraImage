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
    public partial class Axial : Form
    {
        Graphics canvasG;
        
        public Axial()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            /*
            Bitmap img1 = new Bitmap("srcimage.jpg");
            Bitmap img2 = new Bitmap("logo.jpg");
            Bitmap img3 = new Bitmap(img1);
            

            Graphics g = Graphics.FromImage(img3);
            g.DrawImage(img2, img3.Width - img2.Width, img3.Height - img2.Height, img2.Width, img2.Height);
            g.Dispose();
            img1.Dispose();
            img2.Dispose();
            img3.Save("newImage.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            */
            //int i = 0;
            
            
                
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
            sAddFileName = inifile.getIniDataString("SETTINGS", "axial_name");

            //元データ格納フォルダからjpegファイルの一覧を取得(sJpglistへ格納)
            foreach (string sFilePath in System.IO.Directory.GetFiles(@sImgpath_source, "*.jpg"))
            {
                sJpglist += sFilePath + System.Environment.NewLine;
            }
            //取得したjpegファイルの一覧を配列saJpglistへ格納
            sJpglist = sJpglist.Replace("\r", "");
            string[] saJpglist = sJpglist.Split('\n');
            
            //画像切り取り⇒貼り付け時の幅、高さ定義取得
            iPaste_h = inifile.getIniDataInt("A_IMGPASTE", "h");
            iPaste_w = inifile.getIniDataInt("A_IMGPASTE", "w");

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
                        c_img_x = inifile.getIniDataInt("A_IMGCUT", "x" + i);
                        c_img_y = inifile.getIniDataInt("A_IMGCUT", "y" + i);
                        c_img_w = inifile.getIniDataInt("A_IMGCUT", "w" + i);
                        c_img_h = inifile.getIniDataInt("A_IMGCUT", "h" + i);

                        //貼り付け先配置座標x,y、切り取りの幅、高さのw,hをiniファイルより取得
                        p_img_x = inifile.getIniDataInt("A_IMGPASTE", "x" + i);
                        p_img_y = inifile.getIniDataInt("A_IMGPASTE", "y" + i);
                        p_img_to_x = inifile.getIniDataInt("A_IMGPASTE", "to_x" + i);
                        p_img_to_y = inifile.getIniDataInt("A_IMGPASTE", "to_y" + i);

                        /*
                        switch (i)
                        {
                            //I　貼り付け先座標定義
                            case 1:
                                p_img_x = 480;
                                p_img_y = 198;
                                p_to_img_x = 66;
                                p_to_img_y = 198;
                                break;
                            //Ⅱ　貼り付け先座標定義
                            case 2:
                                p_img_x = 392;
                                p_img_y = 381;
                                p_to_img_x = 160;
                                p_to_img_y = 14;
                                break;
                            //Ⅲ　貼り付け先座標定義
                            case 3:
                                p_img_x = 160;
                                p_img_y = 381;
                                p_to_img_x = 390;
                                p_to_img_y = 14;
                                break;
                            //aVR　貼り付け先座標定義
                            case 4:
                                p_img_x = 77;
                                p_img_y = 98;
                                p_to_img_x = 457;
                                p_to_img_y = 291;
                                break;
                            //aVL　貼り付け先座標定義
                            case 5:
                                p_img_x = 460;
                                p_img_y = 104;
                                p_to_img_x = 82;
                                p_to_img_y = 288;
                                break;
                            //aVF　貼り付け先座標定義
                            case 6:
                                p_img_x = 275;
                                p_img_y = 396;
                                p_to_img_x = 275;
                                p_to_img_y = 0;
                                break;
                            
                            //V1　貼り付け先座標定義
                            case 7:
                                p_img_x = 100;
                                p_img_y = 100;
                                p_to_img_x = 500;
                                p_to_img_y = 500;
                                break;
                            //V2　貼り付け先座標定義
                            case 8:
                                p_img_x = 100;
                                p_img_y = 100;
                                p_to_img_x = 500;
                                p_to_img_y = 500;
                                break;
                            //V3　貼り付け先座標定義
                            case 9:
                                p_img_x = 100;
                                p_img_y = 100;
                                p_to_img_x = 500;
                                p_to_img_y = 500;
                                break;
                            //V4　貼り付け先座標定義
                            case 10:
                                p_img_x = 100;
                                p_img_y = 100;
                                p_to_img_x = 500;
                                p_to_img_y = 500;
                                break;
                            //V5　貼り付け先座標定義
                            case 11:
                                p_img_x = 100;
                                p_img_y = 100;
                                p_to_img_x = 500;
                                p_to_img_y = 500;
                                break;
                            //V6　貼り付け先座標定義
                            case 12:
                                p_img_x = 100;
                                p_img_y = 100;
                                p_to_img_x = 500;
                                p_to_img_y = 500;
                                break;
                            
                        }*/
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
                    File.Copy(@"ECG.ini", sCompleteFolder + "\\ECG.ini");
                    //元データ（画像）をPatientFolderに移動
                    File.Copy(@sJpgFile, sCompleteFolder + "\\" + sSourceImageFileName + ".jpg");
                    //File.Copy(@sJpgFile, sCompleteFolder + "\\test.jpg");
                    //pictureBox画像をJPEGファイルとして書き出し、PatientFolderへ出力
                    pictureBox1.Image.Save(sCompleteFolder + "\\" + sSourceImageFileName + sAddFileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch
            {
                //エラー内容をログに出力

            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


    }
}
