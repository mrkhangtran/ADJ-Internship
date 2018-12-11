namespace ADJ.Common
{
    public class ApplicationContext
    {
        public ApplicationPrincipal Principal { get; } = new ApplicationPrincipal();
    }

    public class ApplicationPrincipal
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }
    }
}
