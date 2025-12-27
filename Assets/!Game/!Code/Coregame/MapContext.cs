namespace FlexyTT.GameFlow_MinimalShowcase.Coregame
{
	public class	MapContext : GameContext
	{
		[SerializeField]	Transform			_spawnPointsRoot	= null!;

		public	Transform			SpawnPointsRoot		=> _spawnPointsRoot;

		public	Transform			GetRandomSpawnPoint	( )		
		{
			if (_spawnPointsRoot == null || _spawnPointsRoot.childCount == 0)
				return _spawnPointsRoot!;

			var randomIndex = Random.Range(0, _spawnPointsRoot.childCount);
			return _spawnPointsRoot.GetChild(randomIndex);
		}
	}
}