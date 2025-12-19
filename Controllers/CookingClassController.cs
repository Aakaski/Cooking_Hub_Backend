// using dotnetapp.Models;
// using dotnetapp.Services;
// using dotnetapp.Exceptions;
// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using System;
// using Microsoft.AspNetCore.Authorization;

// namespace dotnetapp.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class CookingClassController : ControllerBase
//     {
//         private readonly CookingClassService _cookingClassService;

//         public CookingClassController(CookingClassService cookingClassService)
//         {
//             _cookingClassService = cookingClassService;
//         }

//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<CookingClass>>> GetAllCookingClasses()
//         {
//             try
//             {
//                 var classes = await _cookingClassService.GetAllCookingClasses();
//                 return Ok(classes);
//             }
//             catch (System.Exception ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//         }

//         [HttpGet("{classId}")]
//         [Authorize(Roles ="Admin")]
//         public async Task<ActionResult<CookingClass>> GetCookingClassById(int classId)
//         {
//             try
//             {
//                 var cookingClass = await _cookingClassService.GetCookingClassById(classId);
//                 if (cookingClass == null)
//                     return NotFound("Cannot find any cooking");
//                 return Ok(cookingClass);
//             }
//             catch (System.Exception ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//         }

//         [HttpPost]
//         [Authorize(Roles ="Admin")]
//         public async Task<ActionResult> AddCookingClass([FromBody] CookingClass cooking)
//         {
//             try
//             {
//                 bool result = await _cookingClassService.AddCookingClass(cooking);
//                 if (result)
//                     return Ok("Cooking class added successfully");
//                 return StatusCode(500, "Failed to add cooking class");
//             }
//             catch (CookingClassException ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//             catch (System.Exception ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//         }

//         [HttpPut("{classId}")]
//         [Authorize(Roles ="Admin")]
//         public async Task<ActionResult> UpdateCookingClass(int classId, [FromBody] CookingClass cooking)
//         {
//             try
//             {
//                 bool result = await _cookingClassService.UpdateCookingClass(classId, cooking);
//                 if (result)
//                     return Ok("Cooking class updated successfully");
//                 return NotFound("Cannot find any cooking");
//             }
//             catch (CookingClassException ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//             catch (System.Exception ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//         }

//         [Authorize(Roles ="Admin")]
//         [HttpDelete("{classId}")]
//         public async Task<ActionResult> DeleteCookingClass(int classId)
//         {
//             try
//             {
//                 bool result = await _cookingClassService.DeleteCookingClass(classId);
//                 if (result)
//                     return Ok("Cooking class deleted successfully");
//                 return NotFound("Cannot find any cooking");
//             }
//             catch (System.Exception ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//         }
//     }
// }
 
using dotnetapp.Models;
using dotnetapp.Services;
using dotnetapp.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CookingClassController : ControllerBase
    {
        private readonly CookingClassService _cookingClassService;

        public CookingClassController(CookingClassService cookingClassService)
        {
            _cookingClassService = cookingClassService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CookingClass>>> GetAllCookingClasses()
        {
            var classes = await _cookingClassService.GetAllCookingClasses();
            return Ok(classes);
        }

        [HttpGet("{classId}")]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CookingClass>> GetCookingClassById(int classId)
        {
            var cookingClass = await _cookingClassService.GetCookingClassById(classId);
            if (cookingClass == null)
                return NotFound(new { message = "Cannot find any cooking class" });
            return Ok(cookingClass);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddCookingClass([FromBody] CookingClass cooking)
        {
            try
            {
                bool result = await _cookingClassService.AddCookingClass(cooking);
                if (result)
                    return Ok(new { message = "Cooking class added successfully" });
                return StatusCode(500, new { message = "Failed to add cooking class" });
            }
            catch (CookingClassException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{classId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCookingClass(int classId, [FromBody] CookingClass cooking)
        {
            try
            {
                bool result = await _cookingClassService.UpdateCookingClass(classId, cooking);
                if (result)
                    return Ok(new { message = "Cooking class updated successfully" });
                return NotFound(new { message = "Cannot find any cooking class" });
            }
            catch (CookingClassException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{classId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCookingClass(int classId)
        {
            try
            {
                bool result = await _cookingClassService.DeleteCookingClass(classId);
                if (result)
                    return Ok(new { message = "Cooking class deleted successfully" });
                return NotFound(new { message = "Cannot find any cooking class" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}