using System;
using System.Collections.Generic;
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

        public void ControlMirror(Guid mirrorId, string action, Dictionary<string, string> payload){
            ControlRequest req = new ControlRequest()
            {
                MirrorId = mirrorId,
                Action = action,
                Payload = payload
            };

            this.Publish("action", req);
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

		#region Nested Types

		public class ControlRequest
		{
			/// <summary>
			/// Id of Mirror to control
			/// </summary>
			public Guid MirrorId { get; set; }

			/// <summary>
			/// Action to execute
			/// </summary>
			public string Action { get; set; }

			/// <summary>
			/// Gets or sets the payload.
			/// </summary>
			/// <value>The payload.</value>
			public Dictionary<string, string> Payload { get; set; }

		}

		#endregion
	}
}
