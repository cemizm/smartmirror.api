using System;
namespace WebApi.Utils
{
    public static class TicketGenerator
    {
        public static string Generate()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Remove(5);
        }
    }
}
