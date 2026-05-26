using Godot;
using System;

public partial class Launcher : Panel
{
	private PanelContainer _sideMenu;
	private Button _menuToggle;
	private bool _isExpanded = false;

	private const float ExpandedWidth = 250f;
	private const float RetractedWidth = 80f;
	private const float AnimTime = 0.3f;

	public override void _Ready()
	{
		_sideMenu = GetNode<PanelContainer>("%SideMenu");
		_menuToggle = GetNode<Button>("%MenuToggle");

		_menuToggle.Pressed += OnToggleMenu;

		_sideMenu.CustomMinimumSize = new Vector2(RetractedWidth, 0);
		UpdateLabelsAlpha(0.0f, null);
	}

	private void OnToggleMenu()
	{
		_isExpanded = !_isExpanded;

		float targetWidth = _isExpanded ? ExpandedWidth : RetractedWidth;
		float targetAlpha = _isExpanded ? 1.0f : 0.0f;

		Tween tween = CreateTween().SetParallel(true);

		tween.TweenProperty(_sideMenu, "custom_minimum_size",
				new Vector2(targetWidth, 0), AnimTime)
			.SetTrans(Tween.TransitionType.Quart)
			.SetEase(Tween.EaseType.Out);

		UpdateLabelsAlpha(targetAlpha, tween);
	}

	private void UpdateLabelsAlpha(float targetAlpha, Tween tween)
	{
		// UPDATED PATH: Added SidebarPadding to the hierarchy
		var sideVBox = GetNode("MainHorizontalLayout/SideMenu/SidebarPadding/VBox");
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

	private void AnimateButtonLabel(Button btn, float targetAlpha, Tween tween)
	{
		var label = btn.FindChild("Label", true) as CanvasItem;
		if (label == null) return;

		if (tween != null)
			tween.TweenProperty(label, "modulate:a", targetAlpha, AnimTime * 0.4f);
		else
			label.Modulate = new Color(1, 1, 1, targetAlpha);
	}
}
