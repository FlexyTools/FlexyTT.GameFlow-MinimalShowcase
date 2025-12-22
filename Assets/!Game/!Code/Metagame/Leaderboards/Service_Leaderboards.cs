namespace FlexyTT.GameFlow_MinimalShowcase.Metagame.Leaderboards
{
	public class Service_Leaderboards : MonoBehaviour, IService
	{
		public void OrderedInit( GameContext ctx )
		{
			Load();
		}
	
		public	BoardData	Board_01	{get; private set;} = null!;
		public	BoardData	Board_02	{get; private set;} = null!;
		public	BoardData	Board_03	{get; private set;} = null!;

		public	void		AddRecord	( Byte map, Single score )	
		{
			var board = map switch
			{
				1 => Board_01,
				2 => Board_02,
				3 => Board_03,
			};
			
			board.Records.Add( score );
			board.Records.Sort();
			
			while (board.Records.Count > 7)
				board.Records.RemoveAt( board.Records.Count-1 );
			
			Save();
		}
		[ContextMenu("Save")]
		public	void		Save		( )	
		{ 
			PlayerPrefs.SetString( "Leaderboard_3x3", JsonUtility.ToJson( Board_01 ) );
			PlayerPrefs.SetString( "Leaderboard_4x4", JsonUtility.ToJson( Board_02 ) );
			PlayerPrefs.SetString( "Leaderboard_5x5", JsonUtility.ToJson( Board_03 ) );
			
			PlayerPrefs.Save( );
		}
		[ContextMenu("Load")]
		public	void		Load		( )	
		{
			Board_01 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_3x3", "{}" ) );
			Board_02 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_4x4", "{}" ) );
			Board_03 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_5x5", "{}" ) );
			
			while (Board_01.Records.Count < 7) Board_01.Records.Add( Single.PositiveInfinity );
			while (Board_02.Records.Count < 7) Board_02.Records.Add( Single.PositiveInfinity );
			while (Board_03.Records.Count < 7) Board_03.Records.Add( Single.PositiveInfinity );
		}
		
		[Serializable]
		public class BoardData
		{
			[SerializeField] List<Single> _records = new();

			public List<Single> Records => _records;
		}
	}
}