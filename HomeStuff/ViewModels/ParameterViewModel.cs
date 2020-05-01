using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Client;
using System.Threading;
using System.Text;

namespace CollectionViewDemos.ViewModels
{
    public class ParameterViewModel : INotifyPropertyChanged
    {
        string _name;

        string _humid;
        bool _ishumid = false;
 
        string _temperature;
        bool _istemp = false;

        string _id;

        string _netid;

        string _mqttserver;

        bool _isfeed = false;

        int _mqttport;

        string _changenamebutton = "Đổi tên";
        bool _isChangeName = false;

        public bool IsChangeName
        {
            get
            {
                return _isChangeName;
            }
            set
            {
                if (_isChangeName != value)
                {
                    _isChangeName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string ChangeNameButton
        {
            get
            {
                return _changenamebutton;
            }
            set
            {
                if (_changenamebutton != value)
                {
                    _changenamebutton = value;
                    NotifyPropertyChanged();
                }
            }
        }
        List<string> _tracked_parameter;

        List<string> _data_receive = new List<string>();

        public List<string> Tracked_Parameter
        {
            get
            {
                return _tracked_parameter;
            }
            set
            {
                if (_tracked_parameter != value)
                {
                    _tracked_parameter = value;
                    for (int i = 0; i < _tracked_parameter.Count; i++)
                    {
                        if (_tracked_parameter[i] == "humid")
                        {
                            _ishumid = true;
                        }
                        if (_tracked_parameter[i] == "temp")
                        {
                            _istemp = true;
                        }
                        if (_tracked_parameter[i] == "c_feed")
                        {
                            _isfeed = true;
                            FeedCommand = new Command(async() =>
                            {
                                string command_topic = _netid + "/" + _id + "/command";
                                MqttFactory factory = new MqttFactory();
                                // Create a new MQTT client.            
                                var mqttClient = factory.CreateMqttClient();

                                var options = new MqttClientOptionsBuilder()
                                .WithTcpServer(MQTTServer, MQTTPort)
                                .Build();
                                await mqttClient.ConnectAsync(options, CancellationToken.None);
                                var message = new MqttApplicationMessageBuilder()
                                .WithTopic(command_topic)
                                .WithPayload("FEED")
                                .Build();
                                Console.WriteLine("Sending command to topic " + command_topic);
                                await mqttClient.PublishAsync(message);
                                await mqttClient.DisconnectAsync();
                            });
                        }    

                    }

                }
            }
        }
        public List<string> Data_Receive
        {
            get
            {
                return _data_receive;
            }
            set
            {
                if (_data_receive != value)
                {
                    _data_receive = value;
                    //Reevalute all field
                    //Humid
                    int humid_index = -1;
                    int temperature_index = -1;
                    for (int i=0;i<_data_receive.Count;i++)
                    {
                        if (_tracked_parameter[i] == "humid")
                        {
                            humid_index = i;
                        }
                        if (_tracked_parameter[i] == "temp")
                        {
                            temperature_index = i;
                        }

                    }
                    if (humid_index != -1)
                    {
                        Humid = _data_receive[humid_index];
                    }
                    if (temperature_index != -1)
                    {
                        Temperature = _data_receive[temperature_index];
                    }
                }
            }
        }



        public string Name 
        {
            get
            {
                return _name;
            }
            set 
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Humid
        {
            get
            {
                return _humid;
            }
            set
            {
                if (_humid!= value)
                {
                    _humid = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool IsFeed
        {
            get
            {
                return _isfeed;
            }
            set
            {
                if (_isfeed != value)
                {
                    _isfeed = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsHumid
        {
            get
            {
                return _ishumid;
            }
            set
            {
                if (_ishumid != value)
                {
                    _ishumid = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsTemperature
        {
            get
            {
                return _istemp;
            }
            set
            {
                if (_istemp != value)
                {
                    _istemp = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string NetID
        {
            get
            {
                return _netid;
            }
            set
            {
                if (_netid != value)
                {
                    _netid = value;
                }
            }
        }
        public string MQTTServer
        {
            get
            {
                return _mqttserver;
            }
            set
            {
                if (_mqttserver != value)
                {
                    _mqttserver = value;
                }
            }
        }
        public int MQTTPort
        {
            get
            {
                return _mqttport;
            }
            set
            {
                if (_mqttport != value)
                {
                    _mqttport = value;
                }
            }
        }
        public ICommand FeedCommand { get; set; }

        public ICommand WhereCommand { get; set; }

        public ICommand ChangeNameCommand { get; set; }
        public ParameterViewModel()
        {

            WhereCommand = new Command<string>(async(a) => 
            {
                string command_topic = _netid + "/" + _id + "/command";
                MqttFactory factory = new MqttFactory();
                // Create a new MQTT client.            
                var mqttClient = factory.CreateMqttClient();

                var options = new MqttClientOptionsBuilder()
                .WithTcpServer(MQTTServer, MQTTPort)
                .Build();
                await mqttClient.ConnectAsync(options, CancellationToken.None);
                var message = new MqttApplicationMessageBuilder()
                .WithTopic(command_topic)
                .WithPayload("WHERE?")
                .Build();
                Console.WriteLine("Sending command to topic " + command_topic);
                await mqttClient.PublishAsync(message);
                await mqttClient.DisconnectAsync();
            });
            ChangeNameCommand = new Command<string>(async (a) =>
            {
                if (!(IsChangeName))
                {
                    IsChangeName = true;
                    ChangeNameButton = "Xác nhận";
                }    
                    
                else
                {
                    string command_topic = _netid + "/" + _id + "/command";
                    MqttFactory factory = new MqttFactory();
                    // Create a new MQTT client.            
                    var mqttClient = factory.CreateMqttClient();

                    var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(MQTTServer, MQTTPort)
                    .Build();
                    await mqttClient.ConnectAsync(options, CancellationToken.None);
                    var message = new MqttApplicationMessageBuilder()
                    .WithTopic(command_topic)
                    .WithPayload("NAME "+ Name)
                    .Build();
                    await mqttClient.PublishAsync(message);
                    await mqttClient.DisconnectAsync();
                    Console.WriteLine("Sending command to topic " + Name);
                    IsChangeName = false;
                    ChangeNameButton = "Đổi tên";
                }    


            });


        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




    }
}
