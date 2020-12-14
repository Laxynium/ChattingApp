namespace InstantMessenger.Groups.Api.ResponseDtos
{
    public enum OverrideTypeDto
    {
        Allow,
        Deny
    }
    public class PermissionOverrideDto
    {
        public string Permission { get; set; }
        public OverrideTypeDto Type { get; set; }
    }
}