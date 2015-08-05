using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class ChestConditionChecker
	{
		public const byte STATE_LOCKED = 0;
		public const byte STATE_FREE = 1;
		public const byte STATE_FAILED = 2;
		public static float HelperFloat;
		public static void SetConditionState(FairyChestObj chest, PlayerObj player)
		{
			int num = 100;
			switch (chest.ConditionType)
			{
			case 0:
			case 10:
				if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < (float)num)
				{
					chest.SetChestUnlocked();
					return;
				}
				break;
			case 1:
				if (player.AttachedLevel.CurrentRoom.ActiveEnemies <= 0)
				{
					chest.SetChestUnlocked();
					return;
				}
				break;
			case 2:
			case 5:
				break;
			case 3:
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (chest.AbsPosition.X < player.AbsPosition.X)
				{
					spriteEffects = SpriteEffects.FlipHorizontally;
				}
				if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < 375f && Vector2.Distance(chest.AbsPosition, player.AbsPosition) > (float)num && player.Flip == spriteEffects)
				{
					chest.SetChestFailed(false);
					return;
				}
				if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < (float)num)
				{
					chest.SetChestUnlocked();
					return;
				}
				break;
			}
			case 4:
				if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < 10000f && player.IsJumping && player.AccelerationY < 0f && Vector2.Distance(chest.AbsPosition, player.AbsPosition) > (float)num)
				{
					chest.SetChestFailed(false);
					return;
				}
				if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < (float)num)
				{
					chest.SetChestUnlocked();
					return;
				}
				break;
			case 6:
				if (player.IsTouchingGround && Vector2.Distance(chest.AbsPosition, player.AbsPosition) > (float)num && Vector2.Distance(chest.AbsPosition, player.AbsPosition) < 1000f)
				{
					chest.SetChestFailed(false);
					return;
				}
				if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < (float)num)
				{
					chest.SetChestUnlocked();
					return;
				}
				break;
			case 7:
				foreach (EnemyObj current in player.AttachedLevel.CurrentRoom.EnemyList)
				{
					if (current.CurrentHealth < current.MaxHealth)
					{
						chest.SetChestFailed(false);
						break;
					}
				}
				if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < (float)num && chest.State == 0)
				{
					chest.SetChestUnlocked();
					return;
				}
				break;
			case 8:
				if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) > (float)num && chest.Timer <= 0f)
				{
					chest.SetChestFailed(false);
					return;
				}
				if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < (float)num && chest.Timer > 0f)
				{
					chest.SetChestUnlocked();
					return;
				}
				break;
			case 9:
				if (player.State == 3)
				{
					chest.SetChestFailed(false);
					return;
				}
				if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < (float)num)
				{
					chest.SetChestUnlocked();
				}
				break;
			default:
				return;
			}
		}
	}
}
