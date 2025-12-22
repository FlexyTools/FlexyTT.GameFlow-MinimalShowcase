namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Kit
{
	public class GameMode_FindExit : MonoBehEx
	{
		public		Single		StartTime	{ get; set; }
		public		Single		RunTime		=> Time.time - StartTime;
		public		Boolean		IsWin		{ get; set; }
		
		public		Single		Result		{ get; private set; }

		private		void		Update		( )		
		{
			#if UNITY_EDITOR
			if (Keyboard.current.wKey.wasPressedThisFrame)
				Win();
			#endif
		}
		public		void		Win			( )		
		{
			IsWin = true;
			Result = RunTime;
		} 
	}
}