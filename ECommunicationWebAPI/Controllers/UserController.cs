using ECommunicationDataLibrary.EntityModel;
using ECommunicationDataLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ECommunicationWebAPI.Controllers
{
    public class UserController : ApiController
    {       
        [HttpGet]
        public IHttpActionResult Users()
        {
            try
            {
                var repository = UserRepository.Instance;
                var list = repository.GetAll().ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
              
        [HttpPost]
        public IHttpActionResult AddUser(User user)
        {
            try
            {
                var repository = UserRepository.Instance;
                var returnValue = repository.Create(user);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateUser(User user)
        {
            try
            {
                var repository = UserRepository.Instance;
                var returnValue = repository.Update(user);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteUser(int id)
        {
            try
            {
                var repository = UserRepository.Instance;
                var returnValue = repository.Delete(id);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
