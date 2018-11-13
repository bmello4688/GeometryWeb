using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeometricLayout.Models
{
    public class Square
    {
        private readonly Triangle topRightTriangle;
        private readonly Triangle bottomLeftTriangle;

        public List<Triangle> Triangles { get; }

        public Vertex UpperLeftCorner { get; }

        public int ColumnSideLength { get; }

        public int RowSideLength { get; }

        public Square(Vertex upperLeftCorner, int columnSideLength, int rowSideLength)
        {
            UpperLeftCorner = upperLeftCorner;
            ColumnSideLength = columnSideLength;
            RowSideLength = rowSideLength;

            topRightTriangle = new Triangle(true, UpperLeftCorner, ColumnSideLength, RowSideLength);
            bottomLeftTriangle = new Triangle(false, UpperLeftCorner, ColumnSideLength, RowSideLength);

            Triangles = new List<Triangle>() { bottomLeftTriangle, topRightTriangle };
        }
    }
}