using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetapp.Models;
using dotnetapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // 1. Get all feedbacks
        [HttpGet]
        // [Authorize(Roles ="Admin")]
        // [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacks();
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // 2. Get feedbacks by userId
        [HttpGet("user/{userId}")]
        [Authorize(Roles ="User")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacksByUserId(int userId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByUserId(userId);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // 3. Add feedback
        [HttpPost]
        [Authorize(Roles ="User")]
        public async Task<ActionResult> AddFeedback([FromBody] Feedback feedback)
        {
            try
            {
                var result = await _feedbackService.AddFeedback(feedback);
                if (result)
                {
                    return Ok("Feedback added successfully");
                }
                return StatusCode(500, "Failed to add feedback");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // 4. Delete feedback
        [HttpDelete("{feedbackId}")]
        [Authorize(Roles ="User")]
        public async Task<ActionResult> DeleteFeedback(int feedbackId)
        {
            try
            {
                var deleted = await _feedbackService.DeleteFeedback(feedbackId);
                if (!deleted)
                {
                    return NotFound("Cannot find any feedback");
                }
                return Ok("Feedback deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
