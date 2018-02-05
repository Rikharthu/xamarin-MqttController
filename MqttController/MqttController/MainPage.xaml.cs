using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Xamarin.Forms;

namespace MqttController
{
    public partial class MainPage : ContentPage
    {
        private const string MQTT_BROKER_ADDRESS = "broker.hivemq.com";
        private MqttClient client;

        public MainPage()
        {
            InitializeComponent();

            new Task(ConnectMqttClient).Start();
          }

        void ConnectMqttClient()
        {
            Debug.WriteLine("Creating MQTT client");
            client = new MqttClient("broker.hivemq.com");
            Debug.WriteLine("MQTT client created");
            client.MqttMsgPublishReceived += onMessagePublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            client.Subscribe(new string[] { "test/messages" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            string strValue = "Hello from Xamarin!";

            client.Publish("test/messages", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);

        }

        private void onMessagePublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Debug.WriteLine("Received event: " + e);
        }

        void OnSendButtonClicked(object sender, EventArgs args)
        {          
            Debug.WriteLine("Sending message");
            client.Publish("test/messages", Encoding.UTF8.GetBytes("Hello from Xamarin!"+new Random().Next(1,100)), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
        }
    }
}
