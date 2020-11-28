
Update-Database -Project Modules\InstantMessenger.Identity -StartupProject InstantMessenger.Api -Verbose -Context IdentityContext
Update-Database -Project Modules\InstantMessenger.Profiles -StartupProject InstantMessenger.Api -Verbose -Context ProfilesContext
Update-Database -Project Modules\InstantMessenger.Friendships -StartupProject InstantMessenger.Api -Verbose -Context FriendshipsContext
Update-Database -Project Modules\InstantMessenger.PrivateMessages -StartupProject InstantMessenger.Api -Verbose -Context PrivateMessagesContext
Update-Database -Project Modules\InstantMessenger.Groups -StartupProject InstantMessenger.Api -Verbose -Context GroupsContext