namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Maps
{
	public class ExitPoint : MonoBehEx
	{
		[SerializeField]	GlobalRef<EnterPoint>	_point;
	
		[Callable]	void	Exit	( ) => this.GetService<Service_SimpleScene>().GoToMapAtPoint(_point).Forget();
	}
}