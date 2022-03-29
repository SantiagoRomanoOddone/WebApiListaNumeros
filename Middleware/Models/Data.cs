
namespace Middlewares.Models
{
    public class Data
    {
        public string channel { get; set; }
        public string endpoint { get; set; }
        public string method { get; set; }
        public Availability availability { get; set; }
        public Config config { get; set; }
    }
}
