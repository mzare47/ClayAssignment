﻿namespace AccessControl.Api.Models.Shared
{
    public abstract class BaseEntity : IAuditable
    {
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
