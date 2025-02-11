using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.models
{
    public class Loginmodel
    {
        public int? Id { get; set; }
        public String? name { get; set; }
        public String? password { get; set; }
    }
}
