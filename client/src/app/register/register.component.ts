import { Component, EventEmitter, inject, input, output, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  cancelRegister = output<boolean>();
  model: any = {}
  private accountService = inject(AccountService);
  private toastr = inject(ToastrService);

  register() {
    this.accountService.register(this.model).subscribe({
      next: response => {
        console.log(response);
        this.cancel();
      },
      error: err => {
        this.toastr.error(err.error)
      }
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
