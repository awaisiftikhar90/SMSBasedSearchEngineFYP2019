using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using GoogleSearcAPClient.DataBase;

namespace GoogleSearcAPClient
{
    public partial class Interface : Form
    {
        private HttpClient _client;
       
        public List<SearchResponseDto> GetGoogleSearchData(string keyword, string number)
            {
            var body = new SearchRequestDto
            {
                Keyword = keyword
            };

            var inputJson = JsonConvert.SerializeObject(body);
            HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json"); //UTF stands for Unicode Transformation Format. The '8' means it uses 8-bit blocks to represent a character.
            // Call Your API
            var response = _client.PostAsync("api/v1/search", inputContent).Result;

            // Extract Data from API the API response
            var finalResult = JsonConvert.DeserializeObject<List<SearchResponseDto>>(response.Content.ReadAsStringAsync().Result);
            
            if (finalResult == null || finalResult.Count() == 0)
            {
                //string ques = keyword;
                //string num = number;
                string a1 = keyword + "," + number;
                string exnum = "+923110873862";
                sendMessage(a1, exnum);
            }
            else
            {
                string link = finalResult.First().Link.ToString();
                String text = finalResult.First().Text.ToString();
                text = text == null ? "" : text;

                byte[] bytes = Encoding.Default.GetBytes(text);
                text = Encoding.UTF8.GetString(bytes);

                if ( text.Length > 100)
                {
                    text = text.Substring(0, 100);
                }
                String message = text +"\n"+ link;
               
                // insert data to database
                DataLayer dl = new DataLayer();
                dl.InsertSMS(keyword, message);

                sendMessage(message, number);
            }
            return finalResult;
        }

        private GsmCommMain comm;
        string[] portnames = { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9" };
        NotifyIcon nt = new NotifyIcon();
        public Interface()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:51343/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

           // DataLayer dl = new DataLayer();
           // string query = "Awais ";
           // var newquery = query.Split(' ');  //split in the base of space  
           //var result = dl.GetDataByQuery(newquery[0] +" " +newquery[1]);   // text ko break kr k query search krna
           // if (!result.Any())
           // {
           //     dl.InsertSMS(new SmsOffline
           //     {
           //         Query = "Awais",
           //         Answer = "XYZ esdfsfsdfs"
           //     });

           // }

            
            InitializeComponent();
        }
        private void SetBalloonTip()
        {
            nt.Icon = SystemIcons.Exclamation;
            nt.BalloonTipTitle = "Balloon Tip Title";
            nt.BalloonTipText = "Balloon Tip Text.";
            nt.BalloonTipIcon = ToolTipIcon.Error;
            this.Click += new EventHandler(Interface_Load);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //GoogleSearchAPIProxy proxy = new GoogleSearchAPIProxy();
            //GetGoogleSearchData("rawalpindi", this.textBox2.Text);
            sendMessage(this.textBox1.Text, this.textBox2.Text);
        }
        private void sendMessage(string msg, string no)
        {
            try
            {
                SmsSubmitPdu sms = new SmsSubmitPdu(msg, no, "+923135013991");  //fix num ko define kr k api k through send kia  apabetility to send msg
                comm.SendMessage(sms);  //
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
        private void Message_Received(object sender, MessageReceivedEventArgs e) //1
        {
            try
            {
                IMessageIndicationObject obj = e.IndicationObject;    //// The interface identifying an object as being used for indicating new incoming messages
                if (obj is ShortMessage)
                {
                }
                if (obj is MemoryLocation)
                {
                    try
                    {

                        Console.Out.WriteLine("Message Receive Started");
                    MemoryLocation loc = (MemoryLocation)obj;
                    DecodedShortMessage messages;
                    messages = comm.ReadMessage(loc.Index, loc.Storage);
                    SmsPdu rawmsg = messages.Data;// smspdu msg send or reveive   converter which is use to conveter
                    SmsDeliverPdu msg = (SmsDeliverPdu)rawmsg;

                        string ss = msg.UserDataText;  //text.num rece
                        string num = msg.OriginatingAddress;//msg and text from user

                       
                        //Database search in db if que exist answer that ques and rply (2

                        DataLayer dl = new DataLayer();

                    var newquery = ss.Trim(); //split in the base of space
                    var results = dl.GetDataByQuery(newquery); //text ko break k query search krna *2*
                    if (results.Any())
                    {
                        String s = (results[0]).Answer;
                            sendMessage(s, num);
                           
                       
                    }
                    else
                    {
                            notifyIcon1.Visible = true;
                            notifyIcon1.ShowBalloonTip(10000, "SMS Recieved", msg.OriginatingAddress + "  " + msg.UserDataText, ToolTipIcon.Info);
                            notifyIcon1.Text = "Message Recieved from " + msg.OriginatingAddress;

                            GetGoogleSearchData(ss, num);  //call service base link


                    }


                    ss = ss + msg.OriginatingAddress;
                    
                    }
                    catch (Exception ex)
                    {
                        notifyIcon1.Icon = SystemIcons.Application;
                        notifyIcon1.Visible = true;
                        notifyIcon1.ShowBalloonTip(100, "Error", "Message Not Received" + ex, ToolTipIcon.Error);
                    }
                }
            }
            catch (Exception exc)
            {
                
                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(100, "Error", "Message Not Sent", ToolTipIcon.Error);
            }
        }

        

        private void Interface_Load(object sender, EventArgs e)  //1  load form 
        {
            notifyIcon1.Icon = SystemIcons.Exclamation;
            int numi = 0;

            comm = new GsmCommMain(portnames[numi], 9600, 300);
            Cursor.Current = Cursors.Default;
            bool retry;

            do //run continously
            {
                retry = false;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;   //processing show
                    comm.Open();
                    comm.EnableMessageNotifications(); //show notification
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

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
