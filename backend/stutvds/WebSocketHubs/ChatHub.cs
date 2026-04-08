using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using stutvds.DAL.Entities;
using stutvds.DAL.Repositories;
using stutvds.Data;
using stutvds.Models.HubDto;

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
        // Добавляем подключение пользователя в группу по его UserId
        var userId = Guid.Parse(Context.UserIdentifier!);
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string receiverId, string message)
    {
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
    }

    // Метод для получения истории сообщений между двумя пользователями
    public async Task<List<ChatMessage>> GetChatHistory(string receiverId)
    {
        var senderId = Context.UserIdentifier!;

        var messages = await GetChatMessages(senderId, receiverId);

        return messages;
    }

    public async Task<ChatUsersDto> GetChatUsers()
    {
        var result = new ChatUsersDto()
        {
            Users = new List<ChatUser>()
        };

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("dbo.ChatGetUsers", conn);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@MyUserId", Context.UserIdentifier);

        await conn.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();

        result.MyUserid = Context.UserIdentifier;

        while (await reader.ReadAsync())
        {
            result.Users.Add(new ChatUser
            {
                Id = reader.GetString(0),
                Name = reader.GetString(1),
                LastMessageDate = reader.IsDBNull(2) 
                    ? null 
                    : reader.GetDateTimeOffset(2)
            });
        }

        return result;
    }

    public async Task DeleteMessage(int messageId, string receiverId)
    {
        await _repository.DeleteByIdAsync(messageId);
        
        var senderId = Context.UserIdentifier!;

        var messages = await GetChatMessages(senderId, receiverId);

        await Clients.Group(receiverId.ToString()).SendAsync("ReceiveMessage", messages);
        await Clients.Group(senderId.ToString()).SendAsync("ReceiveMessage", messages);
    }

    private async Task<List<ChatMessage>> GetChatMessages(string senderId, string receiverId)
    {
        var messages= await _context.ChatMessages
            .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                        (m.SenderId == receiverId && m.ReceiverId == senderId))
            .OrderByDescending(m => m.SentAt)
            .Take(100)  
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        return messages;
    }
}