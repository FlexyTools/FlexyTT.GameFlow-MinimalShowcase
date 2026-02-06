using System.Linq;

namespace FlexyTT.GameFlow_MinimalShowcase.Menu.Leaderboards
{
	public class Window_Leaderboards : UIWindowEx
	{
		[Bindable]	Collection	RecordsArena 	=> new (Game.Leaderboards.Board_Arena		.Records.Select(s => new ScoreView(s)));
		[Bindable]	Collection	RecordsRoomBased=> new (Game.Leaderboards.Board_RoomBased	.Records.Select(s => new ScoreView(s)));
		
		[Bindable]	Boolean		ExitToRight		=> OpenParams is not null;
		[Bindable]	Boolean		IsShowBoard		( Boolean isArena )	=> OpenParams is not Boolean b || b == isArena;
		
		[StateTest]	Object		Board_Arena		( ) => true;
		[StateTest]	Object		Board_RoomBased	( ) => false;
		
		private record ScoreView(Single Score)
		{
			[Bindable]	Single	Score		{get;init;} = Score;
			[Bindable]	String	ScoreStr	=> Single.IsPositiveInfinity(Score) ? "-" : TimeSpan.FromSeconds( Score ).ToString( Score >= 60 ? @"mm\:ss\.ff" : @"ss\.ff" );
		}
		
		public new record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	FlowNode	Open	( )					=> Ctx.Open();
			public	FlowNode	Open	( Boolean isArena )	=> Ctx.Open(isArena);
		}
	}
}