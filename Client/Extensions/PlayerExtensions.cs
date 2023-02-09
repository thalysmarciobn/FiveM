using CitizenFX.Core;
using Shared.Models.Database;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace Client.Extensions
{
    public static class PlayerExtensions
    {
        public static void SetPedHeadBlendDatas(this Player player, AccountCharacterPedHeadDataModel model)
        {
            SetPedHeadBlendData(player.Handle, model.ShapeFirstID, model.ShapeSecondID, model.ShapeThirdID, model.SkinFirstID, model.SkinSecondID, model.SkinThirdID, model.ShapeMix, model.SkinMix, model.ThirdMix, model.IsParent);
        }

        public static void SetPedHead(this Player player, AccountCharacterPedHeadModel model)
        {
            var ped = player.Handle;
            SetPedHairColor(ped, model.HairColorId, model.HairHighlightColor);
            SetPedEyeColor(ped, model.EyeColorId);
        }

        public static void SetPedHeadOverlays(this Player player, ICollection<AccountCharacterPedHeadOverlayModel> model)
        {
            var ped = player.Handle;
            foreach (var overlay in model)
                SetPedHeadOverlay(ped, (int)overlay.OverlayId, overlay.Index, overlay.Opacity);
        }

        public static void SetPedHeadOverlayColors(this Player player, ICollection<AccountCharacterPedHeadOverlayColorModel> model)
        {
            var ped = player.Handle;
            foreach (var overlay in model)
                SetPedHeadOverlayColor(ped, (int)overlay.OverlayId, overlay.ColorType, overlay.ColortId, overlay.SecondColorId);
        }

        public static void SetPedFaceFeatures(this Player player, ICollection<AccountCharacterPedFaceModel> shapes)
        {
            var ped = player.Handle;
            foreach (var shape in shapes)
                SetPedFaceFeature(ped, (int) shape.Index, shape.Scale);
        }

        public static void StyleComponents(this Player player, ICollection<AccountCharacterPedComponentModel> components)
        {
            foreach (var component in components)
                player.Character.Style[(PedComponents)component.ComponentId].SetVariation(component.Index, component.Texture);
        }

        public static void StyleProps(this Player player, ICollection<AccountCharacterPedPropModel> components)
        {
            foreach (var component in components)
                player.Character.Style[(PedProps)component.PropId].SetVariation(component.Index, component.Texture);
        }
    }
}
