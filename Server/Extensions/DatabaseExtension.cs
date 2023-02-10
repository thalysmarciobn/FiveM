using FiveM.Server.Database;
using Microsoft.EntityFrameworkCore;
using Shared.Helper;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Extensions
{
    public static class DatabaseExtension
    {
        public static AccountModel GetAccount(this FiveMContext context, string license)
        {
            return context.Account
                .Include(m => m.Character).ThenInclude(m => m.Position)
                .Include(m => m.Character).ThenInclude(m => m.PedHeadData)
                .Include(m => m.Character).ThenInclude(m => m.PedHead)
                .Include(m => m.Character).ThenInclude(m => m.PedFace)
                .Include(m => m.Character).ThenInclude(m => m.PedComponent)
                .Include(m => m.Character).ThenInclude(m => m.PedProp)
                .Include(m => m.Character).ThenInclude(m => m.PedHeadOverlay)
                .Include(m => m.Character).ThenInclude(m => m.PedHeadOverlayColor)
                .Single(x => x.License == license);
        }
    }
}
