using System;
using InstantMessenger.SharedKernel;
using Newtonsoft.Json;

namespace InstantMessenger.Groups.Application.Queries.ViewModels
{
    public class GroupMessageView
    {
        public Guid GroupId { get; private set; }
        public Guid ChannelId { get; private set; }
        public Guid MessageId { get; private set; }
        public Guid SenderId { get; private set; }
        public string SenderName { get; private set; }
        [JsonIgnore]
        public byte[] SenderAvatar { get; private set; }
        [JsonProperty("senderAvatar")]
        public string SenderAvatarBase64 => SenderAvatar is null ? null : Avatar.ToBase64String(SenderAvatar);
        public string Content { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public GroupMessageView(Guid groupId, Guid channelId, Guid messageId, 
            Guid senderId, string senderName, byte[]? senderAvatar, string content, DateTimeOffset createdAt)
        {
            GroupId = groupId;
            ChannelId = channelId;
            MessageId = messageId;
            SenderId = senderId;
            SenderName = senderName;
            SenderAvatar = senderAvatar;
            Content = content;
            CreatedAt = createdAt;
        }

    }
}