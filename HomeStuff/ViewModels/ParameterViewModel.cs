using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CollectionViewDemos.Models;
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


        public List<ParametersGroup> Device { get; private set; } = new List<ParametersGroup>();
        public ParameterViewModel()
        {

            CreateAnimalsCollection();

        }

        //async Task GetData(string device_id)
        //{
        //    MqttFactory factory = new MqttFactory();
        //    // Create a new MQTT client.            
        //    var mqttClient = factory.CreateMqttClient();

        //    var options = new MqttClientOptionsBuilder()
        //    .WithTcpServer("localhost", 1883)
        //    .Build();


        //    mqttClient.UseApplicationMessageReceivedHandler(ee =>
        //    {

        //        string payload = Encoding.UTF8.GetString(ee.ApplicationMessage.Payload);

        //    });
        //    //// Subcribe to device ack topic first
        //    mqttClient.UseConnectedHandler(async ee =>
        //    {
        //        // Subscribe to a topic
        //        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("devices/" + device_id + "/ack").Build());

        //    });


        //    //// Try to connect to MQTT server
        //    await mqttClient.ConnectAsync(options, CancellationToken.None);

        //    var message = new MqttApplicationMessageBuilder()
        //        .WithTopic("devices/"+device_id+"/scan")
        //        .WithPayload("HELLO?")
        //        .Build();
        //    await mqttClient.PublishAsync(message);
        //}

        void CreateAnimalsCollection()
        {
            Device.Add(new ParametersGroup("May1", new List<Parameter>
            {
                new Parameter
                {
                    Humid = "23244",
                    Id = "5",
                },
            }));
           
        }
        public event PropertyChangedEventHandler PropertyChanged;


        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
