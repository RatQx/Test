using Aukcionas.Auth.Model;
using Aukcionas.Data;
using Aukcionas.Models;
using Aukcionas.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aukcionas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivestreamController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ForumRestUser> _userManager;
        private readonly IDailycoService _dailycoService;
        public LivestreamController(UserManager<ForumRestUser> userManager, DataContext context, IDailycoService dailycoService)
        {
            _userManager = userManager;
            _dataContext = context;
            _dailycoService = dailycoService;
        }
        [HttpPost("start/{auctionId}")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> StartLivestream(int auctionId)
        {
            var auction = await _dataContext.Auctions.FindAsync(auctionId);
            if (auction == null)
            {
                return NoContent();
            }

            if (!string.IsNullOrEmpty(auction.SavedUrl))
            {
                return BadRequest("Livestream is already running for this auction.");
            }

            var livestreamUrl = await _dailycoService.StartLivestream();
            auction.SavedUrl = livestreamUrl;

            _dataContext.Entry(auction).State = EntityState.Modified;

            var result = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            if (result > 0)
            {
                return Ok(new { livestreamUrl });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update auction information.");
            }
        }

        [HttpPost("stop/{auctionId}")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> StopLivestream(int auctionId)
        {
            var auction = await _dataContext.Auctions.FindAsync(auctionId);
            if (auction == null)
            {
                return NoContent();
            }

            if (auction.SavedUrl!= null)
            {
                //await _dailycoService.StopLivestream(auctionId,roomid);
                auction.SavedUrl = null;
                await _dataContext.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest("No current livestream in auction.");
        }
    }
}
