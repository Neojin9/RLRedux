using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;


namespace RogueCastle {
    public class PlayerObj : CharacterObj, IDealsDamageObj {
        private const int DEBUG_INPUT_SWAPWEAPON = 0;
        private const int DEBUG_INPUT_GIVEMANA = 1;
        private const int DEBUG_INPUT_GIVEHEALTH = 2;
        private const int DEBUG_INPUT_LEVELUP_STRENGTH = 3;
        private const int DEBUG_INPUT_LEVELUP_HEALTH = 4;
        private const int DEBUG_INPUT_LEVELUP_ENDURANCE = 5;
        private const int DEBUG_INPUT_LEVELUP_EQUIPLOAD = 6;
        private const int DEBUG_INPUT_TRAITSCREEN = 7;
        private const int DEBUG_UNLOCK_ALL_BLUEPRINTS = 8;
        private const int DEBUG_PURCHASE_ALL_BLUEPRINTS = 9;
        public const int STATE_IDLE = 0;
        public const int STATE_WALKING = 1;
        public const int STATE_JUMPING = 2;
        public const int STATE_HURT = 3;
        public const int STATE_DASHING = 4;
        public const int STATE_LEVELUP = 5;
        public const int STATE_BLOCKING = 6;
        public const int STATE_FLYING = 7;
        public const int STATE_TANOOKI = 8;
        public const int STATE_DRAGON = 9;
        private int ArmorReductionMod;
        private float AxeProjectileSpeed = 1100f;
        private Vector2 AxeSpellScale = new Vector2(3f, 3f);
        private float BaseStatDropChance;
        private float ComboDelay = 1f;
        private float DaggerProjectileSpeed = 900f;
        private Vector2 DaggerSpellScale = new Vector2(3.5f, 3.5f);
        private int DamageGainPerLevel;
        private float DashCoolDown;
        private float DashSpeed;
        private float DashTime;
        private int HealthGainPerLevel;
        private float JumpDeceleration;
        private int ManaGainPerLevel;
        private float RunSpeedMultiplier;
        private float m_Spell_Close_Lifespan = 6f;
        private float m_Spell_Close_Scale = 3.5f;
        private LogicSet m_airAttackLS;
        private byte m_airDashCount;
        private float m_ambilevousTimer = 0.5f;
        private float m_assassinDrainCounter;
        private float m_assassinSmokeTimer = 0.5f;
        private bool m_assassinSpecialActive;
        private float m_attackCounter;
        private int m_attackNumber;
        private byte m_attacksNeededForMana;
        private ProjectileData m_axeProjData;
        private float m_blockInvincibleCounter;
        private float m_blockManaDrain;
        private IPhysicsObj m_closestGround;
        private bool m_collidingLeft;
        private bool m_collidingLeftOnly;
        private bool m_collidingRight;
        private bool m_collidingRightOnly;
        private LogicSet m_currentLogicSet;
        private float m_currentMana;
        private bool m_damageShieldCast;
        private float m_damageShieldDrainCounter;
        private int m_dashCooldownCounter;
        private int m_dashCounter;
        private InputMap m_debugInputMap;
        private byte m_doubleJumpCount;
        private float m_dragonManaRechargeCounter;
        private float m_dropThroughGroundDuration = 0.1f;
        private float m_dropThroughGroundTimer;
        private Vector2 m_enemyKnockBack = Vector2.Zero;
        private LogicSet m_externalLS;
        private float m_flightCounter;
        private TextObj m_flightDurationText;
        private TweenObject m_flipTween;
        private Game m_game;
        private int m_invincibleCounter;
        private bool m_isFlying;
        private bool m_isJumping;
        private TeleporterObj m_lastTouchedTeleporter;
        private Color m_lichColour1 = new Color(255, 255, 255, 255);
        private Color m_lichColour2 = new Color(198, 198, 198, 255);
        private float m_lightDrainCounter;
        private bool m_lightOn;
        private bool m_lockControls;
        private float m_manaGain;
        private bool m_megaDamageShieldCast;
        private float m_ninjaTeleportDelay;
        private byte m_numSequentialAttacks;
        private SpriteObj m_playerHead;
        private PlayerIndex m_playerIndex;
        private SpriteObj m_playerLegs;
        private bool m_previousIsTouchingGround;
        private ProjectileData m_rapidDaggerProjData;
        private float m_rapidSpellCastDelay;
        private Color m_skinColour1 = new Color(231, 175, 131, 255);
        private Color m_skinColour2 = new Color(199, 109, 112, 255);
        private float m_spellCastDelay;
        private LogicSet m_standingAttack3LogicSet;
        private float m_startingAnimationDelay;
        private SpriteObj m_swearBubble;
        private float m_swearBubbleCounter;
        private float m_tanookiDrainCounter;
        private bool m_timeStopCast;
        private float m_timeStopDrainCounter;
        private ObjContainer m_translocatorSprite;
        private FrameSoundObj m_walkDownSound;
        private FrameSoundObj m_walkDownSoundHigh;
        private FrameSoundObj m_walkDownSoundLow;
        private FrameSoundObj m_walkUpSound;
        private FrameSoundObj m_walkUpSoundHigh;
        private FrameSoundObj m_walkUpSoundLow;
        private float m_wizardSparkleCounter = 0.2f;
        private List<byte> m_wizardSpellList;

        public PlayerObj(string spriteName, PlayerIndex playerIndex, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, Game game) : base(spriteName, physicsManager, levelToAttachTo) {
            m_game = game;
            m_playerLegs = (base.GetChildAt(2) as SpriteObj);
            m_playerHead = (base.GetChildAt(12) as SpriteObj);
            m_playerIndex = playerIndex;
            m_currentLogicSet = new LogicSet(null);
            base.State = 0;
            base.CollisionTypeTag = 2;
            m_debugInputMap = new InputMap(PlayerIndex.Two, false);
            InitializeInputMap();
            m_walkDownSound = new FrameSoundObj(m_playerLegs, 4, new[] {
                "Player_WalkDown01",
                "Player_WalkDown02"
            });
            m_walkUpSound = new FrameSoundObj(m_playerLegs, 1, new[] {
                "Player_WalkUp01",
                "Player_WalkUp02"
            });
            m_walkUpSoundHigh = new FrameSoundObj(m_playerLegs, 1, new[] {
                "Player_WalkUp01_High",
                "Player_WalkUp02_High"
            });
            m_walkDownSoundHigh = new FrameSoundObj(m_playerLegs, 4, new[] {
                "Player_WalkDown01_High",
                "Player_WalkDown02_High"
            });
            m_walkUpSoundLow = new FrameSoundObj(m_playerLegs, 1, new[] {
                "Player_WalkUp01_Low",
                "Player_WalkUp02_Low"
            });
            m_walkDownSoundLow = new FrameSoundObj(m_playerLegs, 4, new[] {
                "Player_WalkDown01_Low",
                "Player_WalkDown02_Low"
            });
            m_externalLS = new LogicSet(null);
            m_flightDurationText = new TextObj(Game.JunicodeFont);
            m_flightDurationText.FontSize = 12f;
            m_flightDurationText.Align = Types.TextAlign.Centre;
            m_flightDurationText.DropShadow = new Vector2(2f, 2f);
            base.OutlineWidth = 2;
            m_translocatorSprite = new ObjContainer("PlayerIdle_Character");
            m_translocatorSprite.Visible = false;
            m_translocatorSprite.OutlineColour = Color.Blue;
            m_translocatorSprite.OutlineWidth = 2;
            m_swearBubble = new SpriteObj("SwearBubble1_Sprite");
            m_swearBubble.Scale = new Vector2(2f, 2f);
            m_swearBubble.Flip = SpriteEffects.FlipHorizontally;
            m_swearBubble.Visible = false;
            m_axeProjData = new ProjectileData(this) {
                SpriteName = "SpellAxe_Sprite",
                SourceAnchor = new Vector2(20f, -20f),
                Target = null,
                Speed = new Vector2(AxeProjectileSpeed, AxeProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 10f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(-90f, -90f),
                CollidesWithTerrain = false,
                Scale = AxeSpellScale,
                IgnoreInvincibleCounter = true
            };
            m_rapidDaggerProjData = new ProjectileData(this) {
                SpriteName = "SpellDagger_Sprite",
                SourceAnchor = Vector2.Zero,
                Speed = new Vector2(DaggerProjectileSpeed, DaggerProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = DaggerSpellScale,
                FollowArc = true,
                IgnoreInvincibleCounter = true
            };
        }

        private int BaseDamage { get; set; }
        public float BaseMana { get; internal set; }

        public float CurrentMana {
            get { return m_currentMana; }
            internal set {
                m_currentMana = value;
                if (m_currentMana < 0f)
                    m_currentMana = 0f;
                if (m_currentMana > MaxMana)
                    m_currentMana = MaxMana;
            }
        }

        public int BaseHealth { get; internal set; }
        public float ProjectileLifeSpan { get; internal set; }
        public float AttackDelay { get; internal set; }
        public float BaseInvincibilityTime { get; internal set; }
        public float BaseCriticalChance { get; internal set; }
        public float BaseCriticalDamageMod { get; internal set; }
        public int MaxDamage { get; internal set; }
        public int MinDamage { get; internal set; }
        public int LevelModifier { get; internal set; }
        private float AttackAnimationDelay { get; set; }
        private int StrongDamage { get; set; }
        private Vector2 StrongEnemyKnockBack { get; set; }
        public float AirAttackKnockBack { get; internal set; }
        public bool IsAirAttacking { get; set; }
        private float AirAttackDamageMod { get; set; }
        public float StatDropIncrease { get; set; }
        public float FlightSpeedMod { get; internal set; }
        public int BaseWeight { get; internal set; }
        public float BaseMagicDamage { get; set; }
        public float FlightTime { get; internal set; }
        private float BlockInvincibleTime { get; set; }
        public bool ForceInvincible { get; set; }
        public bool InvincibleToSpikes { get; set; }
        public int NumAirBounces { get; set; }

        private Rectangle GroundCollisionRect {
            get { return new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + StepUp); }
        }

        private Rectangle RotatedGroundCollisionRect {
            get { return new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + 40); }
        }

        public PlayerIndex PlayerIndex {
            get { return m_playerIndex; }
        }

        public ProceduralLevelScreen AttachedLevel {
            get { return m_levelScreen; }
        }

        public float TotalAirAttackDamageMod {
            get {
                float num = SkillSystem.GetSkill(SkillType.Down_Strike_Up).ModifierAmount * NumAirBounces;
                float num2 = AirAttackDamageMod + num;
                if (num2 > 1f)
                    num2 = 1f;
                return num2;
            }
        }

        public int TotalMagicDamage {
            get {
                int num = (int)((BaseMagicDamage + SkillSystem.GetSkill(SkillType.Magic_Damage_Up).ModifierAmount + GetEquipmentMagicDamage() + Game.PlayerStats.BonusMagic) * ClassMagicDamageGivenMultiplier);
                if (num < 1)
                    num = 1;
                return num;
            }
        }

        public int InvulnDamage {
            get { return (int)(RandomDamage * (1f + SkillSystem.GetSkill(SkillType.Invuln_Attack_Up).ModifierAmount) + SkillSystem.GetSkill(SkillType.Attack_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Damage_Up_Final).ModifierAmount); }
        }

        public Vector2 EnemyKnockBack {
            get {
                if (m_currentLogicSet == m_standingAttack3LogicSet)
                    return StrongEnemyKnockBack;
                return m_enemyKnockBack;
            }
            set { m_enemyKnockBack = value; }
        }

        public int RandomDamage {
            get { return CDGMath.RandomInt(MinDamage + GetEquipmentDamage(), MaxDamage + GetEquipmentDamage()) + Game.PlayerStats.BonusStrength + DamageGainPerLevel * Game.PlayerStats.CurrentLevel; }
        }

        public float MaxMana {
            get {
                if (Game.PlayerStats.Traits.X == 12f || Game.PlayerStats.Traits.Y == 12f) {
                    int num = (int)Math.Round((((BaseHealth + GetEquipmentHealth() + HealthGainPerLevel * Game.PlayerStats.CurrentLevel + Game.PlayerStats.BonusHealth * 5) + SkillSystem.GetSkill(SkillType.Health_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Health_Up_Final).ModifierAmount) * ClassTotalHPMultiplier * Game.PlayerStats.LichHealthMod), MidpointRounding.AwayFromZero) + Game.PlayerStats.LichHealth;
                    if (num < 1)
                        num = 1;
                    return num;
                }
                int num2 = (int)((BaseMana + GetEquipmentMana() + (ManaGainPerLevel * Game.PlayerStats.CurrentLevel) + (Game.PlayerStats.BonusMana * 5) + SkillSystem.GetSkill(SkillType.Mana_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Mana_Up_Final).ModifierAmount) * ClassTotalMPMultiplier) + Game.PlayerStats.LichMana;
                if (num2 < 1)
                    num2 = 1;
                return num2;
            }
        }

        public override int MaxHealth {
            get {
                if (Game.PlayerStats.Traits.X == 12f || Game.PlayerStats.Traits.Y == 12f) {
                    int num = (int)((BaseMana + GetEquipmentMana() + (ManaGainPerLevel * Game.PlayerStats.CurrentLevel) + (Game.PlayerStats.BonusMana * 5) + SkillSystem.GetSkill(SkillType.Mana_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Mana_Up_Final).ModifierAmount) * ClassTotalMPMultiplier) + Game.PlayerStats.LichMana;
                    if (num < 1)
                        num = 1;
                    return num;
                }
                int num2 = (int)Math.Round((((BaseHealth + GetEquipmentHealth() + HealthGainPerLevel * Game.PlayerStats.CurrentLevel + Game.PlayerStats.BonusHealth * 5) + SkillSystem.GetSkill(SkillType.Health_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Health_Up_Final).ModifierAmount) * ClassTotalHPMultiplier * Game.PlayerStats.LichHealthMod), MidpointRounding.AwayFromZero) + Game.PlayerStats.LichHealth;
                if (num2 < 1)
                    num2 = 1;
                return num2;
            }
        }

        public float InvincibilityTime {
            get { return BaseInvincibilityTime + SkillSystem.GetSkill(SkillType.Invuln_Time_Up).ModifierAmount; }
        }

        public override Rectangle Bounds {
            get { return this.TerrainBounds; }
        }

        public int CurrentWeight {
            get { return GetEquipmentWeight(); }
        }

        public int MaxWeight {
            get { return (int)(BaseWeight + SkillSystem.GetSkill(SkillType.Equip_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Equip_Up_Final).ModifierAmount) + Game.PlayerStats.BonusWeight * 5; }
        }

        public bool CanFly {
            get { return Game.PlayerStats.Class == 16 || TotalFlightTime > 0f; }
        }

        public bool CanAirDash {
            get { return TotalAirDashes > 0; }
        }

        public bool CanBlock {
            get { return Game.PlayerStats.Class == 8; }
        }

        public bool CanRun {
            get { return true; }
        }

        public bool CanAirAttackDownward {
            get { return true; }
        }

        public bool IsJumping {
            get { return m_isJumping; }
        }

        public byte NumSequentialAttacks {
            get { return m_numSequentialAttacks; }
            set { m_numSequentialAttacks = value; }
        }

        public byte AttacksNeededForMana {
            get { return m_attacksNeededForMana; }
            set { m_attacksNeededForMana = value; }
        }

        public float ManaGain {
            get {
                int num = 0;
                if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
                    num = 6;
                return ((int)((m_manaGain + num + SkillSystem.GetSkill(SkillType.Mana_Regen_Up).ModifierAmount + ((Game.PlayerStats.GetNumberOfEquippedRunes(4) + (int)GetEquipmentSecondaryAttrib(8)) * 2) + Game.PlayerStats.GetNumberOfEquippedRunes(10)) * (1f + Game.PlayerStats.TimesCastleBeaten * 0.5f)));
            }
            set { m_manaGain = value; }
        }

        public float BlockManaDrain {
            get { return m_blockManaDrain - ((int)GetEquipmentSecondaryAttrib(8)) - ((int)SkillSystem.GetSkill(SkillType.Block).ModifierAmount); }
            set { m_blockManaDrain = value; }
        }

        public float TotalStatDropChance {
            get { return StatDropIncrease + BaseStatDropChance; }
        }

        public float TotalCritChance {
            get {
                float num = BaseCriticalChance + SkillSystem.GetSkill(SkillType.Crit_Chance_Up).ModifierAmount + GetEquipmentSecondaryAttrib(1);
                byte @class = Game.PlayerStats.Class;
                switch (@class) {
                    case 3:
                        break;
                    case 4:
                        goto IL_51;
                    default:
                        switch (@class) {
                            case 11:
                                break;
                            case 12:
                                goto IL_51;
                            default:
                                return num;
                        }
                        break;
                }
                return num + 0.15f;
                IL_51:
                return 0f;
            }
        }

        public float TotalCriticalDamage {
            get {
                float num = BaseCriticalDamageMod + SkillSystem.GetSkill(SkillType.Crit_Damage_Up).ModifierAmount + GetEquipmentSecondaryAttrib(2);
                byte @class = Game.PlayerStats.Class;
                if (@class == 3 || @class == 11)
                    return num + 1.25f;
                return num;
            }
        }

        public float TotalXPBonus {
            get { return SkillSystem.GetSkill(SkillType.XP_Gain_Up).ModifierAmount + GetEquipmentSecondaryAttrib(5); }
        }

        public float TotalGoldBonus {
            get {
                float num = SkillSystem.GetSkill(SkillType.Gold_Gain_Up).ModifierAmount + GetEquipmentSecondaryAttrib(3) + Game.PlayerStats.GetNumberOfEquippedRunes(6) * 0.1f + 0.5f * Game.PlayerStats.TimesCastleBeaten;
                byte @class = Game.PlayerStats.Class;
                if (@class == 5 || @class == 13)
                    return num + 0.3f;
                return num;
            }
        }

        public int TotalVampBonus {
            get { return (int)(((Game.PlayerStats.GetNumberOfEquippedRunes(2) * 2) + (int)GetEquipmentSecondaryAttrib(7) * 2 + Game.PlayerStats.GetNumberOfEquippedRunes(10)) * (1f + Game.PlayerStats.TimesCastleBeaten * 0.5f)); }
        }

        public int TotalAirDashes {
            get { return Game.PlayerStats.GetNumberOfEquippedRunes(1) + (int)GetEquipmentSecondaryAttrib(11); }
        }

        public int TotalDoubleJumps {
            get { return Game.PlayerStats.GetNumberOfEquippedRunes(0) + (int)GetEquipmentSecondaryAttrib(9); }
        }

        public float TotalFlightTime {
            get { return FlightTime * (Game.PlayerStats.GetNumberOfEquippedRunes(3) + (int)GetEquipmentSecondaryAttrib(15)); }
        }

        public float TotalArmor {
            get { return SkillSystem.GetSkill(SkillType.Armor_Up).ModifierAmount + (Game.PlayerStats.BonusDefense * 2) + GetEquipmentArmor(); }
        }

        public float TotalDamageReduc {
            get { return TotalArmor / (ArmorReductionMod + TotalArmor); }
        }

        public float TotalMovementSpeed {
            get {
                float num = 0f;
                if (base.State == 7 || base.State == 9)
                    num = FlightSpeedMod;
                return base.Speed * (TotalMovementSpeedPercent + num);
            }
        }

        public float TotalMovementSpeedPercent {
            get {
                float num = 0f;
                if (Game.PlayerStats.Traits.X == 14f || Game.PlayerStats.Traits.Y == 14f)
                    num = 0.3f;
                return 1f + GetEquipmentSecondaryAttrib(10) + num + Game.PlayerStats.GetNumberOfEquippedRunes(7) * 0.2f + ClassMoveSpeedMultiplier;
            }
        }

        public float TotalDamageReturn {
            get { return GetEquipmentSecondaryAttrib(4) + Game.PlayerStats.GetNumberOfEquippedRunes(5) * 0.5f; }
        }

        public bool ControlsLocked {
            get { return m_lockControls; }
        }

        public bool IsFlying {
            get { return m_isFlying; }
        }

        public Game Game {
            get { return m_game; }
        }

        public bool IsInvincible {
            get { return m_invincibleCounter > 0; }
        }

        public float ClassDamageGivenMultiplier {
            get {
                switch (Game.PlayerStats.Class) {
                    case 1:
                    case 9:
                        return 0.5f;
                    case 2:
                    case 10:
                        return 0.75f;
                    case 3:
                    case 11:
                        return 0.75f;
                    case 4:
                    case 12:
                        return 1.75f;
                    case 5:
                    case 13:
                        return 0.75f;
                    case 6:
                    case 14:
                        return 0.75f;
                    case 7:
                    case 15:
                        return 0.75f;
                }
                return 1f;
            }
        }

        public float ClassDamageTakenMultiplier {
            get {
                byte @class = Game.PlayerStats.Class;
                if (@class == 3 || @class == 11)
                    return 1f;
                return 1f;
            }
        }

        public float ClassMagicDamageGivenMultiplier {
            get {
                byte @class = Game.PlayerStats.Class;
                if (@class != 1) {
                    switch (@class) {
                        case 7:
                            break;
                        case 8:
                            goto IL_36;
                        case 9:
                            goto IL_2A;
                        default:
                            if (@class != 15)
                                goto IL_36;
                            break;
                    }
                    return 1.5f;
                    IL_36:
                    return 1f;
                }
                IL_2A:
                return 1.25f;
            }
        }

        public float ClassTotalHPMultiplier {
            get {
                switch (Game.PlayerStats.Class) {
                    case 1:
                    case 9:
                        return 0.5f;
                    case 2:
                    case 10:
                        return 1.5f;
                    case 3:
                    case 11:
                        return 0.75f;
                    case 4:
                    case 12:
                        return 0.6f;
                    case 5:
                    case 13:
                        return 0.5f;
                    case 6:
                    case 14:
                        return 0.75f;
                    case 7:
                    case 15:
                        return 0.35f;
                    case 16:
                        return 0.4f;
                    case 17:
                        return 0.7f;
                }
                return 1f;
            }
        }

        public float ClassTotalMPMultiplier {
            get {
                switch (Game.PlayerStats.Class) {
                    case 1:
                    case 9:
                        return 1.5f;
                    case 2:
                    case 10:
                        return 0.5f;
                    case 3:
                    case 11:
                        return 0.65f;
                    case 4:
                    case 12:
                        return 0.4f;
                    case 5:
                    case 13:
                        return 0.5f;
                    case 6:
                    case 14:
                        return 0.4f;
                    case 7:
                    case 15:
                        return 0.5f;
                    case 16:
                        return 0.25f;
                    case 17:
                        return 0.7f;
                }
                return 1f;
            }
        }

        public float ClassMoveSpeedMultiplier {
            get {
                byte @class = Game.PlayerStats.Class;
                if (@class == 4 || @class == 12)
                    return 0.3f;
                if (@class != 16)
                    return 0f;
                return 0f;
            }
        }

        public float SpellCastDelay {
            get { return m_spellCastDelay; }
        }

        public bool LightOn {
            get { return m_lightOn; }
        }

        public override SpriteEffects Flip {
            get { return base.Flip; }
            set {
                if ((Game.PlayerStats.Traits.X == 18f || Game.PlayerStats.Traits.Y == 18f) && Flip != value) {
                    if (m_flipTween != null && m_flipTween.TweenedObject == this && m_flipTween.Active)
                        m_flipTween.StopTween(false);
                    float x = m_internalScale.X;
                    this.ScaleX = 0f;
                    m_flipTween = Tween.To(this, 0.15f, new Easing(Tween.EaseNone), new[] {
                        "ScaleX",
                        x.ToString()
                    });
                }
                base.Flip = value;
            }
        }

        public bool CastingDamageShield {
            get { return m_damageShieldCast; }
        }

        #region IDealsDamageObj Members

        public int Damage {
            get {
                int num = (int)((RandomDamage + SkillSystem.GetSkill(SkillType.Attack_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Damage_Up_Final).ModifierAmount) * ClassDamageGivenMultiplier);
                if (IsAirAttacking)
                    num = (int)(num * TotalAirAttackDamageMod);
                if (num < 1)
                    num = 1;
                return num;
            }
        }

        #endregion

        protected override void InitializeEV() {
            base.ForceDraw = true;
            base.Speed = 500f;
            RunSpeedMultiplier = 3f;
            base.JumpHeight = 1180f;
            base.DoubleJumpHeight = 845f;
            StepUp = 10;
            BaseHealth = 100;
            JumpDeceleration = 5000f;
            base.KnockBack = new Vector2(300f, 450f);
            BaseInvincibilityTime = 1f;
            BaseWeight = 50;
            BaseMagicDamage = 25f;
            DashSpeed = 900f;
            DashTime = 0.325f;
            DashCoolDown = 0.25f;
            ProjectileLifeSpan = 10f;
            base.AnimationDelay = 0.1f;
            AttackDelay = 0f;
            BaseCriticalChance = 0f;
            BaseCriticalDamageMod = 1.5f;
            EnemyKnockBack = new Vector2(90f, 90f);
            MinDamage = 25;
            MaxDamage = 25;
            ComboDelay = 1.5f;
            AttackAnimationDelay = 1f / (20f + SkillSystem.GetSkill(SkillType.Attack_Speed_Up).ModifierAmount);
            StrongDamage = 25;
            StrongEnemyKnockBack = new Vector2(300f, 360f);
            AirAttackKnockBack = 1425f;
            AirAttackDamageMod = 0.5f;
            LevelModifier = 999999999;
            DamageGainPerLevel = 0;
            ManaGainPerLevel = 0;
            HealthGainPerLevel = 0;
            BlockManaDrain = 25f;
            BaseMana = 100f;
            AttacksNeededForMana = 1;
            ManaGain = 0f;
            BaseStatDropChance = 0.01f;
            ArmorReductionMod = 200;
            BlockInvincibleTime = 1f;
            FlightTime = 0.6f;
            FlightSpeedMod = 0.15f;
            if (LevelEV.ENABLE_PLAYER_DEBUG)
                base.Speed = 1500f;
        }

        protected override void InitializeLogic() {
            if (m_standingAttack3LogicSet != null)
                m_standingAttack3LogicSet.Dispose();
            m_standingAttack3LogicSet = new LogicSet(this);
            m_standingAttack3LogicSet.AddAction(new ChangeSpriteLogicAction("PlayerAttacking3_Character", false, false), Types.Sequence.Serial);
            m_standingAttack3LogicSet.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", AttackAnimationDelay), Types.Sequence.Serial);
            m_standingAttack3LogicSet.AddAction(new PlayAnimationLogicAction(2, 4, false), Types.Sequence.Serial);
            m_standingAttack3LogicSet.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            m_standingAttack3LogicSet.AddAction(new RunFunctionLogicAction(this, "PlayAttackSound", new object[0]), Types.Sequence.Serial);
            m_standingAttack3LogicSet.AddAction(new PlayAnimationLogicAction("AttackStart", "End", false), Types.Sequence.Serial);
            m_standingAttack3LogicSet.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", base.AnimationDelay), Types.Sequence.Serial);
            if (m_airAttackLS != null)
                m_airAttackLS.Dispose();
            m_airAttackLS = new LogicSet(this);
            m_airAttackLS.AddAction(new ChangePropertyLogicAction(this, "IsAirAttacking", true), Types.Sequence.Serial);
            m_airAttackLS.AddAction(new ChangeSpriteLogicAction("PlayerAirAttack_Character", false, false), Types.Sequence.Serial);
            m_airAttackLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", AttackAnimationDelay), Types.Sequence.Serial);
            m_airAttackLS.AddAction(new RunFunctionLogicAction(this, "PlayAttackSound", new object[0]), Types.Sequence.Serial);
            m_airAttackLS.AddAction(new PlayAnimationLogicAction("Start", "Start", false), Types.Sequence.Serial);
            m_airAttackLS.AddAction(new PlayAnimationLogicAction("Frame 3 Test", "Frame 3 Test", false), Types.Sequence.Serial);
            m_airAttackLS.AddAction(new PlayAnimationLogicAction("Attack", "Attack", false), Types.Sequence.Serial);
            m_airAttackLS.AddAction(new DelayLogicAction(AttackDelay, false), Types.Sequence.Serial);
            m_airAttackLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
            m_airAttackLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", base.AnimationDelay), Types.Sequence.Serial);
            m_airAttackLS.AddAction(new ChangePropertyLogicAction(this, "IsAirAttacking", false), Types.Sequence.Serial);
        }

        public void Initialize() {
            InitializeEV();
            m_startingAnimationDelay = base.AnimationDelay;
            InitializeLogic();
            base.CurrentHealth = MaxHealth;
            CurrentMana = MaxMana;
            this.Scale = new Vector2(2f, 2f);
            m_internalScale = this.Scale;
            m_wizardSpellList = new List<byte>();
            m_wizardSpellList.Add((byte)Game.PlayerStats.WizardSpellList.X);
            m_wizardSpellList.Add((byte)Game.PlayerStats.WizardSpellList.Y);
            m_wizardSpellList.Add((byte)Game.PlayerStats.WizardSpellList.Z);
        }

        public void UpdateInternalScale() {
            m_internalScale = this.Scale;
        }

        private void InitializeInputMap() {
            m_debugInputMap.AddInput(0, Keys.D7);
            m_debugInputMap.AddInput(2, Keys.D8);
            m_debugInputMap.AddInput(1, Keys.D9);
            m_debugInputMap.AddInput(3, Keys.D1);
            m_debugInputMap.AddInput(4, Keys.D2);
            m_debugInputMap.AddInput(5, Keys.D3);
            m_debugInputMap.AddInput(6, Keys.D4);
            m_debugInputMap.AddInput(7, Keys.X);
            m_debugInputMap.AddInput(8, Keys.H);
            m_debugInputMap.AddInput(9, Keys.J);
        }

        public override void ChangeSprite(string spriteName) {
            base.ChangeSprite(spriteName);
            if (base.State != 8) {
                string text = (this._objectList[12] as IAnimateableObj).SpriteName;
                int startIndex = text.IndexOf("_") - 1;
                text = text.Remove(startIndex, 1);
                if (Game.PlayerStats.Class == 16)
                    text = text.Replace("_", 6 + "_");
                else if (Game.PlayerStats.Class == 17)
                    text = text.Replace("_", 7 + "_");
                else
                    text = text.Replace("_", Game.PlayerStats.HeadPiece + "_");
                this._objectList[12].ChangeSprite(text);
                string text2 = (this._objectList[4] as IAnimateableObj).SpriteName;
                startIndex = text2.IndexOf("_") - 1;
                text2 = text2.Remove(startIndex, 1);
                text2 = text2.Replace("_", Game.PlayerStats.ChestPiece + "_");
                this._objectList[4].ChangeSprite(text2);
                string text3 = (this._objectList[9] as IAnimateableObj).SpriteName;
                startIndex = text3.IndexOf("_") - 1;
                text3 = text3.Remove(startIndex, 1);
                text3 = text3.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
                this._objectList[9].ChangeSprite(text3);
                string text4 = (this._objectList[3] as IAnimateableObj).SpriteName;
                startIndex = text4.IndexOf("_") - 1;
                text4 = text4.Remove(startIndex, 1);
                text4 = text4.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
                this._objectList[3].ChangeSprite(text4);
                if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14) {
                    if (base.State == 1 && (m_currentLogicSet != m_standingAttack3LogicSet || (m_currentLogicSet == m_standingAttack3LogicSet && !m_currentLogicSet.IsActive)))
                        this._objectList[6].ChangeSprite("PlayerWalkingArmsSpellSword_Sprite");
                    else if (base.State == 2 && (m_currentLogicSet != m_standingAttack3LogicSet || (m_currentLogicSet == m_standingAttack3LogicSet && !m_currentLogicSet.IsActive)) && !IsAirAttacking && !base.IsKilled) {
                        if (base.AccelerationY < 0f)
                            this._objectList[6].ChangeSprite("PlayerFallingArmsSpellSword_Sprite");
                        else
                            this._objectList[6].ChangeSprite("PlayerJumpingArmsSpellSword_Sprite");
                    }
                    this._objectList[10].Opacity = 0f;
                    this._objectList[11].Opacity = 0f;
                }
                else {
                    this._objectList[10].Opacity = 1f;
                    this._objectList[11].Opacity = 1f;
                }
                this._objectList[16].Opacity = 0.3f;
                this._objectList[16].Visible = false;
                if (Game.PlayerStats.Class == 13 && spriteName != "PlayerDeath_Character" && m_lightOn)
                    this._objectList[16].Visible = true;
                if (Game.PlayerStats.Class == 0 || Game.PlayerStats.Class == 8) {
                    string spriteName2 = spriteName.Replace("_Character", "Shield_Sprite");
                    this._objectList[15].Visible = true;
                    this._objectList[15].ChangeSprite(spriteName2);
                }
                else if (Game.PlayerStats.Class == 5 || Game.PlayerStats.Class == 13) {
                    string spriteName3 = spriteName.Replace("_Character", "Lamp_Sprite");
                    this._objectList[15].Visible = true;
                    this._objectList[15].ChangeSprite(spriteName3);
                }
                else if (Game.PlayerStats.Class == 4 || Game.PlayerStats.Class == 12) {
                    string spriteName4 = spriteName.Replace("_Character", "Headband_Sprite");
                    this._objectList[15].Visible = true;
                    this._objectList[15].ChangeSprite(spriteName4);
                }
                else if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9) {
                    string spriteName5 = spriteName.Replace("_Character", "Beard_Sprite");
                    this._objectList[15].Visible = true;
                    this._objectList[15].ChangeSprite(spriteName5);
                }
                else if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10) {
                    string spriteName6 = spriteName.Replace("_Character", "Horns_Sprite");
                    this._objectList[15].Visible = true;
                    this._objectList[15].ChangeSprite(spriteName6);
                }
                else
                    this._objectList[15].Visible = false;
                this._objectList[14].Visible = false;
                if (Game.PlayerStats.SpecialItem == 8)
                    this._objectList[14].Visible = true;
                this._objectList[7].Visible = true;
                if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
                    this._objectList[7].Visible = false;
                if (!Game.PlayerStats.IsFemale) {
                    this._objectList[5].Visible = false;
                    this._objectList[13].Visible = false;
                }
                else {
                    this._objectList[5].Visible = true;
                    this._objectList[13].Visible = true;
                }
                this._objectList[0].Visible = false;
                this._objectList[0].Opacity = 1f;
                if (Game.PlayerStats.Class == 16)
                    this._objectList[0].Visible = true;
                if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14) {
                    this.OutlineColour = Color.White;
                    return;
                }
                this.OutlineColour = Color.Black;
            }
        }

        public void LockControls() {
            m_lockControls = true;
        }

        public void UnlockControls() {
            m_lockControls = false;
        }

        public override void HandleInput() {
            if (!LevelEV.RUN_DEMO_VERSION && !LevelEV.CREATE_RETAIL_VERSION)
                DebugInputControls();
            if (!m_lockControls && !base.IsKilled && base.State != 3)
                InputControls();
            if (Game.PlayerStats.Class == 17 && !(AttachedLevel.CurrentRoom is CarnivalShoot1BonusRoom) && !(AttachedLevel.CurrentRoom is CarnivalShoot2BonusRoom) && !base.IsKilled && base.State == 4 && Game.GlobalInput.JustPressed(24) && m_spellCastDelay <= 0f && CurrentMana >= 30f) {
                m_spellCastDelay = 0.5f;
                CurrentMana -= 30f;
                CastCloseShield();
            }
        }

        private void DebugInputControls() {
            if (m_debugInputMap.JustPressed(0)) {
                PlayerStats expr_13 = Game.PlayerStats;
                expr_13.Spell += 1;
                if (Game.PlayerStats.Spell > 16)
                    Game.PlayerStats.Spell = 1;
                m_levelScreen.UpdatePlayerSpellIcon();
            }
            if (m_debugInputMap.JustPressed(2))
                base.CurrentHealth = MaxHealth;
            if (m_debugInputMap.JustPressed(1))
                CurrentMana = MaxMana;
            if (m_debugInputMap.JustPressed(3))
                Game.PlayerStats.Gold += 1000;
            if (m_debugInputMap.JustPressed(4))
                Game.PlayerStats.Gold += 10000;
            if (m_debugInputMap.JustPressed(6))
                Game.PlayerStats.Gold += 100000;
            m_debugInputMap.JustPressed(5);
            if (m_debugInputMap.JustPressed(7)) {
                RCScreenManager rCScreenManager = m_levelScreen.ScreenManager as RCScreenManager;
                if (rCScreenManager != null)
                    Kill(true);
            }
            if (m_debugInputMap.JustPressed(8))
                Game.EquipmentSystem.SetBlueprintState(1);
            if (m_debugInputMap.JustPressed(9))
                Game.EquipmentSystem.SetBlueprintState(3);
        }

        private void InputControls() {
            if (!LevelEV.CREATE_RETAIL_VERSION && InputManager.JustPressed(Keys.T, null)) {
                SoundManager.PlaySound(new[] {
                    "Fart1",
                    "Fart2",
                    "Fart3"
                });
                m_levelScreen.ImpactEffectPool.DisplayFartEffect(this);
            }
            if (Game.GlobalInput.JustPressed(9) && Game.PlayerStats.TutorialComplete && m_levelScreen.CurrentRoom.Name != "Start" && m_levelScreen.CurrentRoom.Name != "Boss" && m_levelScreen.CurrentRoom.Name != "ChallengeBoss")
                m_levelScreen.DisplayMap(false);
            if (base.State != 8) {
                if (Game.GlobalInput.Pressed(13) && CanBlock && !m_currentLogicSet.IsActive) {
                    if (CurrentMana >= 25f) {
                        if (m_isTouchingGround)
                            base.CurrentSpeed = 0f;
                        if (base.State == 7) {
                            base.CurrentSpeed = 0f;
                            base.AccelerationX = 0f;
                            base.AccelerationY = 0f;
                        }
                        base.State = 6;
                        if (Game.GlobalInput.JustPressed(13))
                            SoundManager.PlaySound("Player_Block_Action");
                    }
                    else if (Game.GlobalInput.JustPressed(13))
                        SoundManager.PlaySound("Error_Spell");
                }
                else if (!m_isTouchingGround) {
                    if (IsFlying) {
                        if (base.State == 9)
                            base.State = 9;
                        else
                            base.State = 7;
                    }
                    else
                        base.State = 2;
                }
                else
                    base.State = 0;
            }
            if (base.State != 6 && base.State != 8) {
                if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21) || Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23)) {
                    if (m_isTouchingGround)
                        base.State = 1;
                    if ((Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23)) && (!m_collidingRight || m_isTouchingGround)) {
                        base.HeadingX = 1f;
                        base.CurrentSpeed = TotalMovementSpeed;
                    }
                    else if ((Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21)) && !Game.GlobalInput.Pressed(22) && !Game.GlobalInput.Pressed(23) && (!m_collidingLeft || m_isTouchingGround)) {
                        base.HeadingX = -1f;
                        base.CurrentSpeed = TotalMovementSpeed;
                    }
                    else
                        base.CurrentSpeed = 0f;
                    if (!LevelEV.RUN_DEMO_VERSION && !LevelEV.CREATE_RETAIL_VERSION && (InputManager.Pressed(Keys.LeftShift, PlayerIndex.One) || InputManager.Pressed(Buttons.LeftShoulder, PlayerIndex.One)) && CanRun && m_isTouchingGround)
                        base.CurrentSpeed *= RunSpeedMultiplier;
                    if (!m_currentLogicSet.IsActive || (m_currentLogicSet.IsActive && (Game.PlayerStats.Traits.X == 27f || Game.PlayerStats.Traits.Y == 27f))) {
                        if (Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23))
                            Flip = SpriteEffects.None;
                        else if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
                            Flip = SpriteEffects.FlipHorizontally;
                    }
                    if (m_isTouchingGround && m_currentLogicSet == m_standingAttack3LogicSet && m_currentLogicSet.IsActive && m_playerLegs.SpriteName != "PlayerWalkingLegs_Sprite") {
                        m_playerLegs.ChangeSprite("PlayerWalkingLegs_Sprite");
                        m_playerLegs.PlayAnimation(base.CurrentFrame, base.TotalFrames, false);
                        m_playerLegs.Y += 4f;
                        m_playerLegs.OverrideParentAnimationDelay = true;
                        m_playerLegs.AnimationDelay = 0.1f;
                    }
                }
                else {
                    if (m_isTouchingGround)
                        base.State = 0;
                    base.CurrentSpeed = 0f;
                }
            }
            bool flag = false;
            if (base.State != 6 && base.State != 7 && base.State != 8 && Game.PlayerStats.Class != 16) {
                if ((Game.GlobalInput.JustPressed(10) || Game.GlobalInput.JustPressed(11)) && m_isTouchingGround && m_dropThroughGroundTimer <= 0f) {
                    base.State = 2;
                    base.AccelerationY = -base.JumpHeight;
                    m_isJumping = true;
                    if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f) {
                        SoundManager.PlaySound("Player_Jump_04_Low");
                        SoundManager.PlaySound("Player_WalkUp01_Low");
                    }
                    if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f) {
                        SoundManager.PlaySound("Player_Jump_04_High");
                        SoundManager.PlaySound("Player_WalkUp01_High");
                    }
                    else {
                        SoundManager.PlaySound("Player_Jump_04");
                        SoundManager.PlaySound("Player_WalkUp01");
                    }
                    if ((Game.PlayerStats.Traits.X == 19f || Game.PlayerStats.Traits.Y == 19f) && CDGMath.RandomInt(0, 100) >= 91) {
                        SoundManager.PlaySound(new[] {
                            "Fart1",
                            "Fart2",
                            "Fart3"
                        });
                        m_levelScreen.ImpactEffectPool.DisplayDustEffect(this);
                    }
                    flag = true;
                }
                else if ((Game.GlobalInput.JustPressed(10) || Game.GlobalInput.JustPressed(11)) && !m_isTouchingGround && m_doubleJumpCount < TotalDoubleJumps && m_dropThroughGroundTimer <= 0f) {
                    base.State = 2;
                    base.AccelerationY = -base.DoubleJumpHeight;
                    m_levelScreen.ImpactEffectPool.DisplayDoubleJumpEffect(new Vector2(base.X, (Bounds.Bottom + 10)));
                    m_isJumping = true;
                    m_doubleJumpCount += 1;
                    SoundManager.PlaySound("Player_DoubleJump");
                    if ((Game.PlayerStats.Traits.X == 19f || Game.PlayerStats.Traits.Y == 19f) && CDGMath.RandomInt(0, 100) >= 91) {
                        SoundManager.PlaySound(new[] {
                            "Fart1",
                            "Fart2",
                            "Fart3"
                        });
                        m_levelScreen.ImpactEffectPool.DisplayDustEffect(this);
                    }
                    flag = true;
                }
                if (!m_isTouchingGround) {
                    if (m_currentLogicSet == m_standingAttack3LogicSet && m_currentLogicSet.IsActive) {
                        if (base.AccelerationY > 0f && m_playerLegs.SpriteName != "PlayerAttackFallingLegs_Sprite")
                            m_playerLegs.ChangeSprite("PlayerAttackFallingLegs_Sprite");
                        else if (base.AccelerationY < 0f && m_playerLegs.SpriteName != "PlayerAttackJumpingLegs_Sprite")
                            m_playerLegs.ChangeSprite("PlayerAttackJumpingLegs_Sprite");
                    }
                    if (base.State != 7)
                        base.State = 2;
                }
            }
            if (!m_currentLogicSet.IsActive && base.State != 6 && base.State != 8 && Game.PlayerStats.Class != 16) {
                if ((Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19)) && CanAirAttackDownward && Game.GameConfig.QuickDrop && base.State == 2 && m_dropThroughGroundTimer <= 0f) {
                    m_currentLogicSet = m_airAttackLS;
                    if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
                        FadeSword();
                    if (m_assassinSpecialActive)
                        DisableAssassinAbility();
                    m_currentLogicSet.Execute();
                }
                else if (Game.GlobalInput.JustPressed(12)) {
                    if (base.State == 2) {
                        if ((Game.GlobalInput.Pressed(18) || Game.GlobalInput.Pressed(19)) && CanAirAttackDownward)
                            m_currentLogicSet = m_airAttackLS;
                        else
                            m_currentLogicSet = m_standingAttack3LogicSet;
                        if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
                            FadeSword();
                        if (m_assassinSpecialActive)
                            DisableAssassinAbility();
                        m_currentLogicSet.Execute();
                    }
                    else {
                        if (!m_isTouchingGround)
                            base.CurrentSpeed = 0f;
                        if (m_attackCounter > 0f)
                            m_attackNumber++;
                        m_attackCounter = ComboDelay;
                        if (m_attackNumber == 0)
                            m_currentLogicSet = m_standingAttack3LogicSet;
                        else {
                            m_currentLogicSet = m_standingAttack3LogicSet;
                            m_attackNumber = 0;
                            m_attackCounter = 0f;
                        }
                        if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
                            FadeSword();
                        if (m_assassinSpecialActive)
                            DisableAssassinAbility();
                        m_playerLegs.OverrideParentAnimationDelay = false;
                        m_currentLogicSet.Execute();
                    }
                }
            }
            if (Game.PlayerStats.TutorialComplete) {
                bool flag2 = false;
                if (Game.PlayerStats.Spell == 15 && (Game.GlobalInput.Pressed(12) || Game.GlobalInput.Pressed(24)) && m_rapidSpellCastDelay <= 0f) {
                    m_rapidSpellCastDelay = 0.2f;
                    CastSpell(false, false);
                    flag2 = true;
                }
                if ((m_spellCastDelay <= 0f || Game.PlayerStats.Class == 16) && (Game.GlobalInput.JustPressed(24) || (Game.PlayerStats.Class == 16 && Game.GlobalInput.JustPressed(12))) && (Game.PlayerStats.Class != 16 || !flag2))
                    CastSpell(false, false);
                if (Game.GlobalInput.JustPressed(13)) {
                    RoomObj currentRoom = m_levelScreen.CurrentRoom;
                    if (!(currentRoom is CarnivalShoot1BonusRoom) && !(currentRoom is CarnivalShoot2BonusRoom) && !(currentRoom is ChestBonusRoomObj)) {
                        if (Game.PlayerStats.Class == 14 && m_spellCastDelay <= 0f)
                            CastSpell(false, true);
                        else if (Game.PlayerStats.Class == 15)
                            ConvertHPtoMP();
                        else if (Game.PlayerStats.Class == 11 && CurrentMana > 0f) {
                            if (!m_assassinSpecialActive)
                                ActivateAssassinAbility();
                            else
                                DisableAssassinAbility();
                        }
                        else if (Game.PlayerStats.Class == 9)
                            SwapSpells();
                        else if (Game.PlayerStats.Class == 12)
                            NinjaTeleport();
                        else if (Game.PlayerStats.Class == 8) {
                            if (base.State == 8)
                                DeactivateTanooki();
                            else if (Game.GlobalInput.Pressed(18) || Game.GlobalInput.Pressed(19))
                                ActivateTanooki();
                        }
                        else if (Game.PlayerStats.Class == 10)
                            CastFuhRohDah();
                        else if (Game.PlayerStats.Class == 17 && CurrentMana >= 30f && m_spellCastDelay <= 0f) {
                            CurrentMana -= 30f;
                            m_spellCastDelay = 0.5f;
                            ThrowAxeProjectiles();
                        }
                    }
                    else if (base.State == 8)
                        DeactivateTanooki();
                    if (Game.PlayerStats.Class == 16) {
                        if (base.State != 9) {
                            base.State = 9;
                            base.DisableGravity = true;
                            m_isFlying = true;
                            base.AccelerationY = 0f;
                        }
                        else {
                            base.State = 2;
                            base.DisableGravity = false;
                            m_isFlying = false;
                        }
                    }
                    else if (Game.PlayerStats.Class == 13) {
                        if (m_lightOn) {
                            SoundManager.PlaySound("HeadLampOff");
                            m_lightOn = false;
                            this._objectList[16].Visible = false;
                        }
                        else {
                            SoundManager.PlaySound("HeadLampOn");
                            m_lightOn = true;
                            this._objectList[16].Visible = true;
                        }
                    }
                }
                if (Game.PlayerStats.Class == 16 && (Game.GlobalInput.JustPressed(10) || Game.GlobalInput.JustPressed(11))) {
                    if (base.State != 9) {
                        base.State = 9;
                        base.DisableGravity = true;
                        m_isFlying = true;
                        base.AccelerationY = 0f;
                    }
                    else {
                        base.State = 2;
                        base.DisableGravity = false;
                        m_isFlying = false;
                    }
                }
            }
            if (m_dashCooldownCounter <= 0 && (m_isTouchingGround || (!m_isTouchingGround && m_airDashCount < TotalAirDashes)) && base.State != 6 && base.State != 8 && CanAirDash) {
                if (Game.GlobalInput.JustPressed(14)) {
                    m_airDashCount += 1;
                    base.State = 4;
                    base.AccelerationYEnabled = false;
                    m_dashCooldownCounter = (int)(DashCoolDown * 1000f);
                    m_dashCounter = (int)(DashTime * 1000f);
                    LockControls();
                    base.CurrentSpeed = DashSpeed;
                    base.HeadingX = -1f;
                    base.AccelerationY = 0f;
                    if (m_currentLogicSet.IsActive)
                        m_currentLogicSet.Stop();
                    base.AnimationDelay = m_startingAnimationDelay;
                    m_levelScreen.ImpactEffectPool.DisplayDashEffect(new Vector2(base.X, (float)this.TerrainBounds.Bottom), true);
                    SoundManager.PlaySound("Player_Dash");
                    if ((Game.PlayerStats.Traits.X == 19f || Game.PlayerStats.Traits.Y == 19f) && CDGMath.RandomInt(0, 100) >= 91) {
                        m_levelScreen.ImpactEffectPool.DisplayDustEffect(this);
                        SoundManager.PlaySound(new[] {
                            "Fart1",
                            "Fart2",
                            "Fart3"
                        });
                    }
                }
                else if (Game.GlobalInput.JustPressed(15)) {
                    m_airDashCount += 1;
                    base.AnimationDelay = m_startingAnimationDelay;
                    base.State = 4;
                    base.AccelerationYEnabled = false;
                    m_dashCooldownCounter = (int)(DashCoolDown * 1000f);
                    m_dashCounter = (int)(DashTime * 1000f);
                    LockControls();
                    base.CurrentSpeed = DashSpeed;
                    base.HeadingX = 1f;
                    base.AccelerationY = 0f;
                    if (m_currentLogicSet.IsActive)
                        m_currentLogicSet.Stop();
                    m_levelScreen.ImpactEffectPool.DisplayDashEffect(new Vector2(base.X, (float)this.TerrainBounds.Bottom), false);
                    SoundManager.PlaySound("Player_Dash");
                    if ((Game.PlayerStats.Traits.X == 19f || Game.PlayerStats.Traits.Y == 19f) && CDGMath.RandomInt(0, 100) >= 91) {
                        m_levelScreen.ImpactEffectPool.DisplayDustEffect(this);
                        SoundManager.PlaySound(new[] {
                            "Fart1",
                            "Fart2",
                            "Fart3"
                        });
                    }
                }
            }
            if (base.State == 7 || base.State == 9) {
                if (Game.GlobalInput.Pressed(16) || Game.GlobalInput.Pressed(17) || InputManager.Pressed(Buttons.LeftThumbstickUp, PlayerIndex.One))
                    base.AccelerationY = -TotalMovementSpeed;
                else if (Game.GlobalInput.Pressed(18) || Game.GlobalInput.Pressed(19) || InputManager.Pressed(Buttons.LeftThumbstickDown, PlayerIndex.One))
                    base.AccelerationY = TotalMovementSpeed;
                else
                    base.AccelerationY = 0f;
                if (!m_isTouchingGround && m_currentLogicSet == m_standingAttack3LogicSet && m_currentLogicSet.IsActive) {
                    if (base.AccelerationY > 0f && m_playerLegs.SpriteName != "PlayerAttackFallingLegs_Sprite")
                        m_playerLegs.ChangeSprite("PlayerAttackFallingLegs_Sprite");
                    else if (base.AccelerationY <= 0f && m_playerLegs.SpriteName != "PlayerAttackJumpingLegs_Sprite")
                        m_playerLegs.ChangeSprite("PlayerAttackJumpingLegs_Sprite");
                }
                if ((Game.GlobalInput.JustPressed(10) || Game.GlobalInput.JustPressed(11)) && base.State != 9) {
                    base.State = 2;
                    base.DisableGravity = false;
                    m_isFlying = false;
                    return;
                }
            }
            else if ((Game.GlobalInput.JustPressed(10) || Game.GlobalInput.JustPressed(11)) && !m_isTouchingGround && !flag && m_doubleJumpCount >= TotalDoubleJumps && m_dropThroughGroundTimer <= 0f && CanFly && m_flightCounter > 0f && base.State != 7 && base.State != 9 && base.State != 6 && base.State != 8) {
                base.AccelerationY = 0f;
                base.State = 7;
                base.DisableGravity = true;
                m_isFlying = true;
            }
        }

        public void PerformDelayedAirAttack() {
            if ((Game.GlobalInput.Pressed(18) || Game.GlobalInput.Pressed(19)) && CanAirAttackDownward)
                m_currentLogicSet = m_airAttackLS;
            else
                m_currentLogicSet = m_standingAttack3LogicSet;
            if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
                FadeSword();
            m_currentLogicSet.Execute();
        }

        public override void Update(GameTime gameTime) {
            float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (m_dropThroughGroundTimer > 0f)
                m_dropThroughGroundTimer -= num;
            if (m_ninjaTeleportDelay > 0f)
                m_ninjaTeleportDelay -= num;
            if (m_rapidSpellCastDelay > 0f)
                m_rapidSpellCastDelay -= num;
            if (!(m_levelScreen.CurrentRoom is EndingRoomObj) && this.ScaleX > 0.1f) {
                if ((Game.PlayerStats.Traits.Y == 22f || Game.PlayerStats.Traits.X == 22f) && base.CurrentSpeed == 0f && m_ambilevousTimer > 0f) {
                    m_ambilevousTimer -= num;
                    if (m_ambilevousTimer <= 0f) {
                        m_ambilevousTimer = 0.4f;
                        m_levelScreen.ImpactEffectPool.DisplayQuestionMark(new Vector2(base.X, Bounds.Top));
                    }
                }
                if ((Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14) && m_wizardSparkleCounter > 0f) {
                    m_wizardSparkleCounter -= num;
                    if (m_wizardSparkleCounter <= 0f) {
                        m_wizardSparkleCounter = 0.2f;
                        m_levelScreen.ImpactEffectPool.DisplayChestSparkleEffect(base.Position);
                        m_levelScreen.ImpactEffectPool.DisplayChestSparkleEffect(base.Position);
                    }
                }
                if ((Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11) && m_assassinSmokeTimer > 0f) {
                    m_assassinSmokeTimer -= num;
                    if (m_assassinSmokeTimer <= 0f) {
                        m_assassinSmokeTimer = 0.15f;
                        if (base.CurrentSpeed > 0f)
                            m_assassinSmokeTimer = 0.05f;
                        m_levelScreen.ImpactEffectPool.BlackSmokeEffect(this);
                    }
                }
            }
            if (m_swearBubbleCounter > 0f) {
                m_swearBubbleCounter -= num;
                if (m_swearBubbleCounter <= 0f)
                    m_swearBubble.Visible = false;
            }
            if (m_blockInvincibleCounter > 0f)
                m_blockInvincibleCounter -= num;
            if (IsFlying) {
                if (base.State != 9)
                    m_flightCounter -= num;
                if (m_flightCounter <= 0f && base.State != 9) {
                    base.State = 2;
                    base.DisableGravity = false;
                    m_isFlying = false;
                }
            }
            if (base.AccelerationX < 0f)
                base.AccelerationX += 200f * num;
            else if (base.AccelerationX > 0f)
                base.AccelerationX -= 200f * num;
            if (base.AccelerationX < 3.6f && base.AccelerationX > -3.6f)
                base.AccelerationX = 0f;
            base.X += base.Heading.X * (base.CurrentSpeed * num);
            if (base.State == 1) {
                if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f) {
                    m_walkDownSoundLow.Update();
                    m_walkUpSoundLow.Update();
                }
                else if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f) {
                    m_walkDownSoundHigh.Update();
                    m_walkUpSoundHigh.Update();
                }
                else {
                    m_walkDownSound.Update();
                    m_walkUpSound.Update();
                }
            }
            if (!m_externalLS.IsActive) {
                if (m_attackCounter > 0f)
                    m_attackCounter -= num;
                else
                    m_attackNumber = 0;
                if (m_currentLogicSet.IsActive)
                    m_currentLogicSet.Update(gameTime);
                if (m_dashCooldownCounter > 0)
                    m_dashCooldownCounter -= gameTime.ElapsedGameTime.Milliseconds;
                if (m_dashCounter > 0) {
                    m_dashCounter -= gameTime.ElapsedGameTime.Milliseconds;
                    if (m_dashCounter <= 0 && base.State != 3) {
                        UnlockControls();
                        base.AccelerationYEnabled = true;
                    }
                }
                if (m_invincibleCounter > 0) {
                    m_invincibleCounter -= gameTime.ElapsedGameTime.Milliseconds;
                    if (!m_assassinSpecialActive && base.Opacity != 0.6f)
                        base.Opacity = 0.6f;
                }
                else if (!m_assassinSpecialActive && base.Opacity == 0.6f)
                    base.Opacity = 1f;
                if (!base.IsPaused && (m_currentLogicSet == null || !m_currentLogicSet.IsActive))
                    UpdateAnimationState();
                CheckGroundCollision();
                if (base.State != 3 && ((!Game.GlobalInput.Pressed(10) && !Game.GlobalInput.Pressed(11)) || (m_currentLogicSet == m_airAttackLS && m_currentLogicSet.IsActive && !IsAirAttacking)) && !m_isTouchingGround && base.AccelerationY < 0f)
                    base.AccelerationY += JumpDeceleration * num;
                if (Game.PlayerStats.Class == 16 && CurrentMana < MaxMana) {
                    m_dragonManaRechargeCounter += num;
                    if (m_dragonManaRechargeCounter >= 0.33f) {
                        m_dragonManaRechargeCounter = 0f;
                        CurrentMana += 4f;
                    }
                }
                if (m_assassinSpecialActive) {
                    m_assassinDrainCounter += num;
                    if (m_assassinDrainCounter >= 0.33f) {
                        m_assassinDrainCounter = 0f;
                        CurrentMana -= 7f;
                        if (CurrentMana <= 0f)
                            DisableAssassinAbility();
                    }
                }
                if (m_timeStopCast) {
                    m_timeStopDrainCounter += num;
                    if (m_timeStopDrainCounter >= 0.33f) {
                        m_timeStopDrainCounter = 0f;
                        CurrentMana -= 8f;
                        if (CurrentMana <= 0f) {
                            AttachedLevel.StopTimeStop();
                            m_timeStopCast = false;
                        }
                    }
                }
                if (m_damageShieldCast) {
                    m_damageShieldDrainCounter += num;
                    if (m_damageShieldDrainCounter >= 0.33f) {
                        m_damageShieldDrainCounter = 0f;
                        if (m_megaDamageShieldCast)
                            CurrentMana -= 12f;
                        else
                            CurrentMana -= 6f;
                        if (CurrentMana <= 0f) {
                            m_damageShieldCast = false;
                            m_megaDamageShieldCast = false;
                        }
                    }
                }
                if (m_lightOn) {
                    m_lightDrainCounter += num;
                    if (m_lightDrainCounter >= 1f) {
                        m_lightDrainCounter = 0f;
                        CurrentMana -= 0f;
                        if (CurrentMana <= 0f) {
                            m_lightOn = false;
                            this._objectList[16].Visible = false;
                        }
                    }
                }
                if (base.State == 8) {
                    m_tanookiDrainCounter += num;
                    if (m_tanookiDrainCounter >= 0.33f) {
                        m_tanookiDrainCounter = 0f;
                        CurrentMana -= 6f;
                        if (CurrentMana <= 0f)
                            DeactivateTanooki();
                    }
                }
                if (m_spellCastDelay > 0f)
                    m_spellCastDelay -= num;
                base.Update(gameTime);
                return;
            }
            if (m_externalLS.IsActive)
                m_externalLS.Update(gameTime);
        }

        private void UpdateAnimationState() {
            switch (base.State) {
                case 0:
                    if (base.SpriteName != "PlayerIdle_Character")
                        ChangeSprite("PlayerIdle_Character");
                    if (!base.IsAnimating && m_playerHead.SpriteName != "PlayerIdleHeadUp_Sprite") {
                        base.PlayAnimation(true);
                        return;
                    }
                    break;
                case 1:
                    if (base.SpriteName != "PlayerWalking_Character")
                        ChangeSprite("PlayerWalking_Character");
                    if (!base.IsAnimating) {
                        base.PlayAnimation(true);
                        return;
                    }
                    break;
                case 2:
                case 7:
                case 9:
                    if (base.AccelerationY <= 0f) {
                        if (base.SpriteName != "PlayerJumping_Character")
                            ChangeSprite("PlayerJumping_Character");
                    }
                    else if (base.AccelerationY > 0f && base.SpriteName != "PlayerFalling_Character")
                        ChangeSprite("PlayerFalling_Character");
                    if (!base.IsAnimating) {
                        base.PlayAnimation(true);
                        return;
                    }
                    break;
                case 3:
                    if (base.SpriteName != "PlayerHurt_Character")
                        ChangeSprite("PlayerHurt_Character");
                    if (base.IsAnimating) {
                        base.StopAnimation();
                        return;
                    }
                    break;
                case 4:
                    if (base.HeadingX < 0f && Flip == SpriteEffects.None) {
                        if (base.SpriteName != "PlayerDash_Character")
                            ChangeSprite("PlayerDash_Character");
                    }
                    else if (base.HeadingX < 0f && Flip == SpriteEffects.FlipHorizontally && base.SpriteName != "PlayerFrontDash_Character")
                        ChangeSprite("PlayerFrontDash_Character");
                    if (base.HeadingX > 0f && Flip == SpriteEffects.None) {
                        if (base.SpriteName != "PlayerFrontDash_Character")
                            ChangeSprite("PlayerFrontDash_Character");
                    }
                    else if (base.HeadingX > 0f && Flip == SpriteEffects.FlipHorizontally && base.SpriteName != "PlayerDash_Character")
                        ChangeSprite("PlayerDash_Character");
                    if (!base.IsAnimating) {
                        base.PlayAnimation(false);
                        return;
                    }
                    break;
                case 5:
                    break;
                case 6:
                    if (base.SpriteName != "PlayerBlock_Character") {
                        ChangeSprite("PlayerBlock_Character");
                        base.PlayAnimation(false);
                    }
                    break;
                case 8:
                    if (base.SpriteName != "Tanooki_Character") {
                        ChangeSprite("Tanooki_Character");
                        return;
                    }
                    break;
                default:
                    return;
            }
        }

        private void CheckGroundCollision() {
            m_previousIsTouchingGround = m_isTouchingGround;
            m_isTouchingGround = false;
            m_collidingLeft = (m_collidingRight = false);
            m_collidingLeftOnly = (m_collidingRightOnly = false);
            bool flag = false;
            IPhysicsObj physicsObj = null;
            float num = 3.40282347E+38f;
            IPhysicsObj physicsObj2 = null;
            float num2 = 3.40282347E+38f;
            Rectangle rectangle = new Rectangle(this.TerrainBounds.Left, this.TerrainBounds.Bottom - 78, this.TerrainBounds.Width, 88);
            foreach (IPhysicsObj current in base.PhysicsMngr.ObjectList) {
                if (((Game.PlayerStats.Traits.X != 33f && Game.PlayerStats.Traits.Y != 33f) || !(current is PhysicsObj) || current is HazardObj) && current != this && current.Visible && current.IsCollidable && (current.CollidesTop || current.CollidesLeft || current.CollidesRight) && current.HasTerrainHitBox && (current.CollisionTypeTag == 1 || current.CollisionTypeTag == 10 || current.CollisionTypeTag == 5 || current.CollisionTypeTag == 4) && (!current.CollidesTop || current.CollidesBottom || (base.State != 7 && base.State != 9)) && (!current.CollidesTop || !current.CollidesBottom || current.CollidesLeft || current.CollidesRight)) {
                    HazardObj hazardObj = current as HazardObj;
                    if (current.Rotation == 0f || hazardObj != null) {
                        Rectangle rectangle2 = current.TerrainBounds;
                        if (hazardObj != null)
                            rectangle2 = current.Bounds;
                        Vector2 vector = CollisionMath.CalculateMTD(rectangle, rectangle2);
                        Rectangle rectA = new Rectangle(this.TerrainBounds.X, this.TerrainBounds.Y, this.TerrainBounds.Width, this.TerrainBounds.Height);
                        Vector2 intersectionDepth = CollisionMath.GetIntersectionDepth(rectA, rectangle2);
                        Rectangle left = new Rectangle(rectangle.X - 10, rectangle.Y, rectangle.Width + 20, rectangle.Height);
                        Vector2 vector2 = CollisionMath.CalculateMTD(left, rectangle2);
                        if (vector2.X > 0f && current.CollidesRight)
                            m_collidingLeft = true;
                        if (vector2.X < 0f && current.CollidesLeft)
                            m_collidingRight = true;
                        Vector2 vector3 = CollisionMath.CalculateMTD(this.TerrainBounds, rectangle2);
                        if (vector3.X > 0f)
                            m_collidingLeftOnly = true;
                        else if (vector3.X < 0f)
                            m_collidingRightOnly = true;
                        else if (vector3.Y != 0f)
                            flag = true;
                        if (flag)
                            m_collidingRightOnly = (m_collidingLeftOnly = false);
                        if (current.CollidesBottom || Math.Abs(rectangle2.Top - this.TerrainBounds.Bottom) <= 20 || base.AccelerationY >= 1100f) {
                            int num3 = (int)Math.Abs(intersectionDepth.X);
                            int num4 = (int)Math.Abs(intersectionDepth.Y);
                            if ((num3 <= 1 || num3 >= num4) && ((!m_isJumping && base.AccelerationY >= 0f) || (m_isJumping && base.AccelerationY >= 0f)) && (vector.Y < 0f || (vector.Y == 0f && vector.X != 0f && rectangle2.Top > this.TerrainBounds.Top && current.Y > base.Y))) {
                                int num5 = Math.Abs(rectangle2.Top - this.TerrainBounds.Bottom);
                                if (num5 < num) {
                                    physicsObj = current;
                                    num = num5;
                                    m_closestGround = physicsObj;
                                }
                            }
                        }
                    }
                    else if (((!m_isJumping && base.AccelerationY >= 0f) || (m_isJumping && base.AccelerationY >= 0f)) && !(current is HazardObj)) {
                        Vector2 vector4 = CollisionMath.RotatedRectIntersectsMTD(rectangle, base.Rotation, Vector2.Zero, current.TerrainBounds, current.Rotation, Vector2.Zero);
                        if (vector4.Y < 0f) {
                            float y = vector4.Y;
                            if (y < num2) {
                                physicsObj2 = current;
                                num2 = y;
                            }
                        }
                    }
                }
            }
            if (physicsObj != null && base.State != 9) {
                if (m_dropThroughGroundTimer > 0f && !physicsObj.CollidesBottom && physicsObj.CollidesTop)
                    m_isTouchingGround = false;
                else
                    m_isTouchingGround = true;
            }
            if (physicsObj2 != null && base.State != 9) {
                HookToSlope(physicsObj2);
                m_isTouchingGround = true;
            }
            if (m_isTouchingGround) {
                if (base.State == 2 || base.State == 7 || base.State == 3) {
                    if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f) {
                        SoundManager.PlaySound("Player_Land_Low");
                        if (base.AccelerationY > 1400f) {
                            SoundManager.PlaySound("TowerLand");
                            AttachedLevel.ImpactEffectPool.DisplayDustEffect(new Vector2((float)this.TerrainBounds.Left, Bounds.Bottom));
                            AttachedLevel.ImpactEffectPool.DisplayDustEffect(new Vector2((float)this.TerrainBounds.X, Bounds.Bottom));
                            AttachedLevel.ImpactEffectPool.DisplayDustEffect(new Vector2((float)this.TerrainBounds.Right, Bounds.Bottom));
                        }
                    }
                    if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
                        SoundManager.PlaySound("Player_Land_High");
                    else
                        SoundManager.PlaySound("Player_Land");
                }
                if (base.State == 3)
                    m_invincibleCounter = (int)(InvincibilityTime * 1000f);
                if (IsAirAttacking) {
                    IsAirAttacking = false;
                    CancelAttack();
                }
                base.AccelerationX = 0f;
                m_flightCounter = TotalFlightTime;
                if (base.State != 4) {
                    m_airDashCount = 0;
                    base.AccelerationYEnabled = true;
                }
                NumAirBounces = 0;
                m_isFlying = false;
                base.DisableGravity = false;
                if (base.State == 8)
                    base.CurrentSpeed = 0f;
                m_isJumping = false;
                m_doubleJumpCount = 0;
                if (base.State != 6 && base.State != 8 && base.State != 4)
                    base.State = 0;
                if (base.State != 6 && base.State != 8 && base.State != 4 && physicsObj != null && !ControlsLocked && (((Game.GlobalInput.Pressed(18) || Game.GlobalInput.Pressed(19)) && (Game.GlobalInput.JustPressed(10) || Game.GlobalInput.JustPressed(11))) || (Game.GameConfig.QuickDrop && (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19)))) && !physicsObj.CollidesBottom && base.State != 8) {
                    base.AccelerationY = 0f;
                    base.Y += 15f;
                    m_isTouchingGround = false;
                    m_isJumping = true;
                    m_dropThroughGroundTimer = m_dropThroughGroundDuration;
                }
            }
        }

        private void HookToSlope(IPhysicsObj collisionObj) {
            if (base.State != 4) {
                base.UpdateCollisionBoxes();
                Rectangle terrainBounds = this.TerrainBounds;
                terrainBounds.Height += 20;
                float num = base.X;
                if (CollisionMath.RotatedRectIntersectsMTD(terrainBounds, base.Rotation, Vector2.Zero, collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero).Y < 0f) {
                    bool flag = false;
                    Vector2 vector;
                    Vector2 vector2;
                    if (collisionObj.Width > collisionObj.Height) {
                        vector = CollisionMath.UpperLeftCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                        vector2 = CollisionMath.UpperRightCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                        if (Math.Abs(collisionObj.Rotation - 45f) <= 1f)
                            num = (float)this.TerrainBounds.Left;
                        else
                            num = (float)this.TerrainBounds.Right;
                        if (num > vector.X && num < vector2.X)
                            flag = true;
                    }
                    else if (Math.Abs(collisionObj.Rotation - 45f) <= 1f) {
                        vector = CollisionMath.LowerLeftCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                        vector2 = CollisionMath.UpperLeftCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                        num = (float)this.TerrainBounds.Right;
                        if (num > vector.X && num < vector2.X)
                            flag = true;
                    }
                    else {
                        vector = CollisionMath.UpperRightCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                        vector2 = CollisionMath.LowerRightCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
                        num = (float)this.TerrainBounds.Left;
                        if (num > vector.X && num < vector2.X)
                            flag = true;
                    }
                    if (flag) {
                        float num2 = vector2.X - vector.X;
                        float num3 = vector2.Y - vector.Y;
                        float x = vector.X;
                        float y = vector.Y;
                        float num4 = y + (num - x) * (num3 / num2);
                        num4 -= (float)this.TerrainBounds.Bottom - base.Y - 2f;
                        base.Y = (float)((int)num4);
                    }
                }
            }
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
            IPhysicsObj physicsObj = otherBox.AbsParent as IPhysicsObj;
            TeleporterObj teleporterObj = otherBox.Parent as TeleporterObj;
            if (teleporterObj != null && !ControlsLocked && base.IsTouchingGround && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))) {
                StopAllSpells();
                LockControls();
                m_lastTouchedTeleporter = teleporterObj;
                Tween.RunFunction(0f, AttachedLevel, "DisplayMap", new object[] {
                    true
                });
            }
            DoorObj doorObj = otherBox.Parent as DoorObj;
            if (doorObj != null && !ControlsLocked && base.IsTouchingGround && doorObj.IsBossDoor && !doorObj.Locked && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))) {
                if (doorObj.Name == "FinalBossDoor")
                    Game.ScreenManager.DisplayScreen(24, true, null);
                else {
                    RoomObj linkedRoom = doorObj.Room.LinkedRoom;
                    if (linkedRoom != null) {
                        foreach (DoorObj current in linkedRoom.DoorList) {
                            if (current.IsBossDoor) {
                                if (linkedRoom is LastBossChallengeRoom)
                                    linkedRoom.LinkedRoom = AttachedLevel.CurrentRoom;
                                StopAllSpells();
                                base.CurrentSpeed = 0f;
                                LockControls();
                                (m_levelScreen.ScreenManager as RCScreenManager).StartWipeTransition();
                                Vector2 vector = new Vector2(current.X + (float)current.Width / 2f, (float)current.Bounds.Bottom - ((float)Bounds.Bottom - base.Y));
                                Tween.RunFunction(0.2f, this, "EnterBossRoom", new object[] {
                                    vector
                                });
                                Tween.RunFunction(0.2f, m_levelScreen.ScreenManager, "EndWipeTransition", new object[0]);
                                break;
                            }
                        }
                    }
                }
            }
            BreakableObj breakableObj = physicsObj as BreakableObj;
            if (breakableObj != null && IsAirAttacking && thisBox.Type == 1) {
                IsAirAttacking = false;
                base.AccelerationY = -AirAttackKnockBack;
                NumAirBounces++;
            }
            if (Game.PlayerStats.Traits.X == 33f || Game.PlayerStats.Traits.Y == 33f) {
                if (breakableObj != null && !breakableObj.Broken)
                    breakableObj.Break();
                if (physicsObj.GetType() == typeof(PhysicsObj) && (physicsObj as PhysicsObj).SpriteName != "CastleEntranceGate_Sprite")
                    return;
            }
            if (collisionResponseType == 1 && (physicsObj.CollisionTypeTag == 1 || physicsObj.CollisionTypeTag == 5 || physicsObj.CollisionTypeTag == 4 || physicsObj.CollisionTypeTag == 10)) {
                Vector2 vector2 = CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect);
                float arg_325_0 = base.AccelerationY;
                Vector2 vector3 = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                bool flag = true;
                if (m_dropThroughGroundTimer > 0f && !physicsObj.CollidesBottom && physicsObj.CollidesTop)
                    flag = false;
                if (m_isTouchingGround && !physicsObj.CollidesBottom && physicsObj.CollidesTop && physicsObj.TerrainBounds.Top < this.TerrainBounds.Bottom - 10)
                    flag = false;
                if (!physicsObj.CollidesBottom && Bounds.Bottom > physicsObj.TerrainBounds.Top + 10 && !m_isTouchingGround)
                    flag = false;
                if (!physicsObj.CollidesBottom && physicsObj.CollidesTop && (base.State == 7 || base.State == 9))
                    flag = false;
                if ((m_collidingLeftOnly || m_collidingRightOnly) && Math.Abs(vector2.X) < 10f && !m_isTouchingGround && !(physicsObj is HazardObj))
                    flag = false;
                if (!physicsObj.CollidesLeft && !physicsObj.CollidesRight && physicsObj.CollidesTop && physicsObj.CollidesBottom && !(physicsObj is HazardObj)) {
                    if (Game.PlayerStats.Traits.X != 7f && Game.PlayerStats.Traits.Y != 7f) {
                        if (base.X < (float)physicsObj.TerrainBounds.Center.X)
                            base.X -= (float)(this.TerrainBounds.Right - physicsObj.TerrainBounds.Left);
                        else
                            base.X += (float)(physicsObj.TerrainBounds.Right - this.TerrainBounds.Left);
                    }
                    else
                        flag = false;
                }
                if (m_isTouchingGround && m_closestGround == physicsObj) {
                    flag = false;
                    if (physicsObj is HazardObj && physicsObj.Rotation == -90f)
                        base.Y += (float)(m_closestGround.Bounds.Top - this.TerrainBounds.Bottom + 15);
                    else
                        base.Y += (float)(m_closestGround.TerrainBounds.Top - this.TerrainBounds.Bottom);
                    base.AccelerationY = 0f;
                }
                if (flag)
                    base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                if (vector3.Y != 0f && otherBox.AbsRotation != 0f)
                    base.X -= vector3.X;
            }
            if (thisBox.Type == 2 && otherBox.Type == 1 && (physicsObj.CollisionTypeTag == 3 || physicsObj.CollisionTypeTag == 4 || physicsObj.CollisionTypeTag == 10) && base.State != 3 && m_invincibleCounter <= 0) {
                EnemyObj enemyObj = physicsObj as EnemyObj;
                if (enemyObj != null && enemyObj.IsDemented)
                    return;
                ProjectileObj projectileObj = physicsObj as ProjectileObj;
                if (projectileObj != null && projectileObj.IsDemented)
                    return;
                if (!LevelEV.ENABLE_PLAYER_DEBUG) {
                    if (base.State == 6 && (CurrentMana > 0f || m_blockInvincibleCounter > 0f) && (projectileObj == null || (projectileObj != null && projectileObj.Spell != 8 && projectileObj.Spell != 12))) {
                        if (base.CanBeKnockedBack) {
                            Point center = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
                            Vector2 position = new Vector2(center.X, center.Y);
                            if (position == Vector2.Zero)
                                position = base.Position;
                            m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(position, Vector2.One);
                            base.CurrentSpeed = 0f;
                            if ((float)(otherBox.AbsParent.Bounds.Left + otherBox.AbsParent.Bounds.Width / 2) > base.X)
                                base.AccelerationX = -base.KnockBack.X;
                            else
                                base.AccelerationX = base.KnockBack.X;
                            base.AccelerationY = -base.KnockBack.Y;
                            base.Blink(Color.LightBlue, 0.1f);
                        }
                        if (m_blockInvincibleCounter <= 0f) {
                            CurrentMana -= BlockManaDrain;
                            m_blockInvincibleCounter = BlockInvincibleTime;
                            m_levelScreen.TextManager.DisplayNumberStringText(-25, "mp", Color.SkyBlue, new Vector2(base.X, Bounds.Top));
                        }
                        SoundManager.PlaySound("Player_Block");
                    }
                    else if (m_invincibleCounter <= 0)
                        HitPlayer(otherBox.AbsParent);
                    ProjectileObj projectileObj2 = otherBox.AbsParent as ProjectileObj;
                    if (projectileObj2 != null && projectileObj2.DestroysWithEnemy && !m_assassinSpecialActive)
                        projectileObj2.RunDestroyAnimation(true);
                }
            }
            ItemDropObj itemDropObj = physicsObj as ItemDropObj;
            if (itemDropObj != null && itemDropObj.IsCollectable) {
                itemDropObj.GiveReward(this, m_levelScreen.TextManager);
                itemDropObj.IsCollidable = false;
                itemDropObj.IsWeighted = false;
                itemDropObj.AnimationDelay = 0.0166666675f;
                itemDropObj.AccelerationY = 0f;
                itemDropObj.AccelerationX = 0f;
                Tween.By(itemDropObj, 0.4f, new Easing(Quad.EaseOut), new[] {
                    "Y",
                    "-120"
                });
                Tween.To(itemDropObj, 0.1f, new Easing(Linear.EaseNone), new[] {
                    "delay",
                    "0.6",
                    "Opacity",
                    "0"
                });
                Tween.AddEndHandlerToLastTween(m_levelScreen.ItemDropManager, "DestroyItemDrop", new object[] {
                    itemDropObj
                });
                SoundManager.PlaySound(new[] {
                    "CoinDrop1",
                    "CoinDrop2",
                    "CoinDrop3",
                    "CoinDrop4",
                    "CoinDrop5"
                });
            }
            ChestObj chestObj = physicsObj as ChestObj;
            if (chestObj != null && !ControlsLocked && m_isTouchingGround && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)) && !chestObj.IsOpen)
                chestObj.OpenChest(m_levelScreen.ItemDropManager, this);
        }

        public void PlayAttackSound() {
            if (Game.PlayerStats.IsFemale) {
                SoundManager.PlaySound(new[] {
                    "Player_Female_Effort_03",
                    "Player_Female_Effort_04",
                    "Player_Female_Effort_05",
                    "Player_Female_Effort_06",
                    "Player_Female_Effort_07",
                    "Blank",
                    "Blank",
                    "Blank"
                });
            }
            else {
                SoundManager.PlaySound(new[] {
                    "Player_Male_Effort_01",
                    "Player_Male_Effort_02",
                    "Player_Male_Effort_04",
                    "Player_Male_Effort_05",
                    "Player_Male_Effort_07",
                    "Blank",
                    "Blank",
                    "Blank"
                });
            }
            if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14) {
                SoundManager.PlaySound(new[] {
                    "Player_Attack_Sword_Spell_01",
                    "Player_Attack_Sword_Spell_02",
                    "Player_Attack_Sword_Spell_03"
                });
                return;
            }
            if (!IsAirAttacking) {
                if (Game.PlayerStats.Traits.X == 16f || Game.PlayerStats.Traits.Y == 16f) {
                    SoundManager.PlaySound(new[] {
                        "Player_Attack01_Low",
                        "Player_Attack02_Low"
                    });
                    return;
                }
                if (Game.PlayerStats.Traits.X == 17f || Game.PlayerStats.Traits.Y == 17f) {
                    SoundManager.PlaySound(new[] {
                        "Player_Attack01_High",
                        "Player_Attack02_High"
                    });
                    return;
                }
                SoundManager.PlaySound(new[] {
                    "Player_Attack01",
                    "Player_Attack02"
                });
                return;
            }
            else {
                if (Game.PlayerStats.Traits.X == 16f || Game.PlayerStats.Traits.Y == 16f) {
                    SoundManager.PlaySound(new[] {
                        "Player_AttackDown01_Low",
                        "Player_AttackDown02_Low"
                    });
                    return;
                }
                if (Game.PlayerStats.Traits.X == 17f || Game.PlayerStats.Traits.Y == 17f) {
                    SoundManager.PlaySound(new[] {
                        "Player_AttackDown01_High",
                        "Player_AttackDown02_High"
                    });
                    return;
                }
                SoundManager.PlaySound(new[] {
                    "Player_AttackDown01",
                    "Player_AttackDown02"
                });
                return;
            }
        }

        public void EnterBossRoom(Vector2 position) {
            base.Position = position;
        }

        public void TeleportPlayer(Vector2 position, TeleporterObj teleporter = null) {
            base.CurrentSpeed = 0f;
            this.Scale = m_internalScale;
            if (teleporter == null)
                teleporter = m_lastTouchedTeleporter;
            Console.WriteLine(string.Concat(new object[] {
                "Player pos: ",
                base.Position,
                " teleporter: ",
                teleporter.Position
            }));
            Tween.To(this, 0.4f, new Easing(Linear.EaseNone), new[] {
                "X",
                teleporter.X.ToString()
            });
            Tween.To(this, 0.05f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "1.5",
                "ScaleX",
                "0"
            });
            Vector2 scale = this.Scale;
            this.ScaleX = 0f;
            Tween.To(this, 0.05f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "3.3",
                "ScaleX",
                scale.X.ToString()
            });
            this.ScaleX = scale.X;
            Vector2 relativePos = new Vector2(position.X, position.Y - ((float)this.TerrainBounds.Bottom - base.Y));
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new RunFunctionLogicAction(this, "LockControls", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerJumping_Character", true, true), Types.Sequence.Serial);
            logicSet.AddAction(new JumpLogicAction(500f), Types.Sequence.Serial);
            logicSet.AddAction(new PlaySoundLogicAction(new[] {
                "Player_Jump_04"
            }), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(teleporter, "SetCollision", new object[] {
                true
            }), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.4f, false), Types.Sequence.Serial);
            logicSet.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character", true, false), Types.Sequence.Parallel);
            logicSet.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(AttachedLevel.ImpactEffectPool, "DisplayTeleportEffect", new object[] {
                new Vector2(teleporter.X, (float)teleporter.Bounds.Top)
            }), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
            logicSet.AddAction(new PlaySoundLogicAction(new[] {
                "Teleport_Disappear"
            }), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(AttachedLevel.ImpactEffectPool, "MegaTeleport", new object[] {
                new Vector2(teleporter.X, (float)teleporter.Bounds.Top),
                this.Scale
            }), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.8f, false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(m_levelScreen.ScreenManager, "StartWipeTransition", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(teleporter, "SetCollision", new object[] {
                false
            }), Types.Sequence.Serial);
            logicSet.AddAction(new TeleportLogicAction(null, relativePos), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.05f, false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(m_levelScreen.ScreenManager, "EndWipeTransition", new object[0]), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(AttachedLevel.ImpactEffectPool, "MegaTeleportReverse", new object[] {
                new Vector2(position.X - 5f, position.Y + 57f),
                scale
            }), Types.Sequence.Serial);
            logicSet.AddAction(new PlaySoundLogicAction(new[] {
                "Teleport_Reappear"
            }), Types.Sequence.Serial);
            logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
            logicSet.AddAction(new RunFunctionLogicAction(this, "LastBossDoorHack", new object[0]), Types.Sequence.Serial);
            RunExternalLogicSet(logicSet);
        }

        public void LastBossDoorHack() {
            if (m_levelScreen.CurrentRoom is CastleEntranceRoomObj && Game.PlayerStats.EyeballBossBeaten && Game.PlayerStats.FairyBossBeaten && Game.PlayerStats.BlobBossBeaten && Game.PlayerStats.FireballBossBeaten && !Game.PlayerStats.FinalDoorOpened) {
                (m_levelScreen.CurrentRoom as CastleEntranceRoomObj).PlayBossDoorAnimation();
                Game.PlayerStats.FinalDoorOpened = true;
                m_levelScreen.RunCinematicBorders(6f);
                return;
            }
            UnlockControls();
        }

        public void RunExternalLogicSet(LogicSet ls) {
            if (m_currentLogicSet != null && m_currentLogicSet.IsActive)
                m_currentLogicSet.Stop();
            base.AnimationDelay = 0.1f;
            if (m_externalLS != null)
                m_externalLS.Dispose();
            m_externalLS = ls;
            m_externalLS.Execute();
        }

        public void HitPlayer(GameObj obj) {
            bool flag = true;
            if (obj is HazardObj && ((Game.PlayerStats.SpecialItem == 4 && (float)obj.Bounds.Top > base.Y) || InvincibleToSpikes))
                flag = false;
            ProjectileObj projectileObj = obj as ProjectileObj;
            if (projectileObj != null) {
                if (projectileObj.IsDemented)
                    flag = false;
                else if (projectileObj.Spell == 12 || projectileObj.Spell == 8) {
                    flag = false;
                    projectileObj.KillProjectile();
                    m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, CDGMath.AngleBetweenPts(base.Position, projectileObj.Position), false);
                }
                EnemyObj enemyObj = projectileObj.Source as EnemyObj;
                if (enemyObj != null && (enemyObj.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS || enemyObj is EnemyObj_LastBoss) && enemyObj.CurrentHealth <= 0)
                    flag = false;
            }
            EnemyObj enemyObj2 = obj as EnemyObj;
            if (enemyObj2 != null && enemyObj2.IsDemented)
                flag = false;
            if (enemyObj2 != null && (enemyObj2.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS || enemyObj2 is EnemyObj_LastBoss) && enemyObj2.CurrentHealth <= 0)
                flag = false;
            if (flag && (!ForceInvincible || (ForceInvincible && obj is HazardObj))) {
                base.Blink(Color.Red, 0.1f);
                m_levelScreen.ImpactEffectPool.DisplayPlayerImpactEffect(base.Position);
                base.AccelerationYEnabled = true;
                UnlockControls();
                int num = (obj as IDealsDamageObj).Damage;
                num = (int)((num - num * TotalDamageReduc) * ClassDamageTakenMultiplier);
                if (num < 0)
                    num = 0;
                if (!Game.PlayerStats.TutorialComplete)
                    num = 0;
                base.CurrentHealth -= num;
                EnemyObj enemyObj3 = obj as EnemyObj;
                if (enemyObj3 != null && base.CurrentHealth > 0) {
                    int num2 = (int)(num * TotalDamageReturn);
                    if (num2 > 0)
                        enemyObj3.HitEnemy(num2, enemyObj3.Position, true);
                }
                if (projectileObj != null && projectileObj.CollisionTypeTag == 3) {
                    EnemyObj enemyObj4 = projectileObj.Source as EnemyObj;
                    if (enemyObj4 != null && !enemyObj4.IsKilled && !enemyObj4.IsDemented && base.CurrentHealth > 0) {
                        int num3 = (int)(num * TotalDamageReturn);
                        if (num3 > 0)
                            enemyObj4.HitEnemy(num3, enemyObj4.Position, true);
                    }
                }
                m_isJumping = false;
                m_isFlying = false;
                base.DisableGravity = false;
                if (base.CanBeKnockedBack) {
                    if (Game.PlayerStats.Traits.X == 13f || Game.PlayerStats.Traits.Y == 13f) {
                        int num4 = CDGMath.RandomInt(1, 4);
                        m_swearBubble.ChangeSprite("SwearBubble" + num4 + "_Sprite");
                        m_swearBubble.Visible = true;
                        m_swearBubbleCounter = 1f;
                    }
                    base.State = 3;
                    UpdateAnimationState();
                    if (m_currentLogicSet.IsActive)
                        m_currentLogicSet.Stop();
                    IsAirAttacking = false;
                    base.AnimationDelay = m_startingAnimationDelay;
                    base.CurrentSpeed = 0f;
                    float num5 = 1f;
                    if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
                        num5 = 1.85f;
                    if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
                        num5 = 0.5f;
                    if ((float)(obj.Bounds.Left + obj.Bounds.Width / 2) > base.X)
                        base.AccelerationX = -base.KnockBack.X * num5;
                    else
                        base.AccelerationX = base.KnockBack.X * num5;
                    base.AccelerationY = -base.KnockBack.Y * num5;
                }
                else
                    m_invincibleCounter = (int)(InvincibilityTime * 1000f);
                if (base.CurrentHealth <= 0) {
                    if (Game.PlayerStats.SpecialItem == 3) {
                        base.CurrentHealth = (int)(MaxHealth * 0.25f);
                        Game.PlayerStats.SpecialItem = 0;
                        (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerHUDSpecialItem();
                        m_invincibleCounter = (int)(InvincibilityTime * 1000f);
                        (m_levelScreen.ScreenManager as RCScreenManager).DisplayScreen(21, true, null);
                    }
                    else {
                        int num6 = CDGMath.RandomInt(1, 100);
                        if (num6 <= SkillSystem.GetSkill(SkillType.Death_Dodge).ModifierAmount * 100f) {
                            base.CurrentHealth = (int)(MaxHealth * 0.1f);
                            m_invincibleCounter = (int)(InvincibilityTime * 1000f);
                            (m_levelScreen.ScreenManager as RCScreenManager).DisplayScreen(21, true, null);
                        }
                        else {
                            ChallengeBossRoomObj challengeBossRoomObj = AttachedLevel.CurrentRoom as ChallengeBossRoomObj;
                            if (challengeBossRoomObj != null)
                                challengeBossRoomObj.KickPlayerOut();
                            else {
                                AttachedLevel.SetObjectKilledPlayer(obj);
                                Kill(true);
                            }
                        }
                    }
                }
                if (!m_levelScreen.IsDisposed) {
                    if (Game.PlayerStats.Traits.X == 25f || Game.PlayerStats.Traits.Y == 25f)
                        m_levelScreen.TextManager.DisplayNumberText(num * 100 + CDGMath.RandomInt(1, 99), Color.Red, new Vector2(base.X, Bounds.Top));
                    else
                        m_levelScreen.TextManager.DisplayNumberText(num, Color.Red, new Vector2(base.X, Bounds.Top));
                }
                if (Game.PlayerStats.SpecialItem == 2) {
                    int num7 = (int)(Game.PlayerStats.Gold * 0.25f / 10f);
                    if (num7 > 50)
                        num7 = 50;
                    if (num7 > 0 && AttachedLevel.ItemDropManager.AvailableItems > num7) {
                        float num8 = 1f;
                        if (Game.PlayerStats.HasArchitectFee)
                            num8 = 0.6f;
                        int num9 = (int)((num7 * 10) * (1f + TotalGoldBonus) * num8);
                        Game.PlayerStats.Gold -= num9;
                        for (int i = 0; i < num7; i++)
                            m_levelScreen.ItemDropManager.DropItemWide(base.Position, 1, 10f);
                        if (num9 > 0)
                            AttachedLevel.TextManager.DisplayNumberStringText(-num9, "gold", Color.Yellow, new Vector2(base.X, Bounds.Top));
                    }
                }
                if (Game.PlayerStats.IsFemale) {
                    SoundManager.PlaySound(new[] {
                        "Player_Female_Damage_03",
                        "Player_Female_Damage_04",
                        "Player_Female_Damage_05",
                        "Player_Female_Damage_06",
                        "Player_Female_Damage_07"
                    });
                }
                else {
                    SoundManager.PlaySound(new[] {
                        "Player_Male_Injury_01",
                        "Player_Male_Injury_02",
                        "Player_Male_Injury_03",
                        "Player_Male_Injury_04",
                        "Player_Male_Injury_05",
                        "Player_Male_Injury_06",
                        "Player_Male_Injury_07",
                        "Player_Male_Injury_08",
                        "Player_Male_Injury_09",
                        "Player_Male_Injury_10"
                    });
                }
                SoundManager.PlaySound(new[] {
                    "EnemyHit1",
                    "EnemyHit2",
                    "EnemyHit3",
                    "EnemyHit4",
                    "EnemyHit5",
                    "EnemyHit6"
                });
            }
        }

        public void KickInHitInvincibility() {
            m_invincibleCounter = (int)(InvincibilityTime * 1000f);
        }

        public override void Kill(bool giveXP = true) {
            ChallengeBossRoomObj challengeBossRoomObj = AttachedLevel.CurrentRoom as ChallengeBossRoomObj;
            if (challengeBossRoomObj != null) {
                challengeBossRoomObj.LoadPlayerData();
                Game.SaveManager.LoadFiles(AttachedLevel, new[] {
                    SaveType.UpgradeData
                });
                base.CurrentHealth = 0;
            }
            m_translocatorSprite.Visible = false;
            m_swearBubble.Visible = false;
            m_swearBubbleCounter = 0f;
            Game.PlayerStats.IsDead = true;
            m_isKilled = true;
            AttachedLevel.RunGameOver();
            base.Kill(giveXP);
        }

        public void RunDeathAnimation1() {
            if (Game.PlayerStats.IsFemale) {
                SoundManager.PlaySound(new[] {
                    "Player_Female_Death_01",
                    "Player_Female_Death_02"
                });
            }
            else {
                SoundManager.PlaySound(new[] {
                    "Player_Male_Death_01",
                    "Player_Male_Death_02",
                    "Player_Male_Death_03",
                    "Player_Male_Death_04",
                    "Player_Male_Death_05",
                    "Player_Male_Death_06",
                    "Player_Male_Death_07",
                    "Player_Male_Death_08",
                    "Player_Male_Death_09"
                });
            }
            ChangeSprite("PlayerDeath_Character");
            base.PlayAnimation(false);
            if (this._objectList[0].Visible) {
                Tween.To(this._objectList[0], 0.5f, new Easing(Tween.EaseNone), new[] {
                    "delay",
                    "0.5",
                    "Opacity",
                    "0"
                });
            }
        }

        public void RunGetItemAnimation() {
            if (m_currentLogicSet != null && m_currentLogicSet.IsActive)
                m_currentLogicSet.Stop();
            base.AnimationDelay = m_startingAnimationDelay;
            ChangeSprite("PlayerLevelUp_Character");
            base.PlayAnimation(false);
        }

        public void CancelAttack() {
            if (m_currentLogicSet == m_standingAttack3LogicSet || m_currentLogicSet == m_airAttackLS)
                m_currentLogicSet.Stop();
            base.AnimationDelay = m_startingAnimationDelay;
        }

        public void RoomTransitionReset() {
            m_timeStopCast = false;
            m_translocatorSprite.Visible = false;
        }

        public override void Reset() {
            if (m_currentLogicSet.IsActive)
                m_currentLogicSet.Stop();
            base.State = 0;
            m_invincibleCounter = 0;
            InitializeEV();
            base.AnimationDelay = m_startingAnimationDelay;
            InitializeLogic();
            IsAirAttacking = false;
            NumSequentialAttacks = 0;
            m_flightCounter = TotalFlightTime;
            m_isFlying = false;
            base.AccelerationYEnabled = true;
            base.Position = Vector2.One;
            UpdateEquipmentColours();
            m_assassinSpecialActive = false;
            m_wizardSparkleCounter = 0.2f;
            base.DisableGravity = false;
            InvincibleToSpikes = false;
            ForceInvincible = false;
            base.DisableAllWeight = false;
            base.Reset();
        }

        public void ResetLevels() {
            base.CurrentHealth = MaxHealth;
            CurrentMana = MaxMana;
        }

        public void StopDash() {
            m_dashCounter = 0;
        }

        public void UpdateEquipmentColours() {
            if (base.State != 8) {
                for (int i = 0; i < Game.PlayerStats.GetEquippedArray.Length; i++) {
                    int num = Game.PlayerStats.GetEquippedArray[i];
                    if (num != -1) {
                        EquipmentData equipmentData = Game.EquipmentSystem.GetEquipmentData(i, num);
                        Vector3 partIndices = PlayerPart.GetPartIndices(i);
                        if (partIndices.X != -1f)
                            base.GetChildAt((int)partIndices.X).TextureColor = equipmentData.FirstColour;
                        if (partIndices.Y != -1f)
                            base.GetChildAt((int)partIndices.Y).TextureColor = equipmentData.SecondColour;
                        if (partIndices.Z != -1f)
                            base.GetChildAt((int)partIndices.Z).TextureColor = equipmentData.SecondColour;
                        if (i == 2 && partIndices.X != -1f)
                            base.GetChildAt(5).TextureColor = equipmentData.FirstColour;
                    }
                    else {
                        Vector3 partIndices2 = PlayerPart.GetPartIndices(i);
                        if (partIndices2.X != -1f)
                            base.GetChildAt((int)partIndices2.X).TextureColor = Color.White;
                        if (partIndices2.Y != -1f)
                            base.GetChildAt((int)partIndices2.Y).TextureColor = Color.White;
                        if (partIndices2.Z != -1f)
                            base.GetChildAt((int)partIndices2.Z).TextureColor = Color.White;
                        if (i == 2)
                            base.GetChildAt(5).TextureColor = Color.White;
                        if (i == 1)
                            base.GetChildAt(7).TextureColor = Color.Red;
                        else if (i == 4) {
                            if (partIndices2.X != -1f)
                                base.GetChildAt((int)partIndices2.X).TextureColor = Color.Red;
                            if (partIndices2.Y != -1f)
                                base.GetChildAt((int)partIndices2.Y).TextureColor = Color.Red;
                        }
                        else if (i == 0 && partIndices2.Y != -1f)
                            base.GetChildAt((int)partIndices2.Y).TextureColor = new Color(11, 172, 239);
                        Color textureColor = new Color(251, 156, 172);
                        base.GetChildAt(13).TextureColor = textureColor;
                    }
                }
            }
        }

        public void CastSpell(bool activateSecondary, bool megaSpell = false) {
            byte spell = Game.PlayerStats.Spell;
            Color white = Color.White;
            ProjectileData projData = SpellEV.GetProjData(spell, this);
            float damageMultiplier = SpellEV.GetDamageMultiplier(spell);
            projData.Damage = (int)(TotalMagicDamage * damageMultiplier);
            int num = (int)(SpellEV.GetManaCost(spell) * (1f - SkillSystem.GetSkill(SkillType.Mana_Cost_Down).ModifierAmount));
            if (CurrentMana >= num) {
                m_spellCastDelay = 0.5f;
                if (!(AttachedLevel.CurrentRoom is CarnivalShoot1BonusRoom) && !(AttachedLevel.CurrentRoom is CarnivalShoot2BonusRoom) && (Game.PlayerStats.Traits.X == 31f || Game.PlayerStats.Traits.Y == 31f) && Game.PlayerStats.Class != 16 && Game.PlayerStats.Class != 17) {
                    byte[] spellList = ClassType.GetSpellList(Game.PlayerStats.Class);
                    do {
                        Game.PlayerStats.Spell = spellList[CDGMath.RandomInt(0, spellList.Length - 1)];
                    } while (Game.PlayerStats.Spell == 6 || Game.PlayerStats.Spell == 4 || Game.PlayerStats.Spell == 11);
                    AttachedLevel.UpdatePlayerSpellIcon();
                }
            }
            float xValue = SpellEV.GetXValue(spell);
            float yValue = SpellEV.GetYValue(spell);
            if (megaSpell) {
                num = (int)(num * 2f);
                projData.Scale *= 1.75f;
                projData.Damage = (int)(projData.Damage * 2f);
            }
            if (CurrentMana < num)
                SoundManager.PlaySound("Error_Spell");
            else if (spell != 6 && spell != 5 && !m_damageShieldCast && num > 0)
                m_levelScreen.TextManager.DisplayNumberStringText(-num, "mp", Color.SkyBlue, new Vector2(base.X, Bounds.Top));
            if (spell != 12 && spell != 11 && (Game.PlayerStats.Traits.X == 22f || Game.PlayerStats.Traits.Y == 22f))
                projData.SourceAnchor = new Vector2(projData.SourceAnchor.X * -1f, projData.SourceAnchor.Y);
            byte b = spell;
            switch (b) {
                case 1:
                case 2:
                case 3:
                case 7:
                case 9:
                case 10:
                case 13:
                case 15:
                    if (CurrentMana >= num && !activateSecondary) {
                        if (spell == 15) {
                            projData.Lifespan = 0.75f;
                            projData.WrapProjectile = true;
                        }
                        if (spell == 1)
                            SoundManager.PlaySound("Cast_Dagger");
                        else if (spell == 2)
                            SoundManager.PlaySound("Cast_Axe");
                        else if (spell == 9)
                            SoundManager.PlaySound("Cast_Chakram");
                        else if (spell == 10)
                            SoundManager.PlaySound("Cast_GiantSword");
                        else if (spell == 13 || spell == 15) {
                            SoundManager.PlaySound(new[] {
                                "Enemy_WallTurret_Fire_01",
                                "Enemy_WallTurret_Fire_02",
                                "Enemy_WallTurret_Fire_03",
                                "Enemy_WallTurret_Fire_04"
                            });
                        }
                        ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projData);
                        projectileObj.Spell = spell;
                        projectileObj.TextureColor = white;
                        projectileObj.AltY = yValue;
                        projectileObj.AltX = xValue;
                        if (spell == 8 && Flip == SpriteEffects.FlipHorizontally)
                            projectileObj.AltX = -xValue;
                        if (spell == 10) {
                            projectileObj.LifeSpan = xValue;
                            projectileObj.Opacity = 0f;
                            projectileObj.Y -= 20f;
                            Tween.By(projectileObj, 0.1f, new Easing(Tween.EaseNone), new[] {
                                "Y",
                                "20"
                            });
                            Tween.To(projectileObj, 0.1f, new Easing(Tween.EaseNone), new[] {
                                "Opacity",
                                "1"
                            });
                        }
                        if (spell == 9) {
                            projData.Angle = new Vector2(-10f, -10f);
                            if (Game.PlayerStats.Traits.X == 22f || Game.PlayerStats.Traits.Y == 22f) {
                                projData.SourceAnchor = new Vector2(-50f, -30f);
                                m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, -projectileObj.Rotation, megaSpell);
                            }
                            else {
                                projData.SourceAnchor = new Vector2(50f, -30f);
                                m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, projectileObj.Rotation, megaSpell);
                            }
                            projData.RotationSpeed = -20f;
                            projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projData);
                        }
                        if (spell == 3) {
                            projectileObj.ShowIcon = true;
                            projectileObj.Rotation = 0f;
                            projectileObj.BlinkTime = xValue / 1.5f;
                            projectileObj.LifeSpan = 20f;
                        }
                        if (spell == 7) {
                            projectileObj.Rotation = 0f;
                            projectileObj.RunDisplacerEffect(m_levelScreen.CurrentRoom, this);
                            projectileObj.KillProjectile();
                        }
                        if (spell == 10)
                            m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, 90f, megaSpell);
                        else if (Game.PlayerStats.Traits.X == 22f || Game.PlayerStats.Traits.Y == 22f)
                            m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, -projectileObj.Rotation, megaSpell);
                        else
                            m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, projectileObj.Rotation, megaSpell);
                        CurrentMana -= num;
                    }
                    break;
                case 4:
                    if (m_timeStopCast) {
                        AttachedLevel.StopTimeStop();
                        m_timeStopCast = false;
                    }
                    else if (CurrentMana >= num && !activateSecondary) {
                        CurrentMana -= num;
                        AttachedLevel.CastTimeStop(0f);
                        m_timeStopCast = true;
                    }
                    break;
                case 5: {
                    int num2 = AttachedLevel.CurrentRoom.ActiveEnemies;
                    int num3 = 9;
                    if (num2 > num3)
                        num2 = num3;
                    if (CurrentMana >= num && !activateSecondary && num2 > 0) {
                        SoundManager.PlaySound("Cast_Crowstorm");
                        int num4 = 200;
                        float num5 = 360f / num2;
                        float num6 = 0f;
                        int num7 = 0;
                        foreach (EnemyObj current in AttachedLevel.CurrentRoom.EnemyList) {
                            if (!current.NonKillable && !current.IsKilled) {
                                ProjectileObj projectileObj2 = m_levelScreen.ProjectileManager.FireProjectile(projData);
                                projectileObj2.LifeSpan = 10f;
                                projectileObj2.AltX = 0.25f;
                                projectileObj2.AltY = 0.05f;
                                projectileObj2.Orientation = MathHelper.ToRadians(num6);
                                projectileObj2.Spell = spell;
                                projectileObj2.TurnSpeed = 0.075f;
                                projectileObj2.IgnoreBoundsCheck = true;
                                projectileObj2.Target = current;
                                projectileObj2.CollisionTypeTag = 1;
                                projectileObj2.Position = CDGMath.GetCirclePosition(num6, (float)num4, base.Position);
                                m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj2.Position, projectileObj2.Rotation, megaSpell);
                                num6 += num5;
                                num7++;
                            }
                            if (num7 > num3)
                                break;
                        }
                        foreach (EnemyObj current2 in AttachedLevel.CurrentRoom.TempEnemyList) {
                            if (!current2.NonKillable && !current2.IsKilled) {
                                ProjectileObj projectileObj3 = m_levelScreen.ProjectileManager.FireProjectile(projData);
                                projectileObj3.LifeSpan = 99f;
                                projectileObj3.AltX = 0.25f;
                                projectileObj3.AltY = 0.05f;
                                projectileObj3.Orientation = MathHelper.ToRadians(num6);
                                projectileObj3.Spell = spell;
                                projectileObj3.TurnSpeed = 0.05f;
                                projectileObj3.IgnoreBoundsCheck = true;
                                projectileObj3.Target = current2;
                                projectileObj3.CollisionTypeTag = 1;
                                projectileObj3.Position = CDGMath.GetCirclePosition(num6, (float)num4, base.Position);
                                m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj3.Position, projectileObj3.Rotation, megaSpell);
                                num6 += num5;
                                num7++;
                            }
                            if (num7 > num3)
                                break;
                        }
                        CurrentMana -= num;
                        m_levelScreen.TextManager.DisplayNumberStringText(-num, "mp", Color.SkyBlue, new Vector2(base.X, Bounds.Top));
                    }
                    break;
                }
                case 6:
                    if (!m_translocatorSprite.Visible && CurrentMana >= num) {
                        CurrentMana -= num;
                        m_translocatorSprite.ChangeSprite(base.SpriteName);
                        m_translocatorSprite.GoToFrame(base.CurrentFrame);
                        m_translocatorSprite.Visible = true;
                        m_translocatorSprite.Position = base.Position;
                        m_translocatorSprite.Flip = Flip;
                        m_translocatorSprite.TextureColor = Color.Black;
                        m_translocatorSprite.Scale = Vector2.Zero;
                        for (int i = 0; i < base.NumChildren; i++) {
                            (m_translocatorSprite.GetChildAt(i) as SpriteObj).ChangeSprite((this._objectList[i] as SpriteObj).SpriteName);
                            m_translocatorSprite.GetChildAt(i).Visible = this._objectList[i].Visible;
                        }
                        m_translocatorSprite.GetChildAt(16).Visible = false;
                        if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14) {
                            m_translocatorSprite.GetChildAt(10).Visible = false;
                            m_translocatorSprite.GetChildAt(11).Visible = false;
                        }
                        m_levelScreen.TextManager.DisplayNumberStringText(-num, "mp", Color.SkyBlue, new Vector2(base.X, Bounds.Top));
                        AttachedLevel.ImpactEffectPool.StartInverseEmit(m_translocatorSprite.Position);
                        Tween.To(m_translocatorSprite, 0.4f, new Easing(Linear.EaseNone), new[] {
                            "ScaleX",
                            this.ScaleX.ToString(),
                            "ScaleY",
                            this.ScaleY.ToString()
                        });
                        SoundManager.PlaySound("mfqt_teleport_out");
                    }
                    else if (m_translocatorSprite.Visible && m_translocatorSprite.Scale == this.Scale) {
                        SoundManager.PlaySound("mfqt_teleport_in");
                        Translocate(m_translocatorSprite.Position);
                        m_translocatorSprite.Visible = false;
                    }
                    break;
                case 8:
                    if (CurrentMana >= num && !activateSecondary) {
                        SoundManager.PlaySound("Cast_Boomerang");
                        ProjectileObj projectileObj4 = m_levelScreen.ProjectileManager.FireProjectile(projData);
                        projectileObj4.Spell = spell;
                        projectileObj4.IgnoreBoundsCheck = true;
                        projectileObj4.TextureColor = white;
                        projectileObj4.ShowIcon = true;
                        projectileObj4.AltX = xValue;
                        if ((Flip == SpriteEffects.FlipHorizontally && Game.PlayerStats.Traits.X != 22f && Game.PlayerStats.Traits.Y != 22f) || (Flip == SpriteEffects.None && (Game.PlayerStats.Traits.X == 22f || Game.PlayerStats.Traits.Y == 22f)))
                            projectileObj4.AltX = -xValue;
                        projectileObj4.AltY = 0.5f;
                        CurrentMana -= num;
                        m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj4.Position, projectileObj4.Rotation, megaSpell);
                    }
                    break;
                case 11:
                    if (m_damageShieldCast) {
                        m_damageShieldCast = false;
                        m_megaDamageShieldCast = false;
                    }
                    else if (CurrentMana >= num && !activateSecondary) {
                        m_damageShieldCast = true;
                        if (megaSpell)
                            m_megaDamageShieldCast = true;
                        SoundManager.PlaySound("Cast_FireShield");
                        int num8 = 200;
                        for (int j = 0; j < (int)yValue; j++) {
                            float altX = 360f / yValue * j;
                            ProjectileObj projectileObj5 = m_levelScreen.ProjectileManager.FireProjectile(projData);
                            projectileObj5.LifeSpan = xValue;
                            projectileObj5.AltX = altX;
                            projectileObj5.AltY = num8;
                            projectileObj5.Spell = spell;
                            projectileObj5.AccelerationXEnabled = false;
                            projectileObj5.AccelerationYEnabled = false;
                            projectileObj5.IgnoreBoundsCheck = true;
                            m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj5.Position, projectileObj5.Rotation, megaSpell);
                        }
                        CurrentMana -= num;
                    }
                    break;
                case 12:
                    if (CurrentMana >= num && !activateSecondary) {
                        SoundManager.PlaySound("Cast_Dagger");
                        for (int k = 0; k < 4; k++) {
                            ProjectileObj projectileObj6 = m_levelScreen.ProjectileManager.FireProjectile(projData);
                            projectileObj6.Orientation = MathHelper.ToRadians(projData.Angle.X);
                            projectileObj6.Spell = spell;
                            projectileObj6.ShowIcon = true;
                            projectileObj6.AltX = xValue;
                            projectileObj6.AltY = 0.5f;
                            switch (k) {
                                case 0:
                                    projData.SourceAnchor = new Vector2(10f, -10f);
                                    break;
                                case 1:
                                    projData.SourceAnchor = new Vector2(10f, 10f);
                                    break;
                                case 2:
                                    projData.SourceAnchor = new Vector2(-10f, 10f);
                                    break;
                            }
                            projData.Angle = new Vector2(projData.Angle.X + 90f, projData.Angle.Y + 90f);
                            m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj6.Position, projectileObj6.Rotation, megaSpell);
                        }
                        CurrentMana -= num;
                    }
                    break;
                case 14:
                    if (CurrentMana >= num) {
                        CurrentMana -= num;
                        ThrowDaggerProjectiles();
                    }
                    break;
                default:
                    if (b == 100) {
                        if (CurrentMana >= num && !activateSecondary) {
                            ProjectileObj projectileObj7 = m_levelScreen.ProjectileManager.FireProjectile(projData);
                            projectileObj7.AltX = 1f;
                            projectileObj7.AltY = 0.5f;
                            projectileObj7.Opacity = 0f;
                            projectileObj7.X = AttachedLevel.CurrentRoom.X;
                            projectileObj7.Y = base.Y;
                            projectileObj7.Scale = new Vector2((float)(AttachedLevel.CurrentRoom.Width / projectileObj7.Width), 0f);
                            projectileObj7.IgnoreBoundsCheck = true;
                            projectileObj7.Spell = spell;
                            CurrentMana -= num;
                        }
                    }
                    break;
            }
            projData.Dispose();
        }

        public void Translocate(Vector2 position) {
            base.DisableAllWeight = true;
            Tween.To(this, 0.08f, new Easing(Linear.EaseNone), new[] {
                "ScaleX",
                "3",
                "ScaleY",
                "0"
            });
            Tween.To(this, 0f, new Easing(Linear.EaseNone), new[] {
                "delay",
                "0.1",
                "X",
                position.X.ToString(),
                "Y",
                position.Y.ToString()
            });
            Tween.AddEndHandlerToLastTween(this, "ResetTranslocution", new object[0]);
            AttachedLevel.UpdateCamera();
        }

        public void OverrideInternalScale(Vector2 internalScale) {
            m_internalScale = internalScale;
        }

        public void ResetTranslocution() {
            base.DisableAllWeight = false;
            this.Scale = m_internalScale;
        }

        public void ConvertHPtoMP() {
            if (MaxHealth > 1) {
                int num = (int)((MaxHealth - Game.PlayerStats.LichHealth) * 0.5f);
                int num2 = (int)(Game.PlayerStats.LichHealth * 0.5f);
                int num3 = (int)((BaseMana + GetEquipmentMana() + (Game.PlayerStats.BonusMana * 5) + SkillSystem.GetSkill(SkillType.Mana_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Mana_Up_Final).ModifierAmount) * 2f);
                if (MaxMana + num + num2 < num3) {
                    SoundManager.PlaySound("Lich_Swap");
                    Game.PlayerStats.LichHealthMod *= 0.5f;
                    Game.PlayerStats.LichHealth -= num2;
                    Game.PlayerStats.LichMana += num + num2;
                    CurrentMana += (num + num2);
                    if (base.CurrentHealth > MaxHealth)
                        base.CurrentHealth = MaxHealth;
                    m_levelScreen.UpdatePlayerHUD();
                    m_levelScreen.TextManager.DisplayNumberStringText(num + num2, "max mp", Color.RoyalBlue, new Vector2(base.X, (Bounds.Top - 30)));
                    m_levelScreen.TextManager.DisplayNumberStringText(-(num + num2), "max hp", Color.Red, new Vector2(base.X, (Bounds.Top - 60)));
                    return;
                }
                SoundManager.PlaySound("Error_Spell");
                m_levelScreen.TextManager.DisplayStringText("Max MP Converted. Need higher level.", Color.RoyalBlue, new Vector2(base.X, (Bounds.Top - 30)));
            }
        }

        public void ActivateAssassinAbility() {
            if (CurrentMana >= 5f) {
                SoundManager.PlaySound("Assassin_Stealth_Enter");
                CurrentMana -= 5f;
                Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new[] {
                    "Opacity",
                    "0.05"
                });
                m_assassinSpecialActive = true;
                ForceInvincible = true;
                m_levelScreen.ImpactEffectPool.AssassinCastEffect(base.Position);
            }
        }

        public void DisableAssassinAbility() {
            Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "1"
            });
            m_assassinSpecialActive = false;
            ForceInvincible = false;
        }

        public void SwapSpells() {
            SoundManager.PlaySound("Spell_Switch");
            m_wizardSpellList[0] = (byte)Game.PlayerStats.WizardSpellList.X;
            m_wizardSpellList[1] = (byte)Game.PlayerStats.WizardSpellList.Y;
            m_wizardSpellList[2] = (byte)Game.PlayerStats.WizardSpellList.Z;
            int num = m_wizardSpellList.IndexOf(Game.PlayerStats.Spell);
            num++;
            if (num >= m_wizardSpellList.Count)
                num = 0;
            Game.PlayerStats.Spell = m_wizardSpellList[num];
            m_levelScreen.UpdatePlayerSpellIcon();
            if (m_damageShieldCast) {
                m_damageShieldCast = false;
                m_megaDamageShieldCast = false;
            }
            if (m_timeStopCast) {
                AttachedLevel.StopTimeStop();
                m_timeStopCast = false;
            }
        }

        public void NinjaTeleport() {
            if (CurrentMana >= 5f && m_ninjaTeleportDelay <= 0f) {
                CurrentMana -= 5f;
                m_ninjaTeleportDelay = 0.5f;
                m_levelScreen.ImpactEffectPool.NinjaDisappearEffect(this);
                int num = 350;
                int num2 = 2147483647;
                TerrainObj terrainObj = CalculateClosestWall(out num2);
                if (terrainObj != null) {
                    if (num2 < num) {
                        if (Flip == SpriteEffects.None) {
                            if (terrainObj.Rotation == 0f)
                                base.X += (float)num2 - (float)this.TerrainBounds.Width / 2f;
                            else
                                base.X += (float)num2 - (float)this.Width / 2f;
                        }
                        else if (terrainObj.Rotation == 0f)
                            base.X -= (float)num2 - (float)this.TerrainBounds.Width / 2f;
                        else
                            base.X -= (float)num2 - (float)this.Width / 2f;
                    }
                    else if (Flip == SpriteEffects.FlipHorizontally)
                        base.X -= (float)num;
                    else
                        base.X += (float)num;
                }
                else {
                    if (Flip == SpriteEffects.FlipHorizontally)
                        base.X -= (float)num;
                    else
                        base.X += (float)num;
                    if (base.X > (float)m_levelScreen.CurrentRoom.Bounds.Right)
                        base.X = (float)(m_levelScreen.CurrentRoom.Bounds.Right - 5);
                    else if (base.X < m_levelScreen.CurrentRoom.X)
                        base.X = m_levelScreen.CurrentRoom.X + 5f;
                }
                SoundManager.PlaySound("Ninja_Teleport");
                m_levelScreen.ImpactEffectPool.NinjaAppearEffect(this);
                m_levelScreen.UpdateCamera();
            }
        }

        public TerrainObj CalculateClosestWall(out int dist) {
            int num = 2147483647;
            TerrainObj result = null;
            Vector2 value = Vector2.Zero;
            RoomObj currentRoom = m_levelScreen.CurrentRoom;
            foreach (TerrainObj current in currentRoom.TerrainObjList) {
                if (current.CollidesBottom || current.CollidesLeft || current.CollidesRight) {
                    value = Vector2.Zero;
                    float num2 = 3.40282347E+38f;
                    if (Flip == SpriteEffects.None) {
                        if (current.X > base.X && current.Bounds.Top + 5 < this.TerrainBounds.Bottom && current.Bounds.Bottom > this.TerrainBounds.Top) {
                            if (current.Rotation < 0f)
                                value = CollisionMath.LineToLineIntersect(base.Position, new Vector2(base.X + 6600f, base.Y), CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
                            else if (current.Rotation > 0f)
                                value = CollisionMath.LineToLineIntersect(base.Position, new Vector2(base.X + 6600f, base.Y), CollisionMath.LowerLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
                            if (value != Vector2.Zero)
                                num2 = value.X - base.X;
                            else
                                num2 = (float)(current.Bounds.Left - this.TerrainBounds.Right);
                        }
                    }
                    else if (current.X < base.X && current.Bounds.Top + 5 < this.TerrainBounds.Bottom && current.Bounds.Bottom > this.TerrainBounds.Top) {
                        if (current.Rotation < 0f)
                            value = CollisionMath.LineToLineIntersect(new Vector2(base.X - 6600f, base.Y), base.Position, CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.LowerRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
                        else if (current.Rotation > 0f)
                            value = CollisionMath.LineToLineIntersect(new Vector2(base.X - 6600f, base.Y), base.Position, CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
                        if (value != Vector2.Zero)
                            num2 = base.X - value.X;
                        else
                            num2 = (float)(this.TerrainBounds.Left - current.Bounds.Right);
                    }
                    if (num2 < num) {
                        num = (int)num2;
                        result = current;
                    }
                }
            }
            dist = num;
            return result;
        }

        public void ActivateTanooki() {
            if (CurrentMana >= 25f) {
                CurrentMana -= 25f;
                m_levelScreen.ImpactEffectPool.DisplayTanookiEffect(this);
                base.TextureColor = Color.White;
                this._objectList[0].TextureColor = Color.White;
                base.State = 8;
            }
        }

        public void DeactivateTanooki() {
            m_levelScreen.ImpactEffectPool.DisplayTanookiEffect(this);
            base.State = 0;
        }

        public void CastFuhRohDah() {
            if (CurrentMana >= 20f) {
                CurrentMana -= 20f;
                ProjectileData data = new ProjectileData(this) {
                    SpriteName = "SpellShout_Sprite",
                    IsWeighted = false,
                    Lifespan = 2f,
                    CollidesWith1Ways = false,
                    CollidesWithTerrain = false,
                    DestroysWithEnemy = false,
                    DestroysWithTerrain = false,
                    Damage = 0,
                    Scale = Vector2.One
                };
                ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(data);
                projectileObj.Opacity = 0f;
                projectileObj.CollisionTypeTag = 2;
                projectileObj.Spell = 20;
                projectileObj.IgnoreBoundsCheck = true;
                Tween.To(projectileObj, 0.2f, new Easing(Tween.EaseNone), new[] {
                    "ScaleX",
                    "100",
                    "ScaleY",
                    "50"
                });
                Tween.AddEndHandlerToLastTween(projectileObj, "KillProjectile", new object[0]);
                SoundManager.PlaySound("Cast_FasRoDus");
                m_levelScreen.ImpactEffectPool.DisplayFusRoDahText(new Vector2(base.X, Bounds.Top));
                m_levelScreen.ShoutMagnitude = 0f;
                Tween.To(m_levelScreen, 1f, new Easing(Tween.EaseNone), new[] {
                    "ShoutMagnitude",
                    "3"
                });
            }
        }

        private void CastCloseShield() {
            ProjectileData projectileData = new ProjectileData(this) {
                SpriteName = "SpellClose_Sprite",
                Speed = new Vector2(0f, 0f),
                IsWeighted = false,
                RotationSpeed = 0f,
                DestroysWithEnemy = false,
                DestroysWithTerrain = false,
                CollidesWithTerrain = false,
                Scale = new Vector2(m_Spell_Close_Scale, m_Spell_Close_Scale),
                Damage = Damage,
                Lifespan = m_Spell_Close_Lifespan,
                LockPosition = true
            };
            projectileData.Damage = (int)(TotalMagicDamage * 1f);
            SoundManager.PlaySound("Cast_GiantSword");
            m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 90f, true);
            ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            projectileObj.TextureColor = Color.CadetBlue;
            projectileData.Dispose();
        }

        private void ThrowAxeProjectiles() {
            m_axeProjData.AngleOffset = 0f;
            m_axeProjData.Damage = (int)(TotalMagicDamage * 1f);
            Tween.RunFunction(0f, this, "CastAxe", new object[0]);
            Tween.RunFunction(0.15f, this, "CastAxe", new object[0]);
            Tween.RunFunction(0.3f, this, "CastAxe", new object[0]);
            Tween.RunFunction(0.45f, this, "CastAxe", new object[0]);
            Tween.RunFunction(0.6f, this, "CastAxe", new object[0]);
        }

        public void CastAxe() {
            m_axeProjData.AngleOffset = (float)CDGMath.RandomInt(-70, 70);
            ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(m_axeProjData);
            projectileObj.TextureColor = Color.CadetBlue;
            SoundManager.PlaySound("Cast_Axe");
            m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 45f, true);
        }

        private void ThrowDaggerProjectiles() {
            m_rapidDaggerProjData.AngleOffset = 0f;
            m_rapidDaggerProjData.Damage = (int)(TotalMagicDamage * SpellEV.GetDamageMultiplier(14));
            Tween.RunFunction(0f, this, "CastDaggers", new object[] {
                false
            });
            Tween.RunFunction(0.05f, this, "CastDaggers", new object[] {
                true
            });
            Tween.RunFunction(0.1f, this, "CastDaggers", new object[] {
                true
            });
            Tween.RunFunction(0.15f, this, "CastDaggers", new object[] {
                true
            });
            Tween.RunFunction(0.2f, this, "CastDaggers", new object[] {
                true
            });
        }

        public void CastDaggers(bool randomize) {
            if (randomize)
                m_rapidDaggerProjData.AngleOffset = (float)CDGMath.RandomInt(-8, 8);
            ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(m_rapidDaggerProjData);
            projectileObj.TextureColor = Color.CadetBlue;
            SoundManager.PlaySound("Cast_Dagger");
            m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 0f, true);
        }

        public void StopAllSpells() {
            if (base.State == 8)
                DeactivateTanooki();
            if (m_damageShieldCast) {
                m_damageShieldCast = false;
                m_megaDamageShieldCast = false;
            }
            if (m_timeStopCast) {
                AttachedLevel.StopTimeStop();
                m_timeStopCast = false;
            }
            if (m_assassinSpecialActive)
                DisableAssassinAbility();
            m_lightOn = false;
            m_translocatorSprite.Visible = false;
            if (base.State == 9) {
                base.State = 2;
                base.DisableGravity = false;
                m_isFlying = false;
            }
        }

        public override void Dispose() {
            if (!base.IsDisposed) {
                if (m_currentLogicSet.IsActive)
                    m_currentLogicSet.Stop();
                m_currentLogicSet = null;
                m_standingAttack3LogicSet.Dispose();
                m_standingAttack3LogicSet = null;
                m_airAttackLS.Dispose();
                m_airAttackLS = null;
                m_debugInputMap.Dispose();
                m_debugInputMap = null;
                m_playerHead = null;
                m_playerLegs = null;
                m_walkDownSound.Dispose();
                m_walkDownSound = null;
                m_walkUpSound.Dispose();
                m_walkUpSound = null;
                m_walkDownSoundLow.Dispose();
                m_walkDownSoundLow = null;
                m_walkUpSoundLow.Dispose();
                m_walkUpSoundLow = null;
                m_walkDownSoundHigh.Dispose();
                m_walkDownSoundHigh = null;
                m_walkUpSoundHigh.Dispose();
                m_walkUpSoundHigh = null;
                if (m_externalLS != null)
                    m_externalLS.Dispose();
                m_externalLS = null;
                m_lastTouchedTeleporter = null;
                m_flightDurationText.Dispose();
                m_flightDurationText = null;
                m_game = null;
                m_translocatorSprite.Dispose();
                m_translocatorSprite = null;
                m_swearBubble.Dispose();
                m_swearBubble = null;
                m_flipTween = null;
                m_wizardSpellList.Clear();
                m_wizardSpellList = null;
                m_closestGround = null;
                m_rapidDaggerProjData.Dispose();
                m_axeProjData.Dispose();
                base.Dispose();
            }
        }

        public override void Draw(Camera2D camera) {
            m_swearBubble.Scale = new Vector2(this.ScaleX * 1.2f, this.ScaleY * 1.2f);
            m_swearBubble.Position = new Vector2(base.X - 30f * this.ScaleX, base.Y - 35f * this.ScaleX);
            m_swearBubble.Draw(camera);
            m_translocatorSprite.Draw(camera);
            base.Draw(camera);
            if (IsFlying && base.State != 9) {
                m_flightDurationText.Text = string.Format("{0:F1}", m_flightCounter);
                m_flightDurationText.Position = new Vector2(base.X, (this.TerrainBounds.Top - 70));
                m_flightDurationText.Draw(camera);
            }
            camera.End();
            Game.ColourSwapShader.Parameters["desiredTint"].SetValue(m_playerHead.TextureColor.ToVector4());
            if (Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15) {
                Game.ColourSwapShader.Parameters["Opacity"].SetValue(base.Opacity);
                Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(m_lichColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(m_lichColour2.ToVector4());
            }
            else if (Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11) {
                Game.ColourSwapShader.Parameters["Opacity"].SetValue(base.Opacity);
                Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
            }
            else {
                Game.ColourSwapShader.Parameters["Opacity"].SetValue(base.Opacity);
                Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(m_skinColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(m_skinColour2.ToVector4());
            }
            camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader, camera.GetTransformation());
            m_playerHead.Draw(camera);
            camera.End();
            camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
            if (Game.PlayerStats.IsFemale)
                this._objectList[13].Draw(camera);
            this._objectList[14].Draw(camera);
            this._objectList[15].Draw(camera);
        }

        public void FadeSword() {
            SpriteObj spriteObj = this._objectList[10] as SpriteObj;
            SpriteObj spriteObj2 = this._objectList[11] as SpriteObj;
            Tween.StopAllContaining(spriteObj, false);
            Tween.StopAllContaining(spriteObj2, false);
            spriteObj.Opacity = 0f;
            spriteObj2.Opacity = 0f;
            spriteObj.TextureColor = Color.White;
            Tween.To(spriteObj, 0.1f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "1"
            });
            spriteObj.Opacity = 1f;
            Tween.To(spriteObj, 0.1f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "0.2",
                "Opacity",
                "0"
            });
            spriteObj.Opacity = 0f;
            spriteObj2.TextureColor = Color.White;
            Tween.To(spriteObj2, 0.1f, new Easing(Tween.EaseNone), new[] {
                "Opacity",
                "1"
            });
            spriteObj2.Opacity = 1f;
            Tween.To(spriteObj2, 0.1f, new Easing(Tween.EaseNone), new[] {
                "delay",
                "0.2",
                "Opacity",
                "0"
            });
            spriteObj2.Opacity = 0f;
        }

        public void ChangePartColour(int playerPart, Color colour) {
            if (playerPart == 1 || playerPart == 8) {
                GetPlayerPart(8).TextureColor = colour;
                GetPlayerPart(1).TextureColor = colour;
                return;
            }
            GetPlayerPart(playerPart).TextureColor = colour;
        }

        public SpriteObj GetPlayerPart(int playerPart) {
            return this._objectList[playerPart] as SpriteObj;
        }

        public void AttachLevel(ProceduralLevelScreen level) {
            m_levelScreen = level;
        }

        public int GetEquipmentDamage() {
            int num = 0;
            int num2 = 0;
            sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
            for (int i = 0; i < getEquippedArray.Length; i++) {
                sbyte b = getEquippedArray[i];
                if (b > -1)
                    num += Game.EquipmentSystem.GetEquipmentData(num2, b).BonusDamage;
                num2++;
            }
            return num;
        }

        public int GetEquipmentMana() {
            int num = 0;
            int num2 = 0;
            sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
            for (int i = 0; i < getEquippedArray.Length; i++) {
                sbyte b = getEquippedArray[i];
                if (b > -1)
                    num += Game.EquipmentSystem.GetEquipmentData(num2, b).BonusMana;
                num2++;
            }
            return num;
        }

        public int GetEquipmentHealth() {
            int num = 0;
            int num2 = 0;
            sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
            for (int i = 0; i < getEquippedArray.Length; i++) {
                sbyte b = getEquippedArray[i];
                if (b > -1)
                    num += Game.EquipmentSystem.GetEquipmentData(num2, b).BonusHealth;
                num2++;
            }
            return num;
        }

        public int GetEquipmentArmor() {
            int num = 0;
            int num2 = 0;
            sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
            for (int i = 0; i < getEquippedArray.Length; i++) {
                sbyte b = getEquippedArray[i];
                if (b > -1)
                    num += Game.EquipmentSystem.GetEquipmentData(num2, b).BonusArmor;
                num2++;
            }
            return num;
        }

        public int GetEquipmentWeight() {
            int num = 0;
            int num2 = 0;
            sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
            for (int i = 0; i < getEquippedArray.Length; i++) {
                sbyte b = getEquippedArray[i];
                if (b > -1)
                    num += Game.EquipmentSystem.GetEquipmentData(num2, b).Weight;
                num2++;
            }
            return num;
        }

        public int GetEquipmentMagicDamage() {
            int num = 0;
            int num2 = 0;
            sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
            for (int i = 0; i < getEquippedArray.Length; i++) {
                sbyte b = getEquippedArray[i];
                if (b > -1)
                    num += Game.EquipmentSystem.GetEquipmentData(num2, b).BonusMagic;
                num2++;
            }
            return num;
        }

        public float GetEquipmentSecondaryAttrib(int secondaryAttribType) {
            float num = 0f;
            int num2 = 0;
            sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
            for (int i = 0; i < getEquippedArray.Length; i++) {
                sbyte b = getEquippedArray[i];
                if (b > -1) {
                    EquipmentData equipmentData = Game.EquipmentSystem.GetEquipmentData(num2, b);
                    Vector2[] secondaryAttribute = equipmentData.SecondaryAttribute;
                    for (int j = 0; j < secondaryAttribute.Length; j++) {
                        Vector2 vector = secondaryAttribute[j];
                        if ((int)vector.X == secondaryAttribType)
                            num += vector.Y;
                    }
                }
                num2++;
            }
            return num;
        }
    }
}
