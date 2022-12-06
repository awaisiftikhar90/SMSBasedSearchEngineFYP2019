using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.ComponentModel;
using System.IO.Ports;
using System.Threading;

namespace GoogleSearcAPClient
{
    public class SMSCOMMS
    {
        private SerialPort SMSPort;
        private Thread SMSThread;
        private Thread ReadThread;
        public static bool _Continue = false;
        public static bool _ContSMS = false;
        private bool _Wait = false;
        public static bool _ReadPort = false;
        public delegate void SendingEventHandler(bool Done);
        public event SendingEventHandler Sending;
        public delegate void DataReceivedEventHandler(string Message);
        public event DataReceivedEventHandler DataReceived;

        public SMSCOMMS(ref string COMMPORT)
        {
            SMSPort = new SerialPort();
            SMSPort.PortName = COMMPORT;
            SMSPort.BaudRate = 9600;
            SMSPort.Parity = Parity.None;
            SMSPort.DataBits = 8;
            SMSPort.StopBits = StopBits.One;
            SMSPort.Handshake = Handshake.RequestToSend;
            SMSPort.DtrEnable = true;
            SMSPort.RtsEnable = true;
            SMSPort.NewLine = System.Environment.NewLine;
            ReadThread = new Thread(
                new System.Threading.ThreadStart(ReadPort));

            SendSMS("+923155117446", "THIS IS YOUR MESSAGE");
        }

        

        private void ReadPort()
        {
            string SerialIn = null;
            byte[] RXBuffer = new byte[SMSPort.ReadBufferSize + 1];
            string SMSMessage = null;
            int Strpos = 0;
            string TmpStr = null;
            while (SMSPort.IsOpen == true)
            {
                if ((SMSPort.BytesToRead != 0) & (SMSPort.IsOpen == true))
                {
                    while (SMSPort.BytesToRead != 0)
                    {
                        SMSPort.Read(RXBuffer, 0, SMSPort.ReadBufferSize);
                        SerialIn =
                            SerialIn + System.Text.Encoding.ASCII.GetString(
                            RXBuffer);
                        if (SerialIn.Contains(">") == true)
                        {
                            _ContSMS = true;
                        }
                        if (SerialIn.Contains("+CMGS:") == true)
                        {
                            _Continue = true;
                            if (Sending != null)
                                Sending(true);
                            _Wait = false;
                            SerialIn = string.Empty;
                            RXBuffer = new byte[SMSPort.ReadBufferSize + 1];
                        }
                    }
                    if (DataReceived != null)
                        DataReceived(SerialIn);
                    SerialIn = string.Empty;
                    RXBuffer = new byte[SMSPort.ReadBufferSize + 1];
                }
            }
        }

        public bool SendSMS(string CellNumber, string SMSMessage)
        {
            string MyMessage = null;
            if (SMSMessage.Length <= 160)
            {
                MyMessage = SMSMessage;
            }
            else
            {
                MyMessage = SMSMessage.Substring(0, 160);
            }
            if (SMSPort.IsOpen == true)
            {
                SMSPort.WriteLine("AT+CMGS=" + CellNumber + "r");
                _ContSMS = false;
                SMSPort.WriteLine(
                    MyMessage + System.Environment.NewLine + (char)(26));
                _Continue = false;
                if (Sending != null)
                    Sending(false);
            }
            return false;
        }

        public void Open()
        {
            if (SMSPort.IsOpen == false)
            {
                SMSPort.Open();
                ReadThread.Start();
            }
        }

        public void Close()
        {
            if (SMSPort.IsOpen == true)
            {
                SMSPort.Close();
            }
        }

    }
}
