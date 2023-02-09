using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Database
{
    public class AccountCharacterModel
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateCreated { get; set; }
        public int Gender { get; set; }
        public int Armor { get; set; }
        public string Model { get; set; }
        public AccountCharacterPositionModel Position { get; set; }
        public AccountCharacterPedHeadDataModel PedHeadData { get; set; }
        public AccountCharacterPedHeadModel PedHead { get; set; }
        public ICollection<AccountCharacterPedFaceModel> PedFace { get; set; }
        public ICollection<AccountCharacterPedComponentModel> PedComponent { get; set; }
        public ICollection<AccountCharacterPedPropModel> PedProp { get; set; }
        public ICollection<AccountCharacterPedHeadOverlayModel> PedHeadOverlay { get; set; }
        public ICollection<AccountCharacterPedHeadOverlayColorModel> PedHeadOverlayColor { get; set; }
    }
}
