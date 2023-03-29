namespace Shared.Models.Server
{
    public enum VehicleModsEnum
    {
        // types engine, brake, transmission, suspension: [ -1 standard, 0 street, 1 sport, 2 race ]
        Spoilers        = 0,
        FrontBumper     = 1,
        RearBumper      = 2,
        SideSkirt       = 3,
        Exhaust         = 4,
        Frame           = 5,
        Grille          = 6,
        Hood            = 7,
        Fender          = 8,
        RightFender     = 9,
        Roof            = 10,
        Engine          = 11,       // -1,0,1,2,3
        Breaks          = 12,       // -1, 0, 1, 2
        Transmission    = 13,       // -1,0,1,2
        Horns           = 14,       // 0 - 57 [ default: -1 to 34 ]
        Suspension      = 15,       // -1,0,1,2,3
        Armor           = 16,       // -1,0,1,2,3,4 [ no armour, 20%, 40%, 60%, 80%, 100% ]
        Turbo           = 18,       // -1,0
        Xenon           = 22,       // -1,0
        FrontWheels     = 23,
        BackWheels      = 24,       // Only for Mortocycles
        PlaterHolders   = 25,
        TrimDesign      = 27,       // -1,0
        Ornaments       = 28,
        DialDesign      = 30,
        SteeringWheel   = 33,
        ShiftLever      = 34,
        Plaques         = 35,
        Hydraulics      = 38,
        Boost           = 40,       // -1,0,1,2,3 [ none, 20%, 60%, 100%, ram ]
        WindowTypes     = 46,       // -1,0,1,2 [ none, light smokeglas, dark smokeglas, limousine ]
        WindowTint      = 55,
        Livery          = 48,
        Plate           = 53,       // 0,1,2,3,4,5 [ blue on white2, blue on white3, yellow on blue, yellow on black, .., .. ]
        Colour1         = 66,       // 0 - 159
        Colour2         = 67        // 0 - 159
    }
}