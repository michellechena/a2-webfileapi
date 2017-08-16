using ECommunicationDataLibrary.EntityModel;
using ECommunicationDataLibrary.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommunicationDataLibrary.Repository
{
    public sealed class EmailRepository
    {
        private static ECommunicationEntities dbContext;

        private static readonly Lazy<EmailRepository> Repository = new Lazy<EmailRepository>(() => new EmailRepository());
        private EmailRepository()
        {
            dbContext = DBContextRepository.Instance;
        }

        public static EmailRepository Instance
        {
            get
            {
                return Repository.Value;
            }
        }

        public IList<Mailboxes> GetUserMailboxes()
        {
            List<Mailboxes> ListMailboxes = new List<Mailboxes>();
            try
            {
                var id = 1;
                var Usermailboxes = (dbContext.Mailboxes
                            .Join(dbContext.UserMailboxes,
                             mailbox => mailbox.MailboxId,
                             usermailbox => usermailbox.MailboxId,
                             (mailbox, usermailbox) => new { MailboxDetails = mailbox, UsermailboxDetails = usermailbox })
                            .Where(UserandMailbox => UserandMailbox.UsermailboxDetails.UserId == id)).AsQueryable();
                if (Usermailboxes != null)
                {
                    return ListMailboxes = Usermailboxes.Select(x => new Mailboxes
                    {
                        MailboxId = x.MailboxDetails.MailboxId,
                        ShortName = x.MailboxDetails.ShortName,
                        FullName = x.MailboxDetails.FullName,
                        StatusId = x.MailboxDetails.StatusId,
                        IsMainContact = x.UsermailboxDetails.IsMainContact,
                        IsDefaultMailbox = x.UsermailboxDetails.IsDefaultMailbox,
                        UserMailBoxId = x.UsermailboxDetails.UserMailboxId,
                        UserId = x.UsermailboxDetails.UserId
                    }).ToList();
                }
                else
                {
                    return ListMailboxes;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IList<EmailFolder> GetFolderByUserMailbox(int UserMailboxId)
        {
            List<EmailFolder> ListEmailFolder = new List<EmailFolder>();
            try
            {
                var ListFolders = dbContext.Folders.Where(x => x.MailboxId == UserMailboxId).ToList();
                var FirstPositionRecord = ListFolders.Where(x => x.TypeId == 0).Take(1).ToList();
                var SecondPositionRecord = ListFolders.Where(x => x.TypeId == 0).Skip(1).ToList();
                var ThirdPositionRecord = ListFolders.Where(x => x.TypeId == 1).OrderBy(x => x.FolderName).ToList();
                var NewListFolders = FirstPositionRecord.Concat(SecondPositionRecord).Concat(ThirdPositionRecord).ToList();
                if (NewListFolders != null && ListFolders.Count > 0)
                {
                    return ListEmailFolder = NewListFolders.AsEnumerable().Select(x => new EmailFolder
                    {
                        FolderId = x.FolderId,
                        MailboxId = Convert.ToInt64(x.MailboxId),
                        FolderName = x.FolderName,
                        TypeId = x.TypeId,
                        StatusId = x.StatusId
                    }).ToList();
                }
                else
                {
                    return ListEmailFolder;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<EmailObject> GetFolderWithDetails(int UserMailboxId)
        {
            List<EmailObject> EmailObjectList = new List<EmailObject>();
            try
            {
                var Usermailbox = dbContext.Mailboxes.Where(x => x.MailboxId == UserMailboxId).ToList();
                var Folder = dbContext.Folders.Where(x => x.MailboxId == UserMailboxId).ToList();

                EmailObject objEmail;
                UsermailboxList objUsermail;
                UserFolderList objUserfolder;
                UserFileList objUserfiles;
                List<UsermailboxList> UsermailboxList = new List<UsermailboxList>();
                List<UserFolderList> UserFolderList = new List<UserFolderList>();
                List<UserFileList> UserFileList = new List<UserFileList>();

                if (Usermailbox != null && Usermailbox.Count > 0)
                {
                    foreach (var objUsermailbox in Usermailbox)
                    {
                        objEmail = new EmailObject();
                        objUsermail = new UsermailboxList();
                        objUsermail.MailboxId = objUsermailbox.MailboxId;
                        objUsermail.ShortName = objUsermailbox.ShortName;
                        objUsermail.FullName = objUsermailbox.FullName;
                        objUsermail.StatusId = objUsermailbox.StatusId;
                        if (Folder != null && Folder.Count > 0)
                        {
                            foreach (var objfolder in Folder)
                            {
                                objUserfolder = new UserFolderList();
                                objUserfolder.FolderId = objfolder.FolderId;
                                objUserfolder.MailboxId = objfolder.MailboxId;
                                objUserfolder.FolderName = objfolder.FolderName;
                                objUserfolder.TypeId = objfolder.TypeId;
                                objUserfolder.StatusId = objfolder.StatusId;
                                objUserfolder.TotalActiveFiles = dbContext.Files.Where(x => x.FolderId == objfolder.FolderId && x.StatusId == 1).Count();
                                objUserfolder.TotalDisableFiles = dbContext.Files.Where(x => x.FolderId == objfolder.FolderId && x.StatusId == 0).Count();
                                var FileList = dbContext.Files.Where(x => x.FolderId == objfolder.FolderId && objfolder.TypeId == 0).ToList(); // Default Folder
                                if (FileList != null && FileList.Count > 0)
                                {
                                    foreach (var objFile in FileList)
                                    {
                                        objUserfiles = new UserFileList();
                                        objUserfiles.FileId = objFile.FileId;
                                        objUserfiles.FolderId = objFile.FolderId;
                                        objUserfiles.FilePath = objFile.FilePath;
                                        objUserfiles.FileName = objFile.FileName;
                                        objUserfiles.TypeId = objFile.TypeId;
                                        objUserfiles.StatusId = objFile.StatusId;
                                        objUserfiles.IsValid = objFile.IsValid;
                                        UserFileList.Add(objUserfiles);
                                        objUserfolder.UserFileList = UserFileList;
                                    }
                                }
                                UserFolderList.Add(objUserfolder);
                                objUsermail.UserFolderList = UserFolderList;
                            }
                        }
                        UsermailboxList.Add(objUsermail);

                        objEmail.UserId = 1;
                        objEmail.UsermailboxID = 1;
                        objEmail.UsermailboxList = UsermailboxList;
                        objEmail.TotalFolder = UserFolderList.Count();
                        objEmail.TotalFiles = UserFileList.Count();
                        EmailObjectList.Add(objEmail);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return EmailObjectList;
        }
        public IList<UserEmailFolderList> GetUserFolder(int UserMailboxId, string searchUserFolder)
        {
            List<UserEmailFolderList> UserFolderList = new List<UserEmailFolderList>();
            try
            {
                UserEmailFolderList objUserfolder;
                var ListFolderDetails = dbContext.Folders.Where(x => x.MailboxId == UserMailboxId).ToList();
                if (!String.IsNullOrEmpty(searchUserFolder))
                {
                    ListFolderDetails = ListFolderDetails.Where(x => x.FolderName.ToLower().Contains(searchUserFolder.ToLower())).ToList();
                }

                var FirstPositionRecord = ListFolderDetails.Where(x => x.TypeId == 0).Take(1).ToList();
                var SecondPositionRecord = ListFolderDetails.Where(x => x.TypeId == 0).Skip(1).ToList();
                var ThirdPositionRecord = ListFolderDetails.Where(x => x.TypeId == 1).OrderBy(x => x.FolderName).ToList();
                var NewListFolders = FirstPositionRecord.Concat(SecondPositionRecord).Concat(ThirdPositionRecord).ToList();
                if (NewListFolders != null && NewListFolders.Count > 0)
                {
                    foreach (var objfolder in NewListFolders)
                    {
                        objUserfolder = new UserEmailFolderList();
                        objUserfolder.FolderId = objfolder.FolderId;
                        objUserfolder.MailboxId = objfolder.MailboxId;
                        objUserfolder.FolderName = objfolder.FolderName;
                        objUserfolder.TypeId = objfolder.TypeId;
                        objUserfolder.StatusId = objfolder.StatusId;
                        objUserfolder.TotalFiles = dbContext.Files.Where(x => x.FolderId == objfolder.FolderId).Count();
                        objUserfolder.TotalActiveFiles = dbContext.Files.Where(x => x.FolderId == objfolder.FolderId && x.StatusId == 1).Count();
                        objUserfolder.TotalDisableFiles = dbContext.Files.Where(x => x.FolderId == objfolder.FolderId && x.StatusId == 0).Count();
                        UserFolderList.Add(objUserfolder);
                    }
                    return UserFolderList;
                }
                else
                {
                    return UserFolderList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IList<EmailFolder> GetAllFolder()
        {
            List<EmailFolder> ListEmailFolder = new List<EmailFolder>();
            try
            {
                var ListFolders = dbContext.Folders.ToList();
                if (ListFolders != null && ListFolders.Count > 0)
                {
                    return ListEmailFolder = ListFolders.Select(x => new EmailFolder
                    {
                        FolderId = x.FolderId,
                        MailboxId = Convert.ToInt64(x.MailboxId),
                        FolderName = x.FolderName,
                        TypeId = x.TypeId,
                        StatusId = x.StatusId
                    }).ToList();
                }
                else
                {
                    return ListEmailFolder;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<UserFileList> GetFilesByFolder(int folderId, string SearchUserFiles)
        {
            List<UserFileList> ListUserFileList = new List<UserFileList>();
            try
            {
                var ListFiles = dbContext.Files.Where(x => x.FolderId == folderId).ToList();
                if (!String.IsNullOrEmpty(SearchUserFiles))
                {
                    ListFiles = ListFiles.Where(x => x.FileName.ToLower().Contains(SearchUserFiles.ToLower())).ToList();
                }
                if (ListFiles != null && ListFiles.Count > 0)
                {
                    return ListUserFileList = ListFiles.AsEnumerable().Select(x => new UserFileList
                    {
                        FileId = x.FileId,
                        FolderId = x.FolderId,
                        FilePath = x.FilePath,
                        FileName = x.FileName,
                        TypeId = x.TypeId,
                        StatusId = x.StatusId,
                        IsValid = x.IsValid,
                        IsSelect = false
                    }).ToList();
                }
                else
                {
                    return ListUserFileList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<UserFileList> GetFilesByFolderWithPaging(int folderId)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            List<UserFileList> ListUserFileList = new List<UserFileList>();
            try
            {
                var ListFiles = dbContext.Files.Where(x => x.FolderId == folderId).ToList();
                if (ListFiles != null && ListFiles.Count > 0)
                {
                    return ListUserFileList = ListFiles.AsEnumerable().Select(x => new UserFileList
                    {
                        FileId = x.FileId,
                        FolderId = x.FolderId,
                        FilePath = x.FilePath,
                        FileName = x.FileName,
                        TypeId = x.TypeId,
                        StatusId = x.StatusId,
                        IsValid = x.IsValid,
                        IsSelect = false
                    }).ToList();
                }
                else
                {
                    return ListUserFileList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string MoveFilesIntoFolder(long FolderId, List<File> files)
        {
            try
            {
                if (files != null)
                {

                    foreach (var objfile in files)
                    {
                        var existing = dbContext.Files.FirstOrDefault(x => x.FileId == objfile.FileId);
                        existing.FolderId = FolderId;
                        dbContext.SaveChanges();
                    }
                    return "File has been moved successfully.";
                }
                else
                {
                    return "Internal server error";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SetFilesToDisable(List<File> files)
        {
            try
            {
                if (files != null)
                {
                    foreach (var objfile in files)
                    {
                        var existing = dbContext.Files.FirstOrDefault(x => x.FileId == objfile.FileId);
                        if (existing.StatusId == 1)
                        {
                            existing.StatusId = 0;
                        }
                        dbContext.SaveChanges();
                    }
                    return "File has been disabled successfully.";
                }
                else
                {
                    return "Internal server error";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SetFilesToEnable(List<File> files)
        {
            try
            {
                if (files != null)
                {

                    foreach (var objfile in files)
                    {
                        var existing = dbContext.Files.FirstOrDefault(x => x.FileId == objfile.FileId);
                        if (existing.StatusId == 0)
                        {
                            existing.StatusId = 1;
                        }
                        dbContext.SaveChanges();
                    }
                    return "File has been enabled successfully.";
                }
                else
                {
                    return "Internal server error";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteFiles(List<File> files)
        {
            try
            {
                if (files != null)
                {
                    foreach (var objfile in files)
                    {
                        var filesToRemove = dbContext.Files.FirstOrDefault(x => x.FileId == objfile.FileId);
                        dbContext.Files.Remove(filesToRemove);
                        dbContext.SaveChanges();
                    }

                    return "File has been deleted successfully.";
                }
                else
                {
                    return "Internal server error";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string Create(Folder folder)
        {
            try
            {
                if (folder != null)
                {
                    dbContext.Folders.Add(folder);
                    dbContext.SaveChanges();
                }
                return "Folder has been added successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Update(Folder folder)
        {
            try
            {
                if (folder != null)
                {
                    var existsfolder = dbContext.Folders.FirstOrDefault(x => x.FolderId == folder.FolderId);
                    existsfolder.FolderName = folder.FolderName;
                    existsfolder.StatusId = folder.StatusId;
                    existsfolder.MailboxId = folder.MailboxId;
                    existsfolder.TypeId = folder.TypeId;
                    dbContext.SaveChanges();
                }
                return "Folder has been updated successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    var existsFolder = dbContext.Folders.Where(x => x.FolderId == id).FirstOrDefault();
                    if (existsFolder != null)
                    {
                        dbContext.Folders.Remove(existsFolder);
                        dbContext.SaveChanges();
                    }
                    return "Folder has been deleted successfully";
                }
                else
                {
                    return "There isn't any folder exists";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CreateFiles(File files)
        {
            try
            {
                if (files != null)
                {
                    dbContext.Files.Add(files);
                    dbContext.SaveChanges();
                }
                long id = files.FileId;
                return "File has been added successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string UpdateFiles(File files)
        {
            try
            {
                if (files != null)
                {
                    var existsfolder = dbContext.Files.FirstOrDefault(x => x.FileId == files.FileId);
                    existsfolder.FolderId = files.FolderId;
                    existsfolder.FileName = files.FileName;
                    existsfolder.FilePath = files.FilePath;
                    existsfolder.StatusId = files.StatusId;
                    existsfolder.TypeId = files.TypeId;
                    existsfolder.IsValid = files.IsValid;
                    dbContext.SaveChanges();
                }
                return "File has been updated successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
