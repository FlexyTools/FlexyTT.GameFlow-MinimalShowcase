using Flexy.UI.Extra;

namespace FlexyTT.GameFlow_MinimalShowcase.Play
{
	public class State_Escaped : StateEx
    {
        [Bindable]		Single		Seconds				=> (Single)OpenParams!;
        [Bindable]		String		FormattedSeconds	=> Seconds >= 60 ? TimeSpan.FromSeconds( Seconds ).ToString( @"mm\:ss\.ff" ) : TimeSpan.FromSeconds( Seconds ).ToString( @"ss\.ff" );

        protected override	Boolean	TryGoBack	( )		=> false;

        [Callable]			void	Continue	( )		=> Close();

        public new record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	FlowNode	Open	( Single seconds ) => Ctx.Open( seconds );
		}
		
		[StateTest]		Object	Score_98	( ) => 98.1f;
		[StateTest]		Object	Score_23	( ) => 23.5f;
		[StateTest]		Object	Score_052	( ) => 0.52f; 
    }
}