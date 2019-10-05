using System;
using System.Windows.Forms;
                                                
namespace DurakGame
{                                       
    static class Program
    {                                           //По тихоньку, по маленьку, я добью свою идейку! 
        [STAThread]
        static void Main()
        {
#if !DEBUG
            try
            {
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var splash = new SplashScreen();
            splash.ShowDialog();

            Application.Run(new Main());

#if !DEBUG
            } catch (Exception e)
            {
                MessageBox.Show($"A fatal error has occured:\n{e.Message} \n{e.StackTrace}" +
                    $"\nProgram will now close", "Fatal Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }
    }
}
