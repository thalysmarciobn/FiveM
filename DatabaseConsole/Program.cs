using Microsoft.EntityFrameworkCore;
using Shared.Models.Database;
using System.ComponentModel;

using var context = new Context();

var account = context.Account
    .Include(m => m.Character).ThenInclude(m => m.Position)
    .Include(m => m.Character).ThenInclude(m => m.PedHeadData)
    .Include(m => m.Character).ThenInclude(m => m.PedHead)
    .Include(m => m.Character).ThenInclude(m => m.PedFace)
    .Include(m => m.Character).ThenInclude(m => m.PedComponent)
    .Include(m => m.Character).ThenInclude(m => m.PedProp)
    .Include(m => m.Character).ThenInclude(m => m.PedHeadOverlay)
    .Include(m => m.Character).ThenInclude(m => m.PedHeadOverlayColor)
    .Single(x => x.License == "07041d870811cccd5a93a5a012970b341d168b9a");

var character = account.Character.First();
Console.WriteLine(character.Position.X);