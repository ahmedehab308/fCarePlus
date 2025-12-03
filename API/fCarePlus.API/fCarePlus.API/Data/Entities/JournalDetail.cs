using System;
using System.Collections.Generic;

namespace fCarePlus.API.Data.Entities;

public partial class JournalDetail
{
    public long DetailId { get; set; }

    public long JournalId { get; set; }

    public Guid AccountId { get; set; }

    public decimal? DebitAmount { get; set; }

    public decimal? CreditAmount { get; set; }

    public string? DetailStatement { get; set; }

    public bool IsDeleted { get; set; }

    public virtual AccountsChart Account { get; set; } = null!;

    public virtual JournalHeader Journal { get; set; } = null!;
}
