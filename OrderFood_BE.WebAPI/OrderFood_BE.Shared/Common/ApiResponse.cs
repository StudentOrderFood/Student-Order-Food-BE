using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Shared.Common
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string MessageId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
