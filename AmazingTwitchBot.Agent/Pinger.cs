//using System.Threading;

//namespace AmazingTwitchBot.Agent
//{
//    public class Pinger
//    {
//        //vars
//        public TwitchChatBot myClient;
//        public Thread pingSender;

//        //constructor
//        public Pinger(TwitchChatBot myClient)
//        {
//            this.myClient = myClient;
//            pingSender = new Thread(new ThreadStart(this.Run));
//        }

//        // Starts the thread
//        public void Start()
//        {
//            pingSender.IsBackground = true;
//            pingSender.Start();
//        }

//        // Send PING to irc server every 5 minutes
//        public void Run()
//        {
//            while (true)
//            {
//                myClient.SendPing();
//                Thread.Sleep(300000); // 5 minutes
//            }
//        }
//    }
//}
