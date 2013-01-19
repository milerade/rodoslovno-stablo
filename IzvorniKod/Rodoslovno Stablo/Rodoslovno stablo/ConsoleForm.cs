﻿using ApplicationLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rodoslovno_stablo
{
    public partial class ConsoleForm : Form
    {
        TextWriter _writer = null;
        

        private static ApplicationLogic.QueryProcessor qpro;

        public QueryProcessor MyQueryProcessor
        {
            get
            {
                return qpro;
            }
        }

      

        public ConsoleForm()
        {
            InitializeComponent();
            comboBoxInput.Select();
            

            qpro = new ApplicationLogic.QueryProcessor(QueryDisambiguator, GetLine);
            

        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            _writer = new TextBoxStreamWriter(textBoxOutput);
            Console.SetOut(_writer);
            refreshQueries();
        }
       
        static Person QueryDisambiguator(IEnumerable<Person> kandidati, string pitanje = "")
        {
            // TODO resolvanje dvosmislenosti upita
            return kandidati.ElementAt(0);
        }

        static string GetLine()
        {
            return System.Console.ReadLine();
        }

        private void executeText(string request)
        {
            Query q = new Query();
            q.command = request;
            SharedObjects.userManager.StoreQuery(q);

            comboBoxInput.Text = "";
            Console.WriteLine("> "+request);
            qpro.ProcessQuery(request);
            refreshQueries();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            executeText("help");
            comboBoxInput.Select();
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            

            executeText(comboBoxInput.Text);
            comboBoxInput.Select();
        }

        private void ConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;

        }
        private void refreshQueries() {
            comboBoxInput.Items.Clear();
            IEnumerable<Query> list = SharedObjects.userManager.GetQueries();
            list = list.Reverse().Take(7);


            foreach (Query item in list)
                comboBoxInput.Items.Add(item.command);
        }
       

    }
}
