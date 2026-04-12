using System;
using System.Collections.Generic;
namespace stutvds.Models.HubDto;

public class ChatUsersDto 
{
    public string MyUserid { get; set; }
    public List<ChatUser> Users { get; set; }
}
public class ChatUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LastMessage { get; set; }
    public DateTimeOffset? LastMessageDate { get; set; }
    public int UnreadCount { get; set; }
}