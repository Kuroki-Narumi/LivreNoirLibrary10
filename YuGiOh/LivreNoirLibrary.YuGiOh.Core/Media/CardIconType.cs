using System;

namespace LivreNoirLibrary.YuGiOh.Media
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
        Token,
        Monster = Token,

        Pendulum = 64,
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
    }
}
