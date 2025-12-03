import { Component, OnInit } from '@angular/core';
import {  FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, filter, firstValueFrom, Observable, of, switchMap } from 'rxjs';
import { AccountSearchDto, JournalDetailInputDto, JournalVoucherInputModel } from '../../shared/models/journal.models';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../core/services/account.service';
import { JournalService } from '../../core/services/journal.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-journal',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './journal.component.html',
  styleUrl: './journal.component.css',
  host: { 'dir': 'rtl' }
})
export class JournalComponent implements OnInit {

  voucherForm!: FormGroup;
  suggestions: AccountSearchDto[][] = [];
  private readonly LOCAL_STORAGE_KEY = 'journalVoucherDraft';

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private journalVoucherService: JournalService
  ) { }

  ngOnInit(): void {
  this.initForm();
  this.loadFromLocalStorage();
  this.subscribeToDetailChanges();
  this.subscribeToAutoSave();

  }

  initForm(): void {
    this.voucherForm = this.fb.group({
      JournalDate: [null, Validators.required],
      JournalDescription: ['', [Validators.required, Validators.maxLength(500)]],

      TotalDebit: [{ value: 0, disabled: true }],
      TotalCredit: [{ value: 0, disabled: true }],
      Difference: [{ value: 0, disabled: true }],

      Details: this.fb.array([this.createDetailRow()], Validators.minLength(2))
    });
  }

  createDetailRow(): FormGroup {
    const newRow = this.fb.group({
      AccountId: [null, Validators.required],
      AccountDisplay: ['', Validators.required],
      DebitAmount: [0, [Validators.min(0)]],
      CreditAmount: [0, [Validators.min(0)]],
      DetailStatement: [null, Validators.maxLength(500)],
    });

    this.setupDebitCreditLogic(newRow);

    return newRow;
  }

  get detailsArray(): FormArray<FormGroup> {
    return this.voucherForm.get('Details') as FormArray<FormGroup>;
  }

  calculateTotals(): void {
    const details = this.detailsArray.controls;
    let totalDebit = 0;
    let totalCredit = 0;

    details.forEach(group => {
      totalDebit += parseFloat(group.get('DebitAmount')?.value || 0);
      totalCredit += parseFloat(group.get('CreditAmount')?.value || 0);
    });

    const difference = totalDebit - totalCredit;

    this.voucherForm.get('TotalDebit')?.setValue(totalDebit, { emitEvent: false });
    this.voucherForm.get('TotalCredit')?.setValue(totalCredit, { emitEvent: false });
    this.voucherForm.get('Difference')?.setValue(difference, { emitEvent: false });
  }

  subscribeToDetailChanges(): void {
    this.detailsArray.valueChanges.subscribe(() => {
      this.calculateTotals();
    });

    this.detailsArray.controls.forEach(control => {
      this.setupDebitCreditLogic(control as FormGroup);
    });
  }

  setupDebitCreditLogic(detailGroup: FormGroup): void {
    const debitControl = detailGroup.get('DebitAmount');
    const creditControl = detailGroup.get('CreditAmount');

    debitControl?.valueChanges.pipe(
        filter(val => val > 0)
    ).subscribe(() => {
        if (creditControl?.value !== 0) {
          creditControl?.setValue(0, { emitEvent: false });
        }
    });

    creditControl?.valueChanges.pipe(
        filter(val => val > 0)
    ).subscribe(() => {
        if (debitControl?.value !== 0) {
          debitControl?.setValue(0, { emitEvent: false });
        }
    });
  }






  addRow(): void {
    const newRow = this.createDetailRow();
    this.detailsArray.push(newRow);
  }


removeRow(index: number): void {
  if (this.detailsArray.length <= 1) return;

  Swal.fire({
    title: 'هل أنت متأكد؟',
    text: "سيتم حذف هذا الصف نهائيًا!",
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'نعم، احذفه!',
    cancelButtonText: 'إلغاء'
  }).then((result) => {
    if (result.isConfirmed) {
      this.detailsArray.removeAt(index);
      Swal.fire(
        'تم الحذف!',
        'تم حذف الصف بنجاح.',
        'success'
      );
    }
  });
}

  async saveVoucher(): Promise<void> {
   if (this.voucherForm.invalid) {
      Swal.fire({
        icon: 'error',
        title: 'خطأ في الإدخال',
        text: 'الرجاء تعبئة جميع الحقول المطلوبة بشكل صحيح.',
        confirmButtonText: 'حسناً'
      });
      this.voucherForm.markAllAsTouched();
      return;
    }

    if (this.voucherForm.get('Difference')?.value !== 0) {
      Swal.fire({
        icon: 'warning',
        title: 'القيد غير متوازن',
        text: 'يجب أن يتوازن القيد (المدين يساوي الدائن).',
        confirmButtonText: 'حسناً'
      });
      return;
    }

    if (this.voucherForm.get('TotalDebit')?.value == 0 && this.voucherForm.get('TotalCredit')?.value == 0) {
      Swal.fire({
        icon: 'warning',
        title: 'بيانات القيد فارغة',
        text: 'يجب أن يتواجد بيانات في القيد (المدين والدائن لا يساويان صفر).',
        confirmButtonText: 'حسناً'
      });
      return;
    }
    if (this.voucherForm.get('TotalDebit')?.value < 0 || this.voucherForm.get('TotalCredit')?.value < 0) {
      Swal.fire({
        icon: 'warning',
        title: 'بيانات القيد سالبة',
        text: 'يجب أن يتواجد بيانات موجبة فقط).',
        confirmButtonText: 'حسناً'
      });
      return;
    }

    const rawValue = this.voucherForm.getRawValue();

    rawValue.Details = rawValue.Details.filter((d: any) => d.DebitAmount > 0 || d.CreditAmount > 0);

    const finalVoucherData: JournalVoucherInputModel = {
      journalDate: rawValue.JournalDate,
      journalDescription: rawValue.JournalDescription,
      totalDebit: rawValue.TotalDebit,
      totalCredit: rawValue.TotalCredit,
      details: rawValue.Details.map((detail: any): JournalDetailInputDto => ({
        accountId: detail.AccountId,
        debitAmount: detail.DebitAmount,
        creditAmount: detail.CreditAmount,
        detailStatement: detail.DetailStatement
      }))
    };

    console.log('بيانات القيد جاهزة للإرسال:', finalVoucherData);


     try {
        const response = await firstValueFrom(this.journalVoucherService.saveVoucher(finalVoucherData));
        Swal.fire({ icon: 'success', title: 'نجاح!', text: 'تم حفظ القيد بنجاح.' });
        this.resetForm();
      } catch (error: any) {
        Swal.fire({ icon: 'error', title: 'فشل الحفظ', text: error.error || 'فشل حفظ القيد.' });
      }
  }

  searchAccounts(query: string, rowIndex: number): Observable<AccountSearchDto[]> {
    if (!query || query.length < 2) {
        return of([]);
    }
    return this.accountService.searchAccounts(query);
  }

  onAccountSelected(account: AccountSearchDto, rowIndex: number): void {
    const detailGroup = this.detailsArray.at(rowIndex) as FormGroup;

    detailGroup.get('AccountId')?.setValue(account.id);

    detailGroup.get('AccountDisplay')?.setValue(account.fullAccountName);

    this.suggestions[rowIndex] = [];
  }

  handleSearchInput(event: Event, rowIndex: number): void {
    const inputElement = event.target as HTMLInputElement;
    const query = inputElement.value;

    of(query).pipe(
        debounceTime(400),
        distinctUntilChanged(),
        switchMap(q => this.searchAccounts(q, rowIndex))
    ).subscribe(results => {

        this.suggestions[rowIndex] = results;
    });
  }

  saveToLocalStorage(data: any): void {
    try {
      localStorage.setItem(this.LOCAL_STORAGE_KEY, JSON.stringify(data));
      console.log('Draft saved to local storage.');
    } catch (e) {
      console.error('Error saving to local storage', e);
    }
  }

  loadFromLocalStorage(): void {
    try {
      const draft = localStorage.getItem(this.LOCAL_STORAGE_KEY);
      if (draft) {
        const data = JSON.parse(draft);

        const detailsArray = this.detailsArray;
        detailsArray.clear();

        data.Details.forEach((detail: any) => {
          detailsArray.push(this.createDetailRow());
        });

        this.voucherForm.patchValue(data, { emitEvent: false });

        this.calculateTotals();

        console.log('Draft loaded from local storage.');


      }
    } catch (e) {
      console.error('Error loading from local storage', e);
    }
  }

  clearLocalStorage(): void {
    localStorage.removeItem(this.LOCAL_STORAGE_KEY);
  }

  resetForm(): void {
    this.voucherForm.reset({ TotalDebit: 0, TotalCredit: 0, Difference: 0 });
    this.clearLocalStorage();
    this.detailsArray.clear();
    this.detailsArray.push(this.createDetailRow());
  }

  subscribeToAutoSave(): void {
    this.voucherForm.valueChanges.pipe(
        debounceTime(1500)
    ).subscribe(value => {
        this.saveToLocalStorage(this.voucherForm.getRawValue());
    });
  }



}
