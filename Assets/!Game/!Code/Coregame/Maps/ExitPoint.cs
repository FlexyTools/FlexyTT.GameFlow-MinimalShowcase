namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Maps
{
	public class ExitPoint : MonoBehEx
	{
		[SerializeField]	GlobalRef<EnterPoint>	_point;
	
		[Callable]	void	Go	( )
		{
			this.GetService<Stage_Coregame>().GoToMapPoint(_point);
		}
	}
}