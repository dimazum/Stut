using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using stutvds.DAL.Entities;
using stutvds.DAL.Repositories;
using stutvds.Data;
using stutvds.Models.HubDto;

namespace stutvds.WebSocketHubs;

public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;
    private readonly ChatMessageRepository _repository;
    private readonly string _connectionString;


    public ChatHub(ApplicationDbContext context, ConnectionStringProvider connectionStringProvider,
        ChatMessageRepository repository)
    {
        _context = context;
        _repository = repository;
        _connectionString = connectionStringProvider.GetConnectionString();
    }

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier!);
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string receiverId, string message)
    {
        if (message.Length > 100)
        {
            throw new StuException(ErrorCodes.ValidationError.Message);
        }
        
        var senderId = Context.UserIdentifier!;

        var chatMessage = new ChatMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Message = message,
            SentAt = DateTime.UtcNow
        };

        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();
        
        var messages = await GetChatMessages(senderId, receiverId);

        await Clients.Group(receiverId).SendAsync("ReceiveMessage", messages);
        await Clients.Group(senderId).SendAsync("ReceiveMessage", messages);
        
        // обновляем список чатов (lastMessage + unread counters)
        await Clients.User(receiverId).SendAsync("UpdateChatUsers");
        await Clients.User(senderId).SendAsync("UpdateChatUsers");
    }
    
    public async Task<List<ChatMessage>> GetChatHistory(string receiverId)
    {
        var senderId = Context.UserIdentifier!;

        var messages = await GetChatMessages(senderId, receiverId);

        return messages;
    }

    public async Task<ChatUsersDto> GetChatUsers()
    {
        using var conn = new SqlConnection(_connectionString);

        var users = await conn.QueryAsync<ChatUser>(
            "dbo.ChatGetUsers",
            new
            {
                MyUserId = Context.UserIdentifier
            },
            commandType: CommandType.StoredProcedure);

        return new ChatUsersDto
        {
            MyUserid = Context.UserIdentifier,
            Users = users.ToList()
        };
    }
    
    public async Task MarkAsRead(string otherUserId)
    {
        using var conn = new SqlConnection(_connectionString);

        await conn.ExecuteAsync(
            "dbo.ChatMarkAsRead",
            new
            {
                MyUserId = Context.UserIdentifier,
                OtherUserId = otherUserId
            },
            commandType: CommandType.StoredProcedure);

        await Clients.User(otherUserId)
            .SendAsync("MessagesRead", Context.UserIdentifier);
    }
    
    public async Task DeleteMessage(int messageId, string receiverId)
    {
        await _repository.DeleteByIdAsync(messageId);
        
        var senderId = Context.UserIdentifier!;

        var messages = await GetChatMessages(senderId, receiverId);

        await Clients.Group(receiverId).SendAsync("ReceiveMessage", messages);
        await Clients.Group(senderId).SendAsync("ReceiveMessage", messages);
    }

    public async Task<List<ChatMessage>> GetChatMessages(
        string senderId, string receiverId)
    {
        using var conn = new SqlConnection(_connectionString);

        var messages = await conn.QueryAsync<ChatMessage>(
            "dbo.GetChatMessages",
            new
            {
                SenderId = senderId,
                ReceiverId = receiverId
            },
            commandType: CommandType.StoredProcedure);

        return messages.ToList();
    }
}