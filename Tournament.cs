using System;
using System.IO;

namespace ImageTournament;

public class Tournament
{
    public EventHandler<NewImagesEventArgs>? ImagesChanged;
    public EventHandler<NewImagesEventArgs>? Ended;
    private string?[] _imageArray;
    private string?[] _nextArray;
    private int _currentRoundSize;
    private int _index;
    
    public Tournament(string path)
    {
        string[] images = Directory.GetFiles(path);
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
            Console.WriteLine("Winner: "+_imageArray[0]);
            Ended?.Invoke(null,new NewImagesEventArgs(_imageArray[0],_imageArray[0]));
            return;
        }
        
        ImagesChanged?.Invoke(null, new NewImagesEventArgs(_imageArray[0], _imageArray[1]));
    }
}