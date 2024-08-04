using System;

namespace ImageTournament;

public class NewImagesEventArgs(string? leftImage, string? rightImage) : EventArgs
{
    public string? LeftImage { get; } = leftImage;
    public string? RightImage { get; } = rightImage;
}