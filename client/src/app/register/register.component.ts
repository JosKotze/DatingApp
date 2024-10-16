import { Component, inject, OnInit, output } from '@angular/core';
import { AccountService } from '../services/account.service';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { JsonPipe, NgIf } from '@angular/common';
import { TextInputComponent } from "../forms/text-input/text-input.component";
import { DatePickerComponent } from '../forms/date-picker/date-picker.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, JsonPipe, NgIf, TextInputComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  cancelRegister = output<boolean>();
  private accountService = inject(AccountService);
  private fb = inject(FormBuilder);
  registerForm: FormGroup = new FormGroup({});
  maxDate = new Date();
  router = inject(Router);
  validationErrors: string[] | undefined;

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18)
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }
  

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value == control.parent?.get(matchTo)?.value ? null : {isMatching: true}
    }
  }

  register() {
    // const dob = this.getDateOnly(this.registerForm.get('sateOfBirth')?.value);
    // this.registerForm.patchValue({dateOfBirth: dob});
    const dob = this.getDateOnly(this.registerForm.get('dateOfBirth')?.value);
    this.registerForm.value.dateOfBirth = dob;
    this.accountService.register(this.registerForm.value).subscribe({
      next: _ => this.router.navigateByUrl('/member'),
      error: err => this.validationErrors = err
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

  private getDateOnly(dob: string | undefined){
    if (!dob) return;
    return new Date(dob).toISOString().slice(0, 10);
  }
}
