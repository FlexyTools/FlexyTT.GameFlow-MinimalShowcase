using FlexyTT.GameFlow_MinimalShowcase.Coregame.Mode;
using FlexyTT.GameFlow_MinimalShowcase.Coregame.Play;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame
{
	// Visually this state is Coregame HUD
    public class State_Play : StateEx
    {
		[Bindable]	String	RunMinutes		=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"mm" );
		[Bindable]	String	RunSeconds		=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"ss" );
		[Bindable]	String	RunMilliseconds	=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"ff" );

		private GameMode_Escape _gameMode = null!;

		protected override void		OnShow		( )		
		{
			if (OpenParams is SceneRef sceneRef)
			{
				// We started from test scene because runtime flow dont have Params at all for this State
				// So close to GameStage with replay request to load requested map
				GameStage.CloseSubStates(true, (true, sceneRef));
				return;
			}
		
			_gameMode = gameObject.GetService<GameMode_Escape>();
		}
		protected override	void	OnHide		( )		
		{
			if (_gameMode.PlayerMob is {} mob)
				mob.ResetInput();
		}
		protected override Boolean	TryGoBack	( )		
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
				GameStage.CloseSubStates(true);
				Game.States.PlayComplete.Open(_gameMode.EscapeTime);
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
        
        [StateTest]	Object	Play_3x3 	( ) => new SceneRef("1afdd18b2b9c65e4b866284337ba9044");
        [StateTest]	Object	Play_4x4 	( ) => new SceneRef("6bfc812ba38db7b47bc55722377eb3a5");
        [StateTest]	Object	Play_5x5 	( ) => new SceneRef("3fb4186ae0d813b44b8097bf7a82b451");
    }
}