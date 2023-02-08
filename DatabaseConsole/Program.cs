using Microsoft.EntityFrameworkCore;

using var context = new Context();

var account = context.AccountCharacters.Include(x => x.Position).SingleOrDefault(x => x.Id == 1);

Console.WriteLine(account.Position.X);