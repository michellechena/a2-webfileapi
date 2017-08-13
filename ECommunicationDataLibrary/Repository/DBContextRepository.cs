using ECommunicationDataLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommunicationDataLibrary.Repository
{
    public sealed class DBContextRepository
    {
        private static readonly Lazy<ECommunicationEntities> Context = new Lazy<ECommunicationEntities>(() => new ECommunicationEntities());

        private DBContextRepository() { }

        public static ECommunicationEntities Instance
        {
            get { return Context.Value; }
        }
    }
}
