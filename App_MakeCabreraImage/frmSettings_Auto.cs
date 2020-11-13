using System;
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
    public partial class frmSettings_Auto : Form
    {
        private int    WM_SYSCOMMAND = 0x112;
        private IntPtr SC_MINIMIZE   = (IntPtr)0xF020;

        public frmSettings_Auto()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            //最小化されたときにフォームを非表示にする
            if ((m.Msg == WM_SYSCOMMAND) && (m.WParam == SC_MINIMIZE))
            {
                this.Hide();
            }
            //上記以外はデフォルトの処理をおこなう
            else
            {
                base.WndProc(ref m);
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //タスクトレイのアイコンダブルクリックでフォームを表示、アクティブ化
            this.Show();
            this.Activate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームを閉じる時、タスクトレイに表示されている場合は閉じずに非表示
            if (notifyIcon1.Visible)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        //NotifyIconのコンテキストメニューに追加した「閉じる」メニュークリックイベント        
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //NotifyIconのコンテキストメニューで閉じるメニューを実行した場合
            //NotifyIconを非表示にしてアプリケーションを終了する
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        private void frmSettings_Auto_Load(object sender, EventArgs e)
        {
            //画面初期化設定
            //
            //config設定読み込み
            //モード選択設定

            //設定タブ選択
            //モード確認
                //自動出力
                    //自動出力間隔設定
                    //元画像自動削除設定
                //マニュアル出力
                    //ショートカットキー読み込み
                    //元画像自動削除設定
            //源画像読み込み元パス取得
            //縦断面図加工後画像保存場所
            //

        }

        private void 自動出力ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("心電図縦断面図自動作成ツールを終了しますか？");
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
