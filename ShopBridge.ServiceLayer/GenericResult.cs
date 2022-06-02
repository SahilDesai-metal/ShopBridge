using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ShopBridge.ServiceLayer
{
    public class GenericResult<T> where T: class
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Value { get; set; }
        public string[] Errors { get; set; }
        public bool HasErrors { get; set; } = false;
    }
}
