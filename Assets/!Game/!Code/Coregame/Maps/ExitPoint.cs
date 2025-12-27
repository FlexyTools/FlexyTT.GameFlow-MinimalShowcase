namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Maps
{
	public class ExitPoint : MonoBehEx
	{
		[SerializeField]	SceneRef				_map;
		[SerializeField]	GlobalRef<EnterPoint>	_point;
	
		[Callable]	void	Go	( )
		{
			this.GetService<Stage_Coregame>().GoToMap(_map, _point);
		}
	}
}