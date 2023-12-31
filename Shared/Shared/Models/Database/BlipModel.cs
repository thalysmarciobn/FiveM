﻿using Shared.Interface;

namespace Shared.Models.Database
{
    public class BlipModel : ICoord
    {
        public long Id { get; set; }
        public int BlipId { get; set; }
        public int DisplayId { get; set; }
        public string Title { get; set; }
        public int Color { get; set; }
        public float Scale { get; set; }
        public bool ShortRange { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}