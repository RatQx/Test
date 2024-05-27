import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { RessetPasword } from '../../models/reset-password.model';
import { ConfirmPasswordValidator } from '../../utils/confirm-password.validator';

@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.scss'],
})
export class ResetComponent implements OnInit {
  resetPasswordForm!: FormGroup;
  emailToReset!: string;
  emailToken!: string;
  resetPasswordObj = new RessetPasword();

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.resetPasswordForm = this.fb.group(
      {
        password: [null, Validators.required],
        confirmPassword: [null, [Validators.required, Validators.minLength(8)]],
      },
      {
        validators: ConfirmPasswordValidator('password', 'confirmPassword'),
      }
    );
    this.activatedRoute.queryParams.subscribe((val) => {
      this.emailToReset = val['email'];
      let urlToken = val['code'];
      this.emailToken = urlToken.replace(/ /g, '+');
      console.log(this.emailToken);
      console.log(this.emailToReset);
    });
  }

  onSubmit() {
    if (this.resetPasswordForm.invalid) {
      return;
    }

    this.resetPasswordObj.email = this.emailToReset;
    this.resetPasswordObj.newPassword = this.resetPasswordForm.value.password;
    this.resetPasswordObj.confirmPassword =
      this.resetPasswordForm.value.confirmPassword;
    this.resetPasswordObj.emailtoken = this.emailToken;

    this.userService.resetPassword(this.resetPasswordObj).subscribe({
      next: (res: any) => {
        console.log('Password reset successfully');
        if (res.statusCode === 200) {
          alert('Password reset successfully');
          this.router.navigate(['/login']);
        } else {
          alert('Unexpected error occurred while resetting password');
        }
      },
      error: (err: any) => {
        console.log('Password not reset', err);
        if (err.status === 400) {
          alert('Invalid password reset link');
        } else if (err.status === 404) {
          alert('User does not exist');
        } else {
          alert('Unexpected error occurred while resetting password');
        }
      },
    });
  }
}
