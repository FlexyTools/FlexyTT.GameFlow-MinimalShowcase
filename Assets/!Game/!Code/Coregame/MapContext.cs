namespace FlexyTT.GameFlow_MinimalShowcase.Coregame
{
	public class	MapContext : GameContext
	{
		[SerializeField]	Transform	_spawnPointsRoot	= null!;
		
		[Header("Editor Data")]
		[SerializeField]	Int32		_debugExitsCount;
		

		public	Transform	SpawnPointsRoot		=> _spawnPointsRoot;
		public	Int32		DebugExitsCount		=> _debugExitsCount;

		public	Transform	GetRandomSpawnPoint	( )		
		{
			if (_spawnPointsRoot == null || _spawnPointsRoot.childCount == 0)
				return _spawnPointsRoot!;

			var randomIndex = Random.Range(0, _spawnPointsRoot.childCount);
			return _spawnPointsRoot.GetChild(randomIndex);
		}
	}
}