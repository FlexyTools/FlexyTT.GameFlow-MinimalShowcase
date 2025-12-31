namespace FlexyTT.GameFlow_MinimalShowcase.Metagame.Leaderboards
{
	public class Service_Leaderboards : MonoBehaviour, IService
	{
		public void OrderedInit( GameContext ctx )
		{
			Load();
		}
	
		public	BoardData	Board_Arena		{get; private set;} = null!;
		public	BoardData	Board_RoomBased	{get; private set;} = null!;
		
		public	void		AddRecord	( Boolean isArena, Single score )	
		{
			var board = isArena ? Board_Arena : Board_RoomBased;
			
			board.Records.Add( score );
			board.Records.Sort();
			
			while (board.Records.Count > 7)
				board.Records.RemoveAt( board.Records.Count-1 );
			
			Save();
		}
		[ContextMenu("Save")]
		public	void		Save		( )	
		{ 
			PlayerPrefs.SetString( "Leaderboard_3x3", JsonUtility.ToJson( Board_Arena ) );
			PlayerPrefs.SetString( "Leaderboard_4x4", JsonUtility.ToJson( Board_RoomBased ) );
			
			PlayerPrefs.Save( );
		}
		[ContextMenu("Load")]
		public	void		Load		( )	
		{
			Board_Arena		= JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_Arena", "{}" ) );
			Board_RoomBased	= JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_RoomBased", "{}" ) );
			
			while (Board_Arena.Records.Count < 7)		Board_Arena		.Records.Add( Single.PositiveInfinity );
			while (Board_RoomBased.Records.Count < 7)	Board_RoomBased	.Records.Add( Single.PositiveInfinity );
		}
		
		[Serializable]
		public class BoardData
		{
			[SerializeField] List<Single> _records = new();

			public List<Single> Records => _records;
		}
	}
}