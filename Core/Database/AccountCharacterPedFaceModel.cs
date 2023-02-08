﻿using Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Database
{
    public class AccountCharacterPedFaceModel
    {
        public long CharacterId { get; set; }
        public FaceShapeEnum Index { get; set; }
        public float Scale { get; set; }
    }
}
