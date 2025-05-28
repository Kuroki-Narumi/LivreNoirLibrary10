using System;

namespace LivreNoirLibrary.YuGiOh.Controls
{
    public enum CardIconType
    {
        Normal,
        Effect,
        Fusion,
        Ritual,
        Synchro,
        Xyz,
        Link,
        Monster = 7,

        Pendulum = 8,
        Effect_Pendulum = Effect | Pendulum,
        Fusion_Pendulum = Fusion | Pendulum,
        Ritual_Pendulum = Ritual | Pendulum,
        Synchro_Pendulum = Synchro | Pendulum,
        Xyz_Pendulum = Xyz | Pendulum,

        Spell = 16,
        Field_Spell,
        Equip_Spell,
        Continuous_Spell,
        Quick_Spell,
        Ritual_Spell,

        Trap = 32,
        Continuous_Trap,
        Counter_Trap,

        Token = 64,
    }
}
