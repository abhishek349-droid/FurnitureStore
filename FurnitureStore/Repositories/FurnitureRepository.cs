using FurnitureStore.Contracts;
using FurnitureStore.Entity;
using FurnitureStore.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Repositories
{
    public class FurnitureRepository : IFurnitureRepository
    {
        private readonly ApplicationDbContext _db;

        public FurnitureRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool AddFurniture(FurnitureModel furniture)
        {
            try
            {
                _db.FurnitureStore.Add(furniture);
                var result = _db.SaveChanges();
                return result >= 1 ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool DeleteFurniture(int furnitureId)
        {
            try
            {
                var furniture = _db.FurnitureStore.Where(f => f.FurnitureId == furnitureId).FirstOrDefault();
                if (furniture != null)
                {
                    _db.FurnitureStore.Remove(furniture);
                    var result = _db.SaveChanges();
                    return result >= 1 ? true : false;
                }
                throw new Exception("No furniture with provided ID");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public FurnitureModel GetFurniture(int furnitureId)
        {
            try
            {
                var furniture = _db.FurnitureStore.Where(f => f.FurnitureId == furnitureId).FirstOrDefault();
                if (furniture != null) { return furniture; }
                throw new Exception("No furniture with the provided ID");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<FurnitureModel> GetFurnitures()
        {
            try
            {
                var furnitures = _db.FurnitureStore.ToList();
                if (furnitures.Count > 0) { return furnitures; }
                throw new Exception("No furniture to display");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool UpdateFurniture(int id, FurnitureModel furniture)
        {
            try
            {
                var furnitureToBeUpdated = _db.FurnitureStore.Where(f => f.FurnitureId == id).FirstOrDefault();
                if (furnitureToBeUpdated != null)
                {
                    furnitureToBeUpdated.FurnitureName = furniture.FurnitureName;
                    furnitureToBeUpdated.Cost = furniture.Cost;
                    furnitureToBeUpdated.Quantity = furniture.Quantity;
                    _db.FurnitureStore.Update(furnitureToBeUpdated);
                    var result = _db.SaveChanges();
                    return result >= 1 ? true : false;
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
