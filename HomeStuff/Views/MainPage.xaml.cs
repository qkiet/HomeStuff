using CollectionViewDemos.ViewModels;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HomeStuff.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public class StringNullOrEmptyBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var s = value as string;
            return !string.IsNullOrWhiteSpace(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class MainPage : ContentPage
    {
        ViewCell lastCell;
        private const string MQTTServerUrl = "test.mosquitto.org";
        private const int MQTTPort = 1883;
        string net_id, pass, topic_device_dack, topic_device_scan;
        List<string> device_list = new List<string>();
        public ObservableCollection<ParameterViewModel> devices { get; set; }
        public MainPage(string my_net, string my_pass)
        {
            net_id = my_net;
            pass = my_pass;
            topic_device_dack = net_id + "/device/dack";
            topic_device_scan = net_id + "/device/scan";
            InitializeComponent();

            devices = new ObservableCollection<ParameterViewModel>();
        }
        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightBlue;
                lastCell = viewCell;
            }
        }
        protected override async void OnAppearing()
        {
            await Task.Delay(200);
            MqttFactory factory = new MqttFactory();
            // Create a new MQTT client.            
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
            .WithTcpServer(MQTTServerUrl, MQTTPort)
            .Build();

            // Reconnecting event handler
            mqttClient.UseDisconnectedHandler(async e =>
            {
                Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await mqttClient.ConnectAsync(options, CancellationToken.None);
                }
                catch
                {
                    Console.WriteLine("### RECONNECTING FAILED ###");
                }
            });

            // DACK handler
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                //payload format: DACK Name ID field1 field2...
                Console.WriteLine("### RECEIVED DEVICE DACK ###");
                string msg_payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                string[] elements = msg_payload.Split(';');
                List<string> ele_list = new List<string>(elements);
                int dack_index = ele_list.FindIndex(a => a == "DACK");
                if (ele_list[0] == "DACK") //is OUR data, let's processed
                {
                    #region Check_ID_In_Collection
                    int index = -1;
                    for (int i=0;i<devices.Count();i++)
                    {
                        if (devices[i].ID == ele_list[2])
                        {
                            index = i;
                            break;
                        }    
                    }
                    #endregion
                    if (index == -1)
                    {
                        devices.Add(new ParameterViewModel
                        {
                            Name = ele_list[1],
                            ID = ele_list[2],
                            NetID=net_id,
                            MQTTServer = MQTTServerUrl,
                            MQTTPort = MQTTPort,
                            Tracked_Parameter = ele_list.GetRange(3, ele_list.Count()-3)
                        });
                        device_list.Add(elements[2]);
                        Console.WriteLine("Total devices: " + devices.Count().ToString());
                        MainThread.BeginInvokeOnMainThread(() => scanning_result.Text = "Tìm thấy " + devices.Count().ToString() + " thiết bị");
                    }
                }
            });


            // Connected event handler
            mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");
                Console.WriteLine("Trying to listen to " + topic_device_dack);
                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic_device_dack).Build());

                Console.WriteLine("### SUBSCRIBED ###");
            });
            Console.WriteLine("Trying to connect");
            // Try to connect to MQTT server
            await mqttClient.ConnectAsync(options, CancellationToken.None);


            // Publishing messages  
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic_device_scan)
                .WithPayload("HELLO? "+pass)
                .Build();
            Console.WriteLine("Sending message to topic " + topic_device_scan);
            await mqttClient.PublishAsync(message);
            Console.WriteLine($"### SENT MESSAGE {Encoding.UTF8.GetString(message.Payload)} TO SERVER ");
            Console.WriteLine("Pass length is: " + pass.Length);


            await Task.Delay(7000);

            // When finished scan, begin to subscribe
            #region SubscribeTopic
            foreach (string item in device_list)
            {
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(net_id + "/"+item+ "/data").Build());
            }
            #endregion


            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                string device_payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                string device_topic = e.ApplicationMessage.Topic; // The topic contain ID
                string[] topic_ele = device_topic.Split('/');
                string device_id = topic_ele[1]; //The ID stand in index 1
                string[] elements = device_payload.Split(';');
                List<string> ele_list = new List<string>(elements);

                int index = -1;
                for (int i = 0;i < devices.Count;i++)
                {
                    Console.WriteLine("Looping..." + devices[i].ID);
                    if (devices[i].ID == device_id)
                    {
                        index = i;
                        break;
                    }
                }
                if (index != -1) //Begin to load data
                {
                    devices[index].Data_Receive = ele_list;
                }
                Console.WriteLine("Payload is " + device_payload + ". Index is " + index + ". ID is " + device_id);
            });
            scanning_status.Text = "Đã xong";
            indicator.IsRunning = false;
            lstView.ItemsSource = devices;

        }

    }

    
}
