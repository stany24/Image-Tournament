using System;
using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageTournament.Logic;

namespace ImageTournament.ViewModels;

public partial class ResultWindowViewModel(string tournamentPath) : ObservableObject
{
    [ObservableProperty] private Bitmap? _winner = GetBitmap(Directory.GetFiles(Path.Combine(tournamentPath,Convert.ToString(1)))[0]);
    [ObservableProperty] private Bitmap? _second = GetBitmap(Directory.GetFiles(Path.Combine(tournamentPath,Convert.ToString(2)))[0]);
    [ObservableProperty] private Bitmap? _third = GetBitmap(Directory.GetFiles(Path.Combine(tournamentPath,Convert.ToString(4)))[0]);
    [ObservableProperty] private Bitmap? _fourth = GetBitmap(Directory.GetFiles(Path.Combine(tournamentPath,Convert.ToString(4)))[1]);

    private static Bitmap? GetBitmap(string? path)
    {
        return path == null ? null : new Bitmap(path);
    }
}