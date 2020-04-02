using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CollectionViewDemos.Models
{
    public class Parameter
    {
        public string Humid { get; set; }
        public string Id { get; set; }

    }
}
