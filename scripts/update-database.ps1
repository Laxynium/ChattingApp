$projects = @(
    @{project="Identity";context="IdentityContext"}
    @{project="Profiles";context="ProfilesContext"}
    @{project="Friendships";context="FriendshipsContext"}
    @{project="PrivateMessages";context="PrivateMessagesContext"},
    @{project="Groups";context="GroupsContext"}
)
foreach ($project in $projects) { Update-Database -Project "Modules\InstantMessenger.$($project.project)" -StartupProject InstantMessenger.Api -Verbose -Context "$($project.context)"}