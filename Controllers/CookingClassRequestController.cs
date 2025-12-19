// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using dotnetapp.Models;
// using dotnetapp.Services;
// using Microsoft.AspNetCore.Authorization;

// namespace dotnetapp.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class CookingClassRequestController : ControllerBase
//     {
//         private readonly CookingClassRequestService _cookingClassRequestService;

//         public CookingClassRequestController(CookingClassRequestService cookingClassRequestService)
//         {
//             _cookingClassRequestService = cookingClassRequestService;
//         }


//         [HttpGet]
//         [Authorize(Roles ="Admin")]
//         public async Task<ActionResult<IEnumerable<CookingClassRequest>>> GetAllCookingClassRequests()
//         {
//             var requests = await _cookingClassRequestService.GetAllCookingClassRequests();
//             return Ok(requests);
//         }


//         [HttpGet("user/{userId}")]
//         [Authorize(Roles ="User")]
//         public async Task<ActionResult<IEnumerable<CookingClassRequest>>> GetCookingClassRequestsByUserId(int userId)
//         {
//             var requests = await _cookingClassRequestService.GetCookingClassRequestsByUserId(userId);
//             return Ok(requests);
//         }

//         [HttpPost]
//         [Authorize(Roles ="User")]
//         public async Task<ActionResult> AddCookingClassRequest([FromBody] CookingClassRequest request)
//         {
//             try
//             {
//                 await _cookingClassRequestService.AddCookingClassRequest(request);
//                 return Ok("Cooking class request added successfully");
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//         }


//         [HttpPut("{requestId}")]
//         public async Task<ActionResult> UpdateCookingClassRequest(int requestId, [FromBody] CookingClassRequest request)
//         {
//             try
//             {
//                 bool updated = await _cookingClassRequestService.UpdateCookingClassRequest(requestId, request);
//                 if (updated)
//                     return Ok("Cooking class request updated successfully");
//                 else
//                     return NotFound("Cannot find the request");
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//         }


//         [HttpDelete("{requestId}")]
//         [Authorize(Roles ="User")]
//         public async Task<ActionResult> DeleteCookingClassRequest(int requestId)
//         {
//             try
//             {
//                 bool deleted = await _cookingClassRequestService.DeleteCookingClassRequest(requestId);
//                 if (deleted)
//                     return Ok("Cooking class request deleted successfully");
//                 else
//                     return NotFound("Cannot find the request");
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//         }
//     }
// }




using dotnetapp.Models;
using dotnetapp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CookingClassRequestController : ControllerBase
    {
        private readonly CookingClassRequestService _cookingClassRequestService;

        public CookingClassRequestController(CookingClassRequestService cookingClassRequestService)
        {
            _cookingClassRequestService = cookingClassRequestService;
        }

        [HttpGet]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CookingClassRequest>>> GetAllCookingClassRequests()
        {
            var requests = await _cookingClassRequestService.GetAllCookingClassRequests();
            return Ok(requests);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<CookingClassRequest>>> GetCookingClassRequestsByUserId(int userId)
        {
            var requests = await _cookingClassRequestService.GetCookingClassRequestsByUserId(userId);
            return Ok(requests);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AddCookingClassRequest([FromBody] CookingClassRequest request)
        {
            try
            {
                await _cookingClassRequestService.AddCookingClassRequest(request);
                return Ok(new { message = "Cooking class request added successfully" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{requestId}")]
        public async Task<ActionResult> UpdateCookingClassRequest(int requestId, [FromBody] CookingClassRequest request)
        {
            try
            {
                bool updated = await _cookingClassRequestService.UpdateCookingClassRequest(requestId, request);
                if (updated)
                    return Ok(new { message = "Cooking class request updated successfully" });
                return NotFound(new { message = "Cannot find the request" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{requestId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DeleteCookingClassRequest(int requestId)
        {
            try
            {
                bool deleted = await _cookingClassRequestService.DeleteCookingClassRequest(requestId);
                if (deleted)
                    return Ok(new { message = "Cooking class request deleted successfully" });
                return NotFound(new { message = "Cannot find the request" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}