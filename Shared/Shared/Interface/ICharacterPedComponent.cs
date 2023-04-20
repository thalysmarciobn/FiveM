using Shared.Enumerations;
using System;
using System.Collections.Generic;

namespace Shared.Interface
{
    public interface ICharacterPedComponent
    {
        int DrawableId { get; set; }
        int TextureId { get; set; }
    }
}