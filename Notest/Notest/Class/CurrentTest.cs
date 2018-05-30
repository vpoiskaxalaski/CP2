using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;

namespace Notest
{
    public static class CurrentTest
    {
        public static Test  test {get; set;} 
        
        public static void Print()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                try
                {
                    sfd.Title = "Save to document";
                    sfd.InitialDirectory = "E:\\University\\CP\\tests";
                    sfd.Filter = "Text files (*.doc)|*.doc|All files (*.*)|*.*";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.OpenFile()))
                        {
                            sw.WriteLine(test.Header.ToUpper());
                            sw.WriteLine();
                            int QuestionCount = 1;

                            foreach (Question q in test.Questions)
                            {
                                sw.WriteLine(QuestionCount.ToString() + ". " + q.Question1);
                                QuestionCount++;
                                                               
                                int AnswerCount = 0;
                                foreach (Answer a in q.Answers)
                                {
                                    sw.WriteLine("\t" + (char)(97+AnswerCount) + ". " + a.Answer1);
                                    AnswerCount++;
                                }
                            }
                        }
                        System.Diagnostics.Process.Start(sfd.FileName);

                    }                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File error: "+ ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
