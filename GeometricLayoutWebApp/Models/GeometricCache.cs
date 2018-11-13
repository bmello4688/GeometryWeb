using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GeometricLayout.Models
{
    public class GeometricCache
    {
        string targetPath = HttpContext.Current.Server.MapPath("~/UploadedFile/");
        private MemoryCache memoryCache = MemoryCache.Default;
        private const string ImageName = "Image";
        private ManualResetEvent manualResetEvent = new ManualResetEvent(true);
        private CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();

        public Task<GeometricImage> GetGeometricImageAsync()
        {
            return Task.Run(() =>
            {
                manualResetEvent.WaitOne();
                if (memoryCache.Contains(ImageName))
                    return memoryCache.Get(ImageName) as GeometricImage;
                else
                    return null;
            });
        }

        public Task SetGeometricImageAsync(string imageAsBase64)
        {
            manualResetEvent.Reset();
            return Task.Run(() =>
            {
                memoryCache.Set(ImageName, new GeometricImage(imageAsBase64), cacheItemPolicy);
                manualResetEvent.Set();
            });
        }
    }
}