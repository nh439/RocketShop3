namespace RocketShop.Migration.Configuration
{
    public class StartUserConfiguration
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string EmployeeCode { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
