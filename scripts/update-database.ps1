#### Run from checkout directory ###
$projects = @(
    @{context="InstantMessenger.Identity.Infrastructure.Database.IdentityContext"}
    @{context="InstantMessenger.Friendships.Infrastructure.Database.FriendshipsContext"}
    @{context="InstantMessenger.PrivateMessages.Infrastructure.Database.PrivateMessagesContext"}
    @{context="InstantMessenger.Groups.Infrastructure.Database.GroupsContext"}
    @{context="InstantMessenger.Groups.Infrastructure.Database.GroupsViewContext"}
)
foreach ($project in $projects) { dotnet ef database update --project 'src/InstantMessenger.Api' --context $($project.context)  --verbose}
