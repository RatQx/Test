using Aukcionas.Auth.Model;
using Aukcionas.Data;
using Aukcionas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using System.Security.Claims;

namespace Aukcionas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ForumRestUser> _userManager;
        public ChatController(UserManager<ForumRestUser> userManager, DataContext context)
        {
            _userManager = userManager;
            _dataContext = context;
        }
        [HttpGet("chats")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChatsByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chats = await _dataContext.Chats
                .Where(chat => chat.CreatedBy == userId || chat.ReciverUser == userId)
                .ToListAsync();

            if (chats == null)
            {
                return NotFound();
            }

            return chats;
        }
        [HttpGet("chat-messages/{chatId}")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesForChat(int chatId)
        {
            var messages = await _dataContext.Messages
                .Where(msg => msg.ChatId == chatId)
                .ToListAsync();

            if (messages == null)
            {
                return NotFound();
            }

            return messages;
        }
        [HttpGet("getUsers")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var users = await _userManager.Users
                    .Where(u => u.Id != userId)
                    .Select(u => u.UserName)
                    .ToListAsync();

                if (users == null || !users.Any())
                {
                    return NotFound("No users found.");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        [HttpPost("addChat")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Chat>> AddChat([FromBody] string receiver)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var user2 = await _userManager.FindByNameAsync(receiver);

            if (user2 == null)
            {
                return BadRequest($"User with username {receiver} not found.");
            }

            var existingChat = await _dataContext.Chats.FirstOrDefaultAsync(chat =>
                (chat.CreatedBy == userId && chat.ReciverUser == user2.Id) ||
                (chat.CreatedBy == user2.Id && chat.ReciverUser == userId));

            if (existingChat != null)
            {
                return Conflict($"Chat already exists between {userId} and {user2.Id}");
            }

            var chat = new Chat
            {
                CreatedBy = userId,
                ReciverUser = user2.Id,
                CreatedUserName = user.UserName,
                ReciverUserName = user2.UserName
            };

            _dataContext.Chats.Add(chat);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChatsByUserId), new { userId = userId }, chat);
        }

        [HttpPost("addMessage")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Message>> AddMessageToChat(Message message)
        {
            var chatExists = await _dataContext.Chats.AnyAsync(c => c.Id == message.ChatId);
            if (!chatExists)
            {
                return NotFound($"Chat with ID {message.ChatId} does not exist.");
            }
            _dataContext.Messages.Add(message);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMessagesForChat), new { chatId = message.ChatId }, message);
        }
        [HttpDelete("delete-chat/{chatId}")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteChat(int chatId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chat = await _dataContext.Chats.FindAsync(chatId);
            if (chat == null)
            {
                return NotFound();
            }
            if(chat.CreatedBy != userId)
            {
                return BadRequest();
            }
            _dataContext.Chats.Remove(chat);
            await _dataContext.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("delete-message/{messageId}")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var message = await _dataContext.Messages.FindAsync(messageId);
            if (message == null)
            {
                return NotFound();
            }
            if(message.SenderUsername != user.UserName)
            {
                return Forbid();
            }
            _dataContext.Messages.Remove(message);
            await _dataContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
