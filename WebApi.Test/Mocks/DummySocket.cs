using System;
using Microsoft.Extensions.Options;
using WebApi.Utils;

public class DummySocket : SocketPublisher
{
    public DummySocket() : base(new EmptyOptions<SocketSettings>())
    {
    }

    public override void ControlMirror(Guid mirrorId, string action, System.Collections.Generic.Dictionary<string, string> payload)
    {
    }

    public override void UpdateMirror(WebApi.DataLayer.Models.Mirror mirror)
    {
    }

    class EmptyOptions<T> : Microsoft.Extensions.Options.IOptions<T> where T :class, new() 
    {
        public T Value => null;
    }
}