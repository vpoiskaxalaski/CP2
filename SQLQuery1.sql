use [Database]

select * from Users

select * from Tests

select * from Questions
select * from Answers
select * from CompletedTests

SELECT * FROM Questions 
join Tests on Questions.Test_Id = Tests.Id
where Tests.Topic = 'english'
except
select * from Questions
join Tests on Questions.Test_Id = Tests.Id
where Questions.Test_Id = 1