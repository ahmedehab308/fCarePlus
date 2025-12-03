using System;
using System.Collections.Generic;

namespace fCarePlus.API.Data.Entities;

public partial class JournalHeader
{
    public long JournalId { get; set; }

    public DateOnly JournalDate { get; set; }

    public string JournalDescription { get; set; } = null!;

    public decimal TotalDebit { get; set; }

    public decimal TotalCredit { get; set; }

    public DateTime? CreationDate { get; set; }

    public long? UserId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<JournalDetail> JournalDetails { get; set; } = new List<JournalDetail>();
}
