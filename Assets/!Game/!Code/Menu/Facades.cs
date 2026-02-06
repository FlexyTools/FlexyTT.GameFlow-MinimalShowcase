// ReSharper disable AccessToStaticMemberViaDerivedType

using FlexyTT.GameFlow_MinimalShowcase.Menu.Leaderboards;
using FlexyTT.GameFlow_MinimalShowcase.Settings;

namespace FlexyTT.GameFlow_MinimalShowcase.Menu;

public struct	Facade_Game : ICachedContext
{
	public	GameContext				Ctx				{ get; set; }
	public	Component				CallSource		{ get; set; }

	public	Facade_Flow				Flow			=> new( Ctx.GetService<GameStage_Menu>() );
	public	Facade_UIWindows		UI				=> new( Ctx.GetService<GameStage>() );
	public	Service_Audio			Audio			=> Ctx.GetService<Service_Audio>();
	public	Facade_GameSettings		Settings		=> new( Ctx.GetService<Service_GameSettings>() );
	public	Service_Leaderboards	Leaderboards	=> Ctx.GetService<Service_Leaderboards>();
}

public record struct	Facade_GameSettings ( Service_GameSettings Svc )
{
	public	SettingsTab_Audio		Audio		=> Svc.Get<SettingsTab_Audio>();
	public	SettingsTab_Color		Color		=> Svc.Get<SettingsTab_Color>();
}

public readonly record struct	Facade_Flow	( GameStage_Menu Meta )
{
	public	void	PlayMap	( params SceneRef[] maps )	=> Meta.Play_Map(maps).Forget();
}

public readonly record struct	Facade_UIWindows	( LibCtx LibCtx )
{
	public Window_GameSettings		.Opener		Settings		=> LibCtx.GetState<Window_GameSettings>();
	public Window_GameSettings		.Opener		SettingsOld		=> LibCtx.GetState<Window_GameSettings>("59f65f829106a2b418bdb7eefd672868");
		
	public Window_AppInfo			.Opener		AppInfo			=> LibCtx.GetState<Window_AppInfo>();
	public Window_Leaderboards		.Opener		Leaderboards	=> LibCtx.GetOpener<Window_Leaderboards.Opener>();
}