using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Speed.Pages
{
    public class GameModel : PageModel
    {
        public List<Card> PlayerOneHand { get; set; }
        public List<Card> PlayerTwoHand { get; set; }
        public List<Card> PlayerOneDeck { get; set; }
        public List<Card> PlayerTwoDeck { get; set; }
        public List<Card> PlayPileOne { get; set; }
        public List<Card> PlayPileTwo { get; set; }
        public List<Card> PickPileOne { get; set; }
        public List<Card> PickPileTwo { get; set; }
        static readonly Random Random = new();

        public GameModel()
        {
            PlayerOneHand = new List<Card>();
            PlayerTwoHand = new List<Card>();
            PlayerOneDeck = new List<Card>();
            PlayerTwoDeck = new List<Card>();
            PlayPileOne = new List<Card>();
            PlayPileTwo = new List<Card>();
            PickPileOne = new List<Card>();
            PickPileTwo = new List<Card>();

        }
        public void OnGet()
        {
        }

        

    }

    public class Card
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string Suit { get; set; }
        public string Image { get; set; }

        public Card(string name, string suit, string image)
        {
            if (name == "ace")
            {
                Value = 1;
            }
            else if (name == "jack") {
                Value = 11;
            }
            else if (name == "queen")
            {
                Value = 12;
            }
            else if (name == "king")
            {
                Value = 13;
            }
            else
            {
                Value = int.Parse(name);
            }
            Name = name;
            Suit = suit;
            Image = image;
        }
    }
}

//public Game game { get; set; }
//public string player_one { get; set; }
//public string player_two { get; set; }
//private IHubContext<GameHub> _hubContext;

//public GameModel(IHubContext<GameHub> hubContext)
//{
//    _hubContext = hubContext;


//}
//public void OnGet()
//{
//}

//public async Task JoinGame(string connectionId)
//{
//    if (game == null)
//    {
//        game = new Game();
//    }

//    if (string.IsNullOrEmpty(player_one))
//    {
//        player_one = connectionId;
//        await _hubContext.Clients.Client(player_one).SendAsync("UpdatePlayer", "player_one");
//    }
//    else if (string.IsNullOrEmpty(player_two))
//    {
//        player_two = connectionId;
//        await _hubContext.Clients.Client(player_two).SendAsync("UpdatePlayer", "player_two");
//    }
//}

//public async Task StartGame(string connectionId)
//{
//    if ((!string.IsNullOrEmpty(player_one)) && (!string.IsNullOrEmpty(player_two)))
//    {
//        var my_hand = new string[5];
//        var my_count = 0;
//        var play_one = "";
//        var play_two = "";
//        var other_count = 0;
//        if (connectionId == player_one)
//        {
//            my_hand = game.getHand(player_one).ToArray();
//            my_count = game.PlayerOneDeck.Count();
//            play_one = game.PlayPileOne[0].Image;
//            play_two = game.PlayPileTwo[0].Image;
//            other_count = game.PlayerTwoDeck.Count();
//        }
//        else if (connectionId == player_two)
//        {
//            my_hand = game.getHand(player_two).ToArray();
//            my_count = game.PlayerTwoDeck.Count();
//            play_one = game.PlayPileOne[0].Image;
//            play_two = game.PlayPileTwo[0].Image;
//            other_count = game.PlayerOneDeck.Count();
//        }
//        await _hubContext.Clients.Client(connectionId).SendAsync("UpdateGame", my_hand, my_count, play_one, play_two, other_count);
//    }
//}