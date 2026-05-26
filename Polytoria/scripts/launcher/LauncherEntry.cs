namespace Polytoria.scripts.launcher;

using Godot;

public sealed partial class LauncherEntry : Node3D
{
	public LauncherEntry()
	{
	}


	public async void Entry(LauncherEntryData? data = null)
	{

	}

	public struct LauncherEntryData
	{
		public string? retunData;
		public string? Token;
	}

}
