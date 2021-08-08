using FurnitureStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Contracts
{
    public interface IFurnitureRepository
    {
        IEnumerable<FurnitureModel> GetFurnitures();
        FurnitureModel GetFurniture(int furnitureId);
        bool AddFurniture(FurnitureModel furniture);
        bool UpdateFurniture(int id, FurnitureModel furniture);
        bool DeleteFurniture(int furnitureId);
    }
}
