using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(DataContext dataContext) : ControllerBase
{
    public readonly DataContext _dataContext = dataContext;
 //-------------------------------------------------------------------------------------------------       
    [HttpGet] /* api/Users  */
    public async Task <ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _dataContext.Users.ToListAsync();
        return Ok(users);
    }
//-------------------------------------------------------------------------------------------------
    [HttpGet("{Id:int}")] /* api/Users/3  */
    public async Task<ActionResult<AppUser>> GetUsers(int Id)
    {
        var user = await _dataContext.Users.FindAsync(Id);
        if(user == null) return NotFound("User Not Found!!");
        else return Ok(user);
    }
//-------------------------------------------------------------------------------------------------
}
