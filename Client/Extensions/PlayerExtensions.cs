﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using Shared.Models.Database;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;
using static Client.Extensions.PlayerExtensions;

namespace Client.Extensions
{
    public static class PlayerExtensions
    {
        public static void SetEntityNoCollision(this Entity one, Entity two)
        {
            if (one == null || two == null)
                return;

            SetEntityNoCollisionEntity(one.Handle, two.Handle, true);
            SetEntityNoCollisionEntity(two.Handle, one.Handle, true);
        }

        public static Vehicle GetHookedVehicle(this Vehicle vehicle)
        {
            // If the vehicle is invalid, return
            if (vehicle == null || !vehicle.Exists())
                return null;

            // Start by trying to get the vehicle attached as a trailer
            int trailer = 0;
            if (GetVehicleTrailerVehicle(vehicle.Handle, ref trailer))
                return Entity.FromHandle(trailer) as Vehicle;

            // Try to get a hooked cargobob vehicle and return it if there is somehing
            Vehicle cargobobHook = Entity.FromHandle(GetVehicleAttachedToCargobob(vehicle.Handle)) as Vehicle;
            if (cargobobHook != null && cargobobHook.Exists())
                return cargobobHook;

            // Then, try to get it as a tow truck and return it if it does
            Vehicle towHooked = Entity.FromHandle(GetEntityAttachedToTowTruck(vehicle.Handle)) as Vehicle;
            if (towHooked != null && towHooked.Exists())
                return towHooked;
            return null;
        }

        public static void SetAlpha(this Entity entity, int alpha)
        {
            if (alpha == 255)
                ResetEntityAlpha(entity.Handle);
            else
                SetEntityAlpha(entity.Handle, alpha, 0);
        }

        public static void ResetAlpha(this Entity entity)
        {
            ResetEntityAlpha(entity.Handle);
        }

        public static void SetPedHeadBlendDatas(this Player player, AccountCharacterPedHeadDataModel model)
        {
            var ped = player.Character.Handle;
            SetPedHeadBlendData(ped, model.ShapeFirstID, model.ShapeSecondID, model.ShapeThirdID, model.SkinFirstID, model.SkinSecondID, model.SkinThirdID, model.ShapeMix, model.SkinMix, model.ThirdMix, model.IsParent);
        }

        public static void SetPedHead(this Player player, AccountCharacterPedHeadModel model)
        {
            var ped = player.Character.Handle;
            SetPedHairColor(ped, model.HairColorId, model.HairHighlightColor);
            SetPedEyeColor(ped, model.EyeColorId);
        }

        public static void SetPedHeadOverlays(this Player player, ICollection<AccountCharacterPedHeadOverlayModel> model)
        {
            var ped = player.Character.Handle;
            foreach (var overlay in model)
                SetPedHeadOverlay(ped, (int)overlay.OverlayId, overlay.Index, overlay.Opacity);
        }

        public static void SetPedHeadOverlayColors(this Player player, ICollection<AccountCharacterPedHeadOverlayColorModel> model)
        {
            var ped = player.Character.Handle;
            foreach (var overlay in model)
                SetPedHeadOverlayColor(ped, (int)overlay.OverlayId, overlay.ColorType, overlay.ColortId, overlay.SecondColorId);
        }

        public static void SetPedFaceFeatures(this Player player, ICollection<AccountCharacterPedFaceModel> shapes)
        {
            var ped = player.Character.Handle;
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

        public static void Freeze(this Player player, bool freeze = true)
        {
            player.CanControlCharacter = !freeze;
            player.Character.IsVisible = !freeze;
            player.Character.IsCollisionEnabled = !freeze;
            player.Character.IsPositionFrozen = freeze;
            player.Character.IsInvincible = freeze;
            player.Character.Task.ClearAllImmediately();
        }

        public static void Unfreeze(this Player player) => player.Freeze(false);

        public static void Spawn(this Player player, Vector3 position)
        {
            player.Freeze();

            LoadScene(position.X, position.Y, position.Z);
            RequestCollisionAtCoord(position.X, position.Y, position.Z);

            player.Character.Position = position;
            player.Character.ClearBloodDamage();
            player.Character.Weapons.Drop();
            player.WantedLevel = 0;

            player.Unfreeze();
        }

    }
}
