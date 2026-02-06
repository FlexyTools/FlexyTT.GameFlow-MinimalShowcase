using FlexyTT.GameFlow_MinimalShowcase.Menu;

namespace FlexyTT.GameFlow_MinimalShowcase.UIKit
{
	public class Widget_ColoredBorder : UIWidgetEx
	{
		[SerializeField]	Color			_tintColor		= Color.white;
		
		private		SettingsTab_Color		_colorSettings	= null!;
		
		[Bindable]	Color	BorderColor		=> (_colorSettings ??= Game.Settings.Color).Primary * _tintColor;
	
		private		void	OnEnable		( ) => (_colorSettings ??= Game.Settings.Color).Primary.Changed += RebindColor;
		private		void	OnDisable		( ) => _colorSettings.Primary.Changed -= RebindColor;

		private		void	RebindColor		( Color32 color ) => RebindProperty( "BorderColor" );
	}
}