namespace FlexyTT.GameFlow_MinimalShowcase.Metagame
{
	public class Window_MainMenu : UIWindowEx
	{
		[SerializeField]	SceneRef	_map_01;
		[SerializeField]	SceneRef	_map_02;
		[SerializeField]	SceneRef	_map_03;
	
		protected override	Boolean	TryGoBack	( )		=> false;

		[Callable]	void	Play_Map_01			( )		=> Game.Flow.PlayMap( _map_01 );
		[Callable]	void	Play_Map_02			( )		=> Game.Flow.PlayMap( _map_02 );
		[Callable]	void	Play_Map_03			( )		=> Game.Flow.PlayMap( _map_03 );

		[Callable]	void	OpenSettings		( )		
		{
			Game.UI.Settings.Open( );
		}
		[Callable]	void	ExitGame			( )		
		{
			if( !Application.isEditor )
			{
				Application.Quit( );
			}
			else
			{
	#if UNITY_EDITOR
				UnityEditor.EditorApplication.ExitPlaymode( );
	#endif
			}
		}
		
		[Callable]	void	OpenAppInfo			( )		
		{
			Game.UI.AppInfo.Open( );
		}
	}
}