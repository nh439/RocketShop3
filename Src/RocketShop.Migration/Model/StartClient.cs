namespace RocketShop.Migration.Model
{
    public class StartClient
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string? ClientSecret { get; set; }
        public string Application { get; set; }
        public string[]? AllowedReadCollections { get; set; }   
        public string[]? AllowedWriteCollections { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
