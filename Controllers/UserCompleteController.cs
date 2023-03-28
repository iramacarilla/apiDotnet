using DotnetApi.Data;
using DotnetApi.Dtos;
using DotnetApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Controllers;

[ApiController] //attribute for class
[Route("[controller]")]//route attribute
public class UserCompleteController : ControllerBase
{
  DataContextDapper _dapper;
  

    public UserCompleteController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }
    [HttpGet("TestConnection")]
    public DateTime TestConnection() {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers/{userId}/{isActive}")] //
    //public IEnumerable<User> GetUsers()
    public IEnumerable<UserComplete> GetUsers(int userId, bool isActive)
    {
         string sql = @"EXEC TutorialAppSchema.spUsers_Get";
         string parameters = "";
         if (userId != 0) {  
         parameters += ", @UserId=" + userId.ToString();
         } 
          if (isActive) {  
         parameters += ", @Active=" + isActive.ToString();
         } 

         sql += parameters.Substring(1);//(1, parameters.Length)

         IEnumerable<UserComplete> users = _dapper.LoadData<UserComplete>(sql);

         return users;

        //return new string[] {"user1", "user2" };
        /*return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();*/
    }
    /*[HttpGet("GetSingleUsers/{userId}")]
    //public IEnumerable<User> GetUsers()
    public User GetSingleUsers(int userId)
    {
          string sql = @"EXEC TutorialAppSchema.spUsers_Get  @UserId=" + userId.ToString();
         User user = _dapper.LoadDataSingle<User>(sql);

         return user;

    }*/

    [HttpPut("EditUser")]
    //public IEnumerable<User> GetUsers()
    public IActionResult EditUsers(User user) //RETURNS responce and tell what happen without necessarily returning
    {
        string sql = @"
         UPDATE TutorialAppSchema.Users
       SET [FirstName] = '"+ user.FirstName + 
          "',[LastName] = '"+ user.LastName + 
          "', [Email] = '"+ user.Email +
          "', [Gender] = '"+ user.Gender + 
          "', [Active] = '"+ user.Active + 
        "' WHERE UserId = " + user.UserId;
        if (_dapper.ExecuteSql(sql))
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to update user");

    }

    [HttpPost("AddUser")]
    //public IEnumerable<User> GetUsers()
    public IActionResult AddUsers(UserToAddDto user) //RETURNS responce and tell what happen without necessarily returning
    {
         string sql = @"
          INSERT INTO TutorialAppSchema.Users(
     
         [FirstName]
         , [LastName]
         , [Email]
         , [Gender]
         , [Active]
   ) VALUES (" +
         "'"+ user.FirstName + 
          "',  '"+ user.LastName + 
          "','"+ user.Email +
          "',  '"+ user.Gender + 
          "',  '"+ user.Active + 
        "')";
           if (_dapper.ExecuteSql(sql))
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to add user");

    }

     [HttpDelete("DeleteUser/{userId}")]
    //public IEnumerable<User> GetUsers()
    public IActionResult DeleteUsers(int userId) //RETURNS responce and tell what happen without necessarily returning
    {
        string sql = @"
         SELECT FROM TutorialAppSchema.Users
         WHERE UserId = " + userId.ToString();
        if (_dapper.ExecuteSql(sql))
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to delete user");

    }
    /*[HttpGet("UserSalary/{userId}")]
    //public IEnumerable<User> GetUsers()
    public IEnumerable<UserSalary> GetUserSalary(int userId) //RETURNS responce and tell what happen without necessarily returning
    {
        return _dapper.LoadData<UserSalary>(@"
         SELECT UserSalary.UserId, UserSalary.Salary
          FROM TutorialAppSchema.UserSalary
         WHERE UserId = " + userId
         );
       /* if (_dapper.ExecuteSql(sql))
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to delete user");*/

    //}
    [HttpPost("UserSalary")]
    //public IEnumerable<User> GetUsers()
    public IActionResult PostUserSalary(UserSalary userSalaryForInsert) //RETURNS responce and tell what happen without necessarily returning
    {
         string sql = @"
          INSERT INTO TutorialAppSchema.UserSalary(
         [UserId]
         , [Salary]
        
   ) VALUES (" +
         "'"+ userSalaryForInsert.UserId + 
          "',  '"+ userSalaryForInsert.Salary + 
        "')";
           if (_dapper.ExecuteSql(sql))
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Adding user salary failed on save");

    }
     [HttpPut("UserSalary")]
    //public IEnumerable<User> GetUsers()
    public IActionResult EditUserSalary(UserSalary userSalaryForUpdate) //RETURNS responce and tell what happen without necessarily returning
    {
        string sql = @"
         UPDATE TutorialAppSchema.UserSalary
       SET [Salary] = '"+ userSalaryForUpdate.Salary + 
        "' WHERE UserId = " + userSalaryForUpdate.UserId;
        if (_dapper.ExecuteSql(sql))
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to update user salary");

    }
        [HttpDelete("UserSalary/{userId}")]
    //public IEnumerable<User> GetUsers()
    public IActionResult DeleteUserSalary(int userId) //RETURNS responce and tell what happen without necessarily returning
    {
        string sql = @"
         SELECT FROM TutorialAppSchema.UserSalary
         WHERE UserId = " + userId.ToString();
        if (_dapper.ExecuteSql(sql))
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to delete user salary");

    }
      /*[HttpGet("UserJobInfo/{userId}")]
    //public IEnumerable<User> GetUsers()
    public IEnumerable<UserJobInfo> GetUserJobInfo(int userId) //RETURNS responce and tell what happen without necessarily returning
    {
        return _dapper.LoadData<UserJobInfo>(@"
         SELECT UserJobInfo.UserId, UserJobInfo.JobTitle, UserJobInfo.Department
          FROM TutorialAppSchema.UserJobInfo
         WHERE UserId = " + userId
         );
     

    }*/
    [HttpPost("UserJobInfo")]
    //public IEnumerable<User> GetUsers()
    public IActionResult PostUserJobInfo(UserJobInfo UserJobInfoForInsert) //RETURNS responce and tell what happen without necessarily returning
    {
         string sql = @"
          INSERT INTO TutorialAppSchema.UserJobInfo(
         [UserId]
         , [JobbTitle]
          , [Department]
        
   ) VALUES (" + UserJobInfoForInsert.UserId 
          +   "',  '"+ UserJobInfoForInsert.JobTitle +
            "',  '"+ UserJobInfoForInsert.Department +  
        "')";
           if (_dapper.ExecuteSql(sql))
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Adding user job info failed on save");

    }
     [HttpPut("UserJobInfo")]
    //public IEnumerable<User> GetUsers()
    public IActionResult EditUserJobInfo(UserJobInfo UserJobInfoFoUpdate) //RETURNS responce and tell what happen without necessarily returning
    {
        string sql = @"
         UPDATE TutorialAppSchema.UserJobInfo
       SET Department = '"+ UserJobInfoFoUpdate.Department + 
           "'  JobTitle = '"+ UserJobInfoFoUpdate.JobTitle +
        "' WHERE UserId = " + UserJobInfoFoUpdate.UserId;
        if (_dapper.ExecuteSql(sql))
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to update user  job info");

    }
       
    [HttpDelete("UserJobInfo/{userId}")]
    
    public IActionResult DeleteUserJobInfo(int userId) //RETURNS responce and tell what happen without necessarily returning
    {
        string sql = @"
         SELECT FROM TutorialAppSchema.UserJobInfo
         WHERE UserId = " + userId.ToString();
        if (_dapper.ExecuteSql(sql))
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to delete User Job Info");

    }
}



/*

 SELECT  [UserId]
         , [FirstName]
         , [LastName]
         , [Email]
         , [Gender]
         , [Active]
   FROM  TutorialAppSchema.Users;

 SELECT  [UserId]
         , [Salary]
   FROM  TutorialAppSchema.UserSalary;

 SELECT  [UserId]
         , [JobTitle]
         , [Department]
   FROM  TutorialAppSchema.UserJobInfo;
*/