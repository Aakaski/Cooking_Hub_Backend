using dotnetapp.Models;
using dotnetapp.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Data;

namespace dotnetapp.Services
{
    public class CookingClassService
    {
        private readonly ApplicationDbContext _context;

        public CookingClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Get all cooking classes
        public async Task<IEnumerable<CookingClass>> GetAllCookingClasses()
        {
            return await _context.CookingClasses.ToListAsync();
        }

        // 2. Get cooking class by Id
        public async Task<CookingClass> GetCookingClassById(int cookingId)
        {
            return await _context.CookingClasses.FindAsync(cookingId);
        }

        // 3. Add cooking class
        public async Task<bool> AddCookingClass(CookingClass cookingClass)
        {
            bool exists = await _context.CookingClasses
                .AnyAsync(c => c.ClassName == cookingClass.ClassName);

            if (exists)
                throw new CookingClassException("Cooking class with the same name already exists");

            _context.CookingClasses.Add(cookingClass);
            await _context.SaveChangesAsync();
            return true;
        }

        // 4. Update cooking class
        public async Task<bool> UpdateCookingClass(int cookingId, CookingClass cookingClass)
        {
            var existingClass = await _context.CookingClasses.FindAsync(cookingId);
            if (existingClass == null) return false;

            bool nameExists = await _context.CookingClasses
                .AnyAsync(c => c.ClassName == cookingClass.ClassName && c.CookingClassId != cookingId);

            if (nameExists)
                throw new CookingClassException("Cooking class with the same name already exists");


            existingClass.ClassName = cookingClass.ClassName;
            existingClass.ChefName = cookingClass.ChefName;
            existingClass.Location = cookingClass.Location;
            existingClass.DurationInHours = cookingClass.DurationInHours;
              existingClass.CuisineType = cookingClass.CuisineType;
            existingClass.Fee = cookingClass.Fee;
            existingClass.IngredientsProvided = cookingClass.IngredientsProvided;
            existingClass.SkillLevel = cookingClass.SkillLevel;
            existingClass.SpecialRequirements = cookingClass.SpecialRequirements;

            await _context.SaveChangesAsync();
            return true;
        }

        // 5. Delete cooking class
        public async Task<bool> DeleteCookingClass(int cookingId)
        {
            var cookingClass = await _context.CookingClasses.FindAsync(cookingId);
            if (cookingClass == null) return false;

            _context.CookingClasses.Remove(cookingClass);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}