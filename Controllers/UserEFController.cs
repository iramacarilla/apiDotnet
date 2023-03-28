using AutoMapper;
using DotnetApi.Data;
using DotnetApi.Dtos;
using DotnetApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Controllers;

[ApiController] //attribute for class
[Route("[controller]")]//route attribute
public class UserEFController : ControllerBase
{
  IUserRepository _userRepository;
  IMapper _mapper;
  

    public UserEFController(IConfiguration entityFramework, IUserRepository userRepository)/*config*/
    {
        _userRepository = userRepository;
        _mapper = new Mapper(new MapperConfiguration (cfg => {
          cfg.CreateMap<UserToAddDto, User>();
          cfg.CreateMap<UserSalary, UserSalary>();
            cfg.CreateMap<UserJobInfo, UserJobInfo>();
        }));
    }
  

    [HttpGet("GetUsers")]
    //public IEnumerable<User> GetUsers()
    public IEnumerable<User> GetUsers()
    {
        
         IEnumerable<User> users = _userRepository.GetUsers();

         return users;
    }
    [HttpGet("GetSingleUsers/{userId}")]
    //public IEnumerable<User> GetUsers()
    public User GetSingleUsers(int userId)
    {
        return _userRepository.GetSingleUser(userId);
    }

    [HttpPut("EditUser")]
    //public IEnumerable<User> GetUsers()
    public IActionResult EditUsers(User user) //RETURNS responce and tell what happen without necessarily returning
    {
        User? userDB = _userRepository.GetSingleUser(user.UserId);

        if (userDB != null) 
        {
         userDB.Active = user.Active;
         userDB.FirstName = user.FirstName;
         userDB.Email = user.Email;
         userDB.LastName = user.LastName; 
         userDB.Gender = user.Gender;
         
        if (_userRepository.SaveChanges())
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to update user");
        }
        
        throw new Exception("Faild to get user");
    }

    [HttpPost("AddUser")]
    //public IEnumerable<User> GetUsers()
    public IActionResult AddUsers(UserToAddDto user) //RETURNS responce and tell what happen without necessarily returning
    {
       User? userDB = _mapper.Map<User>(user);

         _userRepository.AddEntity<User>(userDB);
         
        if (_userRepository.SaveChanges())
        {
         return Ok(); //coming with ContrillerBase class
        }
        
        throw new Exception("Faild to add user");
    }

     [HttpDelete ("DeleteUser/{userId}")]
    //public IEnumerable<User> GetUsers()
    public IActionResult DeleteUsers(int userId) //RETURNS responce and tell what happen without necessarily returning
    {
       User? userDB = _userRepository.GetSingleUser(userId);

        if (userDB != null) 
        {
          _userRepository.RemoveEntity<User>(userDB); 
         
        if (_userRepository.SaveChanges())
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to delete user");
        }
        
        throw new Exception("Faild to get user");

    }

  
    [HttpGet("UserSalary/{userId}")]
    //public IEnumerable<User> GetUsers()
    public UserSalary GetUserSalaryEF(int userId)
    {
        return _userRepository.GetSingleUserSalary(userId);
       
    }

    [HttpPut("UserSalary")]
    //public IEnumerable<User> GetUsers()
    public IActionResult UpdateUserSalaryEF(UserSalary userForUpdate) //RETURNS responce and tell what happen without necessarily returning
    {
        UserSalary? userToUpdate = _userRepository.GetSingleUserSalary(userForUpdate.UserId);

        if (userToUpdate!= null) 
        {
         _mapper.Map(userToUpdate, userForUpdate);
       
        if (_userRepository.SaveChanges())
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to update UserSalary");
        }
        
        throw new Exception("Faild to get UserSalary");
    }

    [HttpPost("UserSalary")]
    //public IEnumerable<User> GetUsers()
    public IActionResult PostUserSalary(UserSalary userForInsert)
       {

         _userRepository.AddEntity<UserSalary>(userForInsert);
         
        if (_userRepository.SaveChanges())
        {
         return Ok(); //coming with ContrillerBase class
        }
        
        throw new Exception("Faild to add UserSalary");
    }

     [HttpDelete ("UserSalary/{userId}")]
    //public IEnumerable<User> GetUsers()
    public IActionResult DeleteUserSalary(int userId) //RETURNS responce and tell what happen without necessarily returning
    {
       UserSalary? userToDelete = _userRepository.GetSingleUserSalary(userId);

        if (userToDelete != null) 
        {
          _userRepository.RemoveEntity<UserSalary>(userToDelete); 
         
        if (_userRepository.SaveChanges())
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to delete userToDelete");
        }
        
        throw new Exception("Faild to get userToDelete");

    }


     [HttpGet("UserJobInfo/{userId}")]
    //public IEnumerable<User> GetUsers()
    public UserJobInfo GetUserJobInfoEF(int userId)
    {
        return _userRepository.GetSingleUserJobInfo(userId);

    }

    [HttpPut("UserJobInfo")]
    //public IEnumerable<User> GetUsers()
    public IActionResult PutUserJobInfoEF(UserJobInfo userForUpdate) //RETURNS responce and tell what happen without necessarily returning
    {
        UserJobInfo? userToUpdate = _userRepository.GetSingleUserJobInfo(userForUpdate.UserId);
        if (userToUpdate!= null) 
        {
         _mapper.Map(userToUpdate, userForUpdate);
       
        if (_userRepository.SaveChanges())
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to update UserJobInfo");
        }
        
        throw new Exception("Faild to get UserJobInfoy");
    }

    [HttpPost("UserJobInfo")]
    //public IEnumerable<User> GetUsers()
    public IActionResult PostUserJobInfo(UserJobInfo userForInsert)
       {

         _userRepository.AddEntity<UserJobInfo>(userForInsert);
         
        if (_userRepository.SaveChanges())
        {
         return Ok(); //coming with ContrillerBase class
        }
        
        throw new Exception("Faild to add UserJobInfo");
    }

     [HttpDelete ("UserJobInfo/{userId}")]
    //public IEnumerable<User> GetUsers()
    public IActionResult DeleteUserJobInfo(int userId) //RETURNS responce and tell what happen without necessarily returning
    {
       UserJobInfo? userToDelete = _userRepository.GetSingleUserJobInfo(userId);
        if (userToDelete != null) 
        {
          _userRepository.RemoveEntity<UserJobInfo>(userToDelete); 
         
        if (_userRepository.SaveChanges())
        {
         return Ok(); //coming with ContrillerBase class
        }
        throw new Exception("Faild to delete UserJobInfo");
        }
        
        throw new Exception("Faild to get UserJobInfo");

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