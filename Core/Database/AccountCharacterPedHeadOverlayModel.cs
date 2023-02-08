﻿using Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Database
{
    public class AccountCharacterPedHeadOverlayModel
    {
        public long CharacterId { get; set; }
        public OverlayEnum OverlayId { get; set; }
        public int Index { get; set; }
        public float Opacity { get; set; }
    }
}
