﻿using Newtonsoft.Json;
using SIFLess.Model;
using SIFLess.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using SIFLess.Controls;
using SIFLess.Model.Configuration;
using SIFLess.Model.Profiles;

namespace SIFLess
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        #region Events
        private void MainForm_Load(object sender, EventArgs e)
        {


            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }

            RefreshSitecoreProfiles();
            RefreshConnectionProfiles();
            RefreshSolrProfiles();

            //this.Text = $"SIF-less v{this.ProductVersion}";

            //if (!File.Exists(_instancesListPath))
            //    File.WriteAllText(_instancesListPath, "<Instances />");

            //instanceListWatcher.Path = Path.GetDirectoryName(_instancesListPath);
            //instanceListWatcher.Filter = Path.GetFileName(_instancesListPath);

            ////We're not quite ready for prime time on this
            //tabControl1.TabPages.Remove(tabPage3);
            ////LoadInstances();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void manageSitecoreProfilesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SitecoreProfileManager mgr = new SitecoreProfileManager();
            mgr.ShowDialog();
            RefreshSitecoreProfiles();
        }

        private void manageConnectionProfileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ConnectionProfileManager mgr = new ConnectionProfileManager();
            mgr.ShowDialog();
            RefreshConnectionProfiles();

        }

        private void manageSolrLinkButtonsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SolrProfileManager mgr = new SolrProfileManager();
            mgr.ShowDialog();
            RefreshSolrProfiles();
        }

        private void profileListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var profile = profileListBox.SelectedItem as SitecoreProfile;

            //Load all the files for the profile
            var configuration = Utility.GetInstanceConfiguration(profile.Topology, profile.Version);

            var fields = new List<Field>();

            foreach (var file in configuration.Files)
            {
                if (file.FieldMaps != null)
                {
                    foreach (var fieldMap in file.FieldMaps.Fields)
                    {
                        if (fields.Find(f => f.Name == fieldMap.Name) == null)
                        {
                            fields.Add(fieldMap);
                        }
                    }
                }
            }

            customFieldsGroupBox.Controls.Clear();

            var position = 20;
            foreach (var field in fields)
            {
                switch (field.Type.ToLower())
                {
                    case "text":
                        customFieldsGroupBox.Controls.Add(new StringControl(field.Label, field.Map, field.Description) { Top = position, Left = 20 });
                        break;
                }

                position += 25;
            }
        }
        #endregion

        #region Methods

        public void RefreshSitecoreProfiles()
        {
            profileListBox.Items.Clear();

            var currentProfiles = ProfileManager.Fetch();

            currentProfiles.SiteforeProfiles?.ForEach(p => profileListBox.Items.Add(p));
        }

        public void RefreshConnectionProfiles()
        {
            connectionListBox.Items.Clear();

            var currentProfiles = ProfileManager.Fetch();

            currentProfiles.SqlProfiles?.ForEach(p => connectionListBox.Items.Add(p));
        }

        public void RefreshSolrProfiles()
        {
            solrListBox.Items.Clear();

            var currentProfiles = ProfileManager.Fetch();

            currentProfiles.SolrProfiles?.ForEach(p => solrListBox.Items.Add(p));
        }
        #endregion

        private void installButton_Click(object sender, EventArgs e)
        {
            //var ezText = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "EZ.ps1"));

            //var solrPath = solrURLTextBox.Text.EndsWith("/")
            //  ? solrURLTextBox.Text.Substring(0, solrURLTextBox.TextLength - 1)
            //  : solrURLTextBox.Text;
            //ezText = ezText.Replace("[SC_PREFIX]", prefixTextBox.Text);
            //ezText = ezText.Replace("[SCRIPT_ROOT]", configTextBox.Text);
            //ezText = ezText.Replace("[XCONNECT_NAME]", xConnectName.Text);
            //ezText = ezText.Replace("[SITE_NAME]", siteNameTextBox.Text);
            //ezText = ezText.Replace("[SOLR_URL]", solrPath);
            //ezText = ezText.Replace("[SOLR_FOLDER]", solrFolderTextBox.Text);
            //ezText = ezText.Replace("[SOLR_SERVICE]", solrServiceTextBox.Text);
            //ezText = ezText.Replace("[SQL_SERVER]", sqlServerTextBox.Text);
            //ezText = ezText.Replace("[SQL_USER]", sqlLoginTextBox.Text);
            //ezText = ezText.Replace("[SQL_PASSWORD]", sqlPasswordTextBox.Text);
            //ezText = ezText.Replace("[XCONNECT_PACKAGE]", xConnectPackageTextBox.Text);
            //ezText = ezText.Replace("[SITECORE_PACKAGE]", sitecorePackageTextBox.Text);
            //ezText = ezText.Replace("[LICENSE_XML]", licenseTextBox.Text);

            //var fileName = $"SIFless-EZ-{DateTimeOffset.Now.ToUnixTimeSeconds()}.ps1";
            //var fullFileName = Path.Combine(Environment.CurrentDirectory, fileName);
            //File.WriteAllText(fullFileName, ezText);


            //var ezUninstallText = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "EZ-Uninstall.ps1"));

            //ezUninstallText = ezUninstallText.Replace("[SC_PREFIX]", prefixTextBox.Text);
            //ezUninstallText = ezUninstallText.Replace("[SCRIPT_ROOT]", configTextBox.Text);
            //ezUninstallText = ezUninstallText.Replace("[XCONNECT_NAME]", xConnectName.Text);
            //ezUninstallText = ezUninstallText.Replace("[SITE_NAME]", siteNameTextBox.Text);
            //ezUninstallText = ezUninstallText.Replace("[SOLR_URL]", solrPath);
            //ezUninstallText = ezUninstallText.Replace("[SOLR_FOLDER]", solrFolderTextBox.Text);
            //ezUninstallText = ezUninstallText.Replace("[SOLR_SERVICE]", solrServiceTextBox.Text);
            //ezUninstallText = ezUninstallText.Replace("[SQL_SERVER]", sqlServerTextBox.Text);
            //ezUninstallText = ezUninstallText.Replace("[SQL_USER]", sqlLoginTextBox.Text);
            //ezUninstallText = ezUninstallText.Replace("[SQL_PASSWORD]", sqlPasswordTextBox.Text);
            //ezUninstallText = ezUninstallText.Replace("[XCONNECT_PACKAGE]", xConnectPackageTextBox.Text);
            //ezUninstallText = ezUninstallText.Replace("[SITECORE_PACKAGE]", sitecorePackageTextBox.Text);
            //ezUninstallText = ezUninstallText.Replace("[LICENSE_XML]", licenseTextBox.Text);

            //var uninstallFileName = $"SIFless-EZUninstall-{DateTimeOffset.Now.ToUnixTimeSeconds()}.ps1";
            //var fullUninstallFileName = Path.Combine(Environment.CurrentDirectory, uninstallFileName);
            //File.WriteAllText(fullUninstallFileName, ezUninstallText);

            //if (!ezGenOnlyCheckbox.Checked)
            //{
            //    var exeForm = new ExecuteForm();
            //    exeForm.Show();
            //    exeForm.Run(fullFileName);
            //}
        }

        private void runButton_Click_1(object sender, EventArgs e)
        {
            //foreach (var parameter in _parameterList.Parameters)
            //{
            //    //Find the corresponding Control
            //    var control = _controls.Find(c => c.Field == parameter.Name);

            //    if (string.IsNullOrEmpty(control?.Value))
            //    {
            //        MessageBox.Show("Missing Value: " + control.Field, "Requird Field Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    parameter.Value = control.Value;
            //}


            //var paramsListBuilder = new StringBuilder();
            //foreach (var param in _parameterList.Parameters)
            //    paramsListBuilder.AppendFormat(" -{0} {1}", param.Name, param.Value);

            //var templateText = File.ReadAllText(_configFile);

            //templateText = templateText.Replace("[CONFIG]", _configFile);
            //templateText = templateText.Replace("[PARAMS]", paramsListBuilder.ToString());

            //var configFileName = Path.GetFileNameWithoutExtension(_configFile);
            //var tempFileName = $"SIFless-{configFileName}-{DateTimeOffset.Now.ToUnixTimeSeconds()}.ps1";
            //var fullFileName = Path.Combine(Environment.CurrentDirectory, tempFileName);

            //File.WriteAllText(fullFileName, templateText);

            //if (!hcGenerateCheckbox.Checked)
            //{
            //    var exeForm = new ExecuteForm();
            //    exeForm.Show();
            //    exeForm.Run(fullFileName);
            //}
        }
        
    }
}
