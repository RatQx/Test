import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { NgForm } from '@angular/forms';
import { AuctionService } from '../../services/auction.service';
import { PaymentLink } from '../../models/paymentlinks.model';

declare var paypal: any;

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
})
export class UserProfileComponent implements OnInit {
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
  originalUserInfo: User = { ...this.userInfo }; // New property to store original data
  showEditForm: boolean = false;
  showAuctionsWonTable: boolean = false;
  showLikedAuctionsTable: boolean = false;
  userForm!: NgForm;
  paymentLinks: PaymentLink[] = [];
  showPaymentLinksTable: boolean = false;

  constructor(
    private userService: UserService,
    private auctionService: AuctionService
  ) {}

  ngOnInit(): void {
    this.getUserInfo();
    this.getUserPaymentLinks();
  }

  getUserInfo(): void {
    this.userService.getUserInfo().subscribe(
      (response: any) => {
        this.userInfo = response.userInfo;
        this.originalUserInfo = { ...this.userInfo }; // Save the original data
      },
      (error: any) => {
        console.error('Error fetching user info:', error);
      }
    );
  }

  toggleEditForm(): void {
    if (!this.showEditForm) {
      this.originalUserInfo = { ...this.userInfo }; // Save the original data when entering edit mode
    } else {
      this.userInfo = { ...this.originalUserInfo }; // Restore the original data when canceling
    }
    this.showEditForm = !this.showEditForm;
  }

  unlikeAuction(id: number) {
    this.auctionService.unlikeAuction(id).subscribe((data) => {
      const index = this.userInfo.liked_Auctions.findIndex(
        (auction) => auction === id
      );

      if (index !== -1) {
        this.userInfo.liked_Auctions.splice(index, 1);
      }
    });
  }

  submitForm(): void {
    const confirmed = window.confirm(
      'Are you sure you want to save changes on your user information?'
    );
    if (confirmed) {
      this.userInfo = { ...this.userInfo };
      this.toggleEditForm();
      this.userService.updateUserInfo(this.userInfo).subscribe(
        (response) => {
          if (response.status === 200) {
            this.originalUserInfo = { ...this.userInfo }; // Update the original data after successful save
          }
        },
        (error) => {
          console.error(error);
        }
      );
    }
  }

  toggleAuctionsWonTable() {
    this.showAuctionsWonTable = !this.showAuctionsWonTable;
  }

  toggleLikedAuctionsTable() {
    this.showLikedAuctionsTable = !this.showLikedAuctionsTable;
  }

  onPaymentMethodChange(method: string): void {
    if (method === 'paypal') {
      this.userInfo.paypal = true;
      this.userInfo.bank = false;
    } else if (method === 'bank') {
      this.userInfo.bank = true;
      this.userInfo.paypal = false;
    }
  }

  confirmDeleteUser(): void {
    const confirmed = window.confirm(
      'Are you sure you want to delete your account?'
    );
    if (confirmed) {
      this.userService.deleteUser().subscribe(
        () => {},
        (error: any) => {
          console.error('Error deleting user:', error);
        }
      );
    }
  }

  getUserPaymentLinks(): void {
    this.userService.getUserPaymentLinks().subscribe(
      (paymentLinks: PaymentLink[]) => {
        this.paymentLinks = paymentLinks;
      },
      (error: any) => {
        console.error('Error fetching payment links:', error);
      }
    );
  }

  togglePaymentLinksTable(): void {
    this.showPaymentLinksTable = !this.showPaymentLinksTable;
    if (this.showPaymentLinksTable) {
      this.getUserPaymentLinks();
    }
  }

  cancelEdit(): void {
    this.userInfo = { ...this.originalUserInfo }; // Restore the original data
    this.toggleEditForm();
  }
}
