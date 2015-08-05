using System;
using Microsoft.Xna.Framework;


namespace RogueCastle {
    public class EquipmentData {
        public int BonusArmor;
        public int BonusDamage;
        public int BonusHealth;
        public int BonusMagic;
        public int BonusMana;
        public byte ChestColourRequirement;
        public int Cost = 9999;
        public Color FirstColour = Color.White;
        public byte LevelRequirement;
        public Color SecondColour = Color.White;
        public Vector2[] SecondaryAttribute;
        public int Weight;

        public void Dispose() {
            if (SecondaryAttribute != null)
                Array.Clear(SecondaryAttribute, 0, SecondaryAttribute.Length);
            SecondaryAttribute = null;
        }
    }
}
