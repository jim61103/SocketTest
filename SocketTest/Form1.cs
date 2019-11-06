using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SocketTest
{
    public partial class Form1 : Form
    {
        delegate void Display(string msg); 
        static Thread ThreadClient = null;
        static Socket SocketClient = null;
        RichTextBox rtb;
        public Form1()
        {
            InitializeComponent();
            rtb = richTextBox1;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string port = textBox2.Text;
            string ip = textBox1.Text;
            SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(ip);
            System.Net.IPEndPoint remoteEP = new IPEndPoint(ipAdd, int.Parse(port));

            try
            {
                SocketClient.Connect(remoteEP);
            }
            catch (Exception ex)
            {

            }
            ThreadClient = new Thread(Recv);
            ThreadClient.IsBackground = true;
            ThreadClient.Start();
            Thread.Sleep(1000);


        }
        public  void ClientSendMsg(string sendMsg)
        {
            rtb.AppendText("C->H " + sendMsg + "\r\n");
            //將輸入的內容字串轉換為機器可以識別的位元組陣列     
            byte[] arrClientSendMsg = Encoding.ASCII.GetBytes(sendMsg);
            //呼叫客戶端套接字傳送位元組陣列     
            SocketClient.Send(arrClientSendMsg);
            //test
        }

        //接收服務端發來資訊的方法    
        public void Recv()
        {
            //持續監聽服務端發來的訊息 
            while (true)
            {
                try
                {
                    //定義一個1M的記憶體緩衝區，用於臨時性儲存接收到的訊息  
                    byte[] arrRecvmsg = new byte[1024 * 1024];
                    //將客戶端套接字接收到的資料存入記憶體緩衝區，並獲取長度  
                    int length = SocketClient.Receive(arrRecvmsg);
                    //將套接字獲取到的字元陣列轉換為人可以看懂的字串  
                    string strRevMsg = Encoding.ASCII.GetString(arrRecvmsg, 0, length);
                    Display d = new Display(ShowMessage);
                    this.Invoke(d, new Object[] { strRevMsg });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("遠端伺服器已經中斷連線！" + ex.Message + "\r\n");
                    break;
                }
            }
        }

        private void ShowMessage(string Msg)
        {
            rtb.AppendText("H->C " + Msg);
        }

        private void TextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ClientSendMsg(textBox3.Text);
                textBox3.Clear();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //foreach (Process p in Process.GetProcessesByName("EXCEL"))
            //{
            //    MessageBox.Show(p.MainWindowTitle);
            //}
        }
    }
}
