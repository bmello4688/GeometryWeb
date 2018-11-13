using GeometricLayout.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GeometricLayout.Controllers
{
    public class GeometryController : ApiController
    {
        private static GeometricCache cache = new GeometricCache();

        [HttpGet]
        public async Task<string> GetImage()
        {
            var geoImage = await cache.GetGeometricImageAsync();

            return geoImage?.ImageAsBase64;
        }

        [HttpGet]
        public async Task<List<string>> GetAllVertices()
        {
            var geoImage = await cache.GetGeometricImageAsync();

            return geoImage?.Vertices.ConvertAll(v => v.ToString());
        }

        [HttpPost]
        public async Task<string> GetTriangleLabels([FromBody]string vertexString)
        {
            Vertex vertex = Vertex.ConvertTo(vertexString);
            
            var geoImage = await cache.GetGeometricImageAsync();

            var triangles = geoImage?.GetTriangles(vertex);

            return BuildTriangleLabelList(triangles);
        }

        private static string BuildTriangleLabelList(List<Triangle> triangles)
        {
            if (triangles == null)
                return string.Empty;

            const string Comma = ", ";
            StringBuilder triangleLabels = new StringBuilder();

            foreach (var triangle in triangles)
            {
                triangleLabels.Append(triangle);
                triangleLabels.Append(Comma);
            }
            //remove extra comma
            triangleLabels.Remove(triangleLabels.Length - Comma.Length, Comma.Length);

            return triangleLabels.ToString();
        }

        [HttpPost]
        public async Task SetImage([FromBody]string base64Image)
        {
            await cache.SetGeometricImageAsync(base64Image);
        }
    }
}
