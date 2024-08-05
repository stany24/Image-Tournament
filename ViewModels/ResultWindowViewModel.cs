using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageTournament.Logic;

namespace ImageTournament.ViewModels;

public partial class ResultWindowViewModel:ObservableObject
{
    [ObservableProperty] private Bitmap? _winner;
    [ObservableProperty] private Bitmap? _second;
    [ObservableProperty] private Bitmap? _third;
    [ObservableProperty] private Bitmap? _fourth;
    
    public ResultWindowViewModel(TournamentResult result)
    {
        string?[] res =result.GetResults();
        _winner = GetBitmap(res[0]);
        _second = GetBitmap(res[1]);
        _third = GetBitmap(res[2]);
        _fourth = GetBitmap(res[3]);
    }
    
    private static Bitmap? GetBitmap(string? path)
    {
        return path == null ? null : new Bitmap(path);
    }
}