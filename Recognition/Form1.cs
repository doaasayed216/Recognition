using System;
using System.Drawing;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Media;

namespace Recognition
{
    public partial class Form1 : Form
    {
		SpeechSynthesizer ss = new SpeechSynthesizer();
		SpeechRecognitionEngine sr = new SpeechRecognitionEngine();

		bool wake = false;
		public int count = 0;
        public string[] images = null;
        public string id;
        public string path = "..\\..\\keywords";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            string[] keywords = get_keywords(path);
			sr.SetInputToDefaultAudioDevice();
			sr.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(keywords))));
			sr.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Sr_SpeechRecogized);
			sr.RecognizeAsync(RecognizeMode.Multiple);
		}

		public string[] get_keywords(string path)
		{
			string[] dirs = Directory.GetDirectories(path);
			int length = dirs.Length;
			string[] keywords = new string[length + 3];

			for (int i = 0; i < length; i++)
			{
				FileInfo f = new FileInfo(dirs[i]);
				keywords[i] = f.Name;
			}

			keywords[length] = "hey reco";
			keywords[length + 1] = "next";
			keywords[length + 2] = "exit";

			return keywords;
		}


		public void show(string id, int i)
		{
			images = Directory.GetFiles(path + "\\" + id);
			if (images != null && images.Length > 0)
			{
				pictureBox1.Image = new Bitmap(images[i]);
				pictureBox1.Dock = DockStyle.Fill;

			}
			wake = false;
		}


		private void Sr_SpeechRecogized(object sender, SpeechRecognizedEventArgs e)
        {
			string speechSaid = e.Result.Text;
			if (speechSaid == "hey reco" && e.Result.Confidence > 0.95)
			{
				SoundPlayer player = new SoundPlayer(@"C:\Users\Lenovo\source\repos\RECO\RECO\sounds\google.wav");
				player.Play();
				wake = true;
			}

			else if (speechSaid == "exit" && e.Result.Confidence > 0.90)
			{
				this.Close();
			}

			else if (speechSaid == "next")
			{
				if (id != null)
				{
					count++;
					if (count >= images.Length)
						count = count - 1;
					show(id, count);
				}
			}

			else if(wake == true)
            {
				if (e.Result.Confidence > 0.90)
				{
					id = speechSaid;
					count = 0;
					show(id, count);
				}
			}

			
				

			
		}

		private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

	}
}
