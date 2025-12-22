using Flexy.Core.Extensions;
using FlexyTT.GameFlow_MinimalShowcase.Coregame.Play;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Mode
{
	public class GameMode_FindExit : MonoBehEx
	{
		[SerializeField]	AssetRef<Mob_Player>	_playerMobRef;
	
		private Mob_Player?	_playerMob;
	
		public		Single		StartTime	{ get; set; }
		public		Single		RunTime		=> Time.time - StartTime;
		public		Boolean		IsWin		{ get; set; }
		
		public		Single		Result		{ get; private set; }
		public		Mob_Player?	PlayerMob	=> _playerMob;

		public		void		StartPlay	( )		
		{
			var playerMobPrefab = _playerMobRef.LoadAssetSync()!;
			_playerMob = playerMobPrefab.InstantiateInactive();
			
			var spawnPoint = this.GetService<MapContext>().GetRandomSpawnPoint();
			_playerMob.transform.position = spawnPoint.position;
			_playerMob.transform.rotation = spawnPoint.rotation;
			_playerMob.gameObject.SetActive(true);
		}

		private		void		Update		( )		
		{
			#if UNITY_EDITOR
			if (Keyboard.current.wKey.wasPressedThisFrame)
				Win();
			#endif
		}
		public		void		Win			( )		
		{
			IsWin = true;
			Result = RunTime;
		} 
		
		[Callable]	void		Escape		( )		
		{
			Win();
		}
	}
}