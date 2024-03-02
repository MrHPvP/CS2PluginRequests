using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Timers;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace WardenTimer;
public partial class WardenTimer : BasePlugin
{
	public override string ModuleName => "WardenTimer";
	public override string ModuleAuthor => "MrH_PvP";
	public override string ModuleVersion => "1.0.0";
	Timer timewillcatchuptousoneday;

	[GameEventHandler]
	public HookResult whenroundhasreacheditslimitforplayingwiththesepeople(EventRoundEnd @event, GameEventInfo info)
	{
		timewillcatchuptousoneday.Kill();
		return HookResult.Continue;
	}

	[ConsoleCommand("css_time", "creates a timer so the warden knows how long he can ramble on for")]
	[CommandHelper(1, "timernumbers", CommandUsage.CLIENT_AND_SERVER)]
	public void getthisterriblewardenoutoffheretimer(CCSPlayerController? caller, CommandInfo args)
	{
		var timeywhymy = 0;
		if (caller is null) return;
		if (!caller.PlayerName.Contains("Warden"))
		{
			args.ReplyToCommand($"come back when your the warden pleb.");
		}
		if (!int.TryParse(args.ArgByIndex(1), out timeywhymy))
		{
			args.ReplyToCommand($"You need to specify a number");
		}
		timewillcatchuptousoneday = AddTimer(1f, () =>
		{
			var allplayers = Utilities.GetPlayers();
			timeywhymy -= 1;
			foreach (var player in allplayers)
			{
				TimeSpan time = TimeSpan.FromSeconds(timeywhymy);
				string str = time.ToString(@"hh\:mm\:ss\:fff");
				player.PrintToCenter($"you have {time} left");
				if (timeywhymy < 1)
				{
					player.PrintToCenter("The Wardens timer has ended!");
					timewillcatchuptousoneday.Kill();
				}
			}
		}, TimerFlags.REPEAT);
	}
}
