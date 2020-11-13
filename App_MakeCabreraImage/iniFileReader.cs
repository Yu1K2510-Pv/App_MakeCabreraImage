//読み込みクラス
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 心電図縦断面横断面図自動作成常駐アプリケーション
{

    class iniFileReader
    {
        struct SData
        {
            public string value;
            public string section;
            public string name;
        };

        //データ格納変数
        private List<SData> iniData = new List<SData>();

        //コンストラクタ
        public iniFileReader(string path)
        {
            fileRead(path);
        }


        public int getIniDataInt(String section, String name)
        {
            String temp = sarchData(section, name);
            if (temp.Length != 0)
                    return int.Parse(temp);
            return 0;
        }
        public double getIniDataDouble(String section, String name)
        {
            String temp = sarchData(section, name);
            if (temp.Length != 0)
                return double.Parse(temp);
            return 0;
        }
        public String getIniDataString(String section, String name)
        {
            return sarchData(section, name);
        }


        //以下非公開メンバ
        private String sarchData(String section, String name)
        {
            //for (int i = 0; i < iniData.; i++)
            foreach (SData dataElement in iniData) //自動的にdataElementにデータを入れてくれる
            {
                if (dataElement.section == section && dataElement.name == name)//同じものを探す
                {
                    return dataElement.value;
                }
            }
            return "";
        }



        private void fileRead(string path)
        {
            // StreamReader の新しいインスタンスを生成する
            System.IO.StreamReader cReader = (
                new System.IO.StreamReader(path, System.Text.Encoding.Default)
            );

            // 読み込んだ結果をすべて格納するための変数を宣言する
            string stResult = string.Empty;
            string section = string.Empty; //セクション

            // 読み込みできる文字がなくなるまで繰り返す
            while (cReader.Peek() >= 0)
            {
                // ファイルを 1 行ずつ読み込む
                string stBuffer = cReader.ReadLine();
                //Console.WriteLine(stBuffer);


                if (stBuffer.Length != 0) //中身があったら
                {
                    if (stBuffer.ElementAt(0) == '[')
                    {
                        section = stBuffer.Substring(1, stBuffer.Length - 2);
                        //Console.WriteLine(section);//セクションを入れる
                    }
                    else if (stBuffer.ElementAt(0) != ';') //コメント行はスルーする
                    {
                        string[] data = stBuffer.Split('=');
                        //for (int i = 0; i < data.Length; i++)
                        //{
                        //    Console.Write(data[i]);
                        //    Console.Write(" ");
                        //}
                        //Console.WriteLine();

                        if (data.Length >= 2)
                        {
                            SData temp;
                            temp.section = section;
                            temp.name = data[0].Trim();
                            temp.value = data[1].Trim();
                            //Console.WriteLine(temp.section + " " + temp.name + " " + temp.value);
                            iniData.Add(temp);
                        }

                    }
                }



                // 読み込んだものを追加で格納する
                //stResult += stBuffer + System.Environment.NewLine;
            }

            // cReader を閉じる (正しくは オブジェクトの破棄を保証する を参照)
            cReader.Close();

        }
    }
}

