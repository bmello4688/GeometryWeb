﻿using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace GeometricLayoutClient
{
    [ServiceContract]
    public interface IGeometry
    {
        [WebInvoke(UriTemplate = "api/geometry/SetImage", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        void SetImage(string imageAsString);

        [WebInvoke(UriTemplate = "api/geometry/GetImage", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        string GetImage();

        [WebInvoke(UriTemplate = "api/geometry/GetTriangleLabels", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        string GetTriangleLabels(string vertex);

        [WebInvoke(UriTemplate = "api/geometry/GetAllVertices", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        List<string> GetAllVertices();
    }
}
