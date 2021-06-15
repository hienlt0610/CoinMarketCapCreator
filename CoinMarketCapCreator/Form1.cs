
using CoinMarketCapCreator.Models;
using OpenQA.Selenium.Chrome;
using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoinMarketCapCreator
{
    public partial class Form1 : Form
    {
        List<Email> emails;
        bool isRunning = false;
        CancellationTokenSource tokenSource;
        public Form1()
        {
            InitializeComponent();
            AppUtils.SetDoubleBuffering(lv_reg_info, true);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //using (var client = new ImapClient())
            //{
            //    try
            //    {
            //        await client.ConnectAsync("outlook.office365.com", 993, true);
            //        Trace.WriteLine("Connected to Imap");
            //        await client.AuthenticateAsync("analorrika@hotmail.com", "gKsa6p4ht");
            //        var inbox = client.Inbox;
            //        await inbox.OpenAsync(FolderAccess.ReadOnly);
            //        foreach (var uid in inbox.Search(SearchQuery.NotSeen))
            //        {
            //            var message = inbox.GetMessage(uid);
            //            Trace.WriteLine(message.Subject);
            //        }

            //        //List<long> uids = await client.SearchAsync(Flag.All);
            //        //Trace.WriteLine("Total email: "+ uids.Count);
            //        //for (int i=0; i< uids.Count; i++)
            //        //{
            //        //    var email = new MailBuilder().CreateFromEml(await client.GetHeadersByUIDAsync(uids[i]));
            //        //    Trace.WriteLine(email.Subject);
            //        //}

            //        //await client.CloseAsync();
            //        //Trace.WriteLine("Disconnect");
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //    finally
            //    {
            //        await client.DisconnectAsync(true);
            //    }
            //}
        }

        string pathToFile = "";//to save the location of the selected object
        private async void btn_pick_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "TXT files|*.txt";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                pathToFile = theDialog.FileName;//doesn't need .tostring because .filename returns a string// saves the location of the selected object

            }

            if (File.Exists(pathToFile))// only executes if the file at pathtofile exists//you need to add the using System.IO reference at the top of te code to use this
            {
                emails = (await File.ReadAllLinesAsync(pathToFile))
                    .ToList()
                    .Select(i =>
                    {
                        var splits = i.Split("|");
                        if (splits.Length > 1)
                        {
                            return new Email(splits[0], splits[1]);
                        }
                        return null;
                    })
                    .Where(i => i != null).ToList();
                initEmailListView(emails);
            }
        }

        private void initEmailListView(IEnumerable<Email> emails)
        {
            lv_reg_info.Items.Clear();
            var rowsInfo = emails.Select(e => {
                var rowInfo = new RowInfo();
                rowInfo.emailAddress = e.email;
                rowInfo.emailPassword = e.password;
                return rowInfo;
            });
            lv_reg_info.Items.AddRange(rowsInfo.Select(e => {
                var listViewItem = new ListViewItem(makeSubItems(e));
                listViewItem.Tag = e;
                return listViewItem;
            }).ToArray());
        }

        private string[] makeSubItems(RowInfo rowInfo)
        {
            return new string[] { 
                rowInfo.emailAddress,
                rowInfo.username,
                rowInfo.password,
                rowInfo.message
            };
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (lv_reg_info.Items.Count == 0)
            {
                MessageBox.Show("Vui lòng load danh sách email");
                return;
            }

            if (isRunning)
            {
                stopTask();
            }
            else
            {
                startTask();
            }
        }

        private void startTask()
        {
            initEmailListView(emails);
            btn_start.Enabled = false;
            if (tokenSource != null)
            {
                tokenSource.Dispose();
            }
            tokenSource = new CancellationTokenSource();
            Task.Run(() => runBackgroundTask(tokenSource.Token), tokenSource.Token);
            onTaskRunning();
        }

        private void stopTask()
        {
            btn_start.Enabled = false;
            tokenSource.Cancel();
        }

        private async void runBackgroundTask(CancellationToken token)
        {
            try
            {
                Trace.WriteLine("Background task running....");
                token.ThrowIfCancellationRequested();
                int batchSize = 4;
                for (int i = 0; i < emails.Count; i += batchSize)
                {
                    List<Task> tasks = new List<Task>();
                    var batchAddress = emails.Skip(i).Take(batchSize);
                    for (int j = 0; j < batchSize; j++)
                    {
                        var index = i + j;
                        if(index < emails.Count)
                        {
                            var task = onSubTaskRun(index, token);
                            tasks.Add(task);
                        }
                    }
                    await Task.WhenAll(tasks);
                    await Task.Delay(1000);
                }
                Invoke((MethodInvoker)delegate { onTaskSuccessfuly(); });
            }
            catch (OperationCanceledException)
            {
                Invoke((MethodInvoker)delegate { onTaskCanceled(); });
            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate { onTaskFailed(ex); });
            }
            finally
            {
                Invoke((MethodInvoker)delegate { onTaskCompleted(); });
            }
        }

        private void onTaskSuccessfuly()
        {
            MessageBox.Show("Hoàn thành task");
        }

        private async Task onSubTaskRun(int index, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var username = AppUtils.GenerateRandomUsername();
            var pwd = new Password();
            var password = pwd.Next();
            Invoke((MethodInvoker)delegate {
                fillUserNamePasswordAtRow(index, username, password);
            });
            await Task.Delay(500);
            Invoke((MethodInvoker)delegate {
                fillStatusAtRow(index, "Đã thành công", "success");
            });
        }

        private void fillStatusAtRow(int index, string message, string status)
        {
            var rowInfo = lv_reg_info.Items[index].Tag as RowInfo;
            rowInfo.message = message;
            rowInfo.status = status;
            updateRowInfo(index, rowInfo);
        }

        private void updateRowInfo(int index, RowInfo rowInfo)
        {
            var listViewItem = lv_reg_info.Items[index];
            var subItems = listViewItem.SubItems;
            subItems.Clear();
            subItems.AddRange(makeSubItems(rowInfo));
            Color rowColor;
            if (rowInfo.status == "success")
            {
                rowColor = Color.Green;
            }
            else
            {
                rowColor = Color.Black;
            }
            for (int i=0;i< subItems.Count;i++)
            {
                subItems[i].ForeColor = rowColor;
            }
            listViewItem.Tag = rowInfo;
        }

        private void fillUserNamePasswordAtRow(int index, string username, string password)
        {
            var rowInfo = lv_reg_info.Items[index].Tag as RowInfo;
            rowInfo.username = username;
            rowInfo.password = password;
            updateRowInfo(index, rowInfo);
        }

        private void onTaskCompleted()
        {
            isRunning = false;
            btn_start.Enabled = true;
            btn_start.Text = "Start";
        }

        private void onTaskRunning()
        {
            isRunning = true;
            btn_start.Enabled = true;
            btn_start.Text = "Stop";
        }

        private void onTaskCanceled()
        {
            Trace.WriteLine("onTaskCanceled");
            MessageBox.Show("Đã huỷ tiến trình");
        }

        private void onTaskFailed(Exception ex)
        {
            MessageBox.Show(ex.Message);
            Trace.WriteLine("onTaskFailed: "+ex.ToString());
        }

        private ChromeDriver initWebDriver()
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            var options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            //options.AddArgument("headless");
            return new ChromeDriver(driverService, options);
        }
    }
}
