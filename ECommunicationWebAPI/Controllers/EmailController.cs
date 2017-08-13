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
    public class EmailController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetFolderWithDetails(int UserMailboxId)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var list = repository.GetFolderWithDetails(UserMailboxId).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult GetAllUserMailboxes()
        {
            try
            {
                var repository = EmailRepository.Instance;
                var list = repository.GetUserMailboxes().ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        public IHttpActionResult GetFolderByUserMailbox(int UserMailboxId)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var list = repository.GetFolderByUserMailbox(UserMailboxId).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        public IHttpActionResult GetFilesByFolder(int folderId)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var list = repository.GetFilesByFolder(folderId).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        public IHttpActionResult GetUserFolder(int UserMailboxId)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var list = repository.GetUserFolder(UserMailboxId).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        public IHttpActionResult MoveFilesIntoFolder(long FolderId, File Files)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var returnValue = repository.MoveFilesIntoFolder(FolderId, Files);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        public IHttpActionResult SetFilesToDisable(long FilesId,File Files)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var returnValue = repository.SetFilesToDisable(Files);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        public IHttpActionResult SetFilesToEnable(long FilesId, File Files)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var returnValue = repository.SetFilesToEnable(Files);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpDelete]
        public IHttpActionResult DeleteFiles(int id)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var returnValue = repository.DeleteFiles(id);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
