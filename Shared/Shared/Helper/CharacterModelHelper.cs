using Shared.Enumerations;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Helper
{
    public static class CharacterModelHelper
    {
        public static ICollection<T> DefaultList<T>()
        {
            var models = new List<T>();

            switch (typeof(T).Name)
            {
                case nameof(AccountCharacterPedFaceModel):
                    foreach (FaceShapeEnum enumId in Enum.GetValues(typeof(FaceShapeEnum)))
                    {
                        var input = models as List<AccountCharacterPedFaceModel>;
                        input.Add(new AccountCharacterPedFaceModel
                        {
                            Index = enumId,
                            Scale = 0
                        });
                    }
                    break;
                case nameof(AccountCharacterPedComponentModel):
                    foreach (ComponentVariationEnum enumId in Enum.GetValues(typeof(ComponentVariationEnum)))
                    {
                        var input = models as List<AccountCharacterPedComponentModel>;
                        input.Add(new AccountCharacterPedComponentModel
                        {
                            ComponentId = enumId,
                            Index = 0,
                            Texture = 0
                        });
                    }
                    break;
                case nameof(AccountCharacterPedPropModel):
                    foreach (PropVariationEnum enumId in Enum.GetValues(typeof(PropVariationEnum)))
                    {
                        var input = models as List<AccountCharacterPedPropModel>;
                        input.Add(new AccountCharacterPedPropModel
                        {
                            PropId = enumId,
                            Index = -1,
                            Texture = -1
                        });
                    }
                    break;
                case nameof(AccountCharacterPedHeadOverlayModel):
                    foreach (OverlayEnum enumId in Enum.GetValues(typeof(OverlayEnum)))
                    {
                        var input = models as List<AccountCharacterPedHeadOverlayModel>;
                        input.Add(new AccountCharacterPedHeadOverlayModel
                        {
                            OverlayId = enumId,
                        });
                    }
                    break;
            }
            return models;
        }
    }
}
