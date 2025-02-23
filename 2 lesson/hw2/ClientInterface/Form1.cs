using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ClientInterface {
    public partial class Form1 :Form {
        private TcpClient client;
        private NetworkStream stream;

        public Form1() {
            InitializeComponent();

            try {
                client = new TcpClient("127.0.0.1", 49152);
                stream = client.GetStream();


                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex) {
                MessageBox.Show("Клиент: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            if (client != null && client.Connected) {
                string message = textBox2.Text;
                textBox1.Text += "Вы: " + message + Environment.NewLine;
                textBox2.Text = "";

                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            } else {
                MessageBox.Show("Нет соединения с сервером.");
            }
        }

        private void ReceiveMessages() {
            byte[] buffer = new byte[1024];
            int bytesRead;

            try {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0) {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Invoke(new Action(() => textBox1.AppendText("Сообщение от сервера: " + message + Environment.NewLine)));
                }
            }
            catch {
                MessageBox.Show("Отключено от сервера.");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            stream?.Close();
            client?.Close();
            base.OnFormClosing(e);
        }
    }
}