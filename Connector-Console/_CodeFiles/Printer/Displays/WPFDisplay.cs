using System.Drawing;

namespace Connector.Printer.Displays
{
    public class WPFDisplay: IDisplay
    {
        public void ShowMessage(object message)
        {
            // textBox1.Text = message;
        }
        public void ShowMessage(object message, Color color)
        {
            // textBox1.ForeColor = color;
            // ShowMessage(message);
        }
        public void ShowMessage(object message, Color color, Color colorBack)
        {
            // textBox1.Background = colorBack;
            // ShowMessage(message);
        }
    }
}