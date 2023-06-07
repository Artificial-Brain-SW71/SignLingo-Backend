using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignLingo.API.Request;
using SignLingo.API.Response;
using SignLingo.Domain;
using SignLingo.Infrastructure;
using SignLingo.Infrastructure.Models;

namespace SignLingo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserModuleController : ControllerBase
    {
        private readonly IUserModuleInfrastructure _userModuleInfrastructure;
        private readonly IUserModuleDomain _userModuleDomain;
        private readonly IMapper _mapper;

        public UserModuleController(IUserModuleInfrastructure userModuleInfrastructure, IUserModuleDomain userModuleDomain ,IMapper mapper)
        {
            _userModuleInfrastructure = userModuleInfrastructure;
            _userModuleDomain = userModuleDomain;
            _mapper = mapper;
        }
        
        // GET: api/UserModule
        [HttpGet]
        public async Task<IEnumerable<UserModuleResponse>> GetAsync()
        {
            var usersModules = await _userModuleInfrastructure.GetAllAsync();
            var usersModulesResponse = _mapper.Map<List<UserModule>, List<UserModuleResponse>>(usersModules);
            return usersModulesResponse;
        }

        // GET: api/UserModule/5
        [HttpGet("userId={userId:int}&moduleId={moduleId:int}", Name = "GetUSerModuleById")]
        public async Task<UserModuleResponse> GetAsync(int userId, int moduleId)
        {
            var userModule = await _userModuleInfrastructure.GetByIdAsync(userId, moduleId);
            var userModuleResponse = _mapper.Map<UserModule, UserModuleResponse>(userModule);
            return userModuleResponse;
        }

        // POST: api/UserModule
        [HttpPost]
        public async Task PostAsync([FromBody] UserModuleRequest request)
        {
            if (ModelState.IsValid)
            {
                var userModule = _mapper.Map<UserModuleRequest, UserModule>(request);
                await _userModuleDomain.SaveAsync(userModule);
            }
            else
            {
                StatusCode(400);
            }
        }

        // PUT: api/UserModule/5
        [HttpPut("userId={userId:int}&moduleId={moduleId:int}")]
        public async Task PutAsync(int userId, int moduleId, [FromBody] UserModuleRequest request)
        {
            var userModule = _mapper.Map<UserModuleRequest, UserModule>(request);
            await _userModuleDomain.UpdateAsync(userId, moduleId, userModule);
        }

        // DELETE: api/UserModule/5
        [HttpDelete("userId={userId:int}&moduleId={moduleId:int}")]
        public async Task DeleteAsync(int userId, int moduleId)
        {
            await _userModuleDomain.DeleteAsync(userId, moduleId);
        }
    }
}