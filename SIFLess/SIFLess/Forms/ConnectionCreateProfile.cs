﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SIFLess.Model;
using SIFLess.Model.Profiles;
using SIFLess.Properties;

namespace SIFLess
{
    public partial class ConnectionCreateProfile : Form
    {
        private bool _requiresvalidation = true;

        private SqlProfile _profile;
        public ConnectionCreateProfile(SqlProfile profile)
        {
            InitializeComponent();

            _profile = profile;

            profileTextBox.Text = profile.Name;
            connectionNameTextBox.Text = profile.ServerName;
            loginTextBox.Text = profile.Login;
            passwordTextBox.Text = profile.Password;


        }

        public ConnectionCreateProfile()
        {
            InitializeComponent();
        }



        private void validateButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(profileTextBox.Text))
            {
                MessageBox.Show("Need a Profile Name!");
                return;
            }

            if (string.IsNullOrWhiteSpace(connectionNameTextBox.Text))
            {
                MessageBox.Show("Enter a Server");
                return;
            }

            if (string.IsNullOrWhiteSpace(loginTextBox.Text))
            {
                MessageBox.Show("Enter a Login");
                return;
            }

            if (string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                MessageBox.Show("Enter a Password");
                return;
            }
            
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = connectionNameTextBox.Text;
            sqlBuilder.UserID = loginTextBox.Text;
            sqlBuilder.Password = passwordTextBox.Text;

            //check the connection and other things:
            using (SqlConnection connection =new SqlConnection(sqlBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    validateLoginLabel.ForeColor = Color.Green;
                }
                catch (Exception exception)
                {
                    validateLoginLabel.ForeColor = Color.Red;
                    MessageBox.Show("Error Opening SQL: " + exception.Message);
                    connection.Close();
                    return;
                }

                try
                {
                   SqlCommand command = new SqlCommand("SELECT @@VERSION", connection);
                    var result = command.ExecuteScalar().ToString();

                    if (result.IndexOf("SQL Server 2016") >=0 || result.IndexOf("SQL Server 2017") >=0)
                    {
                        validateVersionLabel.ForeColor = Color.Green;
                    }
                    else
                    {
                        validateVersionLabel.ForeColor = Color.Red;
                        MessageBox.Show("Invalid SQL Version: " + result);
                        return;
                    }
                }
                catch (Exception exception)
                {
                    validateVersionLabel.ForeColor = Color.Red;
                    MessageBox.Show("Error Validating Version: " + exception.Message);
                    return;
                }

                try
                {
                    SqlCommand command = new SqlCommand("SELECT has_perms_by_name(null, null, 'CREATE ANY DATABASE')",
                        connection);
                    var result = command.ExecuteScalar().ToString();

                    if (result == "1")
                    {
                        validateDBCreateLabel.ForeColor = Color.Green;

                    }
                    else
                    {
                        validateDBCreateLabel.ForeColor = Color.Red;
                        MessageBox.Show("Unable to Create Databases");
                        return;
                    }


                }
                catch (Exception exception)
                {
                    validateLoginLabel.ForeColor = Color.Red;
                    MessageBox.Show("Error Opening SQL: " + exception.Message);
                    return;
                }
                finally
                {
                    connection.Close();
                }
            }

            SifLessProfiles currentProfiles = ProfileManager.Fetch();

            if (_profile == null)
            {
                SqlProfile newProfile = new SqlProfile
                {
                    Name = profileTextBox.Text,
                    ServerName = connectionNameTextBox.Text,
                    Login = loginTextBox.Text,
                    Password = passwordTextBox.Text,
                    Id = Guid.NewGuid()
                };

                currentProfiles.SqlProfiles.Add(newProfile);
            }
            else
            {
                var profile = currentProfiles.SqlProfiles.Find(p => p.Id == _profile.Id);

                profile.Name = profileTextBox.Text;
                profile.ServerName = connectionNameTextBox.Text;
                profile.Login = loginTextBox.Text;
                profile.Password = passwordTextBox.Text;
            }


            ProfileManager.Update(currentProfiles);

            this.Close();

        }
    }
}
