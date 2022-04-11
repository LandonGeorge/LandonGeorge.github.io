using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Speed.GameLogic;

namespace Speed.Hubs
{
    [HubName("GameHub")]
    public class GameHub : Hub
    {
       
        public static Game? game { get; set; }
        public static string? player_one { get; set; }
        public static string? player_two { get; set; }


        public async Task JoinGame(string token)
        {
            if (string.IsNullOrEmpty(player_one) || player_one == token)
            {
                player_one = token;
                await Groups.AddToGroupAsync(Context.ConnectionId, "player_one");
                await Clients.Caller.SendAsync("UpdatePlayer", "player_one");
            }
            else if (string.IsNullOrEmpty(player_two) || player_two == token)
            {
                player_two = token;
                await Groups.AddToGroupAsync(Context.ConnectionId, "player_two");
                await Clients.Caller.SendAsync("UpdatePlayer", "player_two");
            }
        }

        public async Task StartGame(string token)
        {
            if (game == null)
            {
                game = new Game();
            }
            if ((!string.IsNullOrEmpty(player_one)) && (!string.IsNullOrEmpty(player_two))) {
                await UpdateHands();
            }
            
        }

        public async void PlayCard(string player_number, string hand_index, string play_pile)
        {
            game.PlayCard(player_number, int.Parse(hand_index), play_pile);
            await UpdateHands();
        }

        public async Task UpdateHands()
        {
            game.playPilesPlayable();
            var one_hand = game.GetHand("player_one");
            var two_hand = game.GetHand("player_two");
            var one_count = game.PlayerOneDeck.Count();
            var two_count = game.PlayerTwoDeck.Count();
            var play_one = game.PlayPileOne.Last().Image;
            var play_two = game.PlayPileTwo.Last().Image;

            await Clients.Group("player_one").SendAsync("UpdateGame", one_hand, one_count, play_one, play_two, two_count, two_hand.Count());
            await Clients.Group("player_two").SendAsync("UpdateGame", two_hand, two_count, play_one, play_two, one_count, one_hand.Count());
        }

        public async Task Speed()
        {
            if (player_one == Context.ConnectionId)
            {
                if (game.PlayerOneHand.Count() == 0)
                {
                    await Clients.Group("player_one").SendAsync("GameOver", "Congratulations! You Won!");
                    await Clients.Group("player_two").SendAsync("GameOver", "Game Over. You lose. Better Luck Next Time!");
                }
            }
            else if (player_two == Context.ConnectionId)
            {
                if (game.PlayerTwoHand.Count() == 0)
                {
                    await Clients.Group("player_two").SendAsync("GameOver", "Congratulations! You Won!");
                    await Clients.Group("player_one").SendAsync("GameOver", "Game Over. You lose. Better Luck Next Time!");
                }
            }
            //Reset the players & games after game finished.
            await Groups.RemoveFromGroupAsync(player_one, "player_one");
            await Groups.RemoveFromGroupAsync(player_two, "player_two");
            player_one = null;
            player_two = null;
            game = null;
        }

        public async Task SendMessage(string user, string message)
        {
            // sends to all endpoints
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            // sends to the user that called the function
            //await Clients.Caller.SendAsync("ReceiveMessage", user, message.ToUpper());
            // sends to all other users except the person who called the function
            await Clients.Others.SendAsync("ReceiveMessage", user, message);
        }

        public async Task EchoMessage(string user, string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", user, message.ToUpper());
        }
    }

    
}
