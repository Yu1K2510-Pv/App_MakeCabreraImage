using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;


namespace 心電図縦断面横断面図自動作成常駐アプリケーション
{
    
    
    static class Program
    {
        
        //public System.Windows.Forms.NotifyIcon ECGIcon;
        
        
       [STAThread]


        static void Main()
        {

            Application.Run(new frmCabrera());
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //new Axial();
            //Application.Run(new frmSettings_Auto());
            /*

            Transverse tForm = new Transverse();
            IntPtr dummy_t = tForm.Handle;

            Coronal cForm = new Coronal();
            IntPtr dummy_c = cForm.Handle;
            
            */
            /*
             Application.EnableVisualStyles();
             Application.SetCompatibleTextRenderingDefault(false);
            
             new Transverse();
             new Coronal();
           
           

             
            */
            //フォーム呼び出しモード確認
                //設定フォーム展開
                //編集フォーム展開
    
        }
       
    }


 
}
