using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Features.AcceptInvitation;
using InstantMessenger.Friendships.Api.Features.SendInvitation;
using InstantMessenger.Identity.Api.Features.SignIn;
using InstantMessenger.Identity.Api.Features.SignUp;
using InstantMessenger.Identity.Api.Features.VerifyUser;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.PrivateMessages.Api.Queries;
using InstantMessenger.Shared.MailKit;

namespace InstantMessenger.IntegrationTests.Common
{
    internal static class ServerFixtureExtensions
    {
        internal static async Task<AuthDto> LoginWithDefaultUser(this ServerFixture fixture)
            => await LoginAsUser(fixture,"test@test.com","nickname");

        internal static async Task<AuthDto> LoginAsUser(this ServerFixture fixture, string email, string nickname)
        {
            var password = "TEest12!@";
            var identityApi = fixture.GetClient<IIdentityApi>();
            await identityApi.SignUp(new SignUpCommand(email, password));

            var mailService = (FakeMailService)fixture.GetService<IMailService>();
            var link = EmailContentExtractor.GetUrlFromActivationMail(mailService.Messages.Last());
            var (userId, token) = EmailContentExtractor.GetQueryParams(link);
            await identityApi.ActivateAccount(new ActivateCommand(Guid.Parse(userId), token, nickname));

            var authDto = await identityApi.SignIn(new SignInCommand(email, password));
            return authDto;
        }

        internal static async Task<ConversationDto> CreateConversation(this ServerFixture fixture, AuthDto userA, AuthDto userB)
        {
            var friendshipApi = fixture.GetClient<IFriendshipsApi>();
            await friendshipApi.SendFriendshipInvitation(
                $"Bearer {userA.Token}",
                new SendFriendshipInvitationApiRequest(Guid.Parse(userB.Subject))
            );

            var pendingInvitation = await friendshipApi.GetPendingInvitations($"Bearer {userB.Token}");
            await friendshipApi.AcceptInvitation($"Bearer {userB.Token}", new AcceptFriendshipInvitationApiRequest(pendingInvitation.First().InvitationId));

            var privateMessagesApi = fixture.GetClient<IPrivateMessagesApi>();
            var conversations = await privateMessagesApi.GetConversations($"Bearer {userA.Token}");

            return conversations.First();
        }
    }
}