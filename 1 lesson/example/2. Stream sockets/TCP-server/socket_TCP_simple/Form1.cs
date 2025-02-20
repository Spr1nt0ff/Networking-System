using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace socket_TCP_simple
{
    public partial class Form1 : Form
    {
        public SynchronizationContext uiContext;
        public int count = 0;
        public Queue<(string client, string data, int tl)> orderQueue = new Queue<(string, string, int)>();
        private bool isProcessing = false;

        public Form1()
        {
            InitializeComponent();
            uiContext = SynchronizationContext.Current;
        }

        private async void Receive(Socket handler)
        {
            Dictionary<string, int> menu = new Dictionary<string, int> {
                { "hamburger", 5 },
                { "sprite", 1 },
                { "free potato", 3 }
            };
            await Task.Run(() =>
            {
                try
                {
                    string client = null;
                    string data = null;
                    byte[] bytes = new byte[1024];

                    int bytesRec = handler.Receive(bytes);
                    client = Encoding.Default.GetString(bytes, 0, bytesRec);
                    client += "(" + handler.RemoteEndPoint.ToString() + ")";
                    while (true)
                    {
                        bytesRec = handler.Receive(bytes);
                        if (bytesRec == 0)
                        {
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                            return;
                        }
                        data = Encoding.Default.GetString(bytes, 0, bytesRec);

                        uiContext.Send(d => listBox1.Items.Add(client), null);
                        uiContext.Send(d => listBox1.Items.Add($"Заказ принят: {data}"), null);

                        Dictionary<string, int> order = ParseOrder(data, menu);
                        int totalTime = 0;
                        foreach (var item in order)
                        {
                            totalTime += item.Value;
                        }

                        uiContext.Send(d => listBox1.Items.Add($"Ожидаемое время ожидания: {totalTime} секунд"), null);

                        lock (orderQueue)
                        {
                            orderQueue.Enqueue((client, data, totalTime));
                        }

                        ProcessQueue();

                        if (data.IndexOf("<end>") > -1)
                            break;
                    }

                    string theReply = "Я завершаю обработку сообщений";
                    byte[] msg = Encoding.Default.GetBytes(theReply);
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сервер: " + ex.Message);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            });
        }

        private async void ProcessQueue()
        {
            if (isProcessing)
            {
                return;
            }

            isProcessing = true;

            while (orderQueue.Count > 0)
            {
                (string client, string data, int totalTime) order;
                lock (orderQueue)
                {
                    order = orderQueue.Dequeue();
                }

                uiContext.Send(d => listBox1.Items.Add($"Готовится заказ от {order.client}: {order.data}"), null);
                await Task.Delay(order.totalTime * 1000);
                uiContext.Send(d => listBox1.Items.Add($"Заказ готов: {order.data}"), null);
            }

            isProcessing = false;
        }

        private async void Accept()
        {
            await Task.Run(() =>
            {
                try
                {
                    IPEndPoint ipEndPoint = new IPEndPoint(
                       IPAddress.Any,
                       49152);

                    Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    sListener.Bind(ipEndPoint);

                    sListener.Listen(10);
                    while (true)
                    {
                        Socket handler = sListener.Accept();
                        Receive(handler);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сервер: " + ex.Message);
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Accept();
        }

        static Dictionary<string, int> ParseOrder(string input, Dictionary<string, int> menu)
        {
            var order = new Dictionary<string, int>();

            foreach (var item in menu)
            {
                string pattern = @"(\d+)?\s*" + Regex.Escape(item.Key);
                Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    int count = 1;
                    if (match.Groups[1].Success)
                    {
                        count = int.Parse(match.Groups[1].Value);
                    }

                    order[item.Key] = item.Value * count;
                }
            }

            return order;
        }
    }
}
