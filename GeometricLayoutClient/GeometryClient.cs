using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using GeometricLayoutService.Models;
using System.Drawing;

namespace GeometricLayoutClient
{
    public class GeometryClient : ClientBase<IGeometry>, IGeometry
    {
        private static readonly ILog log = LogManager.GetLogger(nameof(GeometryClient));
        public GeometryClient()
            : base(GetBinding(), new EndpointAddress("http://localhost:55902/"))
        {
            var behavior = new WebHttpBehavior();

            Endpoint.Behaviors.Add(behavior);

            if (Debugger.IsAttached)
            {
                //do not timeout when debugging
                Endpoint.Binding.ReceiveTimeout = TimeSpan.MaxValue;
                Endpoint.Binding.SendTimeout = TimeSpan.MaxValue;
            }
        }

        private static WebHttpBinding GetBinding()
        {
            var binding = new WebHttpBinding
            {
                MaxBufferSize = 10240000
            };

            binding.MaxReceivedMessageSize = binding.MaxBufferSize;
            binding.MaxBufferPoolSize = binding.MaxBufferSize;

            return binding;
        }

        public List<Vertex> GetAllVertices()
        {
            using (new OperationContextScope(InnerChannel))
            {
                return TryCatchEndpointException(() => Channel.GetAllVertices().ConvertAll(v => Vertex.ConvertTo(v)));
            }
        }

        public Task<List<Vertex>> GetAllVerticesAsync()
        {
            return Task.Run((Func<List<Vertex>>)GetAllVertices);
        }

        public string GetImage()
        {
            using (new OperationContextScope(InnerChannel))
            {
                return TryCatchEndpointException(() => Channel.GetImage());
            }
        }

        public Task<string> GetImageAsync()
        {
            return Task.Run((Func<string>)GetImage);
        }

        string IGeometry.GetTriangleLabels(string vertex)
        {
            throw new NotImplementedException();
        }

        List<string> IGeometry.GetAllVertices()
        {
            throw new NotImplementedException();
        }

        public string GetTriangleLabels(Vertex vertex)
        {
            using (new OperationContextScope(InnerChannel))
            {
                return TryCatchEndpointException(() => Channel.GetTriangleLabels(vertex.ToString()));
            }
        }

        public Task<string> GetTriangleLabelsAsync(Vertex vertex)
        {
            return Task.Run(() => GetTriangleLabels(vertex));
        }

        public void SetImage(string imageAsString)
        {
            using (new OperationContextScope(InnerChannel))
            {
                TryCatchEndpointException(() => Channel.SetImage(imageAsString));
            }
        }

        public Task SetImageAsync(string imageAsString)
        {
            return Task.Run(() => SetImage(imageAsString));
        }

        private T TryCatchEndpointException<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (EndpointNotFoundException ex)
            {
                //eat endpoint exceptions because they crash the app
                log.Error(ex);
                throw new CommunicationException("Cannot communicate with server.", ex);
            }
        }

        private void TryCatchEndpointException(Action func)
        {
            try
            {
                func();
            }
            catch (EndpointNotFoundException ex)
            {
                //eat endpoint exceptions because they crash the app
                log.Error(ex);
                throw new CommunicationException("Cannot communicate with server.", ex);
            }
        }
    }
}
