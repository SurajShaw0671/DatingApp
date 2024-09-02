using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

[Authorize]
public class UsersController(DataContext dataContext) : BaseApiController
{
    public readonly DataContext _dataContext = dataContext;
 //-------------------------------------------------------------------------------------------------       
    [AllowAnonymous]
    [HttpGet] /* api/Users  */
    public async Task <ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _dataContext.Users.ToListAsync();
        return Ok(users);
    }
//-------------------------------------------------------------------------------------------------
    [Authorize]
    [HttpGet("{Id:int}")] /* api/Users/3  */
    public async Task<ActionResult<AppUser>> GetUsers(int Id)
    {
        var user = await _dataContext.Users.FindAsync(Id);
        if(user == null) return NotFound("User Not Found!!");
        else return Ok(user);
    }
//-------------------------------------------------------------------------------------------------
}
