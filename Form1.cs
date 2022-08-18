using BetterConsole.Contract;
using System.ComponentModel;
using Timer = System.Windows.Forms.Timer;

namespace HandOfTheConsoler
{
    public partial class Form1 : Form
    {
        private readonly BindingSource bindingSource;
        private readonly BindingList<LogMessageViewModel> messages;

        private object queueLock = new();
        private List<LogMessage>[] queues =
        {
            new(), new()
        };
        private int incomingIndex = 0;

        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            bindingSource = new();
            messages = new();
            dataGridView1.AutoGenerateColumns = true;

            bindingSource.DataSource = messages;
            dataGridView1.DataSource = bindingSource;

            IPC.Instance.ConsumeAll(msg =>
            {
                lock (queueLock)
                {
                    queues[incomingIndex].Add(msg);
                }
            });

            timer = new();
            timer.Interval = 33;
            timer.Tick += (obj, evt) =>
            {
                List<LogMessage> toProcess;
                lock(queueLock)
                {
                    toProcess = queues[incomingIndex];
                    incomingIndex = (incomingIndex == 0) ? 1 : 0;
                }

                //messages.RaiseListChangedEvents = false;
                foreach (var msg in toProcess)
                {
                    if (msg.Control) continue;
                    LogMessageViewModel? prev = messages.Count > 0 ? messages.Last() : null;

                    if (prev?.MergesWith(msg) == true)
                    {
                        prev.MergedCount++;
                    }
                    else
                    {
                        messages.Add(new(msg));
                    }
                }
                //messages.RaiseListChangedEvents = true;
                //bindingSource.ResetBindings(false);
                toProcess.Clear();
            };
            timer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}