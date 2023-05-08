using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ClinetGUI
{
    public partial class Form1 : Form
    {

        Socket sck;
        EndPoint remoteEp;
        byte[] buffer;
        IPAddress ip = IPAddress.Parse("127.0.0.1");

        string clientID = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Allow multiple sockets to bind to the same address and port
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 5001);
            socket.Bind(endPoint);

            byte[] buffer = new byte[1024];

            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Capture advertise packets and print IP and port
            while (true)
            {
                int receivedBytes = socket.ReceiveFrom(buffer, ref remoteEndPoint);

                string advertisePacket = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                string[] parts = advertisePacket.Split(':');

                if (parts.Length == 2)
                {
                    string ipAddress = ((IPEndPoint)remoteEndPoint).Address.ToString();
                    int port = int.Parse(parts[1]);

                    txtIpAddress.Text = ipAddress;
                    txtPort.Text = port.ToString();
                    socket.Close();
                    break;
                }
            }

            remoteEp = new IPEndPoint(IPAddress.Parse(txtIpAddress.Text), Convert.ToInt32(txtPort.Text));
            sck.Connect(remoteEp);

            buffer = new byte[1024];
            sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref remoteEp, new AsyncCallback(MessageCallBack), buffer);
        }

        private void MessageCallBack(IAsyncResult ar)
        {
            try
            {
                byte[] receiveData = new byte[1024];
                receiveData = (byte[])ar.AsyncState;
                // get the length of the message from the first 4 bytes
                int messageLength = BitConverter.ToInt32(receiveData, 0);

                // get the actual message bytes
                byte[] messageData = new byte[messageLength];
                Array.Copy(receiveData, 4, messageData, 0, messageLength);

                //Converting byte[] to string
                string response = Encoding.ASCII.GetString(messageData);

                if (response.Length == 9) 
                    clientID = response;

                string searchTerm = "img";
                if (response.Contains(searchTerm))
                {
                    string filename = "";
                    int fileLength = 0;
                    int bytesRead = 0;
                    int totalBytesRead = 0;

                    // read the length of the filename from the client
                    byte[] lengthBuffer = new byte[sizeof(int)];
                    byte[] lengthReceived;
                    sck.Receive(lengthBuffer, lengthBuffer.Length, SocketFlags.None);
                    messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                    // read the filename from the client
                    byte[] fileBuffer = new byte[messageLength];
                    int bytesReceived = sck.Receive(fileBuffer, fileBuffer.Length, SocketFlags.None);
                    filename = Encoding.ASCII.GetString(fileBuffer, 0, bytesReceived);

                    // read the size of file from the client
                    byte[] sizeBuffer = new byte[sizeof(int)];
                    int sizeReceived = sck.Receive(sizeBuffer, sizeBuffer.Length, SocketFlags.None);
                    int sizeLength = BitConverter.ToInt32(sizeBuffer, 0);

                    // read the full message from the client
                    fileBuffer = new byte[sizeLength];

                    while (totalBytesRead < sizeLength)
                    {
                        bytesRead = sck.Receive(fileBuffer, totalBytesRead, fileBuffer.Length - totalBytesRead, SocketFlags.None);
                        totalBytesRead += bytesRead;
                    }

                    // create a file with the received filename
                    using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                    {
                        // write the received file contents to the file
                        fileStream.Write(fileBuffer, 0, totalBytesRead);
                    }


                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBox1.Image = new Bitmap(filename);
                        textBox2.Text = filename;

                    });
                                   }
                //Adding message to listbox
                this.Invoke((MethodInvoker)delegate
                {
                    MessageList.Items.Add(response);
                });

                //Loop !
                buffer = new byte[1024];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref remoteEp, new AsyncCallback(MessageCallBack), buffer);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
           
            //setup socket
            sck = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // convert string message to byte[]
            ASCIIEncoding ascencoding = new ASCIIEncoding();
            string txtmess = clientID + "~" + "Client #" + textBox1.Text + "~" + textMessage.Text;
            byte[] messageBytes = ascencoding.GetBytes(txtmess);

            // include the length of the message in the first 4 bytes
            byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);
            byte[] sendData = new byte[messageBytes.Length + lengthBytes.Length];
            Array.Copy(lengthBytes, 0, sendData, 0, lengthBytes.Length);
            Array.Copy(messageBytes, 0, sendData, lengthBytes.Length, messageBytes.Length);

            // send the message data
            sck.Send(sendData, 0, sendData.Length, SocketFlags.None);
            if(textMessage.Text =="img")
            {
                string filename = textBox2.Text;
                ascencoding = new ASCIIEncoding(); byte[] filenameBytes = ascencoding.GetBytes(Path.GetFileName(filename));
                lengthBytes = BitConverter.GetBytes(filenameBytes.Length);
                sendData = new byte[filenameBytes.Length + lengthBytes.Length];

                Array.Copy(lengthBytes, 0, sendData, 0, lengthBytes.Length);
                Array.Copy(filenameBytes, 0, sendData, lengthBytes.Length, filenameBytes.Length);

                // send the filename and its length to the server
                sck.Send(sendData, 0, sendData.Length, SocketFlags.None);

                using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    byte[] fileBuffer = new byte[fileStream.Length];
                    fileStream.Read(fileBuffer, 0, fileBuffer.Length);

                    lengthBytes = BitConverter.GetBytes(fileBuffer.Length);
                    sendData = new byte[fileBuffer.Length + lengthBytes.Length];

                    Array.Copy(lengthBytes, 0, sendData, 0, lengthBytes.Length);
                    Array.Copy(fileBuffer, 0, sendData, lengthBytes.Length, fileBuffer.Length);

                    sck.Send(sendData, 0, sendData.Length, SocketFlags.None);
                }
            }

            MessageList.Items.Add("You Said:" + textMessage.Text);
            textMessage.Text = "";
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image Files(*.jpg; *.jpeg; *.gif;*.bmp;)|*.jpg; *.jpeg; *.gif; *.bmp;";
            if (of.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = of.FileName;
                pictureBox1.Image = new Bitmap(of.FileName);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }


            private void button4_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}