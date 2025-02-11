namespace TodoApi.models
{
    public class Application
    {
        public string? Name { get; set; }

        public string? Author { get; set; }

        public string? GitHubUrl { get; set; }

        public string? Version { get; set; }

        public string? Technology { get; set; }

        public Application()
        {
            this.Name = "TeDoServer";
            this.Author = "Miri Kramarsky";
            this.GitHubUrl = "https://github.com/miriam262";
            this.Version = "0.0.1";
            this.Technology = "NET 8 Web API";
        }
    }
}
