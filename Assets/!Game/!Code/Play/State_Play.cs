using FlexyTT.GameFlow_MinimalShowcase.Play.Mode;
using FlexyTT.GameFlow_MinimalShowcase.Play.Play;
using Button = FlexyTT.GameFlow_MinimalShowcase.Play.Controls.Button;

namespace FlexyTT.GameFlow_MinimalShowcase.Play
{
	using Controls_Button = Controls.Button;

	// Visually this state is Coregame HUD
	[DefaultExecutionOrder(-1)]
    public class State_Play : StateEx
    {
		[SerializeField]	Controls_Button	_forward	= null!;
		[SerializeField]	Controls_Button	_left		= null!;
		[SerializeField]	Controls_Button	_right		= null!;
    
		[Bindable]	String	RunMinutes		=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"mm" );
		[Bindable]	String	RunSeconds		=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"ss" );
		[Bindable]	String	RunMilliseconds	=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"ff" );

		private GameMode_Escape _gameMode = null!;

		protected override	UniTask	OnShow		( )		
		{
			_gameMode = gameObject.GetService<GameMode_Escape>();
			return default;
		}
		protected override	UniTask	OnHide		( )		
		{
			if (_gameMode.PlayerMob is {} mob)
				mob.ResetInput();
				
			return default;
		}
		protected override	Boolean	TryGoBack	( )		
		{
			Pause();
			return false;
		}

		private		void	Update				( )						
		{
			RebindAll();
			
			if (_gameMode.PlayerMob is {} mob)
				ControlMob(mob);
				
			if (_gameMode.IsEscaped)
			{
				GameGameStage.CloseSubStates(true);
				Game.States.Escaped.Open(_gameMode.EscapeTime);
			}
		}
		private		void	OnDisable			( )						
		{
			if (_gameMode.PlayerMob is {} mob)
				mob.ResetInput();
		}
		private		void	OnApplicationPause	( Boolean pauseStatus )	
		{
			if (!Application.isEditor)
				Game.States.Pause.Open();
		}
		private		void	ControlMob			( Mob_Player mob )		
		{
			mob.ResetInput();
		
			if (_forward.IsPressed)	mob.MoveForward();
			if (_right.IsPressed)	mob.RotateRight();
			if (_left.IsPressed)	mob.RotateLeft();
			
			if (Keyboard.current.wKey.isPressed) mob.MoveForward();
			if (Keyboard.current.sKey.isPressed) mob.MoveBackward();
			
			if (Keyboard.current.sKey.isPressed)
			{
				if (Keyboard.current.aKey.isPressed) mob.RotateRight();
				if (Keyboard.current.dKey.isPressed) mob.RotateLeft();
			}
			else
			{
				if (Keyboard.current.aKey.isPressed) mob.RotateLeft();
				if (Keyboard.current.dKey.isPressed) mob.RotateRight();
			}
		}
		
        [Callable]	void	Pause				( )		
        {
			Game.States.Pause.Open();
        }
    }
}