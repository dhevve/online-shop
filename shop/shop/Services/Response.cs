using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data)
        {
            Succeeded = true;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }
    }
}