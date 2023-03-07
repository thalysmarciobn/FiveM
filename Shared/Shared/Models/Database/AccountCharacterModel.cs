using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Database
{
    public class AccountCharacterModel
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public int Slot { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateCreated { get; set; }
        public string Model { get; set; }
        public int Health { get; set; }
        public int Armor { get; set; }
        public int MoneyBalance { get; set; }
        public int BankBalance { get; set; }
        public int EyeColorId { get; set; }
        public float Heading { get; set; }
        public AccountCharacterPositionModel Position { get; set; }
        public AccountCharacterRotationModel Rotation { get; set; }
        public AccountCharacterPedHeadModel PedHead { get; set; }
        public AccountCharacterPedHeadDataModel PedHeadData { get; set; }
        public ICollection<AccountCharacterPedFaceModel> PedFace { get; set; }
        public ICollection<AccountCharacterPedComponentModel> PedComponent { get; set; }
        public ICollection<AccountCharacterPedPropModel> PedProp { get; set; }
        public ICollection<AccountCharacterPedHeadOverlayModel> PedHeadOverlay { get; set; }
        public ICollection<AccountCharacterPedHeadOverlayColorModel> PedHeadOverlayColor { get; set; }
    }
}
