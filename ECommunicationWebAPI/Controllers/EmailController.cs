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
        #region Get Method
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
        public IHttpActionResult GetAllFolder()
        {
            try
            {
                var repository = EmailRepository.Instance;
                var list = repository.GetAllFolder().ToList();
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
        public IHttpActionResult GetFilesByFolder(int folderId, string SearchUserFiles)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var list = repository.GetFilesByFolder(folderId, SearchUserFiles).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        public IHttpActionResult GetUserFolder(int UserMailboxId, string searchUserFolder)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var list = repository.GetUserFolder(UserMailboxId, searchUserFolder).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region Move/Enable/Disable Files
        [HttpPost]
        public IHttpActionResult SetFilesToDisable(List<File> Files)
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
        public IHttpActionResult SetFilesToEnable(List<File> Files)
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
        [HttpPost]
        public IHttpActionResult MoveFilesIntoFolder(long FolderId, List<File> Files)
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

        #endregion

        #region Add/Edit/DELETE Folder
        [HttpPost]
        public IHttpActionResult AddFolder(Folder folder)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var returnValue = repository.Create(folder);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateFolder(Folder folder)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var returnValue = repository.Update(folder);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteFolder(int id)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var returnValue = repository.Delete(id);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion

        #region Add/Edit/DELETE Files
        [HttpPost]
        public IHttpActionResult AddFiles(File files)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var returnValue = repository.CreateFiles(files);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateFiles(File files)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var returnValue = repository.UpdateFiles(files);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        public IHttpActionResult DeleteFiles(List<File> files)
        {
            try
            {
                var repository = EmailRepository.Instance;
                var returnValue = repository.DeleteFiles(files);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion
    }
}
