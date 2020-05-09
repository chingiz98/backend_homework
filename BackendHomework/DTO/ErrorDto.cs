using Newtonsoft.Json;

namespace BackendHomework.Models
{
    public class ErrorDto
    {
        public int Code {get;set;}
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}