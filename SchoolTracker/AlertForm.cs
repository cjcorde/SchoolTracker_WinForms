﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolTracker
{
    public partial class AlertForm : Form
    {
        #region FIELDS AND ENUMS

        private int x, y;

        // Defines a public enumeration named Action with three possible values: wait, start, and close.
        // This enumeration represents different states of the alert form.
        public enum Action
        {
            wait,
            start,
            close
        }

        // Defines a public enumeration named Type with four possible values: Success, Warning, Error, and Info.
        // This enumeration represents different types of alerts.
        public enum Type
        {
            Success,
            Warning,
            Error,
            Info
        }

        private AlertForm.Action action;

        #endregion

        public AlertForm()
        {
            InitializeComponent();
        }

        #region FUNCTION FOR TIMER TICK

        private void timer_Tick(object sender, EventArgs e)
        {
            // Event handler for the Timer's tick event.

            switch (action)
            {
                case Action.wait:
                    // If the action is "wait", set the timer interval to 5 seconds and change the action to "close".
                    timer.Interval = 5000;
                    action = Action.close;
                    break;
                case Action.start:
                    // If the action is "start", set the timer interval to 1 millisecond and gradually increase opacity.
                    timer.Interval = 1;
                    Opacity += 0.1;

                    // Move the form to the left until its X coordinate matches the target X coordinate.
                    if (x < Location.X)
                        Left--;
                    else
                        if (Opacity == 1.0)
                            action = Action.wait;
                    break;
                case Action.close:
                    // If the action is "close", set the timer interval to 1 millisecond and gradually decrease opacity.
                    timer.Interval = 1;
                    Opacity -= 0.1;

                    // Move the form to the left and close it when opacity reaches 0.
                    Left -= 3;
                    if (base.Opacity == 0.0)
                        base.Close();
                    break;
            }
        }

        #endregion

        #region FUNCTION FOR REMOVE BUTTON TO CLEAR THE ALERT 

        private void removeBtn_Click(object sender, EventArgs e)
        {
            // Event handler for the remove button's click event.

            timer.Interval = 1;
            action = Action.close;
        }

        #endregion

        #region FUNCTION TO SHOW ALERT

        public void ShowAlert(string msg, Type type)
        {
            // Method to display an alert message with a specified type.

            Opacity = 0.0;
            StartPosition = FormStartPosition.Manual;
            string fname;

            // Loop to find an available form name.
            for (int i = 1; i < 10; i++)
            {
                fname = "alert" + i.ToString();
                AlertForm form = (AlertForm)Application.OpenForms[fname];

                // If no form with the current name exists, use this name for the form.
                if (form == null)
                {
                    Name = fname;
                    x = Screen.PrimaryScreen.WorkingArea.Width - Width + 15; // Calculate the X coordinate.
                    y = Screen.PrimaryScreen.WorkingArea.Height - Height * i - 5 * i; // Calculate the Y coordinate.
                    Location = new Point(this.x, this.y); // Set the form's location.

                    break;
                }

            }

            x = Screen.PrimaryScreen.WorkingArea.Width - base.Width - 5; // Calculate the X coordinate for the form.

            // Set the form's appearance based on the specified alert type.
            switch (type)
            {
                case Type.Success:
                    pictureBox1.Image = SchoolTracker.Properties.Resources.success;
                    BackColor = Color.SeaGreen;
                    break;
                case Type.Error:
                    pictureBox1.Image = SchoolTracker.Properties.Resources.error;
                    BackColor = Color.DarkRed;
                    break;
                case Type.Info:
                    pictureBox1.Image = SchoolTracker.Properties.Resources.info;
                    BackColor = Color.RoyalBlue;
                    break;
                case Type.Warning:
                    pictureBox1.Image = SchoolTracker.Properties.Resources.warning;
                    BackColor = Color.DarkOrange;
                    break;
            }


            labelMessage.Text = msg; // Set the alert message.

            Show();
            action = Action.start; // Set the action to "start" for opening animation.
            timer.Interval = 1;
            timer.Start();
        }

        #endregion
    }
}
