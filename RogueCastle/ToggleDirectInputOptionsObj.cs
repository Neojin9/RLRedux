using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class ToggleDirectInputOptionsObj : OptionsObj
	{
		private TextObj m_toggleText;
		public override bool IsActive
		{
			get
			{
				return base.IsActive;
			}
			set
			{
				base.IsActive = value;
				if (value)
				{
					this.m_toggleText.TextureColor = Color.Yellow;
					return;
				}
				this.m_toggleText.TextureColor = Color.White;
			}
		}
		public ToggleDirectInputOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Use DInput Gamepads")
		{
			this.m_toggleText = (this.m_nameText.Clone() as TextObj);
			this.m_toggleText.X = (float)this.m_optionsTextOffset;
			this.m_toggleText.Text = "No";
			this.AddChild(this.m_toggleText);
		}
		public override void Initialize()
		{
			if (InputManager.UseDirectInput)
			{
				this.m_toggleText.Text = "Yes";
			}
			else
			{
				this.m_toggleText.Text = "No";
			}
			base.Initialize();
		}
		public override void HandleInput()
		{
			if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21) || Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
			{
				SoundManager.PlaySound("frame_swap");
				if (this.m_toggleText.Text == "No")
				{
					this.m_toggleText.Text = "Yes";
				}
				else
				{
					this.m_toggleText.Text = "No";
				}
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
			{
				SoundManager.PlaySound("Option_Menu_Select");
				if (this.m_toggleText.Text == "No")
				{
					InputManager.UseDirectInput = false;
					Game.GameConfig.EnableDirectInput = false;
				}
				else
				{
					InputManager.UseDirectInput = true;
					Game.GameConfig.EnableDirectInput = true;
				}
				this.IsActive = false;
			}
			if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				if (InputManager.UseDirectInput)
				{
					this.m_toggleText.Text = "Yes";
				}
				else
				{
					this.m_toggleText.Text = "No";
				}
				this.IsActive = false;
			}
			base.HandleInput();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_toggleText = null;
				base.Dispose();
			}
		}
	}
}
