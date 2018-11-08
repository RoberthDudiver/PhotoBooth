using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace Twitterpost
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private async  void Form1_Load(object sender, EventArgs e)
        {
            //var twitter = new TwitterApi(ConsumerKey, ConsumerKeySecret, AccessToken, AccessTokenSecret);
            var twitter = new TwitterApi("IEgzSo0YDyybAWf4Ec2s2R5PY", "7VMUS6xqv5SEF0uCk1qNqoJabO7JI6mRbvfV6FQb0WOYf1bI3b", "121770924-1yid1npyGLAYB4ujlVJQD1KpE1Id8MMIY3JoTtpG", "XHNZM8VlQNQpx5By0iAha9PgekEkWwsJifPyg7gt5Gbof");
            Task<string> task = Task.Run(() => twitter.Tweet("Estoy Orgulloso de mi Mejor Amigo @Roberthdum, El mejor programador del mundo"));         
             var response1 = await task;
             var response = response1;
             Console.WriteLine(response);
             MessageBox.Show(response);
        }
    }
}
