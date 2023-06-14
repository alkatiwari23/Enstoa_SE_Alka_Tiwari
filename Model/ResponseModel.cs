using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Model
{
    public class ResponseModel<T>
    {
        public List<T> List { get; set; }
        public T data { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }
    }
}
