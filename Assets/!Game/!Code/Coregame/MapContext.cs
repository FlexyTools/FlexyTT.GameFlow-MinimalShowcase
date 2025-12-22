using FlexyTT.GameFlow_MinimalShowcase.Coregame.Mode;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame
{
	public class	MapContext : GameContext
	{
		[SerializeField]	GameMode_FindExit	_gameMode			= null!;
		[SerializeField]	Transform			_spawnPointsRoot	= null!;

		public	GameMode_FindExit	GameMode			=> _gameMode;
		public	Transform			SpawnPointsRoot		=> _spawnPointsRoot;

		public	Transform			GetRandomSpawnPoint	( )		
		{
			if (_spawnPointsRoot == null || _spawnPointsRoot.childCount == 0)
				return _spawnPointsRoot!;

			var randomIndex = Random.Range(0, _spawnPointsRoot.childCount);
			return _spawnPointsRoot.GetChild(randomIndex);
		}

		protected override void InitializeServices(IService[] services)
		{
			SetService(_gameMode);
		
			base.InitializeServices(services);
		}
	}
}