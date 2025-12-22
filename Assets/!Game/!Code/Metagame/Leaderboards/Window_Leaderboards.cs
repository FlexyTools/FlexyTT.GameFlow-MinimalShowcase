using System.Linq;
using FlexyTT.GameFlow_MinimalShowcase.Common;

namespace FlexyTT.GameFlow_MinimalShowcase.Metagame.Leaderboards
{
	public class Window_Leaderboards : UIWindowEx
	{
		[Bindable]	Collection	Records3x3 	=> new (Game.Leaderboards.Board_01.Records.Select(s => new ScoreView(s)));
		[Bindable]	Collection	Records4x4 	=> new (Game.Leaderboards.Board_02.Records.Select(s => new ScoreView(s)));
		[Bindable]	Collection	Records5x5 	=> new (Game.Leaderboards.Board_03.Records.Select(s => new ScoreView(s)));
		
		[Bindable]	Boolean		ExitToRight	=> OpenParams is not null;
		[Bindable]	Boolean		IsShowBoard	( Byte map )	=> OpenParams is not Byte f || f == map;
		
		[StateTest]	Object		Board_01	( ) => (Byte)1;
		[StateTest]	Object		Board_02	( ) => (Byte)2;
		[StateTest]	Object		Board_03	( ) => (Byte)3;
		
		private record ScoreView(Single Score)
		{
			[Bindable]	Single	Score		{get;init;} = Score;
			[Bindable]	String	ScoreStr	=> Single.IsPositiveInfinity(Score) ? "-" : TimeSpan.FromSeconds( Score ).ToString( Score >= 60 ? @"mm\:ss\.ff" : @"ss\.ff" );
		}
		
		public new record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	FlowNode	Open	( )				=> Ctx.Open();
			public	FlowNode	Open	( Byte map )	=> Ctx.Open(map);
		}
	}
}