using System;
using InstantMessenger.Shared.Queries;

namespace InstantMessenger.Profiles.Api.Queries
{
    public class GetProfile : IQuery<ProfileDto>
    {
        public Guid ProfileId { get; }

        public GetProfile(Guid profileId)
        {
            ProfileId = profileId;
        }
    }
}