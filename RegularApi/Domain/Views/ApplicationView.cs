namespace RegularApi.Domain.Views
{
    public class ApplicationView
    {
        public string Name { get; set; }

        public DockerSetupView DockerSetup { get; set; }

        // [JsonProperty(PropertyName = "HostsSetup")]
        // public IList<HostSetupResource> HostsSetupResources { get; set; }
    }
}