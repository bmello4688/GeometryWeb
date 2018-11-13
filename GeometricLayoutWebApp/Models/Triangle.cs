using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeometricLayout.Models
{
    public class Triangle
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private Vertex cornerVertex, adjacentRightVertex, adjacentLeftVertex;

        public char Row { get; }

        public int Column { get;}

        public bool IsUpperRight { get; }

        public List<Vertex> Vertices { get; }

        public Triangle(bool isUpperRight, Vertex upperLeftCorner, int columnSideLength, int rowSideLength)
        {
            if (upperLeftCorner == null)
            {
                throw new ArgumentNullException(nameof(upperLeftCorner));
            }

            IsUpperRight = isUpperRight;

            int rowIndex = (int)Math.Round(upperLeftCorner.Y / rowSideLength);
            int columnIndex = (int)Math.Round(upperLeftCorner.X / columnSideLength);
            var column = columnIndex * 2 + 1;

            if (isUpperRight)
                column++;

            Row = Alphabet[rowIndex];
            Column = column;

            CreateVertices(upperLeftCorner, columnSideLength, rowSideLength);

            Vertices = new List<Vertex>() { cornerVertex, adjacentRightVertex, adjacentLeftVertex };
        }

        private void CreateVertices(Vertex upperLeftCorner, int columnSideLength, int rowSideLength)
        {
            if (IsUpperRight)
                CreateUpperRightVertices(upperLeftCorner, columnSideLength, rowSideLength);
            else
                CreateBottomLeftVertices(upperLeftCorner, columnSideLength, rowSideLength);
        }

        private void CreateBottomLeftVertices(Vertex upperLeftCorner, int columnSideLength, int rowSideLength)
        {
            adjacentLeftVertex = upperLeftCorner;
            cornerVertex = new Vertex(upperLeftCorner.X, upperLeftCorner.Y + rowSideLength);
            adjacentRightVertex = new Vertex(cornerVertex.X + columnSideLength, cornerVertex.Y);
        }

        private void CreateUpperRightVertices(Vertex upperLeftCorner, int columnSideLength, int rowSideLength)
        {
            adjacentRightVertex = upperLeftCorner;
            cornerVertex = new Vertex(upperLeftCorner.X + columnSideLength, upperLeftCorner.Y);
            adjacentLeftVertex = new Vertex(cornerVertex.X, cornerVertex.Y + rowSideLength);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.ToString() == obj.ToString();
        }

        public override string ToString()
        {
            return $"{Row}{Column}";
        }
    }
}