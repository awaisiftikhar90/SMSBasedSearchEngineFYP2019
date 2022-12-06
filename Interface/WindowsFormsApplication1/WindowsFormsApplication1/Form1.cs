using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using GsmComm.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private GsmCommMain comm;
        string[] portnames = { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9" };
        NotifyIcon nt = new NotifyIcon();
        public Form1()
        {
            InitializeComponent();
        }
        private void SetBalloonTip()
        {
            nt.Icon = SystemIcons.Exclamation;
            nt.BalloonTipTitle = "Balloon Tip Title";
            nt.BalloonTipText = "Balloon Tip Text.";
            nt.BalloonTipIcon = ToolTipIcon.Error;
            this.Click += new EventHandler(Form1_Load);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = SystemIcons.Exclamation;
            int numi = 0;

            comm = new GsmCommMain(portnames[numi], 9600, 300);
            Cursor.Current = Cursors.Default;
            bool retry;
            
            do
            {
                retry = false;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    comm.Open();
                    comm.EnableMessageNotifications();
                    comm.MessageReceived += new MessageReceivedEventHandler(Message_Received);
                    notifyIcon1.Visible = true;
                    notifyIcon1.ShowBalloonTip(10000, "Working Fine", "Connected to Modem", ToolTipIcon.Info);
                    comm.DeleteMessages(DeleteScope.All, "SM");
                    notifyIcon1.Visible = true;
                    notifyIcon1.ShowBalloonTip(10000, "Working Fine", "Connected to GSM Modem", ToolTipIcon.Info);           
                }
                catch (Exception ex)
                {
                    retry = true;
                    numi++;
                    if (numi > 8)
                    {
                        retry = false;
                        Cursor.Current = Cursors.Default;
                        notifyIcon1.Visible = true;
                        notifyIcon1.ShowBalloonTip(10000, "Error", "No GSM Modem Found", ToolTipIcon.Info);
                    }
                    else
                    {
                        comm = new GsmCommMain(portnames[numi], 9600, 300);
                    }
                }
            } while (retry);
        }
        private string GetMessageStorage()
        {
            string storage = string.Empty;
            storage = PhoneStorageType.Sim;

            if (storage.Length == 0)
                throw new ApplicationException("Unknown message storage.");
            else
                return storage;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            sendMessage(this.textBox1.Text, this.textBox2.Text);
        }
        private void sendMessage(string msg, string no)
        {
            try
            {
                SmsSubmitPdu sms = new SmsSubmitPdu(msg, no, "+923135013991");
                comm.SendMessage(sms);
                Cursor.Current = Cursors.Default;
                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(3000, "Success", "Message Sent", ToolTipIcon.Info);
            }
            catch (Exception e)
            {
                Cursor.Current = Cursors.Default;
                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(100, "Error", "Message Not Sent", ToolTipIcon.Error);
            }
        }
        private void Message_Received(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                IMessageIndicationObject obj = e.IndicationObject;
                if (obj is ShortMessage)
                {
                }
                if (obj is MemoryLocation)
                {
                    Console.Out.WriteLine("Message Receive Started");
                    MemoryLocation loc = (MemoryLocation)obj;
                    DecodedShortMessage messages;
                    messages = comm.ReadMessage(loc.Index, loc.Storage);
                    SmsPdu rawmsg = messages.Data;
                    SmsDeliverPdu msg = (SmsDeliverPdu)rawmsg;
                    string ss = msg.UserDataText;
                    ss = ss + msg.OriginatingAddress;
                    notifyIcon1.Visible = true;
                    notifyIcon1.ShowBalloonTip(10000, "SMS Recieved", msg.OriginatingAddress + "  " + msg.UserDataText, ToolTipIcon.Info);
                    notifyIcon1.Text = "Message Recieved from " + msg.OriginatingAddress;
                }
            }
            catch (Exception exc)
            {
                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(100, "Error", "Message Not Received", ToolTipIcon.Error);
            }
        }
    }
}
