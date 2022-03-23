namespace InstantMessenger.Groups.Application.ResponseDtos
{
    public enum OverrideTypeDto
    {
        Allow = 1,
        Deny = -1
    }
    public class PermissionOverrideDto
    {
        public string? Permission { get; set; }
        public OverrideTypeDto Type { get; set; }
    }
}