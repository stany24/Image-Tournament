using System.Collections.Generic;


namespace ImageTournament.Logic;

public class TournamentResult
{
    private List<string?> _demi;
    private List<string?> _final;
    private string? _winner;

    public void SetDemi(List<string?> demi)
    {
        _demi = demi;
    }
    
    public void SetFinal(List<string?> final)
    {
        _final = final;
    }
    
    public void SetWinner(string? winner)
    {
        _winner = winner;
    }
    
    public string?[] GetResults()
    {
        _demi.Remove(_final.ToArray()[0]);
        _demi.Remove(_final.ToArray()[1]);
        _final.Remove(_winner);
        return [_winner, _final.ToArray()[0], _demi.ToArray()[0], _demi.ToArray()[1]];
    }
}