using System.Collections.Generic;
using AutoMapper;
using EmployeeCRUD.API.Data;
using EmployeeCRUD.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRUD.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IEmployeeRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IEmployeeRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// get list of Employee
        /// </summary>
        /// <response code="200">retrieved all users succecfully</response>
        /// <response code="401">if no token provided in header or the token not valid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            return Ok(user);
        }
    }
}