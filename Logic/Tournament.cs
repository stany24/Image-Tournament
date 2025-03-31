using System;
using System.IO;
using System.Linq;
using ImageTournament.ViewModels;
using ImageTournament.Views;

namespace ImageTournament.Logic;

public class Tournament
{
    public EventHandler<NewImagesEventArgs>? ImagesChanged { get; set; }
    public EventHandler<NewImagesEventArgs>? Ended { get; set; }
    private string?[] _imageArray;
    private string?[] _nextArray;
    private int _currentRoundSize;
    private int _index;
    private readonly string _tournamentPath;
    
    public Tournament(string path)
    {
        _tournamentPath = path;
        string[] images = Directory.GetFiles(path);
        images = images.Where(file => file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".webp")).ToArray();
        _currentRoundSize = GetSizeAboveMultipleOf2(images.Length);

        _imageArray = new string[_currentRoundSize];
        _nextArray = new string[_currentRoundSize/2];

        GetBaseTable(images);
    }

    private static int GetSizeAboveMultipleOf2(int value)
    {
        int size = 2;
        while (size < value)
        {
            size *= 2;
        }
        return size;
    }

    private void GetBaseTable(string[] images )
    {
        for (int i = 0; i < _currentRoundSize/2; i++)
        {
            _imageArray[2*i] = new string(images[i]);
        }

        for (int i = 0; i <= images.Length-_currentRoundSize/2-1; i++)
        {
            _imageArray[2*i+1] = new string(images[_currentRoundSize/2+i]);
        }
    }

    public void Begin()
    {
        ImagesChanged?.Invoke(null, new NewImagesEventArgs(_imageArray[0], _imageArray[1]));
    }

    public void Select(bool isLeft)
    {
        Console.WriteLine("Before: "+(_index/2+1)+"/"+_imageArray.Length/2);
        _nextArray[_index/2] =isLeft ? _imageArray[_index] : _imageArray[_index+1];
        SaveLoser(!isLeft ? _imageArray[_index] : _imageArray[_index+1]); 
        _index += 2;
        if (_index >= _imageArray.Length )
        {
            NextRound();
            return;
        }
        if (_imageArray[_index] == null || _imageArray[_index + 1] == null)
        {
            NextRound();
            return;
        }
        ImagesChanged?.Invoke(null, new NewImagesEventArgs(_imageArray[_index], _imageArray[_index + 1]));
    }
    
    private void SaveLoser(string? loser)
    {
        if(loser == null) return;
        string roundPath = Path.Combine(_tournamentPath, Convert.ToString(_currentRoundSize));
        if (!Directory.Exists(roundPath)) { Directory.CreateDirectory(roundPath); }
        string loserPath = Path.Combine(roundPath, Path.GetFileName(loser));
        File.Move(loser, loserPath);
    }
    
    private void SaveWinner(string? winner)
    {
        if(winner == null) return;
        string roundPath = Path.Combine(_tournamentPath, Convert.ToString(1));
        Directory.CreateDirectory(roundPath);
        string loserPath = Path.Combine(roundPath, Path.GetFileName(winner));
        File.Move(winner, loserPath);
    }
    
    private void NextRound()
    {
        for (int i = _index; i < _imageArray.Length; i++)
        {
            if (_imageArray[i] != null) _nextArray[i/2] =_imageArray[i];
        }
        
        _index = 0;
        _currentRoundSize /=2;
        _imageArray = _nextArray;
        _nextArray = new string[_currentRoundSize/2];
        
        if (_imageArray.Length == 1)
        {
            SaveWinner(_imageArray[0]);
            ResultWindow resultWindow = new()
            {
                DataContext = new ResultWindowViewModel(_tournamentPath)
            };
            resultWindow.Show();
            
            Ended?.Invoke(null,new NewImagesEventArgs(_imageArray[0],_imageArray[0]));
            return;
        }
        
        ImagesChanged?.Invoke(null, new NewImagesEventArgs(_imageArray[0], _imageArray[1]));
    }
}