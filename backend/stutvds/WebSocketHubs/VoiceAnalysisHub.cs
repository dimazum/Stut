using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.DAL.Repositories;
using stutvds.Models.ClientDto;

namespace stutvds.WebSocketHubs;

[Authorize]
public class VoiceAnalysisHub : Hub
{
    private readonly string _connectionString;

    public VoiceAnalysisHub(ConnectionStringProvider connectionStringProvider)
    {
        _connectionString = connectionStringProvider.GetConnectionString();
    }
    
    // При подключении добавляем пользователя в группу с именем UserId
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    // Метод для фронта, если клиент хочет подписаться на конкретную сессию
    public async Task JoinSession(string sessionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
    }

    // Метод для отписки от сессии
    public async Task LeaveSession(string sessionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
    }
    
    public async Task AnalyzeVoice(string text, int wordsSpoken, int dailyLessonId)
    {
        await RunUpdateVoiceAnalysis(dailyLessonId, text, wordsSpoken);

        var result = new VoiceAnalysisUpdateDto
        {
            WPM = 0,
            WordsSpoken = 0
        };
        await Clients.Caller.SendAsync("UpdateVoiceAnalysis", result);
    }

    public async Task RunUpdateVoiceAnalysis(int dailyLessonId, string text, int wordsSpoken)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new SqlCommand("UpdateVoiceAnalysis", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@dailyLessonId", SqlDbType.Int) { Value = dailyLessonId });
        cmd.Parameters.Add(new SqlParameter("@text", SqlDbType.NVarChar, -1) { Value = text });
        cmd.Parameters.Add(new SqlParameter("@wordsSpoken", SqlDbType.Int) { Value = wordsSpoken });

        await cmd.ExecuteNonQueryAsync();
    }
}