using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace Server
{
    public partial class Form1 : Form
    {
        Socket mainSoc;

        List<BinaryWriter> lstSoc = new List<BinaryWriter>();

        ArrayList lstID = new ArrayList();

        int count = 0;

        // I used this MRE to synch the asynch calls
        ManualResetEvent mre = new ManualResetEvent(false);

        public Form1()
        {
            InitializeComponent();
        }

        Thread AdvertiseThread;
        int PortNum = 5000;
        public void Advetise()
        {
            // Advertise for a server operating on port 5000, note that the advetisments themselves are 
            //disseminated on port 5001 using the udp socket
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // I used my loopback address beacuse I'm tring offline in home
            //If U R trying in FCI use a real broadcast Address
            IPEndPoint broadCast = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5001);
            // Allow multiple sockets to bind to the same address and port
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

            //every 10 sec
            while (true)
            {
                s.SendTo(Encoding.UTF8.GetBytes(Dns.GetHostName() + ":" + PortNum.ToString()), broadCast);
                Thread.Sleep(5000);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Let it go 

            AdvertiseThread = new Thread(new ThreadStart(Advetise));

            AdvertiseThread.Priority = ThreadPriority.Lowest;

            AdvertiseThread.Start();


            // lets do our main work, serving
            mainSoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            mainSoc.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), PortNum));

            mainSoc.Listen(-1);

            new Thread(new ThreadStart(StartServer)).Start();

            button1.Enabled = false;
        }
        public void StartServer()
        {

            while (true)
            {
                mre.Reset();

                mainSoc.BeginAccept(new AsyncCallback(AcceptClient), null);

                // wait to complete handling 
                mre.WaitOne();
            }
        }
        public void AcceptClient(IAsyncResult iar)
        {

            Socket client = mainSoc.EndAccept(iar);

            new Thread(new ParameterizedThreadStart(ReadData)).Start(client);

            // done handling
            mre.Set();
        }
        public void ReadData(object socket)
        {
            try
            {
                Socket Msoc = socket as Socket;

                ++count;

                lstID.Add("Client #" + count.ToString());

                lstSoc.Add(new BinaryWriter(new NetworkStream(Msoc)));

                this.Invoke((MethodInvoker)delegate {
                    listBox1.Items.Add("Client #" + count.ToString());
                });

                string message = "Client #" + count.ToString();
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);

                // include the length of the message in the first 4 bytes
                byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);
                byte[] sendData = new byte[messageBytes.Length + lengthBytes.Length];
                Array.Copy(lengthBytes, 0, sendData, 0, lengthBytes.Length);
                Array.Copy(messageBytes, 0, sendData, lengthBytes.Length, messageBytes.Length);

                NetworkStream stream = new NetworkStream(Msoc);
                stream.Write(sendData, 0, sendData.Length);

                BinaryReader reader = new BinaryReader(stream);

                string from, to, fullmessage;
                while (Msoc.Connected)
                {
                    // read the length of the message from the client
                    byte[] lengthBuffer = new byte[sizeof(int)];
                    int lengthReceived = stream.Read(lengthBuffer, 0, lengthBuffer.Length);
                    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                    // read the full message from the client
                    byte[] messageBuffer = new byte[messageLength];
                    int bytesReceived = stream.Read(messageBuffer, 0, messageBuffer.Length);
                    fullmessage = Encoding.ASCII.GetString(messageBuffer, 0, bytesReceived);

                    from = fullmessage.Split(new char[] { '~' })[0];

                    to = fullmessage.Split(new char[] { '~' })[1];

                    message = fullmessage.Split(new char[] { '~' })[2];


                    if (message == "Bye")
                    {
                        int x = Find(from);

                        lstID.RemoveAt(x);

                        lstID.TrimToSize();

                        lstSoc.RemoveAt(x);

                        listBox1.Items.Remove(from);

                        break;
                    }
                    else if (message == "img")
                    {
                        // Active the contion on the cilent 
                        this.Invoke((MethodInvoker)delegate
                        {
                            listBox2.Items.Add(from + ":");
                        });

                        this.Invoke((MethodInvoker)delegate
                        {
                            listBox2.Items.Add(message + ".");
                        });

                        string messSend = from + ": " + message;
                        messageBytes = Encoding.ASCII.GetBytes(messSend);
                        int y = Find(to);
                        try
                        {
                            //Recive send word img 
                            lengthBytes = BitConverter.GetBytes(messageBytes.Length);
                            sendData = new byte[messageBytes.Length + lengthBytes.Length];
                            Array.Copy(lengthBytes, 0, sendData, 0, lengthBytes.Length);
                            Array.Copy(messageBytes, 0, sendData, lengthBytes.Length, messageBytes.Length);

                            // write the message to the output stream
                            lstSoc[y].Write(sendData, 0, sendData.Length);
                            lstSoc[y].Flush();
                        }
                        catch (Exception ff) { MessageBox.Show(ff.Message); }
                        string filename = "";
                        int fileLength = 0;
                        int bytesRead = 0;
                        int totalBytesRead = 0;

                        // read the length of the filename from the client
                        lengthBuffer = new byte[sizeof(int)];
                        lengthReceived = stream.Read(lengthBuffer, 0, lengthBuffer.Length);
                        messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                        // read the filename from the client
                        byte[] fileBuffer = new byte[messageLength];
                        bytesReceived = stream.Read(fileBuffer, 0, fileBuffer.Length);
                        filename = Encoding.ASCII.GetString(fileBuffer, 0, bytesReceived);

                        // read the size of file from the client
                        byte[] sizeBuffer = new byte[sizeof(int)];
                        int sizeReceived = stream.Read(sizeBuffer, 0, sizeBuffer.Length);
                        int sizeLength = BitConverter.ToInt32(sizeBuffer, 0);

                        // read the full message from the client
                        fileBuffer = new byte[sizeLength];

                        while (totalBytesRead < sizeLength)
                        {
                            bytesRead = stream.Read(fileBuffer, totalBytesRead, fileBuffer.Length - totalBytesRead);
                            totalBytesRead += bytesRead;
                        }

                        // create a file with the received filename
                        using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                        {
                            // write the received file contents to the file
                            fileStream.Write(fileBuffer, 0, totalBytesRead);
                        }
                        //==============================================================
                       // send the File to it's destntion 
                        ASCIIEncoding ascencoding = new ASCIIEncoding();
                       byte[] filenameBytes = ascencoding.GetBytes(filename);
                        lengthBytes = BitConverter.GetBytes(filenameBytes.Length);
                        sendData = new byte[filenameBytes.Length + lengthBytes.Length];
                        Array.Copy(lengthBytes, 0, sendData, 0, lengthBytes.Length);
                        Array.Copy(filenameBytes, 0, sendData, lengthBytes.Length, filenameBytes.Length);

                        // Send the filename and its length to the server
                        lstSoc[y].Write(sendData, 0, sendData.Length);

                        using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            fileBuffer = new byte[fileStream.Length];
                            fileStream.Read(fileBuffer, 0, fileBuffer.Length);

                            lengthBytes = BitConverter.GetBytes(fileBuffer.Length);
                            sendData = new byte[fileBuffer.Length + lengthBytes.Length];

                            Array.Copy(lengthBytes, 0, sendData, 0, lengthBytes.Length);
                            Array.Copy(fileBuffer, 0, sendData, lengthBytes.Length, fileBuffer.Length);

                            lstSoc[y].Write(sendData, 0, sendData.Length);
                        }

                    }
                    
                    else
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            listBox2.Items.Add(from + ":");
                        });

                        this.Invoke((MethodInvoker)delegate
                        {
                            listBox2.Items.Add(message + ".");
                        });

                        string messSend = from+": "+message ;
                        messageBytes = Encoding.ASCII.GetBytes(messSend);


                        int y = Find(to);
                        try
                        {
                            lengthBytes = BitConverter.GetBytes(messageBytes.Length);
                            sendData = new byte[messageBytes.Length + lengthBytes.Length];
                            Array.Copy(lengthBytes, 0, sendData, 0, lengthBytes.Length);
                            Array.Copy(messageBytes, 0, sendData, lengthBytes.Length, messageBytes.Length);

                            // write the message to the output stream
                            lstSoc[y].Write(sendData, 0, sendData.Length);
                            lstSoc[y].Flush();
                        }
                        catch (Exception ff) { MessageBox.Show(ff.Message); }

                    }

                }
                MessageBox.Show("Disconnect");
                reader.Close();

                Msoc.Close();
            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }
        }

        private int Find(string from)
        {
            for (int i = 0; i < lstID.Count; i++)
            {
                if (lstID[i] as string == from)
                    return i;
            }
            return -1;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < lstSoc.Count; i++)
            {
                string message = "Server~Bye";
                byte[] lengthBytes = BitConverter.GetBytes(message.Length);
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                byte[] sendData = new byte[messageBytes.Length + lengthBytes.Length];
                Array.Copy(lengthBytes, 0, sendData, 0, lengthBytes.Length);
                Array.Copy(messageBytes, 0, sendData, lengthBytes.Length, messageBytes.Length);

                lstSoc[i].Write(sendData, 0, sendData.Length);
            }

            Application.ExitThread();

            Environment.Exit(Environment.ExitCode);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int x = Find(listBox1.SelectedItem.ToString());

            try
            {

                string message = "Server~" + textBox1.Text;

                // include the length of the message in the first 4 bytes
                byte[] lengthBytes = BitConverter.GetBytes(message.Length);
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                byte[] sendData = new byte[messageBytes.Length + lengthBytes.Length];
                Array.Copy(lengthBytes, 0, sendData, 0, lengthBytes.Length);
                Array.Copy(messageBytes, 0, sendData, lengthBytes.Length, messageBytes.Length);

                lstSoc[x].Write(sendData, 0, sendData.Length);
                lstSoc[x].Flush();
                this.Invoke((MethodInvoker)delegate
                {
                    listBox2.Items.Add(message);
                });
                textBox1.Text = "";


            }
            catch (Exception ff) { MessageBox.Show(ff.Message); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
                
        }
    }
}
