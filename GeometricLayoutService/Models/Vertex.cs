using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GeometricLayoutService.Models
{
    [DataContract]
    public class Vertex
    {
        public double X { get; }

        public double Y { get; }

        public Vertex(double x, double y)
        {
            if (x < 0)
                throw new ArgumentException("x cannot be less than 0");
            if (y < 0)
                throw new ArgumentException("y cannot be less than 0");

            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public static Vertex ConvertTo(string vertex)
        {
            var xAndY = vertex.Trim('(', ')').Split(',');

            if (xAndY.Length != 2)
                throw new ArgumentException($"Invalid vertex format {vertex}, Expecting (x,y)");

            return new Vertex(double.Parse(xAndY[0]), double.Parse(xAndY[1]));
        }
    }
}