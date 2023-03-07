using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Core.NUI
{
    internal class NUIRegisterCharacter
    {
        public string Model { get; set; }
        public NUIRegisterCharacterHeadBlend HeadBlend { get; set; }
        public int EyeColor { get; set; }
        public NUIRegisterCharacterHair Hair { get; set; }
        public NUIRegisterCharacterComponents Components { get; set; }
        public NUIRegisterCharacterProps Props { get; set; }
        public NUIRegisterCharacterFaceFeatures FaceFeatures { get; set; }
        public NUIRegisterCharacterHeadOverlays HeadOverlays { get; set; }
        public NUIRegisterCharacterHeadOverlayColors HeadOverlayColors { get; set; }
    }

    internal class NUIRegisterCharacterHeadBlend
    {
        public int ShapeFirst { get; set; }
        public int ShapeSecond { get; set; }
        public int SkinFirst { get; set; }
        public int SkinSecond { get; set; }
        public float ShapeMix { get; set; }
        public float SkinMix { get; set; }
    }

    internal class NUIRegisterCharacterHair
    {
        public int Color { get; set; }
        public int Highlight { get; set; }
    }

    internal class NUIRegisterCharacterComponents
    {
        public NUIRegisterCharacterComponent Face { get; set; }
        public NUIRegisterCharacterComponent Mask { get; set; }
        public NUIRegisterCharacterComponent Hair { get; set; }
        public NUIRegisterCharacterComponent Torso { get; set; }
        public NUIRegisterCharacterComponent Legs { get; set; }
        public NUIRegisterCharacterComponent Bag { get; set; }
        public NUIRegisterCharacterComponent Shoes { get; set; }
        public NUIRegisterCharacterComponent Accessory { get; set; }
        public NUIRegisterCharacterComponent Undershirt { get; set; }
        public NUIRegisterCharacterComponent Kevlar { get; set; }
        public NUIRegisterCharacterComponent Badge { get; set; }
        public NUIRegisterCharacterComponent Torso2 { get; set; }
    }

    internal class NUIRegisterCharacterComponent
    {
        public int ComponentId { get; set; }
        public int DrawableId { get; set; }
        public int TextureId { get; set; }
        public int PalleteId { get; set; }
    }

    internal class NUIRegisterCharacterProps
    {
        public NUIRegisterCharacterProp Hat { get; set; }
        public NUIRegisterCharacterProp Glasses { get; set; }
        public NUIRegisterCharacterProp EarPieces { get; set; }
        public NUIRegisterCharacterProp Watches { get; set; }
        public NUIRegisterCharacterProp Wristbands { get; set; }
    }

    internal class NUIRegisterCharacterProp
    {
        public int PropId { get; set; }
        public int ComponentId { get; set; }
        public int DrawableId { get; set; }
        public int TextureId { get; set; }
        public bool Attach { get; set; }
    }

    internal class NUIRegisterCharacterHeadOverlays
    {
        public NUIRegisterCharacterHeadOverlay Blemishes { get; set; }
        public NUIRegisterCharacterHeadOverlay Beard { get; set; }
        public NUIRegisterCharacterHeadOverlay Eyebrows { get; set; }
        public NUIRegisterCharacterHeadOverlay Ageing { get; set; }
        public NUIRegisterCharacterHeadOverlay MakeUp { get; set; }
        public NUIRegisterCharacterHeadOverlay Blush { get; set; }
        public NUIRegisterCharacterHeadOverlay Complexion { get; set; }
        public NUIRegisterCharacterHeadOverlay SunDamage { get; set; }
        public NUIRegisterCharacterHeadOverlay Lipstick { get; set; }
        public NUIRegisterCharacterHeadOverlay MoleAndFreckles { get; set; }
        public NUIRegisterCharacterHeadOverlay ChestHair { get; set; }
        public NUIRegisterCharacterHeadOverlay BodyBlemishes { get; set; }
        public NUIRegisterCharacterHeadOverlay AddBodyBlemishes { get; set; }
    }

    internal class NUIRegisterCharacterHeadOverlay
    {
        public int Overlay { get; set; }
        public int Index { get; set; }
        public float Opacity { get; set; }
    }

    internal class NUIRegisterCharacterHeadOverlayColors
    {
        public NUIRegisterCharacterHeadOverlayColor Blemishes { get; set; }
        public NUIRegisterCharacterHeadOverlayColor Beard { get; set; }
        public NUIRegisterCharacterHeadOverlayColor Eyebrows { get; set; }
        public NUIRegisterCharacterHeadOverlayColor Ageing { get; set; }
        public NUIRegisterCharacterHeadOverlayColor MakeUp { get; set; }
        public NUIRegisterCharacterHeadOverlayColor Blush { get; set; }
        public NUIRegisterCharacterHeadOverlayColor Complexion { get; set; }
        public NUIRegisterCharacterHeadOverlayColor SunDamage { get; set; }
        public NUIRegisterCharacterHeadOverlayColor Lipstick { get; set; }
        public NUIRegisterCharacterHeadOverlayColor MoleAndFreckles { get; set; }
        public NUIRegisterCharacterHeadOverlayColor ChestHair { get; set; }
        public NUIRegisterCharacterHeadOverlayColor BodyBlemishes { get; set; }
        public NUIRegisterCharacterHeadOverlayColor AddBodyBlemishes { get; set; }
    }

    internal class NUIRegisterCharacterHeadOverlayColor
    {
        public int Overlay { get; set; }
        public int ColorType { get; set; }
        public int ColorId { get; set; }
        public int SecondColorId { get; set; }
    }

    internal class NUIRegisterCharacterFaceFeatures
    {
        public float NoseWidth { get; set; }
        public float NosePeakHeight { get; set; }
        public float NosePeakSize { get; set; }
        public float NoseBoneHeight { get; set; }
        public float NosePeakLowering { get; set; }
        public float NoseBoneTwist { get; set; }
        public float EyeBrownHeight { get; set; }
        public float EyeBrownForward { get; set; }
        public float CheeksBoneHeight { get; set; }
        public float CheeksBoneWidth { get; set; }
        public float CheeksWidth { get; set; }
        public float EyesOpening { get; set; }
        public float LipsThickness { get; set; }
        public float JawBoneWidth { get; set; }
        public float JawBoneBackSize { get; set; }
        public float ChinBoneLowering { get; set; }
        public float ChinBoneLenght { get; set; }
        public float ChinBoneSize { get; set; }
        public float ChinHole { get; set; }
        public float NeckThickness { get; set; }
    }
}
