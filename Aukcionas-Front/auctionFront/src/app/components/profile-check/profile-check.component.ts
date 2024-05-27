import { Component, Inject, Input, OnInit } from '@angular/core';
import { User } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-profile-check',
  templateUrl: './profile-check.component.html',
  styleUrls: ['./profile-check.component.scss'],
})
export class ProfileCheckComponent implements OnInit {
  userInfo: User = {
    userName: '',
    email: '',
    name: '',
    surname: '',
    phoneNumber: '',
    auctions_Won: [],
    liked_Auctions: [],
    can_Bid: false,
    paypal: false,
    bank: false,
    paypal_Email: '',
    account_Holder_Name: '',
    account_Number: '',
    bank_Name: '',
    bic_Swift_Code: '',
    collectData: false,
  };
  constructor(
    private userService: UserService,
    public dialogRef: MatDialogRef<ProfileCheckComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit(): void {
    this.getUserInfo(this.data.username);
  }

  getUserInfo(username: string): void {
    this.userService.getUserProfile(username).subscribe(
      (response: any) => {
        this.userInfo = response.userInfo;
      },
      (error: any) => {
        console.error('Error fetching user info:', error);
      }
    );
  }
  closeModal() {}
}
