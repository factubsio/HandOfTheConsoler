namespace HandOfTheConsoler
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        async static Task Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();

            App.Install();

            Console.WriteLine("waiting for connection...");
            await IPC.Instance.Connect();
            Console.WriteLine("CONNECTED");

            Application.Run(new Form1());
        }
    }
}