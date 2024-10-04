using ChessChallenge.API;
using System;
using ChessChallenge.API;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static ChessChallenge.Application.ConsoleHelper;


namespace ChessChallenge.Example
{
    public class old_AACE : IChessBot
    {
         private static readonly HttpClient client = new HttpClient();


    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();

        
        var (bestMove, evaluation) = GetApiResponse(board).Result;


        return new Move(bestMove, board);


    }

    private async Task<(string bestMove, int evaluation)> GetApiResponse(Board board)
        {
            string url = "http://localhost:8080/search_old";
            string FEN = board.GetFenString();
            string jsonContent = "{ \"fen\": \"" + FEN + "\" }";
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(responseBody);
                //Log("Response from API: " + responseBody, false, ConsoleColor.Green);


                return (result.best_move, result.evaluation);
            }
            catch (HttpRequestException e)
            {
                Log($"Request error: {e.Message}", false, ConsoleColor.Red);
                 return (null, 0);
            }
        }

        private class ApiResponse
            {
                public string best_move { get; set; }
                public int evaluation { get; set; }
                public string fen_after_move { get; set; }
            }
    }
}