using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
namespace RogueCastle
{
	public class EquipmentSystem
	{
		private List<EquipmentData[]> m_equipmentDataArray;
		private List<int[]> m_abilityCostArray;
		private float AbilityCostMod = 175f;
		private int AbilityCostBase = 175;
		public EquipmentSystem()
		{
			this.m_equipmentDataArray = new List<EquipmentData[]>();
			for (int i = 0; i < 5; i++)
			{
				EquipmentData[] array = new EquipmentData[15];
				for (int j = 0; j < 15; j++)
				{
					array[j] = new EquipmentData();
				}
				this.m_equipmentDataArray.Add(array);
			}
			this.m_abilityCostArray = new List<int[]>();
			for (int k = 0; k < 5; k++)
			{
				int[] array2 = new int[11];
				for (int l = 0; l < 11; l++)
				{
					array2[l] = 9999;
				}
				this.m_abilityCostArray.Add(array2);
			}
		}
		public void InitializeAbilityCosts()
		{
			this.m_abilityCostArray[0][0] = this.AbilityCostBase;
			this.m_abilityCostArray[0][1] = this.AbilityCostBase;
			this.m_abilityCostArray[0][2] = this.AbilityCostBase;
			this.m_abilityCostArray[0][3] = this.AbilityCostBase;
			this.m_abilityCostArray[0][4] = this.AbilityCostBase;
			this.m_abilityCostArray[0][7] = this.AbilityCostBase;
			this.m_abilityCostArray[0][5] = this.AbilityCostBase;
			this.m_abilityCostArray[0][6] = this.AbilityCostBase;
			this.m_abilityCostArray[0][10] = this.AbilityCostBase;
			this.m_abilityCostArray[0][9] = this.AbilityCostBase;
			this.m_abilityCostArray[0][8] = this.AbilityCostBase;
			this.m_abilityCostArray[4][0] = this.AbilityCostBase;
			this.m_abilityCostArray[4][1] = this.AbilityCostBase;
			this.m_abilityCostArray[4][2] = this.AbilityCostBase;
			this.m_abilityCostArray[4][3] = this.AbilityCostBase;
			this.m_abilityCostArray[4][4] = this.AbilityCostBase;
			this.m_abilityCostArray[4][7] = this.AbilityCostBase;
			this.m_abilityCostArray[4][5] = this.AbilityCostBase;
			this.m_abilityCostArray[4][6] = this.AbilityCostBase;
			this.m_abilityCostArray[4][10] = this.AbilityCostBase;
			this.m_abilityCostArray[4][9] = this.AbilityCostBase;
			this.m_abilityCostArray[4][8] = this.AbilityCostBase;
			this.m_abilityCostArray[2][0] = this.AbilityCostBase;
			this.m_abilityCostArray[2][1] = this.AbilityCostBase;
			this.m_abilityCostArray[2][2] = this.AbilityCostBase;
			this.m_abilityCostArray[2][3] = this.AbilityCostBase;
			this.m_abilityCostArray[2][4] = this.AbilityCostBase;
			this.m_abilityCostArray[2][7] = this.AbilityCostBase;
			this.m_abilityCostArray[2][5] = this.AbilityCostBase;
			this.m_abilityCostArray[2][6] = this.AbilityCostBase;
			this.m_abilityCostArray[2][10] = this.AbilityCostBase;
			this.m_abilityCostArray[2][9] = this.AbilityCostBase;
			this.m_abilityCostArray[2][8] = this.AbilityCostBase;
			this.m_abilityCostArray[3][0] = this.AbilityCostBase;
			this.m_abilityCostArray[3][1] = this.AbilityCostBase;
			this.m_abilityCostArray[3][2] = this.AbilityCostBase;
			this.m_abilityCostArray[3][3] = this.AbilityCostBase;
			this.m_abilityCostArray[3][4] = this.AbilityCostBase;
			this.m_abilityCostArray[3][7] = this.AbilityCostBase;
			this.m_abilityCostArray[3][5] = this.AbilityCostBase;
			this.m_abilityCostArray[3][6] = this.AbilityCostBase;
			this.m_abilityCostArray[3][10] = this.AbilityCostBase;
			this.m_abilityCostArray[3][9] = this.AbilityCostBase;
			this.m_abilityCostArray[3][8] = this.AbilityCostBase;
			this.m_abilityCostArray[1][0] = this.AbilityCostBase;
			this.m_abilityCostArray[1][1] = this.AbilityCostBase;
			this.m_abilityCostArray[1][2] = this.AbilityCostBase;
			this.m_abilityCostArray[1][3] = this.AbilityCostBase;
			this.m_abilityCostArray[1][4] = this.AbilityCostBase;
			this.m_abilityCostArray[1][7] = this.AbilityCostBase;
			this.m_abilityCostArray[1][5] = this.AbilityCostBase;
			this.m_abilityCostArray[1][6] = this.AbilityCostBase;
			this.m_abilityCostArray[1][10] = this.AbilityCostBase;
			this.m_abilityCostArray[1][9] = this.AbilityCostBase;
			this.m_abilityCostArray[1][8] = this.AbilityCostBase;
		}
		public void InitializeEquipmentData()
		{
			this.CreateEquipmentData(0, 1, 0, 0, 250, 15, 0, 7, 0, 0, 0, new Color(255, 230, 175), new Color(70, 130, 255), new Vector2[0]);
			this.CreateEquipmentData(1, 1, 0, 0, 150, 10, 20, 0, 0, 5, 0, new Color(255, 221, 130), new Color(70, 130, 255), new Vector2[0]);
			this.CreateEquipmentData(4, 3, 2, 0, 300, 10, 0, 0, 0, 0, 0, new Color(255, 221, 130), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(1f, 0.04f)
			});
			this.CreateEquipmentData(3, 1, 0, 0, 200, 10, 0, 0, 0, 0, 5, new Color(210, 195, 125), new Color(210, 195, 125), new Vector2[0]);
			this.CreateEquipmentData(2, 1, 0, 0, 350, 20, 0, 0, 9, 0, 0, new Color(190, 165, 105), new Color(155, 190, 366), new Vector2[0]);
			this.CreateEquipmentData(0, 5, 0, 5, 375, 20, 0, 11, 0, 0, 0, new Color(135, 135, 135), new Color(95, 95, 95), new Vector2[0]);
			this.CreateEquipmentData(1, 3, 0, 5, 250, 15, 25, 0, 0, 20, 0, new Color(165, 165, 165), new Color(125, 235, 80), new Vector2[0]);
			this.CreateEquipmentData(4, 5, 0, 5, 425, 15, 0, 0, 0, 0, 0, new Color(125, 235, 80), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(2f, 0.15f)
			});
			this.CreateEquipmentData(3, 3, 0, 5, 375, 20, 0, 0, 0, 0, 11, new Color(155, 155, 155), new Color(155, 155, 155), new Vector2[0]);
			this.CreateEquipmentData(2, 3, 0, 5, 525, 30, 0, 0, 16, 0, 0, new Color(120, 120, 120), new Color(95, 95, 95), new Vector2[0]);
			this.CreateEquipmentData(0, 1, 2, 10, 500, 35, -30, 9, 0, 0, 0, new Color(105, 0, 0), new Color(105, 0, 0), new Vector2[]
			{
				new Vector2(7f, 1f)
			});
			this.CreateEquipmentData(1, 1, 3, 10, 500, 35, -30, 0, 0, 25, 0, new Color(230, 0, 0), new Color(105, 0, 0), new Vector2[]
			{
				new Vector2(7f, 1f)
			});
			this.CreateEquipmentData(4, 1, 2, 10, 500, 35, -30, 0, 0, 0, 0, new Color(230, 0, 0), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(7f, 1f)
			});
			this.CreateEquipmentData(3, 1, 3, 10, 500, 35, -30, 0, 0, 0, 9, new Color(230, 0, 0), new Color(105, 0, 0), new Vector2[]
			{
				new Vector2(7f, 1f)
			});
			this.CreateEquipmentData(2, 1, 2, 10, 500, 35, -30, 0, 14, 0, 0, new Color(230, 0, 0), new Color(105, 0, 0), new Vector2[]
			{
				new Vector2(7f, 1f)
			});
			this.CreateEquipmentData(0, 10, 2, 1, 750, 35, 0, 17, 0, 0, 0, new Color(245, 255, 175), new Color(215, 215, 215), new Vector2[0]);
			this.CreateEquipmentData(1, 8, 0, 1, 550, 25, 35, 0, 0, 25, 0, new Color(245, 255, 175), new Color(215, 215, 215), new Vector2[0]);
			this.CreateEquipmentData(4, 11, 2, 1, 850, 35, 0, 0, 0, 0, 0, new Color(245, 255, 175), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(8f, 1f)
			});
			this.CreateEquipmentData(3, 8, 0, 1, 750, 35, 0, 0, 0, 0, 17, new Color(245, 255, 175), new Color(245, 255, 175), new Vector2[0]);
			this.CreateEquipmentData(2, 8, 0, 1, 950, 45, 0, 0, 25, 0, 0, new Color(245, 255, 175), new Color(215, 215, 215), new Vector2[0]);
			this.CreateEquipmentData(0, 15, 2, 6, 1150, 40, 0, 22, 0, 0, 0, new Color(10, 150, 50), new Color(10, 150, 50), new Vector2[0]);
			this.CreateEquipmentData(1, 12, 0, 6, 950, 30, 55, 0, 0, 15, 0, new Color(180, 120, 70), new Color(78, 181, 80), new Vector2[0]);
			this.CreateEquipmentData(4, 13, 2, 6, 1300, 40, 0, 0, 0, 0, 0, new Color(135, 200, 130), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(1f, 0.04f),
				new Vector2(2f, 0.15f)
			});
			this.CreateEquipmentData(3, 12, 0, 6, 1075, 40, 0, 0, 0, 0, 22, new Color(160, 125, 80), new Color(160, 125, 80), new Vector2[0]);
			this.CreateEquipmentData(2, 12, 0, 6, 1500, 55, 0, 0, 38, 0, 0, new Color(180, 120, 70), new Color(145, 55, 15), new Vector2[0]);
			this.CreateEquipmentData(0, 10, 2, 11, 1250, 45, 0, -10, 0, 0, 0, new Color(240, 235, 90), new Color(215, 135, 75), new Vector2[]
			{
				new Vector2(3f, 0.1f)
			});
			this.CreateEquipmentData(1, 10, 3, 11, 1250, 45, 0, -10, 0, 0, 0, new Color(240, 235, 90), new Color(215, 135, 75), new Vector2[]
			{
				new Vector2(3f, 0.1f)
			});
			this.CreateEquipmentData(4, 10, 3, 11, 1250, 75, 0, 0, 0, 0, 0, new Color(210, 240, 75), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(3f, 0.1f)
			});
			this.CreateEquipmentData(3, 10, 2, 11, 1250, 45, 0, -10, 0, 0, 0, new Color(235, 220, 135), new Color(235, 220, 135), new Vector2[]
			{
				new Vector2(3f, 0.1f)
			});
			this.CreateEquipmentData(2, 10, 3, 11, 1250, 45, 0, -10, 0, 0, 0, new Color(240, 235, 90), new Color(235, 135, 75), new Vector2[]
			{
				new Vector2(3f, 0.1f)
			});
			this.CreateEquipmentData(0, 18, 0, 2, 1450, 55, 0, 14, 12, 0, 0, new Color(255, 190, 45), new Color(240, 230, 0), new Vector2[0]);
			this.CreateEquipmentData(1, 15, 0, 2, 1200, 45, 20, 0, 18, 20, 0, new Color(255, 190, 45), new Color(240, 230, 0), new Vector2[0]);
			this.CreateEquipmentData(4, 19, 2, 2, 1600, 40, 0, 0, 27, 0, 0, new Color(255, 190, 45), new Color(0, 0, 0), new Vector2[0]);
			this.CreateEquipmentData(3, 15, 0, 2, 1400, 45, 0, 0, 18, 0, 10, new Color(255, 190, 45), new Color(240, 230, 0), new Vector2[0]);
			this.CreateEquipmentData(2, 15, 3, 2, 1875, 80, 0, 0, 70, 0, 0, new Color(255, 190, 45), new Color(240, 230, 0), new Vector2[0]);
			this.CreateEquipmentData(0, 21, 2, 7, 1700, 45, 0, 27, 0, 0, 0, new Color(170, 255, 250), new Color(255, 245, 255), new Vector2[0]);
			this.CreateEquipmentData(1, 18, 0, 7, 1500, 40, 40, 0, 0, 45, 0, new Color(115, 175, 185), new Color(255, 245, 255), new Vector2[0]);
			this.CreateEquipmentData(4, 23, 2, 7, 2100, 55, 0, 0, 0, 0, 0, new Color(170, 255, 250), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(9f, 2f)
			});
			this.CreateEquipmentData(3, 18, 0, 7, 1800, 50, 0, 0, 0, 0, 32, new Color(255, 245, 255), new Color(255, 245, 255), new Vector2[0]);
			this.CreateEquipmentData(2, 18, 0, 7, 1650, 65, 0, 0, 48, 0, 0, new Color(115, 175, 185), new Color(170, 255, 250), new Vector2[0]);
			this.CreateEquipmentData(0, 20, 3, 12, 1600, 50, 0, 15, -15, 0, 0, new Color(60, 60, 60), new Color(30, 30, 30), new Vector2[]
			{
				new Vector2(4f, 0.5f)
			});
			this.CreateEquipmentData(1, 20, 3, 12, 1400, 50, 30, 0, -15, 30, 0, new Color(90, 90, 90), new Color(30, 30, 30), new Vector2[]
			{
				new Vector2(4f, 0.5f)
			});
			this.CreateEquipmentData(4, 20, 2, 12, 1800, 35, 0, 0, -15, 0, 0, new Color(85, 15, 5), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(4f, 0.5f)
			});
			this.CreateEquipmentData(3, 20, 3, 12, 1500, 50, 0, 0, -15, 0, 15, new Color(30, 30, 30), new Color(90, 90, 90), new Vector2[]
			{
				new Vector2(4f, 0.5f)
			});
			this.CreateEquipmentData(2, 20, 2, 12, 1850, 50, 30, 0, -15, 30, 0, new Color(90, 90, 90), new Color(30, 30, 30), new Vector2[]
			{
				new Vector2(4f, 0.5f)
			});
			this.CreateEquipmentData(0, 27, 2, 3, 2300, 55, 0, 36, 0, 0, 0, new Color(240, 230, 0), new Color(240, 230, 0), new Vector2[0]);
			this.CreateEquipmentData(1, 25, 0, 3, 1900, 50, 55, 0, 0, 40, 0, new Color(115, 100, 190), new Color(240, 230, 0), new Vector2[0]);
			this.CreateEquipmentData(4, 29, 2, 3, 2600, 55, 0, 0, 0, 0, 0, new Color(55, 0, 145), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(2f, 0.5f)
			});
			this.CreateEquipmentData(3, 25, 0, 3, 2250, 55, 0, 0, 0, 0, 36, new Color(115, 100, 190), new Color(50, 55, 210), new Vector2[0]);
			this.CreateEquipmentData(2, 25, 2, 3, 3000, 75, 0, 0, 59, 0, 0, new Color(115, 100, 190), new Color(50, 55, 210), new Vector2[0]);
			this.CreateEquipmentData(0, 31, 2, 8, 2800, 75, 0, 46, 0, 0, 0, new Color(200, 0, 0), new Color(50, 50, 50), new Vector2[0]);
			this.CreateEquipmentData(1, 29, 0, 8, 2400, 60, 75, 0, 0, 60, 0, new Color(50, 50, 50), new Color(120, 0, 0), new Vector2[0]);
			this.CreateEquipmentData(4, 34, 3, 8, 3200, 75, 0, 0, 0, 0, 0, new Color(255, 0, 0), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(7f, 2f)
			});
			this.CreateEquipmentData(3, 29, 0, 8, 2600, 60, 0, 0, 0, 0, 42, new Color(50, 50, 50), new Color(50, 50, 50), new Vector2[0]);
			this.CreateEquipmentData(2, 29, 2, 8, 3750, 85, 0, 0, 78, 0, 0, new Color(160, 0, 0), new Color(80, 0, 0), new Vector2[0]);
			this.CreateEquipmentData(0, 30, 3, 13, 3200, 55, 0, 20, 0, 25, 0, new Color(215, 245, 70), new Color(255, 255, 255), new Vector2[]
			{
				new Vector2(8f, 1f)
			});
			this.CreateEquipmentData(1, 30, 3, 13, 2800, 55, 25, 0, 0, 50, 0, new Color(215, 245, 70), new Color(255, 255, 255), new Vector2[]
			{
				new Vector2(8f, 1f)
			});
			this.CreateEquipmentData(4, 30, 3, 13, 3300, 65, 0, 0, 0, 25, 0, new Color(215, 245, 70), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(8f, 1f)
			});
			this.CreateEquipmentData(3, 30, 3, 13, 3000, 100, 0, 0, 0, 25, 72, new Color(215, 245, 70), new Color(255, 255, 255), new Vector2[]
			{
				new Vector2(8f, 1f)
			});
			this.CreateEquipmentData(2, 30, 2, 13, 3800, 65, 0, 0, 40, 25, 0, new Color(215, 245, 70), new Color(255, 255, 255), new Vector2[]
			{
				new Vector2(8f, 1f)
			});
			this.CreateEquipmentData(0, 40, 3, 4, 4250, 85, 0, 60, 0, 0, 0, new Color(20, 60, 255), new Color(255, 40, 40), new Vector2[0]);
			this.CreateEquipmentData(1, 38, 0, 4, 3750, 70, 90, 0, 0, 90, 0, new Color(180, 180, 180), new Color(110, 100, 240), new Vector2[0]);
			this.CreateEquipmentData(4, 43, 2, 4, 4100, 80, 0, 0, 0, 0, 0, new Color(255, 255, 255), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(15f, 3f)
			});
			this.CreateEquipmentData(3, 38, 0, 4, 3800, 85, 0, 0, 0, 0, 60, new Color(150, 150, 150), new Color(120, 120, 120), new Vector2[0]);
			this.CreateEquipmentData(2, 38, 2, 4, 4500, 125, 0, 0, 100, 0, 0, new Color(120, 120, 120), new Color(105, 80, 240), new Vector2[0]);
			this.CreateEquipmentData(0, 52, 2, 9, 4750, 75, 0, 40, 0, 0, 0, new Color(125, 125, 125), new Color(60, 130, 70), new Vector2[]
			{
				new Vector2(1f, 0.04f),
				new Vector2(2f, 0.15f),
				new Vector2(4f, 0.5f)
			});
			this.CreateEquipmentData(1, 49, 2, 9, 3900, 65, 40, 0, 0, 40, 0, new Color(255, 255, 255), new Color(70, 70, 70), new Vector2[]
			{
				new Vector2(1f, 0.04f),
				new Vector2(2f, 0.15f),
				new Vector2(4f, 0.5f)
			});
			this.CreateEquipmentData(4, 57, 2, 9, 5750, 85, 0, 0, 0, 0, 0, new Color(255, 250, 120), new Color(0, 0, 0), new Vector2[]
			{
				new Vector2(1f, 0.04f),
				new Vector2(2f, 0.15f),
				new Vector2(4f, 0.5f)
			});
			this.CreateEquipmentData(3, 46, 2, 9, 4600, 60, 0, 0, 0, 0, 40, new Color(255, 255, 255), new Color(70, 70, 70), new Vector2[]
			{
				new Vector2(1f, 0.04f),
				new Vector2(2f, 0.15f),
				new Vector2(4f, 0.5f)
			});
			this.CreateEquipmentData(2, 46, 2, 9, 6750, 140, 0, 0, 50, 0, 0, new Color(255, 255, 255), new Color(70, 70, 70), new Vector2[]
			{
				new Vector2(1f, 0.04f),
				new Vector2(2f, 0.15f),
				new Vector2(4f, 0.5f)
			});
			this.CreateEquipmentData(0, 50, 3, 14, 6000, 100, 0, 50, 0, 0, 0, new Color(15, 15, 15), new Color(15, 15, 15), new Vector2[]
			{
				new Vector2(7f, 1f),
				new Vector2(8f, 1f),
				new Vector2(11f, 1f)
			});
			this.CreateEquipmentData(1, 50, 3, 14, 6000, 100, 60, 0, 0, 60, 0, new Color(45, 45, 45), new Color(15, 15, 15), new Vector2[]
			{
				new Vector2(7f, 1f),
				new Vector2(8f, 1f),
				new Vector2(9f, 1f)
			});
			this.CreateEquipmentData(4, 50, 3, 14, 6000, 100, 0, 0, 0, 0, 0, new Color(15, 15, 15), new Color(15, 15, 15), new Vector2[]
			{
				new Vector2(7f, 1f),
				new Vector2(8f, 1f),
				new Vector2(9f, 1f)
			});
			this.CreateEquipmentData(3, 50, 3, 14, 6000, 100, 0, 0, 0, 0, 50, new Color(45, 45, 45), new Color(45, 45, 45), new Vector2[]
			{
				new Vector2(7f, 1f),
				new Vector2(8f, 1f),
				new Vector2(9f, 1f)
			});
			this.CreateEquipmentData(2, 50, 3, 14, 6000, 100, 50, 0, 0, 0, 0, new Color(15, 15, 15), new Color(15, 15, 15), new Vector2[]
			{
				new Vector2(7f, 1f),
				new Vector2(8f, 1f),
				new Vector2(11f, 1f)
			});
		}
		private void CreateEquipmentData(int equipmentType, byte levelRequirement, byte chestColourRequirement, int equipmentIndex, int cost, int weight, int bonusHealth, int bonusDamage, int BonusArmor, int bonusMana, int bonusMagic, Color firstColour, Color secondColour, params Vector2[] secondaryAttributes)
		{
			EquipmentData equipmentData = new EquipmentData();
			equipmentData.BonusDamage = bonusDamage;
			equipmentData.BonusHealth = bonusHealth;
			equipmentData.BonusArmor = BonusArmor;
			equipmentData.BonusMana = bonusMana;
			equipmentData.BonusMagic = bonusMagic;
			equipmentData.Weight = weight;
			equipmentData.FirstColour = firstColour;
			equipmentData.SecondColour = secondColour;
			equipmentData.Cost = cost;
			equipmentData.LevelRequirement = levelRequirement;
			equipmentData.ChestColourRequirement = chestColourRequirement;
			equipmentData.SecondaryAttribute = new Vector2[secondaryAttributes.Length];
			for (int i = 0; i < secondaryAttributes.Length; i++)
			{
				equipmentData.SecondaryAttribute[i] = secondaryAttributes[i];
			}
			this.m_equipmentDataArray[equipmentType][equipmentIndex] = equipmentData;
		}
		public EquipmentData GetEquipmentData(int categoryType, int equipmentIndex)
		{
			return this.m_equipmentDataArray[categoryType][equipmentIndex];
		}
		public int GetAbilityCost(int categoryType, int itemIndex)
		{
			return (int)((float)this.m_abilityCostArray[categoryType][itemIndex] + (float)Game.PlayerStats.TotalRunesPurchased * this.AbilityCostMod);
		}
		public int GetBaseAbilityCost(int categoryType, int itemIndex)
		{
			return this.m_abilityCostArray[categoryType][itemIndex];
		}
		public void SetBlueprintState(byte state)
		{
			foreach (byte[] current in Game.PlayerStats.GetBlueprintArray)
			{
				for (int i = 0; i < current.Length; i++)
				{
					if (current[i] < state)
					{
						current[i] = state;
					}
				}
			}
			foreach (byte[] current2 in Game.PlayerStats.GetRuneArray)
			{
				for (int j = 0; j < current2.Length; j++)
				{
					if (current2[j] < state)
					{
						current2[j] = state;
					}
				}
			}
		}
	}
}
