using System;
using DevRite.GenericRepository.Core.Interfaces;

namespace DevRite.GenericRepository.Tests.TestContext.Entities
{
   public  class ActiveAndDelete:ISaveable<string>,ISaveableActive,ISaveableDelete
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
    }
}
