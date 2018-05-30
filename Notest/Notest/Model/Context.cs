namespace Notest
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;

    public class Context : DbContext
    {
        public Context()
            : base("name=Model")
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<CompletedTest> CompletedTest { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public Guid Password { get; set; }
        public string UserType { get; set; }
    }

    public partial class Test
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Topic { get; set; }
        public string Author { get; set; }

        public List<Question> Questions = new List<Question>();
    }

    public partial class CompletedTest
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public int TestId { get; set; }
        public int Result { get; set; }
        public string Date { get; set; }
    }

    public partial class Question
    {
       public int Id { get; set; }
        public string Question1 { get; set; }
        public int Cost { get; set; }
        public string Image { get; set; }
        public int Test_Id { get; set; }  

        public List<Answer> Answers = new List<Answer>();

        public static List<Question> ChangeFromDb(IQueryable<Question> dbSet)
        {
            List<Question> questions = new List<Question>();
            foreach (var q in dbSet)
            {
                Question question = new Question()
                {
                    Id = q.Id,
                    Question1 = q.Question1,
                    Cost = q.Cost,
                    Image = q.Image,
                    Test_Id = q.Test_Id
                };
                questions.Add(question);
            }
            return questions;
        }

    }

    public partial class Answer
    {
        public int Id { get; set; }
        public string Answer1 { get; set; }
        public bool IsRight { get; set; }
        public int Question_Id { get; set; }

        public static List<Answer> ChangeFromDb(IQueryable<Answer> dbSet)
        {
            List<Answer> answers = new List<Answer>();
            foreach (var a in dbSet)
            {
                Answer answer = new Answer()
                {
                    Id = a.Id,
                    Answer1 = a.Answer1,
                    IsRight = a.IsRight,
                    Question_Id = a.Question_Id
                };
                answers.Add(answer);
            }
            return answers;
        }
    }
}