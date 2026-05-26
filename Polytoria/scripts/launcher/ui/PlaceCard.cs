using Godot;
using System;

namespace Polytoria.scripts.launcher.ui
{
    // Changed to PanelContainer to match the fixed .tscn root
    public partial class PlaceCard : PanelContainer
    {
        [Export] public string TitleText { get; set; } = "Game Title";
        [Export] public string VersionText { get; set; } = "2.0";
        [Export] public Texture2D Thumbnail { get; set; } = null!;
        [Export] public int Players { get; set; } = 0;
        [Export] public int LikePercent { get; set; } = 0;

        // Signal to let the main menu know this game was picked
        [Signal] public delegate void PlayPressedEventHandler();

        private Label _title = null!;
        private Label _badge = null!;
        private TextureRect _thumb = null!;
        private Label _playerCount = null!;
        private Label _likePercent = null!;
        private Button _playButton = null!;

        public override void _Ready()
        {
            // 1. Assign nodes using paths that match the fixed VBox/HBox structure
            _thumb = GetNode<TextureRect>("Container/VBox/ThumbPanel/Thumbnail");
            _badge = GetNode<Label>("Container/VBox/ThumbPanel/Badge");

            // Title is now inside a TitleRow HBox
            _title = GetNode<Label>("Container/VBox/TitleRow/Title");

            _playerCount = GetNode<Label>("Container/VBox/StatsRow/Players/PlayerCount");
            _likePercent = GetNode<Label>("Container/VBox/StatsRow/Likes/LikePercent");

            // Reference to the actual button at the bottom
            _playButton = GetNode<Button>("Container/VBox/PlayButton");

            // 2. Connect signals
            _playButton.Pressed += OnPlayButtonPressed;

            // 3. Apply values
            RefreshUI();
        }

        /// <summary>
        /// Updates the visual elements with the current property values
        /// </summary>
        public void RefreshUI()
        {
            if (Thumbnail != null) _thumb.Texture = Thumbnail;

            _title.Text = TitleText;
            _badge.Text = $" {VersionText} "; // Spaces for padding
            _playerCount.Text = Players.ToString();
            _likePercent.Text = $"{LikePercent}%";

            // Logic to color the percentage (optional, matches your UI style)
            if (LikePercent >= 90)
                _likePercent.Modulate = Color.Color8(102, 219, 84); // Green
            else
                _likePercent.Modulate = Colors.White;
        }

        private void OnPlayButtonPressed()
        {
            EmitSignal(SignalName.PlayPressed);
        }
    }
}
