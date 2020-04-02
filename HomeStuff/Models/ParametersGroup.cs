using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CollectionViewDemos.Models
{
    public class ParametersGroup : List<Parameter>
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public ICommand ButtonCommand { private get; set; }
        public ParametersGroup(string name, List<Parameter> para) : base(para)
        {
            Name = name;
            Status = "ON";
            ButtonCommand = new Command(() => {
                Console.WriteLine("---------");
            });
        }

        public override string ToString()
        {
            return Name;
        }
        //async Task GetData(string device_id)
        //{
        //    await Task.Delay(100);
        //    Status = "ON";

        //    //MqttFactory factory = new MqttFactory();
        //    //// Create a new MQTT client.            
        //    //var mqttClient = factory.CreateMqttClient();

        //    //var options = new MqttClientOptionsBuilder()
        //    //.WithTcpServer("localhost", 1883)
        //    //.Build();


        //    //mqttClient.UseApplicationMessageReceivedHandler(ee =>
        //    //{

        //    //    string payload = Encoding.UTF8.GetString(ee.ApplicationMessage.Payload);

        //    //});
        //    ////// Subcribe to device ack topic first
        //    //mqttClient.UseConnectedHandler(async ee =>
        //    //{
        //    //    // Subscribe to a topic
        //    //    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("devices/" + device_id + "/ack").Build());

        //    //});


        //    ////// Try to connect to MQTT server
        //    //await mqttClient.ConnectAsync(options, CancellationToken.None);

        //    //var message = new MqttApplicationMessageBuilder()
        //    //    .WithTopic("devices/" + device_id + "/scan")
        //    //    .WithPayload("HELLO?")
        //    //    .Build();
        //    //await mqttClient.PublishAsync(message);
        //}
    }
}
