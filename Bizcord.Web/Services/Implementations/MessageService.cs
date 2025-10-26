
using Bizcord.Web.Data;
using Bizcord.Web.Shared.DTOs;
using Bizcord.Web.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bizcord.Web.Services.Implementations;

public class MessageService : IMessageService
{
    private const int MaxContentLength = 4000;


    private readonly BizcordDbContext _db;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<MessageService> _logger;


    public MessageService(BizcordDbContext db, IMessagePublisher publisher, ILogger<MessageService> logger)
    {
        _db = db;
        _publisher = publisher;
        _logger = logger;
    }


    public async Task<Message> CreateAsync(int userId, int channelId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content must not be empty", nameof(content));


        content = content.Trim();
        if (content.Length > MaxContentLength)
            content = content.Substring(0, MaxContentLength);


// Validate FK existence and get username efficiently
        var user = await _db.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.Id, u.UserName })
            .FirstOrDefaultAsync();
        if (user == null)
            throw new KeyNotFoundException("User not found");


        var channelExists = await _db.Channels.AnyAsync(c => c.Id == channelId);
        if (!channelExists)
            throw new KeyNotFoundException("Channel not found");


        var message = new Message { UserId = userId, ChannelId = channelId, Content = content };
        _db.Messages.Add(message);
        await _db.SaveChangesAsync();


// Publish event for RabbitMQ -> consumer -> SignalR
        var evt = new MessageEvent
        {
            Id = message.Id,
            ChannelId = message.ChannelId,
            Content = message.Content,
            UserName = user.UserName,
            CreatedAt = message.CreatedAt
        };
    }

    public Task<List<MessageDto>> GetByChannelAsync(int channelId, int take = 100)
    {
        throw new NotImplementedException();
    }
}