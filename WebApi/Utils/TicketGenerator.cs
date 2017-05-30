using System;
namespace WebApi.Utils
{
    public static class TicketGenerator
    {
        public static string Generate()
        {
            return new Guid().ToString().Replace("-", "").Remove(5);
        }
    }
}
