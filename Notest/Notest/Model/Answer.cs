namespace Notest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Spatial;

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
