using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Messaging;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace MSMQDemo
{
    public partial class Form1 : Form
    {
        // Declare a MessageQueue instance to handle messaging
        private MessageQueue myMessageQueue = new MessageQueue();

        // Define a UserID variable, initially set to 0
        private int UserID = 0;

        //--------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Constructor of the Form1 class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Form1()
        {
            InitializeComponent();

            // Set the path of the message queue using UserID
            this.myMessageQueue.Path = @".\private$\messages" + this.UserID;

            // Set the message formatter to use XML formatter
            this.myMessageQueue.Formatter = new XmlMessageFormatter(
            new Type[] { typeof(string) });
        }

        //--------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the button click event to send data to the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSend_Click(object sender, EventArgs e)
        {            
            // Check if the message queue path exists
            if (MessageQueue.Exists(this.myMessageQueue.Path))
            {
                // If queue path exists, send data to the queue
                this.SendData2Queue();
            }
            else
            {
                // If queue path doesn't exist, create a new queue and send data
                MessageQueue.Create(this.myMessageQueue.Path);
                this.SendData2Queue();
            }
        }
        //--------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the button click event to read data from the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the message queue path exists
                if (MessageQueue.Exists(this.myMessageQueue.Path))
                {
                    // If queue path exists and there are messages in the queue, read the message
                    if (this.Count(this.myMessageQueue)>0)
                    {
                        var myMessage = this.myMessageQueue.Receive();
                        this.txtMessageReceived.Text = myMessage.Body.ToString();
                        return;
                    }
                }
                // If queue path doesn't exist or there are no messages, show a message box
                this.Count(this.myMessageQueue);
                MessageBox.Show("No Message");
            }
            catch (Exception ex) 
            { 
            MessageBox.Show(ex.ToString());
            }
        }
        //--------------------------------------------------------------------------------------------------------//

        /// <summary>
        ///  Method to send data to the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendData2Queue()
        {
            try
            {
                // Send the message with text from txtMessageToSend, an alien emoji, and timestamp
                this.myMessageQueue.Send(this.txtMessageToSend.Text.Trim() + J3QQ4.Emoji.Alien + "\r\n" +
                    DateTime.Now.ToString());
                this.myMessageQueue.Close();
                this.Text = "Message Count:" + this.Count(this.myMessageQueue);

            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.myMessageQueue.Close();
            }
        }
        //--------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Method to count the number of messages in the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public int Count(MessageQueue queue)
        {
            int count = 0;
            var emunerator = queue.GetMessageEnumerator();

            while(emunerator.MoveNext())
            { 
              count++;
            }

            this.Text = "Total Messages in Queue" + count; 
            return count;
        }
    }
}
        //---------------------------------------- END OF FILE -------------------------------------------------------//
