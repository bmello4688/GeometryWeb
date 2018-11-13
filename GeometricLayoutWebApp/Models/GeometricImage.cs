using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace GeometricLayout.Models
{
    public class GeometricImage
    {
        private readonly int rowCount;
        private readonly int columnCount;
        private List<Square> squares = new List<Square>();
        private Dictionary<Vertex, List<Triangle>> vertexToTriangleLookup = new Dictionary<Vertex, List<Triangle>>();

        public Bitmap Image { get; }

        public string ImageAsBase64 { get; }

        public List<Vertex> Vertices { get; }

        public GeometricImage(string imageAsBase64, int rowCount = 6, int columnCount = 12)
        {
            if (rowCount <= 0 && rowCount > 26 && rowCount * 2 != columnCount)
                throw new ArgumentException($"{nameof(columnCount)} has to be greater than zero, less than 26, and half of column count to support only right triangles.");
            if (columnCount <= 0 || columnCount % 2 != 0)
                throw new ArgumentException($"{nameof(columnCount)} has to be greater than zero and even");
            if(string.IsNullOrWhiteSpace(imageAsBase64))
                throw new ArgumentNullException(nameof(imageAsBase64));

            Image = CropImage(ImageConverter.ConvertFrom(imageAsBase64));
            ImageAsBase64 = ImageConverter.ConvertTo(Image);

            this.rowCount = rowCount;
            this.columnCount = columnCount;

            BuildSquaresAndTriangles();
            GenerateVertexToTriangleLookup();

            Vertices = vertexToTriangleLookup.Keys.ToList();
        }

        public List<Triangle> GetTriangles(Vertex vertex)
        {
            if (vertexToTriangleLookup.ContainsKey(vertex))
                return vertexToTriangleLookup[vertex];
            else
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = $"Vertex: {vertex} does not exist" });
        }

        private Bitmap CropImage(Image image)
        {
            var length = image.Width < image.Height ? image.Width : image.Height;

            Rectangle cropArea = new Rectangle(0, 0, length, length);
            Bitmap bmpImage = new Bitmap(image);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private void BuildSquaresAndTriangles()
        {
            var rowLength = Image.Height / rowCount;
            for (int i = 0; i < rowCount; i++)
            {
                var y = (i != rowCount) ? rowLength * i : Image.Height;

                var squareColumnCount = columnCount / 2;
                var squareColumnLength = Image.Width / squareColumnCount;

                for (int j = 0; j < squareColumnCount; j++)
                {
                    var x = (j != squareColumnCount) ? squareColumnLength * j : Image.Width;

                    var vertex = new Vertex(x, y);

                    squares.Add(new Square(vertex, squareColumnLength, rowLength));
                }
            }
        }

        private void GenerateVertexToTriangleLookup()
        {
            var allTriangles = squares.SelectMany(square => square.Triangles);

            foreach (var triangle in allTriangles)
            {
                var vertices = triangle.Vertices;

                foreach (var vertex in vertices)
                {
                    if (!vertexToTriangleLookup.ContainsKey(vertex))
                        vertexToTriangleLookup.Add(vertex, new List<Triangle>() { triangle });
                    else
                        vertexToTriangleLookup[vertex].Add(triangle);
                }
            }
        }
    }
}