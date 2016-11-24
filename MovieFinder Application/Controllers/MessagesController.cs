using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Collections.Generic;
using MovieFinder_Application.DataModels;

namespace MovieFinder_Application
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            


            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                var userMessage = activity.Text;

                StateClient stateClient = activity.GetStateClient();
                BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);

                if (userMessage.ToLower().Equals("add movie")) {
                    if (userData.GetProperty<bool>("foundMovie"))
                    {
                        WatchList watchList = new WatchList()
                        {
                            Title = userData.GetProperty<string>("MovieTitle"),
                            
                        };

                        await AzureManager.AzureManagerInstance.AddWatchList(watchList);
                        Activity reply = activity.CreateReply($"Movie added to watchlist.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                    else {
                        Activity reply = activity.CreateReply($"Please select a movie first.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                }


                else if (userMessage.ToLower().Equals("update rating") || (userData.GetProperty<bool>("accessedwatchlist")))
                {
                    if (userData.GetProperty<bool>("accessedwatchlist"))
                    {
                        int rating;

                        List<WatchList> watchList = await AzureManager.AzureManagerInstance.GetWatchList();

                        foreach (WatchList t in watchList)
                        {
                            string movieTitle = t.Title;

                            if (userMessage.Equals(movieTitle))
                            {
                                // delete transaction
                                await AzureManager.AzureManagerInstance.DeleteWatchList(t);
                                endOutput = "Transaction deleted \n\n [" + transactionId + "]";

                                // set property to false
                                userData.SetProperty<bool>("DeleteTransaction", false);
                                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                            }
                        }
                        if () {
                            if (!int.TryParse(userMessage, out rating)) {
                                if (0 < Convert.ToInt32(userMessage) && Convert.ToInt32(userMessage) > 10) {
                                    WatchList watchList = new WatchList()
                                    {
                                        Title = userData.GetProperty<string>("MovieTitle"),

                                    };

                                    await AzureManager.AzureManagerInstance.AddWatchList(watchList);
                                    Activity reply = activity.CreateReply($"rating updated.");
                                    await connector.Conversations.ReplyToActivityAsync(reply); }
                                else {
                                    Activity reply = activity.CreateReply($"Please enter a rating between 0 and 10.");
                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                }

                            }
                            else {
                                Activity reply = activity.CreateReply($"Please enter a rating between 0 and 10.");
                                await connector.Conversations.ReplyToActivityAsync(reply);
                            }
                        }
                        else {
                            Activity reply = activity.CreateReply($"Please enter a movie in your watchlist.");
                            await connector.Conversations.ReplyToActivityAsync(reply);
                        }
                        
                    }
                    else
                    {
                        Activity reply = activity.CreateReply($"Please access watchlist first.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                }



                else if (userMessage.ToLower().Equals("delete movie"))
                {
                    if (userData.GetProperty<bool>("foundMovie"))
                    {
                        WatchList watchList = new WatchList()
                        {
                            Title = userData.GetProperty<string>("MovieTitle"),

                        };

                        await AzureManager.AzureManagerInstance.AddWatchList(watchList);
                        Activity reply = activity.CreateReply($"Movie added to watchlist.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                    else
                    {
                        Activity reply = activity.CreateReply($"Please select a movie first.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                }


                else if (userMessage.ToLower().Equals("mood happy")) {
                    MovieObject.RootObject rootObject;
                    HttpClient client = new HttpClient();

                    string x = await client.GetStringAsync(new Uri("https://api.themoviedb.org/3/genre/35/movies?sort_by=created_at.asc&=&api_key=101d42ee5443e5b374710ce0dbd1eb7f"));
                    rootObject = JsonConvert.DeserializeObject<MovieObject.RootObject>(x);

                    Random random1 = new Random();
                    int randomNumber = random1.Next(0, 21);

                    MovieObject.Result Movie = rootObject.results[randomNumber];

                    string MovieTitle = Movie.title;
                    string MoviePlot = Movie.overview;

                    Activity reply = activity.CreateReply($"{MovieTitle} : {MoviePlot}");
                    await connector.Conversations.ReplyToActivityAsync(reply);

                    userData.SetProperty<string>("MovieTitle", MovieTitle);
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);


                }
                else if (userMessage.ToLower().Equals("mood sad"))
                {
                    MovieObject.RootObject rootObject;
                    HttpClient client = new HttpClient();

                    string x = await client.GetStringAsync(new Uri("https://api.themoviedb.org/3/genre/18/movies?sort_by=created_at.asc&=&api_key=101d42ee5443e5b374710ce0dbd1eb7f"));
                    rootObject = JsonConvert.DeserializeObject<MovieObject.RootObject>(x);

                    Random random1 = new Random();
                    int randomNumber = random1.Next(0, 20);

                    MovieObject.Result Movie = rootObject.results[randomNumber];

                    string MovieTitle = Movie.title;
                    string MoviePlot = Movie.overview;

                    Activity reply = activity.CreateReply($"{MovieTitle} : {MoviePlot}");
                    await connector.Conversations.ReplyToActivityAsync(reply);

                    userData.SetProperty<string>("MovieTitle", MovieTitle);
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);



                }
                else if (userMessage.ToLower().Equals("mood angry"))
                {
                    MovieObject.RootObject rootObject;
                    HttpClient client = new HttpClient();

                    string x = await client.GetStringAsync(new Uri("https://api.themoviedb.org/3/genre/28/movies?sort_by=created_at.asc&=&api_key=101d42ee5443e5b374710ce0dbd1eb7f"));
                    rootObject = JsonConvert.DeserializeObject<MovieObject.RootObject>(x);

                    Random random1 = new Random();
                    int randomNumber = random1.Next(0, 20);

                    MovieObject.Result Movie = rootObject.results[randomNumber];

                    string MovieTitle = Movie.title;
                    string MoviePlot = Movie.overview;

                    Activity reply = activity.CreateReply($"{MovieTitle} : {MoviePlot}");
                    await connector.Conversations.ReplyToActivityAsync(reply);

                    userData.SetProperty<string>("MovieTitle", MovieTitle);
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);

                }
                else if (userMessage.ToLower().Equals("mood neutral"))
                {
                    MovieObject.RootObject rootObject;
                    HttpClient client = new HttpClient();

                    Random random4 = new Random();
                    int randomNumber4 = random4.Next(1, 20);

                    string x = await client.GetStringAsync(new Uri("https://api.themoviedb.org/3/movie/popular?api_key=101d42ee5443e5b374710ce0dbd1eb7f&language=en-US"));
                    rootObject = JsonConvert.DeserializeObject<MovieObject.RootObject>(x);


                    MovieObject.Result Movie = rootObject.results[randomNumber4];

                    string MovieTitle = Movie.title;
                    string MoviePlot = Movie.overview;

                    Activity reply = activity.CreateReply($"{MovieTitle} : {MoviePlot}");
                    await connector.Conversations.ReplyToActivityAsync(reply);

                    userData.SetProperty<string>("MovieTitle", MovieTitle);
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);


                }
                else if (userMessage.ToLower().Equals("get watchlist"))

                {

                    List<WatchList> WatchList = await AzureManager.AzureManagerInstance.GetWatchList();

                    string endOutput = "";
                    int count = 0;
                    foreach (WatchList t in WatchList)

                    {
                        count += 1;
                        endOutput += (count + ". " + t.Title + " " + t.Rating + "\n\n");
                    }

                    Activity reply = activity.CreateReply($"{endOutput}");
                    await connector.Conversations.ReplyToActivityAsync(reply);

                    userData.SetProperty<bool>("accessedwatchlist", true);
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);

                }


                else {

                    MovieObject.RootObject rootObject;
                    HttpClient client = new HttpClient();

                    string x = await client.GetStringAsync(new Uri("https://api.themoviedb.org/3/search/movie?query=" + activity.Text + "&external_source=imdb_id&api_key=101d42ee5443e5b374710ce0dbd1eb7f"));
                    rootObject = JsonConvert.DeserializeObject<MovieObject.RootObject>(x);
                    List<MovieObject.Result> movielist = rootObject.results;

                    if (movielist.Count != 0)
                    {
                        MovieObject.Result Movie = rootObject.results[0];
                        string MovieTitle = Movie.title;
                        string MoviePlot = Movie.overview;

                        Activity reply = activity.CreateReply($"{MovieTitle} : {MoviePlot}");
                        await connector.Conversations.ReplyToActivityAsync(reply);

                        userData.SetProperty<string>("MovieTitle", MovieTitle);
                        await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                    }
                    else {
                        Activity reply = activity.CreateReply($"Not a valid movie or mood, please try again");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                }
                
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}