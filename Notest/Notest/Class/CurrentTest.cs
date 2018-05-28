using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;


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
                    sfd.Title = "Save to print";
                    sfd.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
                    sfd.Filter = "Text files (*.doc)|*.doc|All files (*.*)|*.*";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.OpenFile()))
                        {                        
                            sw.WriteLine(test.Header.PadLeft(100));
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

                    }
                    MessageBox.Show("The test is ready to print", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch 
                {
                    MessageBox.Show("Print error", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
