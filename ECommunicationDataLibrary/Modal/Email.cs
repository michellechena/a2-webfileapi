namespace ECommunicationDataLibrary.Modal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class UserMailboxes
    {
        public long UserMailboxId { get; set; }
        public long UserId { get; set; }
        public long? MailboxId { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public bool StatusId { get; set; }
        public bool IsMainContact { get; set; }
        public bool IsDefaultMailbox { get; set; }

    }
    public class Mailboxes
    {
        public long MailboxId { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int? StatusId { get; set; }
        public bool IsDefaultMailbox { get; set; }
        public bool IsMainContact { get; set; }
        public long UserMailBoxId { get; set; }
        public long UserId { get; set; }
    }

    public class EmailUsers
    {
        public long UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsActive { get; set; }

    }
    public class EmailFolder
    {
        public long FolderId { get; set; }
        public long MailboxId { get; set; }
        public string FolderName { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
    }
    public class UserEmailFolderList
    {
        public long FolderId { get; set; }
        public long? MailboxId { get; set; }
        public string FolderName { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int TotalFiles { get; set; }
        public int TotalActiveFiles { get; set; }
        public int TotalDisableFiles { get; set; }
    }
    public class UserFileList
    {
        public long FileId { get; set; }
        public long? FolderId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public bool IsValid { get; set; }
    }
    public class UserFolderList
    {
        public long FolderId { get; set; }
        public long? MailboxId { get; set; }
        public string FolderName { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int TotalActiveFiles { get; set; }
        public int TotalDisableFiles { get; set; }
        public List<UserFileList> UserFileList { get; set; }
    }
    public class UsermailboxList
    {
        public long MailboxId { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int? StatusId { get; set; }
        public List<UserFolderList> UserFolderList { get; set; }
    }
    public class EmailObject
    {
        public long UserId { get; set; }
        public long UsermailboxID { get; set; }
        public List<UsermailboxList> UsermailboxList { get; set; }
        public int TotalFolder { get; set; }
        public int TotalFiles { get; set; }

        public int DefaultFolder { get; set; }
    }
}
