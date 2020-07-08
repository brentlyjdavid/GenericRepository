using System;
using DevRite.GenericRepository.Core.Interfaces;

namespace DevRite.GenericRepository.Tests.TestContext.Entities
{
    public class DateCreated : ISaveable<string>, ISaveableCreate, ISaveableTracking
    {
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateLastModifiedUtc { get; set; }
    }
}
