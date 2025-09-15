using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace CashierSystem.Models
{
    public class OnlineUsersService
    {
        private readonly IMemoryCache _cache;
        private const string OnlineUsersKey = "OnlineUsers";

        public OnlineUsersService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public int GetOnlineUsers()
        {
            return _cache.GetOrCreate(OnlineUsersKey, entry => 0);
        }

        public void IncrementOnlineUsers()
        {
            int currentCount = GetOnlineUsers();
            _cache.Set(OnlineUsersKey, currentCount + 1);
        }

        public void DecrementOnlineUsers()
        {
            int currentCount = GetOnlineUsers();
            _cache.Set(OnlineUsersKey, currentCount > 0 ? currentCount - 1 : 0);
        }
    }

    public class OnlineUsersMiddleware
    {
        private readonly RequestDelegate _next;

        public OnlineUsersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, OnlineUsersService onlineUsersService)
        {
            if (!context.Session.Keys.Contains("SessionStarted"))
            {
                context.Session.SetString("SessionStarted", "true");
                onlineUsersService.IncrementOnlineUsers();
            }

            try
            {
                await _next(context);
            }
            finally
            {
                // يمكنك تقليل العدد عند انتهاء الجلسة إذا كان ذلك مناسبًا
            }
        }
    }


}
