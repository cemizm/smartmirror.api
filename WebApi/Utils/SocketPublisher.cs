using System;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Quobject.SocketIoClientDotNet.Client;
using WebApi.DataLayer.Models;

namespace WebApi.Utils
{
    public class SocketPublisher
    {
        private SocketSettings settings;
        private Socket socket;

        public SocketPublisher(IOptions<Utils.SocketSettings> options)
        {

            this.settings = options.Value;
        }

        public void UpdateMirror(Mirror mirror){
            this.Publish("update", mirror);
        }

        public void Publish<T>(string eventString, T data){
			var serializer = new JsonSerializer()
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};

            var jobj = JObject.FromObject(data, serializer);

            if(socket == null)
                socket = IO.Socket(settings.ServerUrl);

            socket.Emit(eventString, jobj);

        }
    }
}
