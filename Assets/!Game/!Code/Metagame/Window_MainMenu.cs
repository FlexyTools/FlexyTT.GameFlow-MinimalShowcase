namespace FlexyTT.GameFlow_MinimalShowcase.Metagame
{
	public class Window_MainMenu : UIWindowEx
	{
		[SerializeField]	SceneRef	_map_Arena;
		[SerializeField]	SceneRef	_map_RoomBased;
	
		protected override	Boolean	TryGoBack	( )		=> false;

		[Callable]	void	Play_Arena			( )		=> Game.Flow.PlayMap( _map_Arena );
		[Callable]	void	Play_RoomBased		( )		=> Game.Flow.PlayMap( _map_RoomBased );

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