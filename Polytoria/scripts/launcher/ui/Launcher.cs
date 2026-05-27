using Godot;

namespace Polytoria.scripts.launcher.ui;

public partial class Launcher : Panel
{
	private PanelContainer _sideMenu = null!;
	private Control _mainLayout = null!;
	private Button _menuToggle = null!;
	private bool _isExpanded;
	private TextureRect _navbarLogo = null!;

	private const float ExpandedWidth = 250f;
	private const float RetractedWidth = 80f;
	private const float AnimTime = 0.3f;

	public override void _Ready()
	{
		_sideMenu = GetNode<PanelContainer>("%SideMenu");
		_mainLayout = GetNode<Control>("MainHorizontalLayout");
		_menuToggle = GetNode<Button>("%MenuToggle");

		_menuToggle.Pressed += OnToggleMenu;

		_sideMenu.OffsetRight = RetractedWidth;
		_mainLayout.OffsetLeft = RetractedWidth;
		UpdateLabelsAlpha(0.0f, null);
	}

	private void OnToggleMenu()
	{
		_isExpanded = !_isExpanded;

		float targetWidth = _isExpanded ? ExpandedWidth : RetractedWidth;
		float targetAlpha = _isExpanded ? 1.0f : 0.0f;

		Tween tween = CreateTween().SetParallel();

		tween.TweenProperty(_sideMenu, "offset_right", targetWidth, AnimTime)
			.SetTrans(Tween.TransitionType.Quart)
			.SetEase(Tween.EaseType.Out);

		tween.TweenProperty(_mainLayout, "offset_left", targetWidth, AnimTime)
			.SetTrans(Tween.TransitionType.Quart)
			.SetEase(Tween.EaseType.Out);

		UpdateLabelsAlpha(targetAlpha, tween);
	}

	private void UpdateLabelsAlpha(float targetAlpha, Tween? tween)
	{
		var sideVBox = GetNode("SideMenu/SidebarPadding/VBox");
		foreach (Node child in sideVBox.GetChildren())
		{
			if (child is Button btn) AnimateButtonLabel(btn, targetAlpha, tween);
			if (child is VBoxContainer navGroup)
			{
				foreach (Node navBtn in navGroup.GetChildren())
				{
					if (navBtn is Button b) AnimateButtonLabel(b, targetAlpha, tween);
				}
			}
		}
	}

	private void AnimateButtonLabel(Button btn, float targetAlpha, Tween? tween)
	{
		var label = btn.GetNodeOrNull<CanvasItem>("Label");
		if (label == null) return;

		if (tween != null)
			tween.TweenProperty(label, "modulate:a", targetAlpha, AnimTime * 0.4f);
		else
			label.Modulate = new Color(1, 1, 1, targetAlpha);
	}
}
