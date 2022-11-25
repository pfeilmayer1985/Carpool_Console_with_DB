using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnotherWhatever
{

    public class User
    {
        public int id { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool isDriver { get; set; }

    }

    public class CarpoolShort
    {
        public int CarpoolID { get; set; }
        public int DriverID { get; set; }
        public int TotalSeatsCount { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
    }

    public class CarpoolComplete
    {
        public int CarpoolID { get; set; }
        public User Driver { get; set; }
        public int TotalSeatsCount { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
    }
    class Program
    {
        static string baseUri = "https://localhost:7169/";
        static void Main(string[] args)
        {
            MainScreen();
            //var users = GetAllUsersAsync().Result;

        }
        public static async Task<List<User>> GetAllUsersAsync()
        {
            var Users = new List<User>();
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{baseUri}Users/GetAllUsers");
            if (response.IsSuccessStatusCode)
            {
                Users = await response.Content.ReadAsAsync<List<User>>();
                return Users;
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return null;
            }
        }

        public static async Task<User> GetUserByIdAsync(int userID)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{baseUri}Users/GetUserByID/{userID}");
            var UserJsonString = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<User>(UserJsonString);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return null;
            }
        }

        public static async Task<List<CarpoolShort>> GetAllCarpoolsAsync()
        {
            var Carpools = new List<CarpoolShort>();
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{baseUri}Carpools/GetAllCarpools");
            if (response.IsSuccessStatusCode)
            {
                Carpools = await response.Content.ReadAsAsync<List<CarpoolShort>>();
                return Carpools;
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return null;
            }
        }

        public static async Task<CarpoolComplete> GetCarpoolByIdAsync(int carpoolID)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{baseUri}Carpools/GetCarpoolById/{carpoolID}");
            var UserJsonString = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<CarpoolComplete>(UserJsonString);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return null;
            }
        }

        public static async Task DeleteCarpool(int carpoolID, int userID, string password)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.DeleteAsync($"{baseUri}Carpools/DeleteCarpoolById/{carpoolID}/{userID}/{password}");
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return;
            }
        }

        public static async Task DeleteUserAccount(int userID, string password)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.DeleteAsync($"{baseUri}Users/DeleteUserByID/{userID}?password={password}");
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return;
            }
        }

        public static async Task RemovePassengerFromCarpool(int carpoolID, int userID, string password)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.DeleteAsync($"{baseUri}Users/RemoveUserFromCarpoolID/{carpoolID}/{userID}?password={password}");
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                return;
            }
        }

        public static async Task UpdateUser(int userID, string password, User user)
        {
            HttpClient client = new HttpClient();
            var userString = JsonConvert.SerializeObject(user);
            HttpResponseMessage response = await client.PutAsync($"{baseUri}Users/EditUserById/{userID}?password={password}", new StringContent(userString, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Put successful!");
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }

        public static async Task UpdateCarpool(int carpoolID, int userID, string password, CarpoolShort carpool)
        {
            HttpClient client = new HttpClient();
            var carpoolString = JsonConvert.SerializeObject(carpool);
            HttpResponseMessage response = await client.PutAsync($"{baseUri}Carpools/EditCarpool/{carpoolID}/{userID}/{password}", new StringContent(carpoolString, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Put successful!");
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }

        /// <summary>
        /// The MainScreen method is the main menu of the carpool app, where the user chooses it's class (driver/passenger)
        /// </summary>
        public static void MainScreen()
        {
            bool userClassBool = true;
            do
            {
                //Clearing console and showing the main menu in a loop. User can choose tthe drivers or passengers menu, list all the carpools or exit
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CwL("╔═════════════════════════════════╗\n" +
                    "║        Welcome to Narnia        ║\n" +
                    "╚═════════════════════════════════╝");
                Console.ResetColor();
                CwL("\n( 1 )\tUsers" +
                    "\n( 2 )\tCarpools");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n( 9 )\tFuck you very much");
                Console.ResetColor();
                int userClass;
                //if user chooses a non-existing menu item stays in loop until he enters a existing value
                bool pressedRightKey = false;
                do
                {
                    ChoseOptionAbvTxt();
                    ConsoleKeyInfo userInputKey = Console.ReadKey();
                    string userInput = Convert.ToString(userInputKey.KeyChar);
                    if (!int.TryParse(userInput, out userClass))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Would you like to try again, this time with your brain switched on before typing?");
                        Console.ResetColor();
                        pressedRightKey = true;
                    }
                    else
                    {
                        pressedRightKey = false;
                        continue;
                    }
                } while (pressedRightKey);

                switch (userClass)
                {
                    case 1:
                        UserMenu();
                        userClassBool = true;
                        continue;
                    case 2:
                        CarpoolsMenu();
                        userClassBool = true;
                        continue;
                    case 9:
                        Console.WriteLine("You choose to leave. Have a great one!");
                        userClassBool = false;
                        break;
                    default:
                        userClassBool = true;
                        continue;
                }
            } while (userClassBool);
        }



        /// <summary>
        /// This is the main menu for the User class 
        /// </summary>
        public static void UserMenu()
        {
            bool userPassengerBool = true;
            do
            {

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CwL("╔═════════════════════════════════╗\n" +
                    "║            Users Menu           ║\n" +
                    "╚═════════════════════════════════╝");
                Console.ResetColor();
                CwL("\n( 1 )\tSearch for a specific user by User_ID" +
                    "\n( 2 )\tList all registered users" +
                    "\n( 3 )\t" +
                    "\n( 4 )\tEdit/Update user account" +
                    "\n( 5 )\tDelete own carpool" +
                    "\n( 6 )\tRemove yourself from a carpool" +
                    "\n( 7 )\tDelete own account");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n( 9 )\tBack to the main menu");
                Console.ResetColor();
                //if user chooses a non-existing menu item stays in loop until he enters a existing value
                int passengerMenu;
                bool pressedRightKey = false;
                do
                {
                    ChoseOptionAbvTxt();
                    //string userInput = Console.ReadLine();
                    ConsoleKeyInfo userInputKey = Console.ReadKey();
                    string userInput = Convert.ToString(userInputKey.KeyChar);
                    if (!int.TryParse(userInput, out passengerMenu))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Would you like to try again, this time with your brain switched on before typing?");
                        Console.ResetColor();
                        pressedRightKey = true;
                    }
                    else
                    {
                        pressedRightKey = false;
                        continue;
                    }
                } while (pressedRightKey);

                switch (passengerMenu)
                {
                    case 1:
                        Console.Clear();
                        CwL("User ID to be displayed: ");
                        int choice = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                        var user = GetUserByIdAsync(choice).Result;
                        CwL($"User ID:\t\t{user.id}\nE-Mail:\t\t\t{user.email}\nPhone Number:\t\t{user.phoneNo}\nFirst Name:\t\t{user.firstName}\nLast Name:\t\t{user.lastName}\nIs {user.firstName} a Driver:\t{user.isDriver}");
                        Console.ReadLine();
                        userPassengerBool = true;
                        continue;
                    case 2:
                        Console.Clear();
                        var userList = GetAllUsersAsync().Result;
                        foreach (var individualUser in userList)
                        {
                            CwL($"User ID:\t\t{individualUser.id}\nE-Mail:\t\t\t{individualUser.email}\nPhone Number:\t\t{individualUser.phoneNo}\nFirst Name:\t\t{individualUser.firstName}\nLast Name:\t\t{individualUser.lastName}\nIs {individualUser.firstName} a Driver:\t{individualUser.isDriver}");
                            CwL("\n\n***********************************************************************************\n");
                        }
                        Console.ReadLine();
                        userPassengerBool = true;
                        continue;
                    case 3:
                        userPassengerBool = true;
                        continue;
                    case 4:
                        Console.Clear();
                        CwL("Your User ID to edit: ");
                        int userIDx = Convert.ToInt32(Console.ReadLine());
                        CwL("Your account password: ");
                        string passwordx = Console.ReadLine();
                        Console.Clear();
                        CwL("Enter you new info: ");
                        CwL("Your e-mail address: ");
                        string email = Console.ReadLine();
                        CwL("Your password: ");
                        string passwordN = Console.ReadLine();
                        CwL("Your phone number: ");
                        string phoneno = Console.ReadLine();
                        CwL("Your first name: ");
                        string firstname = Console.ReadLine();
                        CwL("Your last name: ");
                        string lastname = Console.ReadLine();
                        CwL("Are you driving? 1 for yes, 0 for no: ");
                        int isdriver = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();

                        User userN = new User()
                        {
                            id = userIDx,
                            email = email,
                            password = passwordN,
                            phoneNo = phoneno,
                            firstName = firstname,
                            lastName = lastname,
                            isDriver = Convert.ToBoolean(isdriver)
                        };

                        UpdateUser(userIDx, passwordx, userN);
                        Console.ReadLine();
                        userPassengerBool = true;
                        continue;
                    case 5:
                        Console.Clear();
                        CwL("The Carpool ID you want to remove: ");
                        int carpoolID = Convert.ToInt32(Console.ReadLine());
                        CwL("Your User ID to prove you are the driver: ");
                        int userID = Convert.ToInt32(Console.ReadLine());
                        CwL("Your account password: ");
                        string password = Console.ReadLine();
                        Console.Clear();
                        DeleteCarpool(carpoolID, userID, password);
                        Console.ReadLine();
                        userPassengerBool = true;
                        continue;
                    case 6:
                        Console.Clear();
                        CwL("The Carpool ID where the user is to be removed from: ");
                        carpoolID = Convert.ToInt32(Console.ReadLine());
                        CwL("Your User ID to remove: ");
                        userID = Convert.ToInt32(Console.ReadLine());
                        CwL("Your account password: ");
                        password = Console.ReadLine();
                        Console.Clear();
                        DeleteUserAccount(userID, password);
                        Console.ReadLine();
                        userPassengerBool = true;
                        continue;
                    case 7:
                        Console.Clear();
                        CwL("Your User ID that you want to DELETE: ");
                        userID = Convert.ToInt32(Console.ReadLine());
                        CwL("Your account password: ");
                        password = Console.ReadLine();
                        Console.Clear();
                        DeleteUserAccount(userID, password);
                        Console.ReadLine();
                        userPassengerBool = true;
                        continue;
                    case 9:
                        userPassengerBool = false;
                        break;
                    default:

                        userPassengerBool = true;
                        continue;
                }
            } while (userPassengerBool);
        }

        /// <summary>
        /// This is the main menu for the User class 
        /// </summary>
        public static void CarpoolsMenu()
        {
            bool userPassengerBool = true;
            do
            {

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CwL("╔═════════════════════════════════╗\n" +
                    "║         Carpools Menu           ║\n" +
                    "╚═════════════════════════════════╝");
                Console.ResetColor();
                CwL("\n( 1 )\tSearch for a Specific Carpool by Carpool_ID" +
                    "\n( 2 )\tList all registered carpools" +
                    "\n( 3 )\t" +
                    "\n( 4 )\tEdit/Update you carpool" +
                    "\n( 5 )\t" +
                    "\n( 6 )\t" +
                    "\n( 7 )\t");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n( 9 )\tBack to the main menu");
                Console.ResetColor();
                //if user chooses a non-existing menu item stays in loop until he enters a existing value
                int passengerMenu;
                bool pressedRightKey = false;
                do
                {
                    ChoseOptionAbvTxt();
                    //string userInput = Console.ReadLine();
                    ConsoleKeyInfo userInputKey = Console.ReadKey();
                    string userInput = Convert.ToString(userInputKey.KeyChar);
                    if (!int.TryParse(userInput, out passengerMenu))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Would you like to try again, this time with your brain switched on before typing?");
                        Console.ResetColor();
                        pressedRightKey = true;
                    }
                    else
                    {
                        pressedRightKey = false;
                        continue;
                    }
                } while (pressedRightKey);

                switch (passengerMenu)
                {
                    case 1:
                        Console.Clear();
                        CwL("Carpool ID to be displayed: ");
                        int choice = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                        var carpool = GetCarpoolByIdAsync(choice).Result;
                        CwL($"Carpool ID: {carpool.CarpoolID}\nDriver's info:\n\tID: {carpool.Driver.id}\n\tName: {carpool.Driver.firstName} {carpool.Driver.lastName}\n\tE-Mail: {carpool.Driver.email}\n\tPhone No.: {carpool.Driver.phoneNo}\nTotal Seats Count: {carpool.TotalSeatsCount}\nOrigin: {carpool.Origin}\nDestination: {carpool.Destination}\nDeparture Date & Time: {carpool.DepartureDate}");
                        Console.ReadLine();
                        userPassengerBool = true;
                        continue;
                    case 2:
                        Console.Clear();
                        var carpoolsList = GetAllCarpoolsAsync().Result;
                        foreach (var individualCarpool in carpoolsList)
                        {
                            CwL($"Carpool ID:\t\t\t{individualCarpool.CarpoolID}\nDriver\'s ID:\t\t\t{individualCarpool.DriverID}\nTotal Seats Count:\t\t{individualCarpool.TotalSeatsCount}\nOrigin:\t\t\t\t{individualCarpool.Origin}\nDestination:\t\t\t{individualCarpool.Destination}\nDeparture Date & Time:\t\t{individualCarpool.DepartureDate}");
                            CwL("\n\n***********************************************************************************\n");
                        }
                        Console.ReadLine();
                        userPassengerBool = true;
                        continue;
                    case 3:
                        userPassengerBool = true;
                        continue;
                    case 4:
                        Console.Clear();
                        CwL("Your Carpool ID to edit: ");
                        int carpoolID = Convert.ToInt32(Console.ReadLine());
                        CwL("Your User ID: ");
                        int userID = Convert.ToInt32(Console.ReadLine());
                        CwL("Your account password: ");
                        string password = Console.ReadLine();
                        Console.Clear();
                        CwL("Enter you new carpool info: ");
                        CwL("How many total seats are available: ");
                        int totalseats = Convert.ToInt32(Console.ReadLine());
                        CwL("Origin: ");
                        string origin = Console.ReadLine();
                        CwL("Destination: ");
                        string destination = Console.ReadLine();
                        CwL("Departure Date & Time: ");
                        DateTime datetimedeparture = Convert.ToDateTime(Console.ReadLine());
                        Console.Clear();

                        CarpoolShort carpoolx = new CarpoolShort()
                        {
                            CarpoolID= carpoolID,
                            DriverID=userID,
                            TotalSeatsCount= totalseats,
                            Origin= origin,
                            Destination= destination,   
                            DepartureDate= datetimedeparture
                        };

                        UpdateCarpool(carpoolID, userID, password, carpoolx);
                        Console.ReadLine();
                        userPassengerBool = true;
                        continue;
                    case 5:
                        userPassengerBool = true;
                        continue;
                    case 6:
                        userPassengerBool = true;
                        continue;
                    case 7:
                        userPassengerBool = true;
                        continue;
                    case 9:
                        userPassengerBool = false;
                        break;
                    default:

                        userPassengerBool = true;
                        continue;
                }
            } while (userPassengerBool);
        }


        /// <summary>
        /// Quick ConsoleWriteLine
        /// </summary>
        public static void CwL(params string[] words)
        {
            foreach (string word in words)
            {
                Console.WriteLine(word);
            }
        }

        /// <summary>
        /// Standard text for choose an option
        /// </summary>
        public static void ChoseOptionAbvTxt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n╔═════════════════════════════════╗");
            Console.Write("║ Choose one of the options above ║");
            Console.Write("\n╚═════════════════════════════════╝");
            Console.ResetColor();
        }

        /// <summary>
        /// Standard text for choose an option
        /// </summary>
        public static void PressEnterTxt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n╔═══════════════════════════════════════════════╗");
            Console.Write("║ Press <Enter> to return to the previous menu. ║");
            Console.Write("\n╚═══════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.ReadKey();
        }

        public static string readLineWithCancel()
        {
            string result = null;
            StringBuilder buffer = new StringBuilder();
            //The key is read passing true for the intercept argument to prevent
            //any characters from displaying when the Escape key is pressed.
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
            {
                if (info.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Length > 0)
                    {
                        Console.Write("\b\0\b");
                        buffer.Length--;
                    }
                    info = Console.ReadKey(true);
                    continue;
                }
                else
                {
                    Console.Write(info.KeyChar);
                    buffer.Append(info.KeyChar);
                    info = Console.ReadKey(true);
                }
            }
            if (info.Key == ConsoleKey.Enter)
            {
                result = buffer.ToString();
            }
            return result;
        }
    }
}

