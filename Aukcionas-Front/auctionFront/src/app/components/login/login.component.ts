import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { LoginRequest } from '../../types/aukcionas.types';
import { HttpResponse } from '@angular/common/http';

declare const bootstrap: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  public resetPasswordEmail!: string;
  public isValidEmail!: boolean;
  isLoginFailed = false;
  errorMessage = '';
  emailSent = false;
  emailResetInvalid = false;
  displayMessageEmail = '';
  public modalDisabled: boolean = false;
  constructor(private userService: UserService, private router: Router) {}
  public form = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });
  ngOnInit(): void {}

  onSubmit() {
    try {
      this.userService.login(this.form.value as LoginRequest).subscribe(
        () => {
          this.isLoginFailed = false;
          this.router.navigate(['home']).then(() => {
            window.location.reload();
          });
        },
        (error) => {
          this.isLoginFailed = true;
          if (error.error === 'Invalid username or pasword.') {
            this.errorMessage = 'Username or password was incorrect. Try again';
          } else {
            this.handleError(error);
          }
        }
      );
    } catch {}
  }
  private handleError(error: any): void {
    if (error.error instanceof ErrorEvent) {
      this.errorMessage = `An error occurred: ${error.error.message}`;
    } else {
      this.errorMessage = error.error ? error.error : 'Server error';
    }
  }
  checkValidEmail(event: string) {
    const value = event;
    const pattern = /^[\w\-\.]+@([\w\-]+\.)+[\w-]{2,3}$/;
    this.isValidEmail = pattern.test(value);
    return this.isValidEmail;
  }
  resetPasswordSend(): void {
    if (this.checkValidEmail(this.resetPasswordEmail)) {
      this.userService.sendResetPasswordLink(this.resetPasswordEmail).subscribe(
        () => {
          this.modalDisabled = true;
          this.emailSent = true;
          this.hideResetPasswordModal();
          setTimeout(() => {
            window.location.reload();
          }, 500);
          this.showNotification(
            'Password Reset Sent',
            'Password reset link was sent successfully.'
          );
        },
        (error) => {
          if (
            error.status === 404 &&
            error.error?.message === 'Email does not exist'
          ) {
            this.displayMessageEmail =
              'There is no registered user with this email. Try again.';
          } else {
            this.displayMessageEmail =
              'An error occurred while sending the reset link. Please try again.';
          }
        }
      );
    }
  }
  showNotification(title: string, body: string): void {
    if ('Notification' in window) {
      Notification.requestPermission().then((permission) => {
        if (permission === 'granted') {
          new Notification(title, { body });
        }
      });
    }
  }
  hideResetPasswordModal(): void {
    const modalElement = document.getElementById('resetPasswordModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.hide();
    }
  }
}
