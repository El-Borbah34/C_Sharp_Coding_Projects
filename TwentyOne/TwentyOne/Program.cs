using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Casino;
using Casino.TwentyOne;

namespace TwentyOne
{
    class Program
    {
        static void Main(string[] args)
        {
           
            
            const string casinoName = "Grand Hotel and Casino"; // added 3/28
            
            Console.WriteLine("Welcome to the {0}. Let's start by telling me your name.", casinoName);
            string playerName = Console.ReadLine();
            if (playerName.ToLower() == "admin")
            {
                List<ExceptionEntity> Exceptions = ReadExceptions();
                foreach (var exception in Exceptions)                      //added 3/30 for database reasons. 
                {
                    Console.Write(exception.Id + " | ");
                    Console.Write(exception.ExceptionType + " | ");
                    Console.Write(exception.ExceptionMessage + " | ");
                    Console.Write(exception.TimeStamp + " | ");
                    Console.WriteLine();
                }
                Console.Read();
                return;
            }

            bool validAnswer = false;
            int bank = 0;
            while (!validAnswer)
            {
                Console.WriteLine("How much money did you bring today?");
                validAnswer = int.TryParse(Console.ReadLine(), out bank);
                if(!validAnswer) Console.WriteLine("Please enter digits only, no decimals."); // added this while loop on 3/29 (validation)
            }
            
            Console.WriteLine("Hello, {0}. Would you like to join a game of 21 right now?", playerName);
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
            {
                Player player = new Player(playerName, bank);
                player.Id = Guid.NewGuid(); // added 3/28
                using (StreamWriter file = new StreamWriter(@"C:\Users\Hal 9000\logs\log.txt", true)) //added on 3/28 using System. IO to output this log.
                {
                    file.WriteLine(player.Id);
                }

                Game game = new TwentyOneGame();
                game += player; 
                player.isActivelyPlaying = true;
                while(player.isActivelyPlaying && player.Balance > 0)
                {
                    try
                    {
                        game.Play();
                    }
                    catch (FraudException ex)
                    {
                        Console.WriteLine(ex.Message); // 3/30
                        UpdateDbWithException(ex); // 3/30 database log, useful for debugging.
                        Console.ReadLine();
                        return;
                    }
                    catch (Exception ex) //added 3/29, this try/catch to validate the game.play(); making sure the amount given isnt less than 0.
                    {
                        Console.WriteLine("An error occured. Please contact your System Administrator.");
                        UpdateDbWithException(ex); // 3/30 same as above 
                        Console.ReadLine();
                        return;         //added 3/29
                    }
                    
                }
                game -= player;
                Console.WriteLine("Thanks for playing!");
            }
            Console.WriteLine("Feel free to look around the casino. Bye for now.");
            Console.ReadLine();
        }

        private static void UpdateDbWithException(Exception ex)  // Database connection function, we will use a connection string and then our querystring to INSERT(this time) 
        {                                                        // the statement Using will allow the OPENING AND CLOSING of the connection path
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TwentyOneGame;Integrated Security=True;
                                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                        MultiSubnetFailover=False";

            string queryString = @"INSERT INTO Exceptions (ExceptionType, ExceptionMessage, TimeStamp) VALUES 
                                   (@ExceptionType, @ExceptionMessage, @TimeStamp)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);  // the command will see the connection to DB and then see how to parse data because of our Querystring

                command.Parameters.Add("@ExceptionType", SqlDbType.VarChar);
                command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar);
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime);

                command.Parameters["@ExceptionType"].Value = ex.GetType().ToString();
                command.Parameters["@ExceptionMessage"].Value = ex.Message;
                command.Parameters["@TimeStamp"].Value = DateTime.Now;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        private static List<ExceptionEntity> ReadExceptions()  //added 3/30 This SQL function literally READS the data and assigns the data to a LIST object. 
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TwentyOneGame;Integrated Security=True;
                                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                        MultiSubnetFailover=False";

            string queryString = @"Select Id, ExceptionType, ExceptionMessage, TimeStamp From Exceptions"; // choose what and where to grab the data that we need. Again it is just reading it.

            List<ExceptionEntity> Exceptions = new List<ExceptionEntity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader(); // this will read the data written in the DB

                while (reader.Read()) // while the reader goes through each line.
                {
                    ExceptionEntity exception = new ExceptionEntity(); // create the object to place all the data we need into. 
                    exception.Id = Convert.ToInt32(reader["Id"]);
                    exception.ExceptionType = reader["ExceptionType"].ToString();
                    exception.ExceptionMessage = reader["ExceptionMessage"].ToString();
                    exception.TimeStamp = Convert.ToDateTime(reader["TimeStamp"]);

                    Exceptions.Add(exception);
                }
                connection.Close();
            }
            return Exceptions;
        }
    }
}







/*=================Original function===============================*/
//Deck deck = new Deck();

//deck.Shuffle(3);

//foreach (Card card in deck.Cards)
//{
//    Console.WriteLine(card.Face + " of " + card.Suit);
//}
//Console.WriteLine(deck.Cards.Count);
//Console.ReadLine();
//TwentyOneGame game = new TwentyOneGame();
//game.Players = new List<string>() { "Jesse", "Bill", "Bob" };
//game.ListPlayers();
//Console.ReadLine();
//Game game = new TwentyOneGame();
//game.Players = new List<Player>(); //ask fy how this is working, its getting me confused because its being abstracted alot.
//Player player = new Player();
//player.Name = "Jesse";
//game += player;
//game -= player;
//Player<Card> player = new Player<Card>();
//player.Hand = new List<Card>();
//Card card = new Card();
//card.Suit = Suit.Clubs;
//int underlyingValue = (int)Suit.Diamonds; //(int) is the same as Convert.ToInt32
//Console.WriteLine(underlyingValue);
/*=================Lambda Functions===================*/
//int count = deck.Cards.Count(x => x.Face == Face.Ace);
//List<Card> newList = deck.Cards.Where(x => x.Face == Face.King).ToList();

//List<int> numberList = new List<int>() { 1, 2, 3, 535, 342, 23 };
//int sum = numberList.Sum(x => x = 5);
//int sum = numberList.Where(x => x > 20).Sum(); //numberList.Min would get the smallest number around. numberList.Max(), Also you can combine lambda functions. 

//Console.WriteLine(sum);