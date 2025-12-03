export interface JournalDetailInputDto {
  accountId: string;       
  debitAmount: number;
  creditAmount: number;
  detailStatement?: string | null;
}
export interface AccountSearchDto {
  id: string;
  nameAR: string;
  number: string;
  fullAccountName: string;
}

export interface JournalVoucherInputModel {
  journalDate: string;
  journalDescription: string;
  totalDebit: number;
  totalCredit: number;
  details: JournalDetailInputDto[];
}
