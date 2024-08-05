using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageTournament.Logic;

namespace ImageTournament.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private Tournament? _tournament;
    [ObservableProperty] private  string _path = "/path/to/images";
    [ObservableProperty] private Bitmap? _leftImage;
    [ObservableProperty] private Bitmap? _rightImage;
    [ObservableProperty] private bool _enabled;
    
    private static Bitmap? GetBitmap(string? path)
    {
        return path == null ? null : new Bitmap(path);
    }

    public void Start()
    {
        if(!Directory.Exists(Path)){return;}
        _tournament = new Tournament(Path);
        _tournament.ImagesChanged += (_, e) =>
        {
            LeftImage?.Dispose();
            LeftImage = GetBitmap(e.LeftImage);
            RightImage?.Dispose();
            RightImage = GetBitmap(e.RightImage);
        };
        _tournament.Ended += (_, e) =>
        {
            Enabled = false;
            LeftImage?.Dispose();
            LeftImage = GetBitmap(e.LeftImage);
            RightImage?.Dispose();
            RightImage = GetBitmap(e.RightImage);
        };
        _tournament.Begin();
        Enabled = true;
    }
    public void ImageSelected(bool isLeft)
    {
        _tournament?.Select(isLeft);
    }
}