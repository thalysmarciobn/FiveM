using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Database
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
        public AccountCharacterFaceShapeModel FaceShape { get; set; }
    }
}
