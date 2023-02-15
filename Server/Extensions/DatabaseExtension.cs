using FiveM.Server.Database;
using Microsoft.EntityFrameworkCore;
using Shared.Helper;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Extensions
{
    public static class DatabaseExtension
    {
        public static AccountModel GetAccount(this FiveMContext context, string license)
        {
            return context.Account
                .Include(m => m.Character).ThenInclude(m => m.Position)
                .Include(m => m.Character).ThenInclude(m => m.Rotation)
                .Include(m => m.Character).ThenInclude(m => m.PedHeadData)
                .Include(m => m.Character).ThenInclude(m => m.PedHead)
                .Include(m => m.Character).ThenInclude(m => m.PedFace)
                .Include(m => m.Character).ThenInclude(m => m.PedComponent)
                .Include(m => m.Character).ThenInclude(m => m.PedProp)
                .Include(m => m.Character).ThenInclude(m => m.PedHeadOverlay)
                .Include(m => m.Character).ThenInclude(m => m.PedHeadOverlayColor)
                .FirstOrDefault(x => x.License == license);
        }
    }
}
