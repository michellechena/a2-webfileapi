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
                if (ListFolders != null && ListFolders.Count > 0)
                {
                    return ListEmailFolder = ListFolders.AsEnumerable().Select(x => new EmailFolder
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
            //return dbContext.Database.SqlQuery<Mailboxes>("exec GetMailboxesUserWise(5)").ToList();
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
        public IList<UserEmailFolderList> GetUserFolder(int UserMailboxId)
        {
            try
            {
                List<UserEmailFolderList> UserFolderList = new List<UserEmailFolderList>();
                UserEmailFolderList objUserfolder;
                var ListFolderDetails = dbContext.Folders.Where(x => x.MailboxId == UserMailboxId).ToList();
                if (ListFolderDetails != null && ListFolderDetails.Count > 0)
                {
                    foreach (var objfolder in ListFolderDetails)
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
            catch (Exception)
            {
                throw;
            }
        }

        public IList<UserFileList> GetFilesByFolder(int folderId)
        {
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
        public File MoveFilesIntoFolder(long FolderId, File files)
        {
            try
            {
                if (files == null)
                {
                    return null;
                }
                else
                {
                    var existing = dbContext.Files.FirstOrDefault(x => x.FileId == files.FileId);
                    existing.FolderId = FolderId;
                    dbContext.SaveChanges();

                    return files;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public File SetFilesToDisable(File files)
        {
            try
            {
                if (files == null)
                {
                    return null;
                }
                else
                {
                    var existing = dbContext.Files.FirstOrDefault(x => x.FileId == files.FileId);
                    if (existing.StatusId == 1)
                    {
                        existing.StatusId = 0;
                    }
                    dbContext.SaveChanges();

                    return files;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public File SetFilesToEnable(File files)
        {
            try
            {
                if (files == null)
                {
                    return null;
                }
                else
                {
                    var existing = dbContext.Files.FirstOrDefault(x => x.FileId == files.FileId);
                    if (existing.StatusId == 0)
                    {
                        existing.StatusId = 1;
                    }
                    dbContext.SaveChanges();

                    return files;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public File DeleteFiles(int FileId)
        {
            var filesToRemove = dbContext.Files.FirstOrDefault(x => x.FileId == FileId);
            var files = dbContext.Files.Remove(filesToRemove);
            dbContext.SaveChanges();
            return files;
        }
    }
}
