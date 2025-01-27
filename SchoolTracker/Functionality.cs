﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Guna.UI2.WinForms;
using QRCoder;
using System.Data;
using System.Globalization;

namespace SchoolTracker
{
    class Functionality
    {   
        #region FUNCTION THAT WILL RUN SPECIFIC WEBSITE BASED ON THE GIVEN PARAMETER

        // This method is designed to open a specific web URL based on the given index.
        public void OpenWeb(int num)
        {
            // An array containing URLs to various websites.
            string[] urls = {
                "https://www.pup.edu.ph/",
                "https://www.pup.edu.ph/terms/",
                "https://www.pup.edu.ph/privacy/",
                "https://login.microsoftonline.com/common/oauth2/authorize?client_id=c9a559d2-7aab-4f13-a6ed-e7e9c52aec87&resource=c9a559d2-7aab-4f13-a6ed-e7e9c52aec87&response_type=code%20id_token&scope=openid%20profile&state=OpenIdConnect.AuthenticationProperties%3DeyJ2ZXJzaW9uIjoxLCJkYXRhIjp7IklkZW50aXR5UHJvdmlkZXIiOiJBUWlFUTNsTk1vREpEZk1ubG5pNVdzRDdZVWgxZE1sd2NkdElGeU9Hb0llMllBUURQV3c2cVNjY0s3RDlnRkJyUVJXVThCalU3R0VUa2gzNDRXMkxvVXciLCIucmVkaXJlY3QiOiJodHRwczovL2Zvcm1zLm9mZmljZS5jb20vUGFnZXMvUmVzcG9uc2VQYWdlLmFzcHg_aWQ9Y1lXcFRlcmNPVWlQc1F2ZFhjbHAtVlMyTmdXbHlFZEh2TnRnQkJVQjRCZFVRazFXT0VSUlZVbzRXamhTU2pCT1dsaFhNRWN4VlVwWlR5NHUmc2lkPTZmM2MwNWI3LWU4ZmEtNGI0Ny05MGJhLTQzMjUyMjk5YTM4MyJ9fQ&response_mode=form_post&nonce=638218355774463644.Yjc5ZDQ2MTAtM2U3Yi00YmYyLTgxNWQtMjQ4YTMzN2E2ZjM0MmZjNjdiYmUtYjNmYS00MzI2LWFkOTEtZmI5ZjMyZWZkMTg1&redirect_uri=https%3A%2F%2Fforms.office.com%2Flanding&msafed=0&x-client-SKU=ID_NET472&x-client-ver=6.16.0.0&sso_reload=true"
            };

            // Get the specific website URL based on the given index.
            string websiteUrl = urls[num];

            // Attempt to open the specified URL using the default web browser.
            // If an exception occurs, display an error message.
            try
            {
                System.Diagnostics.Process.Start(websiteUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening website: {ex.Message}", "PUP-SIS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region FUNCTION TO COMPARE TWO IMAGES

        public bool ImageEquals(Image image1, Image image2)
        {
            // Enter a using block to ensure proper disposal of resources
            using (MemoryStream ms1 = new MemoryStream(), ms2 = new MemoryStream())
            {
                // Save the content of the first and second image to the MemoryStream ms1 and ms2.
                image1.Save(ms1, image1.RawFormat);
                image2.Save(ms2, image2.RawFormat);

                // Return the result of the comparison between the byte arrays obtained from ms1 and ms2
                // StructuralComparisons.StructuralEqualityComparer.Equals() is used to compare the arrays
                return StructuralComparisons.StructuralEqualityComparer.Equals(ms1.ToArray(), ms2.ToArray());
            }
        }

        #endregion

        #region FUNCTION TO VALIDATE STUDENT AGE AND BIRTH DATE

        // Calculate the age based on birth date and check if it matches the selected age.
        public bool ValidateStudentAgeInBDate(Guna2DateTimePicker bDatePicker, int selectedAge)
        {
            int age = DateTime.Today.Year - bDatePicker.Value.Year;

            if (bDatePicker.Value.AddYears(age) > DateTime.Today)
                age--;

            return age == selectedAge;
        }

        #endregion

        #region FUNCTION TO DISPLAY ALERT

        // This method is used to display an alert message using a custom AlertForm.
        public void Alert(string msg, AlertForm.Type type)
        {
            AlertForm alertForm = new AlertForm();

            // Call the ShowAlert method of the AlertForm instance, passing the message and alert type as parameters.
            alertForm.ShowAlert(msg, type);
        }

        #endregion

        #region FUNCTION TO GENERATE QR CODE

        // This method generates a QR code as a Bitmap image based on the given 'code'.
        public Bitmap GetCode(string code)
        {
            QRCodeGenerator generator = new QRCodeGenerator();

            // Create QRCodeData using the 'code' and specifying error correction level (QRCodeGenerator.ECCLevel.Q).
            QRCodeData data = generator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);

            // Create a QRCode instance with the generated data.
            QRCode qRCode = new QRCode(data);

            // Generate a graphic representation of the QR code with the following parameters:
            // - Size of each module (20 pixels in this case).
            // - Color of the modules (Maroon).
            // - Background color (White).
            // - Optionally, you can use a custom logo if the condition 'true' is met,
            //   otherwise, set it to null.
            return qRCode.GetGraphic(
                20,                         // Size of each module (pixels).
                Color.Maroon,                // Color of the QR code modules.
                Color.White,                // Background color.
                true ? new Bitmap(Properties.Resources.pngkey_com_phillies_logo_png_528919) : null
            );
        }

        #endregion

        #region FUNCTION TO GENERATE TEMPORARY ENROLEE ACCOUNT

        static DBAccess objDBAccess = new DBAccess(); // Creates an instance of the DBAccess class for database operations.
        static DataTable appNumber = new DataTable();  // Creates a DataTable to hold the ApplicantNumber data.

        // Method to generate an enrolee number.
        public string GenerateEnroleeNumber()
        {
            string query = "Select * from ApplicantNumber"; // SQL query to select data from the 'ApplicantNumber' table.
            objDBAccess.readDatathroughAdapter(query, appNumber); // Reads data from the database into the 'appNumber' DataTable.

            int num = Convert.ToInt32(appNumber.Rows[0]["Number"]) + 1; // Extracts the 'Number' column value and increments it by 1.

            // Generates an enrollment number with the format: <Year>-<PaddedNumber>-E.
            string enroleeNumber = $"{DateTime.Now.Year}-{num.ToString("D5")}-E";

            return enroleeNumber; // Returns the generated enrollment number.
        }

        // Method to generate a random password.
        public string GenerateRandomPassword()
        {
            Random random = new Random(); // Creates a random number generator.

            string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"; // Valid characters for the password.

            // Generates an 8-character password by selecting random characters from 'validChars'.
            string password = new string(Enumerable.Repeat(validChars, 8)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());

            return password; // Returns the generated random password.
        }

        #endregion
    }
}