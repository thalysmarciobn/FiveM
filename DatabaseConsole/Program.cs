using Microsoft.EntityFrameworkCore;
using Shared.Models.Database;
using System.ComponentModel;

using var context = new Context();
var transaction = context.Database.BeginTransaction();

var character = new AccountCharacterModel
{
    Slot = 0,
    DateCreated = DateTime.Now,
    Model = "mp_m_freemode_01",
    Position = new AccountCharacterPositionModel
    {
        X = -1062.02f,
        Y = -2711.85f,
        Z = 0.83f
    },
    PedHeadData = new AccountCharacterPedHeadDataModel
    {

    },
    PedHead = new AccountCharacterPedHeadModel
    {

    },
    PedFace = new List<AccountCharacterPedFaceModel>(),
    PedComponent = new List<AccountCharacterPedComponentModel>(),
    PedProp = new List<AccountCharacterPedPropModel>(),
    PedHeadOverlay = new List<AccountCharacterPedHeadOverlayModel>(),
    PedHeadOverlayColor = new List<AccountCharacterPedHeadOverlayColorModel>()
};
var account = new AccountModel()
{
    License = "aa",
    Created = DateTime.Now,
    WhiteListed = true,
    Character = new List<AccountCharacterModel> { character }
};

context.Account.Add(account);
context.SaveChanges();

Thread.Sleep(10000);

transaction.Rollback();