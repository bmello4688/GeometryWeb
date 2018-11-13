using GeometricLayoutClient;
using GeometricLayout.Models;
using GeometricLayoutTest.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricLayoutTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test().Wait();
        }

        private static async Task Test()
        {
            using (GeometryClient client = new GeometryClient())
            {
                var image = await client.GetImageAsync();

                Console.WriteLine($"Image is null: {image == null}");

                Console.WriteLine("Upload image");
                client.SetImage(ImageConverter.ConvertTo(Resources.puppies));

                image = await client.GetImageAsync();

                Console.WriteLine($"Image is null: {image == null}");

                Console.WriteLine("Get all vertices");
                var vertices = await client.GetAllVerticesAsync();

                foreach (var vertex in vertices)
                {
                    string labels = await client.GetTriangleLabelsAsync(vertex);
                    Console.WriteLine($"Vertex {vertex} contains Triangles {labels}");
                }

                Console.ReadKey();
            }
        }
    }
}
