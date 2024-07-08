using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace WebApplication.Sample
{
    public class APIResponse<T> where T : class
    {
        public List<T> Data { get; set; }
    }

    public class APIResponseNew<T> where T : class
    {
        public List<T> message { get; set; }
    }

    public class APIResponsePriceList<T> where T : class
    {
        public T data { get; set; }
    }
}
