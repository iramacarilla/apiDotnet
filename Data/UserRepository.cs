using DotnetApi.Models;

namespace DotnetApi.Data
{
    public class UserRepository : IUserRepository
    {
         DataContextEF _entityFramework;

    public UserRepository(IConfiguration config)/*config*/
    {
        _entityFramework = new DataContextEF(config);/*config*/
    }
    public bool SaveChanges()
    {
        return _entityFramework.SaveChanges() > 0;

    }
     public void AddEntity<T>(T entityToAdd)
    {
        if (entityToAdd != null)
        {
        _entityFramework.Add(entityToAdd);
        }
    }
    public void RemoveEntity<T>(T entityToAdd)
    {
        if (entityToAdd != null)
        {
        _entityFramework.Remove(entityToAdd);
        }
    }
    
    public IEnumerable<User> GetUsers()
    {
        
         IEnumerable<User> users = _entityFramework.Users.ToList<User>();

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
     public User GetSingleUser(int userId)
    {
         User? user = _entityFramework.Users
         .Where(u=>u.UserId == userId)
         .FirstOrDefault<User>();
        if (user != null) 
        {
         return user;
        }
        throw new Exception("Faild to get user");

    }
    public UserSalary GetSingleUserSalary(int userId)
    {
         UserSalary? userSalary = _entityFramework.UserSalary
         .Where(u=>u.UserId == userId)
         .FirstOrDefault<UserSalary>();
        if (userSalary != null) 
        {
         return userSalary;
        }
        throw new Exception("Faild to get userSalary");

    }
     public UserJobInfo GetSingleUserJobInfo(int userId)
    {
         UserJobInfo? userJobInfo = _entityFramework.UserJobInfo
         .Where(u=>u.UserId == userId)
         .FirstOrDefault<UserJobInfo>();
        if (userJobInfo != null) 
        {
         return userJobInfo;
        }
        throw new Exception("Faild to get userJobInfo");

    }
    }
}