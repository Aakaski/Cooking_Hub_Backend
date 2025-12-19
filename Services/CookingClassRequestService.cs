using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Data;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Exceptions;
namespace dotnetapp.Services
{
    public class CookingClassRequestService
    {
        private readonly ApplicationDbContext _context;

        public CookingClassRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CookingClassRequest>> GetAllCookingClassRequests()
        {
            return await _context.CookingClassRequests
                .Include(r => r.CookingClass)
                .Include(r => r.User)
                .ToListAsync();
        }


        public async Task<IEnumerable<CookingClassRequest>> GetCookingClassRequestsByUserId(int userId)
        {
            return await _context.CookingClassRequests
                .Where(r => r.UserId == userId)
                .Include(r => r.CookingClass)
                .Include(r => r.User)
                .ToListAsync();
        }


        public async Task<bool> AddCookingClassRequest(CookingClassRequest request)
        {
            bool exists = await _context.CookingClassRequests
                .AnyAsync(r => r.CookingClassId == request.CookingClassId && r.UserId == request.UserId);

            if (exists)
                throw new CookingClassException("User already requested this cooking class");

            _context.CookingClassRequests.Add(request);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateCookingClassRequest(int requestId, CookingClassRequest request)
        {
            var existingRequest = await _context.CookingClassRequests.FindAsync(requestId);
            if (existingRequest == null)
                return false;

            existingRequest.UserId = request.UserId;
            existingRequest.RequestDate = request.RequestDate;
            existingRequest.Status = request.Status;
            existingRequest.DietaryPreferences = request.DietaryPreferences;
            existingRequest.CookingGoals = request.CookingGoals;
            existingRequest.Comments = request.Comments;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCookingClassRequest(int requestId)
        {
            var request = await _context.CookingClassRequests.FindAsync(requestId);
            if (request == null)
                return false;

            _context.CookingClassRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

