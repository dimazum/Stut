using System;
using StopStatAuth_6_0.Entities.Base;

namespace stutvds.DAL.Entities;

public class ChatMessage: Entity
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string? ReceiverId { get; set; }
    public string Message { get; set; }
    public DateTimeOffset SentAt { get; set; } = DateTimeOffset.Now;
}