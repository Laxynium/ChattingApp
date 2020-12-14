namespace InstantMessenger.Groups.Api.ResponseDtos
{
    public class PermissionDto
    {
        public string Name { get; }
        public int Code { get; }

        public PermissionDto(string name, int code)
        {
            Name = name;
            Code = code;
        }
    }
}