namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Mode
{
	public class EscapePoint : MonoBehEx
	{
		[Callable]	void	Escape	( ) => this.GetService<GameMode_Escape>().Escape();
	}
}